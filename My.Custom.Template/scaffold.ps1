# Set up variables
$connectionString = '"Server=(LocalDB)\MSSQLLocalDB;Database=MyDb;Trusted_Connection=True;MultipleActiveResultSets=true"'
$provider = "Microsoft.EntityFrameworkCore.SqlServer"
$outputDirTemp = "Data\Entities\EF2"
$outputDirMain = "Data\Entities\EF"
$contextDir = "Data"
$contextName = "CustomDbContext"
$project = "My.Custom.Template.csproj"
$namespaceOld = "My.Custom.Template.Data.Entities.EF2"
$namespaceNew = "My.Custom.Template.Data.Entities.EF"

# Function to update namespaces in files with UTF-8 encoding
function Update-FileNamespace {
    param (
        [string]$directory,
        [string]$oldNamespace,
        [string]$newNamespace
    )

    $files = Get-ChildItem -Path $directory -Recurse -File
    foreach ($file in $files) {
        $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8
        $updatedContent = $content -replace [regex]::Escape($oldNamespace), $newNamespace
        $updatedContent = $updatedContent.TrimEnd("`r", "`n")
        Set-Content -Path $file.FullName -Value $updatedContent -NoNewline -Encoding UTF8 -Force
    }
}

# Function to update namespaces in Context.cs file with UTF-8 encoding
function Update-ContextNamespace {
    param (
        [string]$filePath,
        [string]$oldNamespace,
        [string]$newNamespace
    )

    if (Test-Path $filePath) {
        $content = Get-Content -Path $filePath -Raw -Encoding UTF8
        $updatedContent = $content -replace [regex]::Escape($oldNamespace), $newNamespace
        $updatedContent = $updatedContent.TrimEnd("`r", "`n")
        Set-Content -Path $filePath -Value $updatedContent -NoNewline -Encoding UTF8 -Force
    } else {
        Write-Host "Error: File '$filePath' not found."
    }
}

# Function to display loading message
function Show-Loading {
    Write-Host "Loading..."
    Start-Sleep -Seconds 1
}

# Step 0: Ensure EF2 folder exists or create it
Show-Loading
if (-not (Test-Path $outputDirTemp)) {
    New-Item -ItemType Directory -Path $outputDirTemp | Out-Null
}

# Step 1: Scaffold the new entities into the temporary folder (EF2)
Show-Loading
try {
    Write-Host "Scaffolding entities..."
    $commandText = "dotnet ef dbcontext scaffold $connectionString $provider --output-dir $outputDirTemp --context-dir $contextDir --context $contextName --no-onconfiguring --project $project --force"
    Write-Host "Executing command: $commandText"

    Invoke-Expression $commandText

    Write-Host "Entities scaffolded successfully."
} catch {
    Write-Host "Error: Unable to scaffold entities. Error details: $_"
    exit 1
}

# Verify that EF2 folder contains files after scaffolding
Show-Loading
if (!(Get-ChildItem -Path $outputDirTemp -Recurse | Where-Object { $_.PSIsContainer -eq $false })) {
    Write-Host "Error: Temporary folder '$outputDirTemp' is empty after scaffolding."
    exit 1
}

# Step 2: Delete all existing files in the main folder (EF)
try {
    Write-Host "Deleting all existing files in '$outputDirMain'"
    Remove-Item -Path "$outputDirMain\*" -Recurse -Force -ErrorAction Stop
    Write-Host "Existing files in '$outputDirMain' deleted successfully."
} catch {
    Write-Host "Error: Unable to delete existing files in '$outputDirMain'. Error details: $_"
    exit 1
}

# Step 3: Move new entities from the temporary folder (EF2) to the main folder (EF)
try {
    Write-Host "Moving files from '$outputDirTemp' to '$outputDirMain'"
    Move-Item -Path "$outputDirTemp\*" -Destination $outputDirMain -Force -ErrorAction Stop
    Write-Host "Files moved successfully."
} catch {
    Write-Host "Error: Unable to move files from '$outputDirTemp' to '$outputDirMain'. Error details: $_"
    exit 1
}

# Step 4: Remove the temporary folder (EF2)
try {
    Write-Host "Removing temporary folder '$outputDirTemp'"
    Remove-Item -Path $outputDirTemp -Force -ErrorAction Stop
    Write-Host "Temporary folder removed successfully."
} catch {
    Write-Host "Error: Unable to remove temporary folder '$outputDirTemp'. Error details: $_"
    exit 1
}

# Step 5: Update namespaces in the main folder (EF)
Show-Loading
Write-Host "Updating namespaces in files within '$outputDirMain'"
Update-FileNamespace -directory $outputDirMain -oldNamespace $namespaceOld -newNamespace $namespaceNew

# Step 6: Update namespace in CustomDbContext.cs
Show-Loading
$contextFilePath = "$contextDir\$contextName.cs"
Write-Host "Updating namespace in '$contextFilePath'"
Update-ContextNamespace -filePath $contextFilePath -oldNamespace $namespaceOld -newNamespace $namespaceNew

# Finished
Write-Host "Operations completed successfully."
Read-Host -Prompt "Press Enter to exit"
