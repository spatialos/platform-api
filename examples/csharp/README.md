# Prerequisites
* .NET 4.6.1
* C# 7.0
* `nuget`
* An Improbable service account

# Installation

### Configure `nuget`

The Spatial API comes with a `nuget` config file pointed at the correct sources,
but if you have configured `nuget` you may need to adjust the settings.

To use the Spatial API, your `nuget` installation needs to be pointed at the default sources.
If you have disabled the default source `https://www.nuget.org/api/v2/` you will need to enable it with the following command:
```
# Find the name of the source with URL https://www.nuget.org/api/v2/
nuget sources

# Enable the source by name
nuget sources Enable -Name $NameOfTheSource
```

Once `nuget` is pointed at the correct sources, run the following command to ensure the sources' certificates are trusted by Mono:
```
mozroots --import --sync
```

### Run installation commands

```
# Clone the repo
git clone https://github.com/improbable-io/spatial-api-alpha.git
cd spatial-api-alpha/examples/csharp/SpatialAPIExample

# Generate code based on the .proto files
./SpatialAPIExample/generate-protos.sh <windows_x86|windows_x64|linux_x86|linux_x64|macosx_x86|macosx_x64>

# Install NuGet packages using the Spatial API nuget config
nuget install -ConfigFile NuGet.config -OutputDirectory packages SpatialAPIExample/packages.config
```

# Running the example
The code will not compile as it is because the variables `refreshToken, clientId, clientSecret` in `Program.cs` are not initialized by default. To make the example work, please make sure these variables contains your service account's refresh token, client ID and client secret.

## Usage
```bash
Usage: SpatialAPIExample create_token <project_name> <player_identifier>
    <project_name>         - name of the project for which to generate a player token.
    <player_identifier>    - a player identifier.
```

# Interpreting error messages and debugging

The Spatial API uses the gRPC protocol. As you use gRPC and debug your gRPC-enabled application, you may encounter this relatively common gRPC error: `context deadline exceeded`.

At first glance, this error seems to be a timeout, but let's dig deeper. `context deadline exceeded` is returned when your gRPC fails to receive a response from a gRPC server within some time frame (the time frame is configurable). Here are a few reasons why you might see `context deadline exceeded`:
1. Your gRPC client connected to a server, but the server took too long to respond
2. The gRPC server you are trying to reach is offline
3. The gRPC server you are trying to reach is running, but you aren't able to reach it

When you see `context deadline exceeded`, you'll need to check a few things.

**1. Is the gRPC server running?**

To test if the server is up, run the following command:
```
curl https://api-alpha.improbable.io/_healthz --max-time 2 --output /dev/null --silent --write-out '%{http_code}\n'
```

It should output the HTTP code 200.

**2. Can your gRPC client reach the gRPC server?**

Make sure your `IOInvoker` is using the correct URI.

**3. Is the gRPC server responding to _any_ RPCs?**

Try calling other RPCs to determine if any are accessible.
If a specific RPC is timing out, run the code-gen scripts again to rule out `.proto` changes as a cause of the timeout.
If the RPC continues to time out with the latest `.proto`s, you should reach out to your support contact.

# Documentation
* [All available Spatial API methods](https://github.com/improbable-io/spatial-api-alpha/blob/master/docs/index.md)
* [Introduction to gRPC](https://grpc.io/docs/guides/)
* [gRPC C# API](https://grpc.io/grpc/csharp/api/Grpc.Core.html)

