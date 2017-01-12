write-host "configure " $envconfig
.\config\singleconfig\singleconfig.exe "$envconfig"

write-host "db update..."
Push-Location '.\database'
autopatch\autopatch.exe /patchtolatest
Pop-Location

Write-Host "DeployIIS..."
.\DeployIIS.ps1 "RapidSoft.Loaylty.PartnersConnector" ":543:"
.\DeployIIS.ps1 "RapidSoft.Loaylty.PartnersConnector.Internal" ":700:"
.\DeployIIS.ps1 "RapidSoft.Loaylty.PartnersConnector.TestPartner" ":643:"

write-host "deployquartz..." -backgroundcolor green
.\deployquartz.ps1
