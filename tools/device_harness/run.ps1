# 원본(harness.exe)과 C#(NanoImpact.Verify device)에 같은 시나리오를 돌리고 출력을 비교한다.
#   powershell -File run.ps1 scripts\startup.txt [-IniSource <원본 INI 폴더>]
# 각 실행 전에 out\ini에 원본 INI 복사본을 새로 만든다 (시나리오가 INI를 쓸 수 있으므로).
# 결과: out\<이름>.original.txt, out\<이름>.csharp.txt, 첫 차이 몇 줄
param(
    [Parameter(Mandatory = $true)][string]$Script,
    [string]$IniSource = 'C:\Users\Administrator\Downloads\NanoImpact_20230510\NanoImpact\build\Out\Debug'
)
$ErrorActionPreference = 'Stop'
$out = Join-Path $PSScriptRoot 'out'
$ini = Join-Path $out 'ini'
$verify = Join-Path $PSScriptRoot '..\NanoImpact.Verify\bin\Debug\NanoImpact.Verify.exe'
$scriptPath = (Resolve-Path (Join-Path $PSScriptRoot $Script)).Path
$name = [IO.Path]::GetFileNameWithoutExtension($scriptPath)

function Reset-Ini {
    New-Item -ItemType Directory -Force $ini | Out-Null
    foreach ($f in 'USBINTEGRATEDDATA.INI', 'CLIENTPUBLICDATA.INI', 'SROTCALIBRATIONDATA.INI') { Copy-Item (Join-Path $IniSource $f) $ini -Force }
}

Push-Location $out
try {
    Reset-Ini
    $orig = & .\harness.exe $scriptPath
    Reset-Ini
    $cs = & $verify device $scriptPath
}
finally { Pop-Location }

$utf8 = New-Object Text.UTF8Encoding($false)
[IO.File]::WriteAllLines((Join-Path $out "$name.original.txt"), [string[]]$orig, $utf8)
[IO.File]::WriteAllLines((Join-Path $out "$name.csharp.txt"), [string[]]$cs, $utf8)

# 호출(CALL) 단위로 나눠 비교한다: 한 호출의 출력이 다르거나 C#에서 예외가 나도 다음 호출부터 다시 맞춘다
function Split-Calls([string[]]$lines) {
    $blocks = New-Object System.Collections.Generic.List[object]
    $cur = $null
    foreach ($x in $lines) {
        if ($x -like 'CALL *') { $cur = [pscustomobject]@{ Call = $x; Lines = New-Object System.Collections.Generic.List[string] }; $blocks.Add($cur) }
        elseif ($cur) { $cur.Lines.Add($x) }
    }
    $blocks
}
$bo = @(Split-Calls $orig)
$bc = @(Split-Calls $cs)
$same = 0; $diff = 0; $exc = 0; $shown = 0
for ($i = 0; $i -lt [Math]::Min($bo.Count, $bc.Count); $i++) {
    if ($bo[$i].Call -ne $bc[$i].Call) { "호출 순서가 어긋남: {0} / {1}" -f $bo[$i].Call, $bc[$i].Call; break }
    $a = $bo[$i].Lines -join "`n"; $b = $bc[$i].Lines -join "`n"
    if ($a -eq $b) { $same++; continue }
    if ($b -match '(^|\n)EXCEPTION ') { $exc++; $kind = 'C# 예외' } else { $diff++; $kind = '다름' }
    if ($shown -lt 8) {
        $shown++
        "[{0}] #{1} {2}" -f $kind, ($i + 1), $bo[$i].Call
        $la = @($bo[$i].Lines); $lb = @($bc[$i].Lines)
        for ($k = 0; $k -lt [Math]::Max($la.Count, $lb.Count); $k++) {
            $x = if ($k -lt $la.Count) { $la[$k] } else { '<없음>' }; $y = if ($k -lt $lb.Count) { $lb[$k] } else { '<없음>' }
            if ($x -ne $y) { "    원본: {0}`n    C#  : {1}" -f $x, $y; break }
        }
    }
}
"{0}: 호출 원본 {1} / C# {2} — 같음 {3}, 다름 {4}, C# 예외 {5}" -f $name, $bo.Count, $bc.Count, $same, $diff, $exc
$oor = Join-Path $out "$name.out_of_range.txt"
if ((Test-Path $oor) -and (Get-Item $oor).Length -gt 3) { "원본 코드의 배열 범위 밖 접근:"; Get-Content $oor -Encoding UTF8 | ForEach-Object { "  $_" } }