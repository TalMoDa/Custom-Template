# Set up variables
$ConnectionString = "Server=(LocalDB)\MSSQLLocalDB;Database=MyDb;Trusted_Connection=True;MultipleActiveResultSets=true"
$provider = "Microsoft.EntityFrameworkCore.SqlServer"
$outputDirTemp = "Data\Entities\EF2"
$outputDirMain = "Data\Entities\EF"
$contextDir = "Data"
$contextName = "CustomDbContext"
$project = "My.Custom.Template.csproj"

# Function to update namespaces in files with UTF-8 encoding
function Update-FileNamespace {
    param (
        [string]$directory,
        [string]$oldNamespace,
        [string]$newNamespace
    )

    # Get all files in the directory
    $files = Get-ChildItem -Path $directory -Recurse -File

    foreach ($file in $files) {
        # Read file content with UTF-8 encoding
        $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8

        # Replace old namespace with new namespace
        $updatedContent = $content -replace [regex]::Escape($oldNamespace), $newNamespace

        # Remove trailing newline if present
        $updatedContent = $updatedContent.TrimEnd("`r", "`n")

        # Write updated content back to file with UTF-8 encoding
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

    try {
        # Check if the file exists
        if (Test-Path $filePath) {
            # Read file content with UTF-8 encoding
            $content = Get-Content -Path $filePath -Raw -Encoding UTF8

            # Replace old namespace with new namespace
            $updatedContent = $content -replace [regex]::Escape($oldNamespace), $newNamespace

            # Remove trailing newline if present
            $updatedContent = $updatedContent.TrimEnd("`r", "`n")

            # Write updated content back to file with UTF-8 encoding
            Set-Content -Path $filePath -Value $updatedContent -NoNewline -Encoding UTF8 -Force
        }
        else {
            Write-Host "Error: File '$filePath' not found."
        }
    }
    catch {
        Write-Host "Error updating namespace in '$filePath'. $_"
    }
}

# Function to display loading message
function Show-Loading {
    Write-Host "Loading..."
    Start-Sleep -Seconds 1  # Adjust the sleep duration as needed
}


# Step 0: Ensure EF2 folder exists or create it
Show-Loading
if (-not (Test-Path $outputDirTemp)) {
    New-Item -ItemType Directory -Path $outputDirTemp | Out-Null
}

# Step 1: Scaffold the new entities into the temporary folder (EF2)
Show-Loading
try {
    dotnet ef dbcontext scaffold $connectionString $provider --output-dir $outputDirTemp --context-dir $contextDir --context $contextName --no-onconfiguring --project $project --force
}
catch {
    Write-Host "Error: Unable to scaffold entities."
    exit 1
}

# Check if EF2 folder exists and has files
Show-Loading
if (Test-Path $outputDirTemp) {
    # Step 2: Delete all existing files in the main folder (EF)
    try {
        Remove-Item -Path $outputDirMain\* -Recurse -Force -ErrorAction Stop
    }
    catch {
        Write-Host "Error: Unable to delete existing files in '$outputDirMain'."
        exit 1
    }

    # Step 3: Move new entities from the temporary folder (EF2) to the main folder (EF)
    try {
        Move-Item -Path $outputDirTemp\* -Destination $outputDirMain -Force -ErrorAction Stop
    }
    catch {
        Write-Host "Error: Unable to move files from '$outputDirTemp' to '$outputDirMain'."
        exit 1
    }

    # Step 4: Remove the temporary folder (EF2)
    try {
        Remove-Item -Path $outputDirTemp -Force -ErrorAction Stop
    }
    catch {
        Write-Host "Error: Unable to remove temporary folder '$outputDirTemp'."
        exit 1
    }

    # Step 5: Update namespaces in the main folder (EF)
    Show-Loading
    Update-FileNamespace -directory $outputDirMain -oldNamespace "My.Custom.Template.Data.Entities.EF2" -newNamespace "My.Custom.Template.Data.Entities.EF"

    # Step 6: Update namespace in CustomDbContext.cs
    Show-Loading
    $contextFilePath = "$contextDir\$contextName.cs"
    Update-ContextNamespace -filePath $contextFilePath -oldNamespace "My.Custom.Template.Data.Entities.EF2" -newNamespace "My.Custom.Template.Data.Entities.EF"
}
else {
    Write-Host "Error: Temporary folder '$outputDirTemp' not found or empty."
    exit 1
}

# Finished
Write-Host "Operations completed successfully."

# Pause at the end to keep PowerShell window open
Read-Host -Prompt "Press Enter to exit"
