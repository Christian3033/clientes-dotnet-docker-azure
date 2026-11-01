# Validación

- Base reutilizada: SistemaClientesDotNet; API .NET 10, EF Core, Azure SQL y frontend Blazor.
- API: compilación Release completada.
- Las imágenes de API y Blazor se construyeron correctamente en Docker Desktop. La publicación de Blazor en Linux completó sin el fallo del host MSBuild de Windows.
- El estudiante verificó CRUD local y persistencia tras reiniciar los contenedores.
- Imágenes publicadas en Azure Container Registry: api-clientes:v1 y blazor-clientes:v2.
- Dos App Service Linux desplegados en el mismo plan B1 con identidad administrada y rol AcrPull.
- API pública devolvió datos JSON de Azure SQL.
- Se corrigió el error 502 de Nginx ajustando la cadena de confianza HTTPS; el listado público se mostró correctamente y el estudiante confirmó el CRUD público.
- Las credenciales se configuran mediante .env local y variables del App Service. .env está excluido de Git.

Frontend: https://clientes-docker-christian.azurewebsites.net/clientes
API: https://api-clientes-docker-christian.azurewebsites.net/api/Clientes
