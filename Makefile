## ----------------------------------------------------------------------
## This is the Inmobiliaria Makefile.
##
## Help comments are displayed in the order defined within the Makefile.
## ----------------------------------------------------------------------
##

build:
	docker-compose exec web dotnet build

migrate:
	docker run -it --rm -v $(CURDIR)/Inmobiliaria:/app  --network="host" ddotnet \
	  dotnet ef database update

create-migration: ## Create database migration using dotnet ef. Usage: make create-migration name="your-change-description"
# Si queres correr en la terminal cambia CURDIR por PWD
	docker run -it --rm -v $(CURDIR)/Inmobiliaria:/app  --network="host" ddotnet \
	  dotnet ef migrations add $(name)

install-dev:
	scripts/install-dev.sh

scaffolding:
	cd Inmobiliaria

	docker run -it --rm \
  	-v $(pwd):/app \
  	ddotnet dotnet-aspnet-codegenerator controller \
	-name FamiliaController -m Familia \
	-dc ApplicationDbContext \
	--relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

	cd ..