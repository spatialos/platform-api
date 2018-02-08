#!/usr/bin/env bash

set -o errexit

# Requires protoc version 3 and protoc-gen-doc in the PATH
# protoc: https://github.com/google/protobuf/releases
# protoc-gen-doc: https://github.com/pseudomuto/protoc-gen-doc

cd "$(dirname "$0")/../proto"
rm -rf ../docs
mkdir ../docs
protoc --doc_out=markdown,index.md:../docs $(find . -type f -name '*.proto')

