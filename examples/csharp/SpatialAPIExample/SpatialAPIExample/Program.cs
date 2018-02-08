using System;
using Grpc.Core;
using Improbable.Spatialos.Playerauth.V1Alpha1;

namespace SpatialAPIExample
{
    class MainClass
    {
        private const int ErrorExitStatus = 1;

        public static void Main(string[] arguments)
        {
            Action printUsage = () =>
            {
                Console.WriteLine("Usage: SpatialAPIExample create_token <project_name> <player_identifier>");
                Console.WriteLine("    <project_name>         - name of the project for which to generate a player token.");
                Console.WriteLine("    <player_identifier>    - a player identifier.");
            };

            if (arguments.Length < 1)
            {
                printUsage();
                Environment.Exit(ErrorExitStatus);
            }

            // The refresh token, client_id and client_secret for your service account.
            // Note: you *must* set these and uncomment the next lines for this example to compile.
            // string refreshToken = "Set your refresh token here.";
            // string clientId = "Set your client ID here";
            // string clientSecret = "Set your client secret here";

            // IoInvoker will automatically acquire an access token if you don't have one, and retry failed requests 
            // with a new access token.
            var invoker = new IoInvoker(
                "api-alpha.improbable.io",
                refreshToken,
                clientId,
                clientSecret
            );

            // Acquire an intial access token.
            invoker.AcquireAccessToken();

            if (arguments[0] == "create_token")
            {
                try
                {
                    var token = CreatePlayerToken(invoker, arguments[1], arguments[2]);
                    Console.WriteLine("Token: " + token);
                }
                catch (RpcException e) when (e.Status.StatusCode == StatusCode.DeadlineExceeded)
                {
                    Console.WriteLine("Request exceeded timeout");
                    Console.WriteLine(e);
                }
            }
            else
            {
                printUsage();
                Environment.Exit(ErrorExitStatus);
            }

            Console.ReadKey(); // show output
            Environment.Exit(0);
        }

        public static string CreatePlayerToken(IoInvoker invoker, string projectName, string playerIdentifier)
        {
            var client = new PlayerTokenService.PlayerTokenServiceClient(invoker);
            var request = new CreatePlayerTokenRequest
            {
                PlayerIdentifier = playerIdentifier,
                ProjectName = projectName,
            };

            var timeout = DateTime.UtcNow.AddSeconds(5);
            CreatePlayerTokenResponse result =
                client.CreatePlayerToken(request, new CallOptions().WithDeadline(timeout));
            Console.WriteLine("Response");
            Console.WriteLine(result);

            return result.Token.Token;
        }
    }
}
