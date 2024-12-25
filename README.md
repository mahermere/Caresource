# Define the path to the setup executable
$setupPath = "E:\OnBase\Software\Batch-Server\MSI-INSTALL-PACKAGES\OnBase-Studio\setup.exe"

# Define the installation parameters
$installDir = "E:\Program Files\Hyland\"
$logDir = "E:\OnBase\Software\log\"
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$logFile = "${logDir}OnbaseStudioInstall_$timestamp.log"

# Ensure the log directory exists
if (-Not (Test-Path -Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir
}

# Construct the command line arguments
$arguments = "/q /CompleteCommandArgs `"INSTALLDIR=`"$installDir`"` /log `"$logFile`""

# Properly quote the arguments to handle spaces
$quotedArguments = $arguments -replace ' ', '` '

# Execute the setup executable with the specified arguments
try {
    Start-Process -FilePath $setupPath -ArgumentList $quotedArguments -Wait -NoNewWindow
    Write-Output "OnBase Studio installation completed successfully."
} catch {
    Write-Error "Failed to execute the OnBase Studio installation: $_"
}
