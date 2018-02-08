import grpc
from sanction import Client as OauthClient
from urllib.error import HTTPError

TOKEN_ENDPOINT = "https://auth.improbable.io/auth/v1/token"
HOST = "api-alpha.improbable.io"
# REFRESH_TOKEN = "<YOUR REFRESH TOKEN HERE>"
# CLIENT_ID = "<CLIENT ID HERE>"
# CLIENT_SECRET = "<CLIENT SECRET HERE>"


def retry_grpc_with_credentials(to_wrap):
    """retry_grpc_with_credentials is a decorator for gRPC-wrapping methods. It
    will ensure that gRPC methods which fail due to not being authenticated
    have their token refreshed and are retried with this new token."""

    def get_credentials(self):
        return grpc.access_token_call_credentials(
            self.token_client.access_token
        )

    def wrapper(self, *args, **kwargs):
        to_call = to_wrap(self, *args, **kwargs)

        def f(*args, **kwargs):
            try:
                return to_call(
                    *args, credentials=get_credentials(self), **kwargs
                )
            except grpc.RpcError as err:
                if err.code() == grpc.StatusCode.UNAUTHENTICATED:
                    self.token_client.refresh()
                    return to_call(
                        *args, credentials=get_credentials(self), **kwargs
                    )
                raise err
        return f

    return wrapper


class APIChannel(grpc.Channel):
    """APIChannel wraps a standard grpc.Channel, injecting an
       oauth access token to every call,
       and refreshes the token when needed"""

    def __init__(self):
        self.wrapped_channel = grpc.secure_channel(
            HOST, grpc.ssl_channel_credentials()
        )

        self.token_client = OauthClient(
            token_endpoint=TOKEN_ENDPOINT,
            client_id=CLIENT_ID,
            client_secret=CLIENT_SECRET
        )
        self.token_client.refresh_token = REFRESH_TOKEN

        try:
            self.token_client.refresh()
        except HTTPError as e:
            if e.getcode() == 400:
                print("error: refresh token has expired")
            raise e

    def subscribe(self, callback, try_to_connect=False):
        return self.wrapped_channel.subscribe(callback, try_to_connect)

    def unsubscribe(self, callback):
        return self.wrapped_channel.unsubscribe(callback)

    @retry_grpc_with_credentials
    def unary_unary(self, *args, **kwargs):
        return self.wrapped_channel.unary_unary(*args, **kwargs)

    @retry_grpc_with_credentials
    def unary_stream(self, *args, **kwargs):
        return self.wrapped_channel.unary_stream(*args, **kwargs)

    @retry_grpc_with_credentials
    def stream_unary(self, *args, **kwargs):
        return self.wrapped_channel.stream_unary(*args, **kwargs)

    @retry_grpc_with_credentials
    def stream_stream(self, *args, **kwargs):
        return self.wrapped_channel.stream_stream(*args, **kwargs)
