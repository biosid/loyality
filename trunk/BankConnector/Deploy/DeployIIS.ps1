param($siteName, $siteBindings)
#
# Settings
#---------------
$appPoolName = $siteName
$appPoolFrameworkVersion = "v4.0"
$webRoot = (resolve-path $siteName)

# Installation
#---------------
Import-Module WebAdministration

$appPoolPath = ("IIS:\AppPools\" + $appPoolName)
$pool = Get-Item $appPoolPath -ErrorAction SilentlyContinue
if (!$pool) { 
    Write-Host "App pool does not exist, creating..." 
    new-item $appPoolPath
    $pool = Get-Item $appPoolPath
} else {
    Write-Host "App pool exists." 
}

Write-Host "Set .NET framework version:" $appPoolFrameworkVersion
Set-ItemProperty $appPoolPath managedRuntimeVersion $appPoolFrameworkVersion

Write-Host "Set identity..."
Set-ItemProperty $appPoolPath -name processModel -value @{identitytype="NetworkService"}

Write-Host "Checking site..."
$sitePath = ("IIS:\Sites\" + $siteName)
$site = Get-Item $sitePath -ErrorAction SilentlyContinue
if (!$site) { 
    Write-Host "Site does not exist, creating..." 
    $id = (dir iis:\sites | foreach {$_.id} | sort -Descending | select -first 1) + 1
    new-item $sitePath -bindings @{protocol="http";bindingInformation=$siteBindings} -id $id -physicalPath $webRoot
} else {
    Write-Host "Site exists. Complete"

		Write-Host "Set physicalPath..."
		Set-ItemProperty $sitePath -name physicalPath -Value "$webRoot"
}

Write-Host "Set app pool..."
Set-ItemProperty $sitePath -name applicationPool -value $appPoolName

Write-Host "Set bindings..."
Set-ItemProperty $sitePath -name bindings -value @{protocol="http";bindingInformation=$siteBindings}

Write-Host "IIS configuration complete!"
