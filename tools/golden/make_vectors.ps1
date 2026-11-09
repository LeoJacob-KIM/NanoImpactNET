# golden.cpp / NanoImpact.Verify 공용 시험 입력(vectors.txt) 생성. 시드 고정이라 다시 만들어도 같다.
param([string]$Out = (Join-Path $PSScriptRoot 'vectors.txt'))

$rng = New-Object System.Random 12345
$lines = New-Object System.Collections.Generic.List[string]

function Hex([string]$s) { ([Text.Encoding]::ASCII.GetBytes($s) | ForEach-Object { $_.ToString('X2') }) -join ' ' }
function Cs([string]$s) { $sum = 0; foreach ($c in $s.ToCharArray()) { $sum += [int]$c }; $sum % 100 }
function Msg([string]$body) { "${body}?$(Cs $body);" }   # body = 명령+X[,Y][,D] ("$body?"는 변수 이름 body?로 읽힘)

# --- 단일 WORD 명령 ---
$lines.Add('# CMD')
foreach ($p in @(@('D0', 74565), @('EA03', 1000), @('10', -1), @('FFFF', 2147483647), @('1', -123456), @('0', 0), @('EB05', 65535), @('C0', 65536))) {
    $lines.Add("CMD $($p[0]) $($p[1])")
}
for ($i = 0; $i -lt 200; $i++) {
    $c = $rng.Next(0, 65536).ToString('X'); $v = $rng.Next([int]::MinValue, [int]::MaxValue)
    $lines.Add("CMD $c $v")
}

# --- AVR 명령 + 정수 ---
$lines.Add('# AVRV')
foreach ($p in @(@('D3', 'HV', 30000), @('D3', 'FV', -5), @('D2', 'MM1,', 1234), @('D3', 'X', 0), @('D3', 'SP2,', -2147483647))) {
    $lines.Add("AVRV $($p[0]) $($p[1]) $($p[2])")
}
$alpha = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,.-'
for ($i = 0; $i -lt 100; $i++) {
    $n = $rng.Next(1, 12); $cmd = -join (1..$n | ForEach-Object { $alpha[$rng.Next($alpha.Length)] })
    $head = @('D3', 'D2')[$rng.Next(2)]
    $lines.Add("AVRV $head $cmd $($rng.Next(-100000, 100000))")
}

# --- AVR 명령 문자열(';') ---
$lines.Add('# AVRS')
foreach ($c in @('PD;', 'RM0;', 'RP2;', 'UL2,1;', 'LS1;', 'VL1,0;', 'AB;CD')) { $lines.Add("AVRS D3 $c") }
for ($i = 0; $i -lt 100; $i++) {
    $n = $rng.Next(1, 20); $cmd = -join (1..$n | ForEach-Object { $alpha[$rng.Next($alpha.Length)] })
    $lines.Add("AVRS $(@('D3','D2')[$rng.Next(2)]) $cmd;")
}

# --- 수신 파서 ---
$lines.Add('# RX')
$cases = @(
    (Msg 'PD1'),
    (Msg 'RP0,12345,-3.5'),
    'VS7;',
    "EM120?$(((Cs 'EM120') + 1) % 100);",           # checksum 틀림
    (Msg 'QQ5'),                                     # 목록에 없는 명령
    "TS0ABC,5?$(Cs 'TS0,ABC5');",                   # TS: X가 '0'이면 대문자는 Y로
    ('RP' + ('1' * 70) + '?0;'),                     # X 넘침
    ('RP1,' + ('2' * 63) + '?1;'),                   # Y 넘침(63)
    ('RP1,2,' + ('3' * 64) + '?5;'),                 # D 넘침
    ('EV1?' + ('9' * 64) + ';'),                     # S 넘침
    ((Msg 'EV35') + (Msg 'VS4') + (Msg 'RP1,200') + (Msg 'RP2,-100') + (Msg 'LT0,1,1') + (Msg 'EM15')),  # 44바이트 넘는 묶음
    'abc' + [char]0x7F + (Msg 'AR1')
)
foreach ($c in $cases) { $lines.Add("RX $(Hex $c)") }
# 두 번에 나뉘어 온 메시지
$m = Msg 'RP1,200'
$lines.Add("RX $(Hex $m.Substring(0, 5))")
$lines.Add("RX $(Hex $m.Substring(5))")
# 아무 바이트나 섞은 무작위 묶음
$rxAlpha = 'ABCDEFGHIJKLMNOPRSTUVZ0123456789.-,?;;;'
for ($i = 0; $i -lt 300; $i++) {
    $n = $rng.Next(1, 120)
    $bytes = for ($k = 0; $k -lt $n; $k++) {
        if ($rng.Next(10) -eq 0) { $rng.Next(0, 256) } else { [int]$rxAlpha[$rng.Next($rxAlpha.Length)] }
    }
    $lines.Add('RX ' + (($bytes | ForEach-Object { ([int]$_).ToString('X2') }) -join ' '))
}

[IO.File]::WriteAllLines($Out, $lines)
"vectors: $($lines.Count) lines -> $Out"
