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


## base de datos
~~~
para correr base de datos, se debe modificar el archivo appsettings.json.template a appsettings.json y colocar las variables de entorno adecuadas
~~~ 


## Usuario admin 

Se debe crear el siguiente usuario: 
user: admin@gmail.com

al reiniciar la aplicacion, el mismo poseera credenciales de administrador