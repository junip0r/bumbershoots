#!/bin/bash

set -e

case "$1" in
    debug|'')
        dotnet build --configuration Debug
        cp -a bin/Debug/net48/Bumbershoots.{dll,pdb} ../Assemblies
        ;;
    release)
        dotnet build --configuration Release
        cp -a bin/Release/net48/Bumbershoots.dll ../Assemblies
        rm -f ../Assemblies/Bumbershoots.pdb
        ;;
    *)
        echo "Usage: $(basename "$0") [debug|release]"
        false
        ;;
esac
