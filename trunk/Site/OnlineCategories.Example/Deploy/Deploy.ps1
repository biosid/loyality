Write-Host "Configure " $envConfigFront
.\.tools\ctt.exe s:"web.config" d:"web.config" t:"$envConfigFront"

Write-Host "DeployIIS..."
.\Deploy\DeployIIS.ps1 "VTB24.OnlineCategories.Example" ":59384:"