FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY ./Inmobiliaria /app
WORKDIR /app

RUN dotnet tool install --global dotnet-ef
RUN dotnet add package Microsoft.EntityFrameworkCore.SqlServer
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
RUN ["dotnet", "dev-certs", "https"]
EXPOSE 7153/tcp

ENV PATH $PATH:/root/.dotnet/tools
# RUN dotnet ef --version

RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh