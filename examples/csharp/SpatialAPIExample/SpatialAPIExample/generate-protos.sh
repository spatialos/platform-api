#!/usr/bin/env bash

# Generate C# files based on the Spatial API protos.
# 
# Prerequisites:
#     nuget
#
# Usage:
#   ./generate-protos.sh architecture
#   
# Where architecture is one of the following:
#   windows_x86
#   windows_x64
#   linux_x86
#   linux_x64
#   macosx_x86
#   macosx_x64

cd "$(dirname "$0")"

USAGE_TEXT="Usage:\n
\t./generate-protos.sh architecture\n
\n
Where architecture is one of the following:\n
\twindows_x86\n
\twindows_x64\n
\tlinux_x86\n
\tlinux_x64\n
\tmacosx_x86\n
\tmacosx_x64"

function die {
  echo -e $1
  exit 1
}

function exists {
  command -v $1 >/dev/null 2>&1
}

! exists cd && die "cd is required to run this script."
! exists rm && die "rm is required to run this script."
! exists curl && die "curl is required to run this script."
! exists unzip && die "unzip is required to run this script."
! exists mkdir && die "mkdir is required to run this script."
! exists chmod && die "chmod is required to run this script."

ARCH=$1

PROTO_ROOT=../../../../proto

GRPC_TOOLS_VERSION="1.6.0"
GRPC_TOOLS_ROOT="./packages/Grpc.Tools.$GRPC_TOOLS_VERSION"
ARCH_TOOLS="$GRPC_TOOLS_ROOT/tools/$ARCH"
PROTOC="$ARCH_TOOLS/protoc"
PROTOC_PLUGIN="$ARCH_TOOLS/grpc_csharp_plugin"

case $ARCH in
  windows_x86) PROTOC=$PROTOC.exe; PROTOC_PLUGIN=$PROTOC_PLUGIN.exe ;;
  windows_x64) PROTOC=$PROTOC.exe; PROTOC_PLUGIN=$PROTOC_PLUGIN.exe ;;
  linux_x86);;
  linux_x64);;
  macosx_x86);;
  macosx_x64);;
  *) die "Invalid architecture:: ${ARCH}\n${USAGE_TEXT}" ;;
esac

# Install gRPC tools if they aren't already present.
mkdir -p packages
if [ ! -d "packages/Grpc.Tools.$GRPC_TOOLS_VERSION" ]; then
  echo "Installing Grpc.Tools..."
  temp_dir=packages/Grpc.Tools.1.6.0/tmp
  curl_url=https://www.nuget.org/api/v2/package/Grpc.Tools/
  mkdir -p $temp_dir && cd $temp_dir && curl -sL $curl_url > tmp.zip; unzip tmp.zip && cd .. && cp -r tmp/tools . && rm -rf tmp && cd ../..
  chmod +rwx $PROTOC
  chmod +rwx $PROTOC_PLUGIN
fi

echo "Generating files..."
mkdir -p generated
$PROTOC -I$PROTO_ROOT --csharp_out=generated --grpc_out=generated $PROTO_ROOT/improbable/spatialos/playerauth/v1alpha1/*.proto --plugin=protoc-gen-grpc=$PROTOC_PLUGIN

echo "ok"
exit 0
