Write-Host "Configure " $envConfigFront
.\.tools\ctt.exe s:"web.config" d:"web.config" t:"$envConfigFront"

Write-Host "DeployIIS..."
.\DeployIIS.ps1 "VTB24Site" ":3139:"