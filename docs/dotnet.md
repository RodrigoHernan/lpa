dotnet new mvc -o  rramos
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer


create Model/model.cs

docker run -it --rm \
  -v $(pwd):/app \
  ddotnet dotnet-aspnet-codegenerator controller -name MoviesController -m Movie -dc RRamosContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -sqlite

sudo chown -R rramos:rramos *

docker run -it --rm \
  -v $(pwd):/app \
  ddotnet dotnet ef migrations add InitialCreate

docker run -it --rm \
  -v $(pwd):/app \
  ddotnet dotnet ef database update