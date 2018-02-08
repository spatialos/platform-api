# Python examples

This directory contains a simple example for listing deployments for a project using the Spatial API.

## Prerequisites

- A POSIX shell
    - On Windows, you can use Git bash or the Windows Subsystem for Linux
- `make` 3.81 or newer
    - If you're on Windows and don't already have `make` installed, you can download it from 
    the [GNUWin32 Project](https://sourceforge.net/projects/gnuwin32/files/make/3.81/make-3.81.exe/download)
    - `make` must be on your `PATH`
- Python 3.6 or newer (Python 2.7 is not supported)
- `virtualenvwrapper`
    - The `virtualenvwrapper` tools (specifically `mkvirtualenv`) must be on your `PATH`.
    - If the tools are not already on your `PATH`, you can add them by loading the `virtualenvwrapper` 
    shell script (located in the  `virtualenvwrapper` installation directory) from your shell's init
    script (e.g. your `.bash_profile` or `.bashrc`).
    
- A SpatialOS Service Account Refresh Token, Client ID and Client Secret
    - If you don't already have one, please reach out to your support contact and they will generate one for you.

## Installation

```
# Clone the repo
git clone https://github.com/improbable-io/spatial-api-alpha.git
cd spatial-api-alpha/examples/python

# Create a new virtual environment
mkvirtualenv --python=python3.6 spatial-api

# Install dependencies and generate code from .proto files
make install
make generate
```

## Running the examples

Please note that this example will not run as the variables `REFRESH_TOKEN, CLIENT_ID, CLIENT_SECRET` in `spatial.py` are not initialized by default. Please make sure this variables are uncommented, and contains your service account's refresh token, client id and client secret.
These examples should not be used in an untrusted environment, ie. inside your game code. Only your game server should have access to the service account's REFRESH_TOKEN, CLIENT_ID and CLIENT_SECRET.

### Creating Player Tokens
```bash
python create_player_token.py [project_name] [player_identifier]

```


## Documentation

* [All available Spatial API methods](https://github.com/improbable-io/spatial-api-alpha/blob/master/docs/index.md)
* [Introduction to gRPC](https://grpc.io/docs/guides/)
* [gRPC Python API](https://grpc.io/grpc/python/grpc.html)
