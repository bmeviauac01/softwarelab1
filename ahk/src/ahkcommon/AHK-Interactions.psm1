function Write-AHKResult {
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [ValidateNotNullOrEmpty()]
        [string] $ValidationCode,

        [Parameter(Mandatory = $true, Position = 1)]
        [ValidateNotNullOrEmpty()]
        [string] $ExerciseName,

        [Parameter(Mandatory = $true, ParameterSetName = "WithOutcome")]
        [ValidateNotNullOrEmpty()]
        [ValidateSet( "Passed", "Failed", "Inconclusive")]
        [string] $Outcome,

        [Parameter(Mandatory = $true, ParameterSetName = "WithPoints")]
        [ValidateNotNullOrEmpty()]
        [int] $Points,

        [Parameter(Mandatory = $false)]
        [string[]] $Comment = $null
    )

    $OutputLine = "###ahk#" + $ValidationCode + "#testresult#" + $ExerciseName.Replace("#", "-") + "#"

    if ($PSCmdlet.ParameterSetName -eq "WithOutcome") {
        $OutputLine += $outcome.ToLower()
    }
    elseif ($PSCmdlet.ParameterSetName -eq "WithPoints") {
        $OutputLine += $Points.ToString()
    }

    if ($Comment) {
        $OutputLine += "#" + (($Comment.Replace("`r`n", "\`n").Replace("`n", "\`n").Replace("`r", "\`n")) -join "\`n")
    }

    Write-Output $OutputLine
}