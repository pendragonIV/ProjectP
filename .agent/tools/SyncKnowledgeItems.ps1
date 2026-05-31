# SyncKnowledgeItems.ps1
# Script to synchronize Agent rules (.mdc) to Antigravity Knowledge Items (KI)

$rulesDir = "c:\Dev\swordidle\.agent\rules"
$kiPaths = @(
    "C:\Users\NGDA\.gemini\antigravity\knowledge",
    "C:\Users\NGDA\.gemini\antigravity-ide\knowledge"
)

# Filter paths that can actually be targeted or created
$activeKiDirs = @()
foreach ($kiPath in $kiPaths) {
    $parent = Split-Path $kiPath -Parent
    if (Test-Path $parent) {
        if (-not (Test-Path $kiPath)) {
            New-Item -ItemType Directory -Path $kiPath -Force | Out-Null
        }
        $activeKiDirs += $kiPath
    }
}

if ($activeKiDirs.Count -eq 0) {
    Write-Warning "No valid KI directories found to synchronize!"
    exit 0
}

$files = Get-ChildItem -Path $rulesDir -Filter "*.mdc"

foreach ($file in $files) {
    # Generate KI name
    $kiName = $file.BaseName -replace '^\d+[a-z]?_', ''
    $kiName = $kiName -replace '-', '_'
    
    foreach ($kiDir in $activeKiDirs) {
        $targetKiDir = Join-Path $kiDir $kiName
        $artifactsDir = Join-Path $targetKiDir "artifacts"
        
        if (-not (Test-Path $artifactsDir)) {
            New-Item -ItemType Directory -Path $artifactsDir -Force | Out-Null
        }
        
        $content = Get-Content $file.FullName -Raw
        $description = $kiName
        if ($content -match 'description:\s*"([^"]+)"') {
            $description = $matches[1]
        }
        
        $displayName = $kiName
        if ($content -match 'name:\s*"([^"]+)"') {
            $displayName = $matches[1]
        }
        
        $summary = "${displayName}: $description"
        
        $metadata = @{
            summary = $summary
            timestamp = (Get-Date).ToString("o")
            references = @($file.FullName)
        }
        
        $metadataJson = $metadata | ConvertTo-Json -Depth 3
        Set-Content -Path (Join-Path $targetKiDir "metadata.json") -Value $metadataJson -Encoding UTF8
        
        $destFile = Join-Path $artifactsDir ($kiName + ".md")
        Set-Content -Path $destFile -Value $content -Encoding UTF8
        
        Write-Host "Synced KI: $kiName to $kiDir"
    }
}
Write-Host "Knowledge Items sync completed successfully!"
