# Clientes: API .NET 10 + Blazor + Docker + Azure SQL

Se reutilizan ApiEmpresa y BlazorClientes del proyecto original. Se conservan las rutas y los campos del CRUD. El frontend ya contiene Bootstrap 5, tabla, formularios, validaciones y modales.

## Aplicación publicada

- Frontend: https://clientes-docker-christian.azurewebsites.net/clientes
- API: https://api-clientes-docker-christian.azurewebsites.net/api/Clientes
- Repositorio: https://github.com/Christian3033/clientes-dotnet-docker-azure
- ACR: clientesacrchristian.azurecr.io
- Imágenes desplegadas: api-clientes:v1 y blazor-clientes:v2.
- App Service Plan Linux compartido: plan-clientes-docker (B1).

Se verificó el CRUD y la persistencia localmente. El estudiante confirmó el funcionamiento del CRUD público. La API publicada consulta la base Azure SQL existente. La versión v2 del frontend actualiza certificados de confianza y configura proxy_ssl_verify_depth 4 para validar la cadena HTTPS de Azure.

## Proyecto reutilizado

Esta entrega parte de C:\xampp\htdocs\SistemaClientesDotNet. Conserva UseAzureSql, la clave de conexión ConexionSQLAzure, el modelo con tabla dbo.clientes y los cinco endpoints originales. No se utiliza como base el ZIP anterior configurado para MySQL.

La cadena de conexión no está incluida en los archivos originales. Debe configurarse con la base Azure SQL existente. No se crean bases, tablas ni migraciones automáticamente.

## Arquitectura

Navegador -> Blazor WebAssembly servido por Nginx -> API .NET 10 -> Azure SQL existente.

El navegador solicita /api/Clientes al mismo origen del frontend. Nginx reenvía esas solicitudes a API_UPSTREAM. En Compose es http://api:8080; en Azure es la URL HTTPS pública de la API. Esto permite usar el nombre del servicio entre contenedores sin intentar resolverlo desde el navegador. No hay acceso del frontend a SQL.

## Requisitos

Docker Desktop con contenedores Linux, .NET 10 para desarrollo opcional, cuenta Azure y la base Azure SQL anterior. Azure CLI para los comandos de ACR. Los recursos de Azure pueden generar cargos.

## Ejecutar con Docker (PowerShell, desde esta carpeta)

1. Copiar .env.example a .env y completar únicamente en el equipo la cadena de la base existente. No subir .env a GitHub.
2. Autorizar la IP pública del equipo en el firewall del servidor SQL existente y comprobar acceso de red al puerto 1433.
3. Ejecutar:

```powershell
Copy-Item .env.example .env
# Editar .env antes de continuar.
docker build -t api-clientes ./ApiEmpresa
docker build -t blazor-clientes ./BlazorClientes
docker images
docker compose up -d
docker ps
docker compose logs --tail 100
```

Abrir http://localhost:8081/clientes. API: http://localhost:8080/api/Clientes. /health comprueba solo que el proceso responde, no la conexión SQL. Las imágenes se construyen con SDK .NET 10; la API se ejecuta con ASP.NET Runtime y el frontend estático con Nginx.

```powershell
# Ver comunicación entre contenedores:
docker compose exec frontend wget -qO- http://api:8080/health
# Ver proxy y acceso SQL:
Invoke-RestMethod http://localhost:8081/api/Clientes
# Detener:
docker compose down
```

Para reconstruir después de cambiar código: docker compose up -d --build.

## Pruebas y evidencias para entregar

- Capturar docker images y docker ps mostrando las dos imágenes y contenedores.
- Consultar listado y GET /api/Clientes/{id} de un registro existente.
- Crear un cliente de prueba desde el formulario con CUI de 13 dígitos y teléfono de 8 dígitos; guardar su ID y comprobar POST 201 en las herramientas de red del navegador.
- Editar el registro; comprobar PUT 204 y el nuevo valor tras recargar.
- Ejecutar docker compose down y docker compose up -d; comprobar que el mismo ID y sus cambios siguen presentes.
- Consultar ese ID en la tabla existente mediante Azure Query Editor para demostrar persistencia real.
- Eliminar únicamente el registro de prueba mediante el modal; comprobar DELETE 204 y GET por ID 404.
- Intentar enviar un formulario inválido y capturar la validación.
- Repetir el CRUD desde la URL pública después del despliegue.

