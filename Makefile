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

build-ddotnet:
	docker build -t ddotnet -f ./Inmobiliaria/Dockerfile ./Inmobiliaria

install-dev:
	scripts/install-dev.sh

scaffolding:
	cd Inmobiliaria

	docker run -it --rm \
  	-v $(pwd):/app \
  	ddotnet dotnet-aspnet-codegenerator controller \
	-name UserPermisoController -m UserPermiso \
	-dc ApplicationDbContext \
	--relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

	cd ..

api-scaffolding:
	cd Inmobiliaria

	docker run -it --rm \
        -v $(pwd):/app \
        ddotnet dotnet-aspnet-codegenerator controller \
        -name UserPermissionsApiController -m UserPermission \
        -dc ApplicationDbContext \
        --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -api

	cd ..

bulk-code:
	scripts/bulk_code.py "Inmobiliaria/Models" > .idea/bulk_code/models.txt
	scripts/bulk_code.py "Inmobiliaria/Controllers" > .idea/bulk_code/Controllers.txt
	scripts/bulk_code.py "Inmobiliaria/Services" > .idea/bulk_code/Services.txt
	scripts/bulk_code.py "Inmobiliaria/Views" > .idea/bulk_code/Views.txt

