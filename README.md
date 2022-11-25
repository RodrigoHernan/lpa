## Documentos de desarrollador

para instalar requisitos de desarrollador, ver: [./docs/dependencias_de_desarrollador.md](./docs/dependencias_de_desarrollador.md)

## Prerequisitos
 - sql server
 - dotnet o lo que quieran usar para ejecutar el proyecto

### Instalar sql server con docker para dev
~~~
sudo docker run --restart always -v sql-server-data:/data/opt/mssql -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server
~~~

## Correr la aplicación
 - Crear una base de datos ej `app`
 - Crear el archivo app/appsettings.json se puede usar el archivo app/appsettings.json.tamplate como ejemplo
 - al iniciar la primera vez se creara la base de datos
 - Reiniciar la aplicación (I know it's a bullshit) y se creara un usuario admin@gmail.com con contraseña P@ssw0rd


# Misc
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


## instalacion
~~~
# instalar base de datos:
make install
~~~

## Requerimientos
 [*] Login
 [*] Bitacora
 [*] [Verificar integridad del sistema](https://www.codeproject.com/Tips/588941/Check-Digit-Vertical-and-Horizontal)
 [*] Restaurar o recalcular digitos verificadores
 [*] Backup
 [*] Usuario: bloquear luego de 3 intentos
 [*] ABM simple
 [*] Filtro en consulta de bitacora
 [*] Usuario, familia, patente
 [ ] Web service simple
 [*] API AJAX
 [*] Agregar backup
 [ ] Descargar / generar un archivo XML con los datos de una tabla


### En la carpeta:
 [*] Politica de backup
 [*] Ciclo de vida login
 [*] Tipo de conexion y porque
 [*] El codigo por capa y explicar las capas.
 [*] Árbol de navegacion
 [*] Tipo de encriptacion
 [ ] Multi idioma con patron observer


