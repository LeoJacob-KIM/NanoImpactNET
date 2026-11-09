# Scan Bridge 수신 처리기 시나리오: 시작 시나리오 뒤에 명령 44종 × 값 형태 여러 개를 하나씩 받아 AfxScanBridgeDataRead()로 처리.
#   powershell -File make_rx.ps1   → scripts\rx.txt
# 메시지 = 명령 + X[,Y][,D] + "?" + checksum + ";" (checksum = 문자 합 % 100). 한 묶음에 메시지 하나(원본 44바이트 버그와 섞지 않음).
$cmds = 'PD','AR','EV','VL','VS','HV','FV','BV','AV','EM','AE','AT','RF','RG','SN','RN','SA','RA','SP','RP','SM','RM','MH','MC','MP','MA','MM','TS','MG','AP','PR','LT','RB','ZT','SB','TC','LS','TB','BT','EC','VR','TM','UL','BF'
$bodies = '', '1', '0,1', '1,2,3', '2,-1.5', '3,100,25', '12345', '0,0,0', '5,12.75', '4,40000', '4,-250,1'
function Cs([string]$s) { $sum = 0; foreach ($c in $s.ToCharArray()) { $sum += [int]$c }; $sum % 100 }
function Hex([string]$s) { ([Text.Encoding]::ASCII.GetBytes($s) | ForEach-Object { $_.ToString('X2') }) -join ' ' }
$lines = New-Object System.Collections.Generic.List[string]
$lines.Add('# make_rx.ps1로 만든 시나리오 (직접 고치지 말 것)')
Get-Content (Join-Path $PSScriptRoot 'scripts\startup.txt') -Encoding UTF8 | Where-Object { $_ -notmatch '^#' } | ForEach-Object { $lines.Add($_) }
foreach ($c in $cmds) {
    foreach ($b in $bodies) {
        $m = "$c$b"
        $lines.Add("RX 2 " + (Hex "${m}?$(Cs $m);"))
        $lines.Add('CALL AfxScanBridgeDataRead()')
    }
    # checksum 없는 메시지, 틀린 checksum
    $lines.Add("RX 2 " + (Hex "${c}7;"))
    $lines.Add('CALL AfxScanBridgeDataRead()')
    $lines.Add("RX 2 " + (Hex "${c}7?$(((Cs "${c}7") + 1) % 100);"))
    $lines.Add('CALL AfxScanBridgeDataRead()')
}
$out = Join-Path $PSScriptRoot 'scripts\rx.txt'
[IO.File]::WriteAllLines($out, $lines, (New-Object Text.UTF8Encoding($false)))
"$out : $($lines.Count) 줄"