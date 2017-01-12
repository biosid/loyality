Write-Host "Setting up Site... (" $envConfigFront ")"
.\.tools\ctt.exe s:"Site\Web.config" d:"Site\Web.config" t:"Site\$envConfigFront"

Write-Host "Setting up Service... (" $envConfigFrontJobs ")"
.\.tools\ctt.exe s:"Service\Rapidsoft.VTB24.Reports.Service.exe.config" d:"Service\Rapidsoft.VTB24.Reports.Service.exe.config" t:"Service\$envConfigFrontJobs"

Write-Host "Configuring IIS..."
.\DeployIIS.ps1 "VTB24.Statistics" ":61280:"

Write-Host "Configuring Service..."
.\Service\Rapidsoft.VTB24.Reports.Service.exe uninstall
.\Service\Rapidsoft.VTB24.Reports.Service.exe install
