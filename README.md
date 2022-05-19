## Documentos de desarrollador

para instalar requisitos de desarrollador, ver: [./docs/dependencias_de_desarrollador.md](./docs/dependencias_de_desarrollador.md)

## Correr la aplicación
~~~
dotnet run 
~~~


## Migraciones
Al crear un cambio en los modelos debemos reflejarlo en el modelo 
Crear la migración
~~~
dotnet ef migrations add <nombremigracion>
~~~

Impactarla en la base de datos
~~~
dotnet ef database update      
~~~
