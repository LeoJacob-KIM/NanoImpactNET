# Run the original image harness and NanoImpact.Verify image on the same script and compare (block = "> cmd" / "DATA").
#   powershell -File run.ps1 scripts\live.txt
param([Parameter(Mandatory = $true)][string]$Script)
$out = Join-Path $PSScriptRoot 'out'
$verify = Join-Path $PSScriptRoot '..\NanoImpact.Verify\bin\Debug\NanoImpact.Verify.exe'
$scriptPath = (Resolve-Path (Join-Path $PSScriptRoot $Script)).Path
$name = [IO.Path]::GetFileNameWithoutExtension($scriptPath)
Push-Location (Join-Path $PSScriptRoot 'scripts')
try {
    $orig = & (Join-Path $out 'harness.exe') $scriptPath
    $cs = & $verify image $scriptPath (Join-Path $PSScriptRoot 'scan_fields.inc')
} finally { Pop-Location }
$utf8 = New-Object Text.UTF8Encoding($false)
[IO.File]::WriteAllLines((Join-Path $out "$name.original.txt"), [string[]]$orig, $utf8)
[IO.File]::WriteAllLines((Join-Path $out "$name.csharp.txt"), [string[]]$cs, $utf8)
function Split-Blocks([string[]]$lines) {
    $b = New-Object System.Collections.Generic.List[object]; $cur = $null
    foreach ($x in $lines) {
        if ($x -like '> *' -or $x -like 'DATA *' -or $cur -eq $null) { $cur = [pscustomobject]@{ Head = $x; Lines = New-Object System.Collections.Generic.List[string] }; $b.Add($cur) }
        else { $cur.Lines.Add($x) }
    }
    $b
}
$bo = @(Split-Blocks $orig); $bc = @(Split-Blocks $cs)
$same = 0; $diff = 0; $shown = 0
for ($i = 0; $i -lt [Math]::Min($bo.Count, $bc.Count); $i++) {
    if ($bo[$i].Head -ne $bc[$i].Head) { "block order differs at #{0}: {1} / {2}" -f ($i + 1), $bo[$i].Head, $bc[$i].Head; break }
    if (($bo[$i].Lines -join "`n") -eq ($bc[$i].Lines -join "`n")) { $same++; continue }
    $diff++
    if ($shown -lt 6) {
        $shown++
        "[diff] #{0} {1}" -f ($i + 1), $bo[$i].Head
        $la = @($bo[$i].Lines); $lb = @($bc[$i].Lines)
        for ($k = 0; $k -lt [Math]::Max($la.Count, $lb.Count); $k++) {
            $x = if ($k -lt $la.Count) { $la[$k] } else { '<none>' }; $y = if ($k -lt $lb.Count) { $lb[$k] } else { '<none>' }
            if ($x -ne $y) { "    orig: {0}`n    C#  : {1}" -f $x, $y; break }
        }
    }
}
"{0}: blocks orig {1} / C# {2}, same {3}, diff {4}" -f $name, $bo.Count, $bc.Count, $same, $diff