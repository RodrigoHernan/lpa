#!/bin/bash

set -e
export ASPNETCORE_ENVIRONMENT=Development


# until dotnet ef database update; do
# >&2 echo "SQL Server is starting up"
# sleep 1
# done

dotnet dev-certs https

# para correr la app hot hotreloading
dotnet watch --project src run --urls=http://localhost:8000

# para correr la app bien
# dotnet run --urls=http://\*:8000/