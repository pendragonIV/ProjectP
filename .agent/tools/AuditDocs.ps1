# AuditDocs.ps1
# Script to audit documentation coverage for gameplay/UI features in Swordidle

$projectRoot = "c:\Dev\swordidle"
$scriptsDir = Join-Path $projectRoot "Assets\_Game\Scripts"
$docsDir = Join-Path $projectRoot "Assets\Docs\Features"

# Define directories to search for features
$sourcePaths = @(
    (Join-Path $scriptsDir "UI"),
    (Join-Path $scriptsDir "Game")
)

# Ignore these directory names
$ignoredFeatures = @("Common", "Utils", "Base", "Interfaces", "Core")

$features = @{}

# Scan scripts directories to identify features
foreach ($sourcePath in $sourcePaths) {
    if (Test-Path $sourcePath) {
        $subdirs = Get-ChildItem -Path $sourcePath -Directory
        foreach ($subdir in $subdirs) {
            $featureName = $subdir.Name
            if ($ignoredFeatures -contains $featureName) {
                continue
            }
            if (-not $features.ContainsKey($featureName)) {
                $features[$featureName] = @{
                    Name = $featureName
                    ScriptPaths = @($subdir.FullName)
                }
            } else {
                $features[$featureName].ScriptPaths += $subdir.FullName
            }
        }
    }
}

# Add any additional manual features documented that we want to track
if (Test-Path $docsDir) {
    $docDirs = Get-ChildItem -Path $docsDir -Directory
    foreach ($docDir in $docDirs) {
        $featureName = $docDir.Name
        if (-not $features.ContainsKey($featureName)) {
            $features[$featureName] = @{
                Name = $featureName
                ScriptPaths = @()
            }
        }
    }
}

$totalFeatures = $features.Count
$documentedCount = 0
$results = @()

# Audit documentation for each feature
foreach ($key in $features.Keys) {
    $feature = $features[$key]
    $name = $feature.Name
    $specPath = Join-Path $docsDir (Join-Path $name "FEATURE_SPEC.md")
    $setupPath = Join-Path $docsDir (Join-Path $name "SETUP.md")
    
    $hasSpec = Test-Path $specPath
    $hasSetup = Test-Path $setupPath
    
    $status = "[MISSING] No SPEC"
    if ($hasSpec) {
        $documentedCount++
        if ($hasSetup) {
            $status = "[OK] Spec + Setup"
        } else {
            $status = "[WARN] Spec Only"
        }
    }
    
    # Format relative path for output
    $relPaths = @()
    foreach ($p in $feature.ScriptPaths) {
        $relPaths += $p.SubString($projectRoot.Length + 1)
    }
    
    $results += [PSCustomObject]@{
        Feature      = $name
        Status       = $status
        SourceFolders = ($relPaths -join ", ")
    }
}

# Print the results
Write-Host "`n=================== Swordidle Feature Documentation Audit ===================" -ForegroundColor Cyan
$results | Sort-Object Feature | Format-Table -AutoSize
Write-Host "=============================================================================" -ForegroundColor Cyan

$coveragePercent = [Math]::Round(($documentedCount / $totalFeatures) * 100)
$color = "Yellow"
if ($coveragePercent -ge 80) {
    $color = "Green"
}

Write-Host "Total Features identified: $totalFeatures"
Write-Host "Documented Features Spec: $documentedCount"
Write-Host "Documentation Coverage: $coveragePercent%`n" -ForegroundColor $color

if ($documentedCount -lt $totalFeatures) {
    Write-Host "WARNING: Some features are missing specifications! Please create them in Assets/Docs/Features/<FeatureName>/FEATURE_SPEC.md." -ForegroundColor Yellow
} else {
    Write-Host "SUCCESS: All identified features have matching specifications!" -ForegroundColor Green
}
