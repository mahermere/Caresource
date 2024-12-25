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

# Create a scheduled task action
$action = New-ScheduledTaskAction -Execute $setupPath -Argument $arguments

# Create a scheduled task trigger (e.g., run at a specific time or on startup)
$trigger = New-ScheduledTaskTrigger -AtStartup

# Create a scheduled task principal with the gMSA account
$principal = New-ScheduledTaskPrincipal -UserId "Domain\MyGMSA$" -LogonType ServiceAccount -RunLevel Highest

# Register the scheduled task with error handling
try {
    Register-ScheduledTask -TaskName "InstallOnBaseStudio" -Action $action -Trigger $trigger -Principal $principal
    Start-ScheduledTask -TaskName "InstallOnBaseStudio"
} catch {
    Write-Error "Failed to register or start the scheduled task: $_"
}
If you still encounter issues, you might need to check the account settings in Active Directory or consult with your system administrator to ensure the account is correctly configured12. Let me know if you need further assistance!


1
Fix: No Mapping Between Account Names and Security ID was Done - Appuals

2
[SOLVED] No Mapping Between Account Names And Security IDs Was Done

Edit in Pages

5 of 30 responses
AI-generated content may be incorrect


December 25, 2024 at 12:23 PM
You said:How can we change it so it uses the root instead of Microsoft subfolders in the task scheduler
Copilot said:
To change the script so that it uses the root folder instead of the Microsoft subfolders in the Task Scheduler, you need to specify the -TaskPath parameter when registering the scheduled task. This parameter allows you to define the folder path where the task will be created.

Here's the updated script with the -TaskPath parameter set to the root folder (\):

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

# Create a scheduled task action
$action = New-ScheduledTaskAction -Execute $setupPath -Argument $arguments

# Create a scheduled task trigger (e.g., run at a specific time or on startup)
$trigger = New-ScheduledTaskTrigger -AtStartup

# Create a scheduled task principal with the gMSA account
$principal = New-ScheduledTaskPrincipal -UserId "Domain\MyGMSA$" -LogonType ServiceAccount -RunLevel Highest

# Register the scheduled task with error handling
try {
    Register-ScheduledTask -TaskName "InstallOnBaseStudio" -TaskPath "\" -Action $action -Trigger $trigger -Principal $principal
    Start-ScheduledTask -TaskName "InstallOnBaseStudio"
} catch {
    Write-Error "Failed to register or start the scheduled task: $_"
}
