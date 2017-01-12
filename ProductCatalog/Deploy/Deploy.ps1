write-host "configure " $envconfig
.\config\singleconfig\singleconfig.exe "$envconfig"

write-host "db update..."
Push-Location '.\database'
autopatch\autopatch.exe /patchtolatest
Pop-Location

Write-Host "DeployIIS..." -backgroundcolor green
.\DeployIIS.ps1 "RapidSoft.Loaylty.ProductCatalog.WebServices" ":8101:"

Write-Host "Configuring IIS AppPool..." -backgroundcolor green
$pool = Get-Item IIS:\AppPools\RapidSoft.Loaylty.ProductCatalog.WebServices
$pool.recycling.periodicRestart.privateMemory = 3000000
$pool | Set-Item

write-host "deployquartz..." -backgroundcolor green
.\deployquartz.ps1
