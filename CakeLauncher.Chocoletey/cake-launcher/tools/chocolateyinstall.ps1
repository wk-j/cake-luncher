$toolsDir     = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$packageName  = 'cake-launcher'
$fileLocation = Join-Path $toolsDir "CakeLauncher-$env:chocolateyPackageVersion.zip"
$register = Join-Path $toolsDir 'CakeLauncher.Register.exe'

Install-ChocolateyZipPackage -PackageName $packageName `
  -Url $fileLocation `
  -UnzipLocation "$(Split-Path -Parent $MyInvocation.MyCommand.Definition)"

 & $register