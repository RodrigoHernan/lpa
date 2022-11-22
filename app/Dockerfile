FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY . /app
WORKDIR /app

RUN dotnet tool install --global dotnet-ef
RUN dotnet tool install -g dotnet-aspnet-codegenerator --version 6.0.6


RUN dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.5
RUN dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0.5

RUN dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.6
RUN dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 6.0.6

RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
RUN ["dotnet", "dev-certs", "https"]
EXPOSE 7153/tcp

ENV PATH $PATH:/root/.dotnet/tools
# RUN dotnet ef --version

RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh