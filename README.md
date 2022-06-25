## Documentos de desarrollador

para instalar requisitos de desarrollador, ver: [./docs/dependencias_de_desarrollador.md](./docs/dependencias_de_desarrollador.md)

## Correr la aplicación
~~~
make install-dev
~~~


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
[*] v0.1 Login
[*] v0.2 Registrar
[*] v0.3 Bitacoraque
[*] v0.4 [Verificar integridad del sistema](https://www.codeproject.com/Tips/588941/Check-Digit-Vertical-and-Horizontal)
[*] v0.5 Restaurar o recalcular digitos verificadores
[ ] v0.6 Backup
[ ] v0.7 Usuario: bloquear luego de 3 intentos
[*] v0.8 ABM
[ ] v0.9 Usuario, familia, patente
[ ] v0.10 Filtro de fecha en consulta de bitacora

En la carpeta falta:
[ ] Politica de backup
[ ] ciclo de vida login:
    - 1ro compuebra si exste usuario logieado
[ ] tipo de conexion y porque
[ ] el codigo por capa y explicar las capas.
[ ] arbol de navegacion
[ ] tipo de encriptacion
