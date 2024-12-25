# Define the path to the setup executable
$setupPath = "E:\OnBase\Software\Batch-Server\MSI-INSTALL-PACKAGES\OnBase-Studio\setup.exe"

# Define the installation parameters
$installDir = "E:\Program Files\Hyland\"
$logDir = "E:\OnBase\Software\log\"
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$logFile = "${logDir}OnbaseStudioInstall_$timestamp.log"

# Construct the command line arguments
$arguments = "/q /CompleteCommandArgs `"INSTALLDIR=`"$installDir`"` /log `"$logFile`""

# Create a scheduled task action
$action = New-ScheduledTaskAction -Execute $setupPath -Argument $arguments

# Create a scheduled task trigger (e.g., run at a specific time or on startup)
$trigger = New-ScheduledTaskTrigger -AtStartup

# Create a scheduled task principal with the gMSA account
$principal = New-ScheduledTaskPrincipal -UserId "Domain\MyGMSA$" -LogonType Password -RunLevel Highest

# Register the scheduled task
Register-ScheduledTask -TaskName "InstallOnBaseStudio" -Action $action -Trigger $trigger -Principal $principal

# Start the scheduled task
Start-ScheduledTask -TaskName "InstallOnBaseStudio"
