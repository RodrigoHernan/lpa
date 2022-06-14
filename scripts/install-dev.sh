#!/bin/bash

################################################################################
## Instalando dotnet
################################################################################
echo instalando dotnet
sudo snap install dotnet-sdk --classic --channel=6.0
sudo snap alias dotnet-sdk.dotnet dotnet
