param([string]$FrontendUrl = 'http://localhost:8081', [string]$ApiUrl = 'http://localhost:8080')
$ErrorActionPreference = 'Stop'
# Solo lecturas: no imprime los datos personales del listado.
$health = Invoke-WebRequest "$($ApiUrl.TrimEnd('/'))/health"
$direct = Invoke-WebRequest "$($ApiUrl.TrimEnd('/'))/api/Clientes"
$proxy = Invoke-WebRequest "$($FrontendUrl.TrimEnd('/'))/api/Clientes"
$directRows = @($direct.Content | ConvertFrom-Json)
$proxyRows = @($proxy.Content | ConvertFrom-Json)
[pscustomobject]@{
    APIHealth = $health.StatusCode
    APIListado = $direct.StatusCode
    FrontendProxy = $proxy.StatusCode
    ClientesDirectos = $directRows.Count
    ClientesPorFrontend = $proxyRows.Count
}
Write-Host 'Lectura comprobada. El CRUD y la persistencia se verifican siguiendo README.md.'
