# 호출 표(out\src\dispatch.inc)의 모든 함수를 시작 시나리오 뒤에 한 번씩 부르는 전수 시나리오를 만든다.
#   powershell -File make_sweep.ps1 [-Value 1] [-Name sweep]   → scripts\<Name>.txt
# 인자 값: 정수 = $Value, 실수 = $Value + 0.5, 문자열 = "$Value;" (AVR 명령은 ;로 끝나야 원본이 죽지 않는다). 포트를 닫거나 끝내는 함수는 맨 뒤.
param([int]$Value = 1, [string]$Name = 'sweep')
$inc = Join-Path $PSScriptRoot 'out\src\dispatch.inc'
$startup = Get-Content (Join-Path $PSScriptRoot 'scripts\startup.txt') -Encoding UTF8
$keys = Select-String -Path $inc -Pattern '^DISPATCH\("([^"]+)"' | ForEach-Object { $_.Matches[0].Groups[1].Value }

# 시작 시나리오에서 이미 부른 것, 상태를 무너뜨려 뒤 호출을 무의미하게 만드는 것은 뺀다
$skip = @('AfxUSBFT232HLoadDLL()', 'AfxCreateIntegratedCommand()')
# 원본 OnClose 순서: 스레드 정지 표시 → ... (거꾸로 부르면 원본이 해제된 스레드 인자를 건드려 죽는다)
$last = @('AfxIntegratedSetThreadAllStopFlag()', 'AfxFT232HDeviceUse(int)', 'AfxSEMSystemThreadClose()', 'AfxReleaseIntegratedCommand()',
          'AfxReleasePublicParameters()', 'AfxFT232HPortClose(int)', 'AfxUSBFT232HFreeDLL()')

function Args([string]$key) {
    $types = $key.Substring($key.IndexOf('(') + 1).TrimEnd(')') -split ',' | Where-Object { $_ }
    ($types | ForEach-Object {
        switch ($_) { 'double' { "$Value.5" } 'char*' { "$Value;" } 'HWND' { '1' } default { "$Value" } }
    }) -join ' '
}

$lines = New-Object System.Collections.Generic.List[string]
$lines.Add("# make_sweep.ps1 -Value $Value 로 만든 전수 시나리오 (직접 고치지 말 것)")
$startup | Where-Object { $_ -notmatch '^#' } | ForEach-Object { $lines.Add($_) }
foreach ($k in $keys) { if ($skip -notcontains $k -and $last -notcontains $k) { $lines.Add(("CALL $k " + (Args $k)).TrimEnd()) } }
foreach ($k in $last) { if ($keys -contains $k) { $lines.Add(("CALL $k " + (Args $k)).TrimEnd()) } }
$out = Join-Path $PSScriptRoot "scripts\$Name.txt"
[IO.File]::WriteAllLines($out, $lines, (New-Object Text.UTF8Encoding($true)))
"$out : $($lines.Count) 줄"
