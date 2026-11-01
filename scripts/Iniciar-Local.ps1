param([switch]$Build)
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    throw 'Instala Docker Desktop, abre una nueva terminal y vuelve a ejecutar este script.'
}
docker info | Out-Null
if ($LASTEXITCODE -ne 0) { throw 'Abre Docker Desktop y espera a que el motor esté listo.' }
if (-not (Test-Path -LiteralPath '.env')) {
    Copy-Item '.env.example' '.env'
    throw 'Se creó .env. Completa la conexión Azure SQL existente antes de continuar.'
}
docker compose config --quiet
if ($LASTEXITCODE -ne 0) { throw 'Revisa la configuración de Compose y .env.' }
if ($Build) { docker compose up -d --build } else { docker compose up -d }
if ($LASTEXITCODE -ne 0) { throw 'No se pudo iniciar la solución. Revisa docker compose logs.' }
docker ps
Write-Host 'Frontend: http://localhost:8081/clientes'
Write-Host 'API: http://localhost:8080/api/Clientes'
