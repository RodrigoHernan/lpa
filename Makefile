## ----------------------------------------------------------------------
## This is the Inmobiliaria Makefile.
##
## Help comments are displayed in the order defined within the Makefile.
## ----------------------------------------------------------------------
##

build:
	docker-compose exec web dotnet build

migrate:
	docker-compose run web dotnet ef database update

create-migration: ## Create database migration using dotnet ef. Usage: make create-migration name="your-change-description"
	docker-compose run web dotnet ef migrations add $(name)