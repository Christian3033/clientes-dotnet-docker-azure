# Empezar aquí

## 1. Instalar Docker Desktop

La comprobación local indicó que WSL no está instalado y Docker no está disponible.

1. Abrir PowerShell como administrador y ejecutar `wsl --install`. Reiniciar Windows si lo solicita y completar la configuración de WSL.
2. Descargar Docker Desktop para Windows desde https://docs.docker.com/desktop/setup/install/windows-install/ e instalarlo con el motor WSL 2.
3. Abrir Docker Desktop y esperar a que el motor esté listo. Usar contenedores Linux.
4. Abrir una nueva terminal y comprobar `docker version` y `docker compose version`.

Referencia WSL: https://learn.microsoft.com/en-us/windows/wsl/install

## 2. Configurar la base Azure SQL que ya existe

Desde esta carpeta, copiar `.env.example` a `.env`. Completar AZURE_SQL_CONNECTION_STRING con la conexión de la misma base que utilizaba SistemaClientesDotNet. La API recibirá esa cadena como ConnectionStrings__ConexionSQLAzure.

Consultar la conexión en la configuración del App Service de la API anterior o en el servidor/base SQL existentes del portal de Azure. Si estaba en secretos de usuario de .NET, consultar mediante el administrador de secretos del proyecto original. No publicar capturas de estos valores ni pegarlos en el chat.

Mantener Encrypt=True y TrustServerCertificate=False. Ajustar el firewall de ese servidor existente para el equipo local. No hace falta exportar MySQL ni crear otra base.

## 3. Levantar y comprobar

En PowerShell, desde la carpeta del proyecto:

```powershell
.\scripts\Iniciar-Local.ps1 -Build
.\scripts\Verificar-Lectura.ps1
```

Si Windows bloquea scripts por la política local, ejecutar directamente los comandos equivalentes de README.md; no hace falta cambiar la política global del equipo.

Abrir http://localhost:8081/clientes y completar las pruebas de README.md. Registrar resultados en EVIDENCIAS.md.

## 4. Azure y entrega

Seguir README.md para ACR y los dos App Service con imágenes Docker. La API que ya estaba publicada en el proyecto no demuestra por sí sola que estuviera en Docker: verificar la fuente de imagen en App Service. Se puede reutilizar el mismo App Service si se configura para la nueva imagen del registro.

Antes de entregar son obligatorias las pruebas locales, las dos imágenes en ACR, las dos aplicaciones desplegadas y la URL pública del frontend. El código preparado no reemplaza esas evidencias.
