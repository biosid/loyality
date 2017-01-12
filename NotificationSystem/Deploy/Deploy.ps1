Write-Host "Configure " $envConfig
.\Config\SingleConfig\SingleConfig.exe "$envConfig"

Write-Host "DB update..."
Push-Location '.\database'
autopatch\autopatch.exe /patchtolatest
Pop-Location

Write-Host "DeployIIS..."
.\DeployIIS.ps1 "RapidSoft.Loyalty.NotificationSystem" ":5657:"
