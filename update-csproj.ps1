$maxRetries = 10
$retryDelay = 200  # milliseconds
$success = $false

for ($i = 0; $i -lt $maxRetries; $i++) {
    try {
        $content = Get-Content -Path 'PostfixTemplates\PostfixTemplates.csproj' -Raw
        $content = $content -replace '(\<PackageReference Include="Microsoft\.VSSDK\.BuildTools" Version="17\.14\.2120" /\>)', "`$1`r`n    <PackageReference Include=`"Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion`" Version=`"17.0.0`" ExcludeAssets=`"Runtime`" />`r`n    <PackageReference Include=`"Microsoft.CodeAnalysis.CSharp.Workspaces`" Version=`"4.0.0`" ExcludeAssets=`"Runtime`" />"
        $content | Set-Content -Path 'PostfixTemplates\PostfixTemplates.csproj' -NoNewline -ErrorAction Stop
        $success = $true
        Write-Host "Successfully updated PostfixTemplates.csproj"
        break
    }
    catch {
        if ($i -eq ($maxRetries - 1)) {
            throw
        }
        Start-Sleep -Milliseconds $retryDelay
    }
}

if (-not $success) {
    throw "Failed to update PostfixTemplates.csproj after $maxRetries retries"
}
