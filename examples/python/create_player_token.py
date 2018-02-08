import argparse
import grpc
from spatial import APIChannel

try:
    import improbable.spatialos.playerauth.v1alpha1.player_identity_pb2_grpc \
        as player_identity_pb_grpc
    import improbable.spatialos.playerauth.v1alpha1.player_identity_pb2 \
        as player_identity_pb
except ImportError:
    print("error: please run 'make generate' to generate the gRPC interfaces")
    exit(1)

parser = argparse.ArgumentParser(description='Create Player Identity token')
parser.add_argument(
    'project_name',
    help='the name of the project for which to create a player token'
)
parser.add_argument(
    'player_identifier',
    help='identifier for the player we want to crate a token for'
)
args = parser.parse_args()

stub = player_identity_pb_grpc.PlayerTokenServiceStub(APIChannel())

try:
    resp = stub.CreatePlayerToken(
        player_identity_pb.CreatePlayerTokenRequest(
            player_identifier=args.player_identifier,
            project_name=args.project_name,
        ),
        timeout=5
    )
    print(resp)

except grpc.RpcError as err:
    if err.code() == grpc.StatusCode.DEADLINE_EXCEEDED:
        print("Request timed out.")
        exit(1)
    raise err
