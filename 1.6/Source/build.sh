#!/bin/bash

set -e

case "$1" in
    debug|'')
        dotnet build --configuration Debug "-p:DefineConstants=\"$DEFINE\""
        ;;
    release)
        dotnet build --configuration Release "-p:DefineConstants=\"$DEFINE\""
        ;;
    *)
        echo "Usage: $(basename "$0") [debug|release]"
        false
        ;;
esac
