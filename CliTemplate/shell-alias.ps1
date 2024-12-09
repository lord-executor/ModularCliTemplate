# Source this script in your $PROFILE

function Run-CliTemplate {
    $hostColor = "Magenta"
    $cliPath = ""
    $gitRoot = $(git rev-parse --show-toplevel 2> $null)
    if ($LASTEXITCODE -ne 0)
    {
        $cliPath = $PSScriptRoot
        Write-Host "Not in a git repository. Using CLI from $cliPath" -ForegroundColor $hostColor
    }
    else
    {
        $cliPath = Join-Path $gitRoot "PATH/TO/CLI"
        Write-Host "Found CLI under $cliPath" -ForegroundColor $hostColor
    }

    $forceBuild = $false
    if ($Args.Contains("--build")) {
        Write-Host "Force build" -ForegroundColor $hostColor
        # When the filtered $Args only contains a single item after filtering, then PowerShell just
        # assigns the single value to the result instead of an _array_ of that single value. Specifying
        # [array] as the variable type avoids that but does require a temporary variable as it doesn't
        # work when directly re-assigning $Args
        [array] $filtered = $Args | Where-Object { $_ -ne "--build" }
        $Args = $filtered
        $forceBuild = $true
    }

    $tag = git -C $cliPath describe --tags --dirty --always
    $buildVersion = Get-Content "$cliPath/build.version" -ErrorAction Ignore
    if ($buildVersion -ne $null) {
        Write-Host "Latest build version is $buildVersion" -ForegroundColor $hostColor
    }

    $doBuild = $forceBuild
    if ($buildVersion -ne $tag) {
        $doBuild = $true
    }

    if ($doBuild) {
        Write-Host "Building binary with version $tag" -ForegroundColor $hostColor
        dotnet publish "$cliPath/CliTemplate.csproj" | Write-Host -ForegroundColor $hostColor
        Write-Output $tag > "$cliPath/build.version"
    }

    # https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_automatic_variables?view=powershell-7.4#input
    # $input is an _enumerator_ and if we want to do something useful with it, then we have to first put it all back together into an
    # array and then text. Since the CLI binary is _not_ a PowerShell cmd-let, we want to treat the input to the program as "text" (or "bytes"?)
    $inputObjects = @()
    $input | % { $inputObjects += $_ }
    $stdin = $inputObjects -join "`n"

    if ($stdin -eq "") {
        & "$cliPath\bin\Release\net9.0\publish\CliTemplate.exe" @Args
    } else {
        $stdin | & "$cliPath\bin\Release\net9.0\publish\CliTemplate.exe" @Args
    }
}

# Set-Alias -Name "clitemplate" -Value "Run-CliTemplate"