No se han ejecutado estas operaciones contra una base real ni se han generado capturas ficticias.

## Publicar imágenes en Azure Container Registry

Crear en Azure Portal un Container Registry (por ejemplo SKU Basic), sin crear una base de datos. Su nombre debe ser globalmente único. En PowerShell, reemplazar REGISTRO por el nombre real y usar el Login server que muestra Azure:

```powershell
az login
az acr login --name REGISTRO
docker tag api-clientes:latest REGISTRO.azurecr.io/api-clientes:v1
docker tag blazor-clientes:latest REGISTRO.azurecr.io/blazor-clientes:v1
docker push REGISTRO.azurecr.io/api-clientes:v1
docker push REGISTRO.azurecr.io/blazor-clientes:v1
az acr repository list --name REGISTRO --output table
```

## Publicar en dos Azure App Service

1. Crear un App Service Plan Linux y dos Web Apps con publicación mediante contenedor: una API y un frontend. Pueden compartir el plan.
2. Configurar cada una para obtener su imagen v1 del ACR: api-clientes y blazor-clientes respectivamente.
3. Activar identidad administrada en ambas y conceder a cada identidad el rol AcrPull sobre el registro (registro con permisos RBAC clásicos). Seleccionar identidad administrada como autenticación de imagen en Deployment Center.
4. Configurar puerto de destino 8080 en cada contenedor. En la modalidad clásica de contenedor único, establecer WEBSITES_PORT=8080; si el portal usa configuración sidecar, establecer el puerto del contenedor principal en 8080.
5. En variables de entorno de la API establecer ConnectionStrings__ConexionSQLAzure con la cadena real de Azure SQL existente y ASPNETCORE_ENVIRONMENT=Production. Mantener HTTPS Only activo en Azure.
6. Autorizar las IP de salida de la API en el firewall de SQL existente, o usar la conectividad privada que ya tenga la base. No crear otro servidor SQL.
7. Abrir https://DOMINIO-REAL-API/api/Clientes para verificar la conexión antes de publicar el frontend.
8. Primero probar el frontend Docker local con la API publicada: cambiar temporalmente API_UPSTREAM en Compose a https://DOMINIO-REAL-API (sin barra final), ejecutar docker compose up -d --force-recreate frontend y verificar CRUD en localhost:8081/clientes. Luego restablecer http://api:8080 para las pruebas locales entre contenedores.
9. En variables del App Service del frontend establecer API_UPSTREAM=https://DOMINIO-REAL-API (sin barra final). Reiniciar el frontend. La misma imagen sirve en ambos entornos; no requiere recompilar.
10. Abrir https://DOMINIO-REAL-FRONTEND/clientes y repetir las pruebas. Copiar los dominios reales del portal, que pueden incluir un sufijo regional.

Si el frontend devuelve 502, comprobar API_UPSTREAM, DNS y logs del contenedor. Si la API falla al listar, comprobar cadena SQL, firewall y esquema de la tabla. No agregar EnsureCreated o migraciones automáticas para ocultar un problema de esquema.

## Entrega

Subir esta carpeta a un repositorio GitHub sin .env, bin ni obj. Entregar la URL real del repositorio y la URL pública verificada del frontend. No hay enlaces públicos hasta desplegar en la suscripción del estudiante.

## Referencias oficiales

- https://learn.microsoft.com/en-us/ef/core/providers/sql-server/
- https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
- https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly/nginx?view=aspnetcore-10.0
- https://learn.microsoft.com/en-us/azure/app-service/configure-custom-container
- https://learn.microsoft.com/en-us/azure/app-service/tutorial-custom-container?pivots=container-linux


