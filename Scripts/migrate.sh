#!/bin/bash

CommonProjectPath=$PWD/../SCHALE.Common/SCHALE.Common.csproj
GameServerProjectPath=$PWD/../SCHALE.GameServer/SCHALE.GameServer.csproj

dotnet-ef migrations add $1 --project $CommonProjectPath --startup-project $GameServerProjectPath --context SCHALEContext