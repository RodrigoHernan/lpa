## ----------------------------------------------------------------------
## This is the app Makefile.
##
## Help comments are displayed in the order defined within the Makefile.
## ----------------------------------------------------------------------
##

build:
	docker-compose exec web dotnet build

migrate:
	docker run -it --rm -v $(CURDIR)/app:/app  --network="host" ddotnet \
	  dotnet ef database update

create-migration: ## Create database migration using dotnet ef. Usage: make create-migration name="your-change-description"
# Si queres correr en la terminal cambia CURDIR por PWD
	docker run -it --rm -v $(CURDIR)/app:/app  --network="host" ddotnet \
	  dotnet ef migrations add $(name)

build-ddotnet:
	docker build -t ddotnet -f ./app/Dockerfile ./app

install-dev:
	scripts/install-dev.sh

scaffolding:
	cd app

	docker run -it --rm \
  	-v $(pwd):/app \
  	ddotnet dotnet-aspnet-codegenerator controller \
	-name UserPermisoController -m UserPermiso \
	-dc ApplicationDbContext \
	--relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

	cd ..

api-scaffolding:
	cd app

	docker run -it --rm \
        -v $(pwd):/app \
        ddotnet dotnet-aspnet-codegenerator controller \
        -name UserPermissionsApiController -m UserPermission \
        -dc ApplicationDbContext \
        --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -api

	cd ..

bulk-code:
	scripts/bulk_code.py "app/Models" > .idea/bulk_code/models.txt
	scripts/bulk_code.py "app/Controllers" > .idea/bulk_code/Controllers.txt
	scripts/bulk_code.py "app/Services" > .idea/bulk_code/Services.txt
	scripts/bulk_code.py "app/Views" > .idea/bulk_code/Views.txt

