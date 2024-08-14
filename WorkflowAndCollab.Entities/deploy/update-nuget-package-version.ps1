param (
    [Parameter(Mandatory=$true)][string]$sourceDirectory,
	[Parameter(Mandatory=$true)][string]$name,
	[Parameter(Mandatory=$true)][string]$branchName
)

function Main() {
	Write-Deploy-Message ("---- STARTING [update-nuget-package-version.ps1] ------------")

	Write-Deploy-Message ("")
	$version = Get-Project-Version $name;

	Write-Deploy-Message ("")
	Validate-Version-Format $version;

	Write-Deploy-Message ("")
	$versionSuffix = Determine-Version-Suffix $branchName;

	Write-Deploy-Message ("")
	Set-Project-Nuget-Version $name $version $versionSuffix;

	Write-Deploy-Message ("")
	Write-Deploy-Message ("---- FINISHED ----------------------------------------------")
	Clear-Host
}

function Get-Project-Version($projectName) {
	Write-Deploy-Message ("+++ Get-Project-Version +++")

	$projectFilePath = $sourceDirectory + "\" + $projectName + ".csproj";

	Write-Deploy-Message ("ProjectFilePath = " + $projectFilePath)

	$projectFile = [xml](Get-Content -Path $projectFilePath)

	$foundVersion = $projectFile.SelectNodes('//Project/PropertyGroup/Version').Item(0).InnerText

	Write-Deploy-Message ("Found version [" + $foundVersion + "] in [" + $projectName + ".csproj] file.")

	return $foundVersion
}

function Validate-Version-Format($projectVersion) {
	Write-Deploy-Message ("+++ Validate-Version-Format +++")

	if (-not $projectVersion -match "^[0-9]+.[0-9]+") {
		throw 'Project version value is not in the format "<number>.<number>" in the csproj file.';
	}

	Write-Deploy-Message ("Version [" + $projectVersion + "] is in a validate format.")
}

function Determine-Version-Suffix($branch) {
	Write-Deploy-Message ("+++ Determine-Version-Suffix +++")

	$suffix = "alpha"; # This is the default and will be used for feature branches.

	If ($branch -match "^refs/heads/master") {
		$suffix = "";
	} ElseIf ($branch -match "^refs/heads/develop") {
		$suffix = "beta";
	} ElseIf ($branch -match "^refs/heads/release/.*") {
		$suffix = "rc";
	}

	Write-Deploy-Message ("Version suffix [" + $suffix + "] will be used for build from branch [" + $branch + "].")
	return $suffix
}

function Set-Project-Nuget-Version($projectName, $projectVersion, $projectVersionSuffix) {
	Write-Deploy-Message ("+++ Set-Project-Nuget-Version +++")

	$projectFilePath = $sourceDirectory + "\" + $projectName + ".csproj";

	Write-Deploy-Message ("ProjectFilePath = " + $projectFilePath)

	$projectFile = [xml](Get-Content -Path $projectFilePath)

	$version = $projectVersion + '.$(BUILD_BUILDID)';
	if (-not [string]::IsNullOrEmpty($projectVersionSuffix)) {
		$version = $version + "-" + $projectVersionSuffix;
		Write-Deploy-Message ("Project version change to [" + $version + "] to include suffix.")
	}

	$projectFile.SelectNodes('//Project/PropertyGroup/Version').Item(0).InnerText = $version;
	Write-Deploy-Message ("Changed Version element to [" + $version + "].")

	$versionAttr = $projectFile.CreateAttribute("Condition");
	$versionAttr.Value = " " + '$(BUILD_BUILDID)' + " != '' "
	$projectFile.SelectNodes('//Project/PropertyGroup/Version').Item(0).Attributes.Append($versionAttr)
	Write-Deploy-Message ("Added [" + $versionAttr.Name + "] attriubute with value [" + $versionAttr.Value + "] to Version element.")

	$assemblyVersionElement = $projectFile.SelectNodes('//Project/PropertyGroup/AssemblyVersion').Item(0);
	if ($assemblyVersionElement)
	{
		throw [System.Exception] "Found element 'AssemblyVersion' in file '$projectName.csproj'. Please remove this element and re-build."
	}
	$fileVersionElement = $projectFile.SelectNodes('//Project/PropertyGroup/FileVersion').Item(0);
	if ($fileVersionElement)
	{
		throw [System.Exception] "Found element 'FileVersion' in file '$projectName.csproj'. Please remove this element and re-build."
	}

	$projectFile.Save($projectFilePath);

	Write-Deploy-Message ("Modifed [" + $projectName + ".csproj] to include conditional Version element.")
}

function Write-Deploy-Message($message) {
	Write-Host ($message)
}

Main | Out-Null