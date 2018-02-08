using System;
using System.Net;
using System.Text;
using System.IO;
using Grpc.Core;
using Newtonsoft.Json.Linq;

namespace SpatialAPIExample
{
    public class IoInvoker : DefaultCallInvoker
    {
        private readonly string refreshToken;
        private readonly string clientId;
        private readonly string clientSecret;
        private string accessToken;

        private static Channel GetChannelForInvoker(string host, string certificate)
        {
            if (!string.IsNullOrWhiteSpace(certificate))
            {
                return new Channel(host, new SslCredentials(certificate));
            }

            return new Channel(host, new SslCredentials());
        }

        public IoInvoker(string host, string refreshToken, string clientId, string clientSecret, string certificate = null) : base(
            GetChannelForInvoker(host, certificate))
        {
            this.refreshToken = refreshToken;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        /// <summary>
        /// Acquire a fresh access token using your refresh token, replacing the existing access token.
        /// </summary>
        public void AcquireAccessToken()
        {
            // The token API request body contains the refresh token.
            var requestBody = $"client_id={clientId}&client_secret={clientSecret}&refresh_token={refreshToken}&grant_type=refresh_token";

            var request = (HttpWebRequest) WebRequest.Create("https://auth.improbable.io/auth/v1/token");
            var data = Encoding.ASCII.GetBytes(requestBody);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse) request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var responseJObject = JObject.Parse(responseString);
            var token = responseJObject["access_token"];

            accessToken = (string) token;
        }

        /// <summary>
        /// Returns a copy of Metadata. Does not modify the input Metadata.
        /// </summary>
        private static Metadata CloneMetadata(Metadata source)
        {
            if (source == null)
            {
                return new Metadata();
            }

            var entries = new Metadata.Entry[source.Count];
            source.CopyTo(entries, 0);

            var dest = new Metadata();

            foreach (var e in entries)
            {
                dest.Add(e);
            }

            return dest;
        }


        /// <summary>
        ///  Generate CallOptions with the Authorization header populated with the access token.
        ///  Does not modify the input CallOptions.
        /// </summary>
        private CallOptions OptionsWithAuthorization(CallOptions baseOptions)
        {
            // Create an Authorization header with the access token and add it to the provided metadata.
            var metadata = CloneMetadata(baseOptions.Headers);
            metadata.Add(new Metadata.Entry("Authorization", "Bearer " + accessToken));

            return baseOptions.WithHeaders(metadata);
        }

        // Override all of the gRPC calls, and retry each of them with a new access token if they fail.

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method,
            string host, CallOptions options, TRequest request)
        {
            try
            {
                return base.BlockingUnaryCall(method, host, OptionsWithAuthorization(options), request);
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.Unauthenticated ||
                                         e.Status.StatusCode == StatusCode.PermissionDenied)
            {
                AcquireAccessToken();
                return base.BlockingUnaryCall(method, host, OptionsWithAuthorization(options),
                    request);
            }
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
        {
            try
            {
                return base.AsyncUnaryCall(method, host, OptionsWithAuthorization(options), request);
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.Unauthenticated ||
                                         e.Status.StatusCode == StatusCode.PermissionDenied)
            {
                AcquireAccessToken();
                return base.AsyncUnaryCall(method, host, OptionsWithAuthorization(options), request);
            }
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
        {
            try
            {
                return base.AsyncServerStreamingCall(method, host, OptionsWithAuthorization(options), request);
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.Unauthenticated ||
                                         e.Status.StatusCode == StatusCode.PermissionDenied)
            {
                AcquireAccessToken();
                return base.AsyncServerStreamingCall(method, host, OptionsWithAuthorization(options), request);
            }
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method, string host, CallOptions options)
        {
            try
            {
                return base.AsyncClientStreamingCall(method, host, OptionsWithAuthorization(options));
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.Unauthenticated ||
                                         e.Status.StatusCode == StatusCode.PermissionDenied)
            {
                AcquireAccessToken();
                return base.AsyncClientStreamingCall(method, host, OptionsWithAuthorization(options));
            }
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method, string host, CallOptions options)
        {
            try
            {
                return base.AsyncDuplexStreamingCall(method, host, OptionsWithAuthorization(options));
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.Unauthenticated ||
                                         e.Status.StatusCode == StatusCode.PermissionDenied)
            {
                AcquireAccessToken();
                return base.AsyncDuplexStreamingCall(method, host, OptionsWithAuthorization(options));
            }
        }
    }
}