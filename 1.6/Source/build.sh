#!/bin/bash

set -e

#LOG="DEBUG_DATAHELPER;DEBUG_MAPSTATE;DEBUG_PAWNSTATE"
LOG="DEBUG_DATAHELPER"

case "$1" in
    debug-log|'')
        dotnet build --configuration Debug "-p:DefineConstants=\"$LOG\""
        cp -a bin/Debug/net48/Bumbershoots.{dll,pdb} ../Assemblies
        ;;
    debug)
        dotnet build --configuration Debug
        cp -a bin/Debug/net48/Bumbershoots.{dll,pdb} ../Assemblies
        ;;
    release-log)
        dotnet build --configuration Release "-p:DefineConstants=\"$LOG\""
        cp -a bin/Release/net48/Bumbershoots.dll ../Assemblies
        rm -f ../Assemblies/Bumbershoots.pdb
        ;;
    release)
        dotnet build --configuration Release
        cp -a bin/Release/net48/Bumbershoots.dll ../Assemblies
        rm -f ../Assemblies/Bumbershoots.pdb
        ;;
    *)
        echo "Usage: $(basename "$0") [debug-log|debug|release-log|release]"
        false
        ;;
esac
