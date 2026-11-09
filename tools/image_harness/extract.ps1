# Copy original source lines byte-for-byte into an include file. Keep ASCII.
#   extract.ps1 <source> <out> <startLine> [<endLine>]
# Without endLine: from startLine to the brace that closes the function starting there.
# Bytes are copied raw. If the source starts with a UTF-8 BOM, the include gets one too, so cl reads it as UTF-8
# like the original build did (NanoImpact2020cfeDlg.cpp has a BOM and mixes CP949 and UTF-8 comments).
param([string]$Source, [string]$Out, [int]$Start, [int]$End = 0)
$bytes = [IO.File]::ReadAllBytes($Source)
$bom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
# line N starts at $starts[N-1]
$starts = New-Object System.Collections.Generic.List[int]
$starts.Add($(if ($bom) { 3 } else { 0 }))
for ($i = 0; $i -lt $bytes.Length; $i++) { if ($bytes[$i] -eq 10) { $starts.Add($i + 1) } }
$starts.Add($bytes.Length)
if ($End -le 0) {
    # braces are ASCII and never appear as CP949 or UTF-8 trail bytes, so counting raw bytes is safe
    $depth = 0; $seen = $false
    for ($n = $Start; $n -lt $starts.Count; $n++) {
        for ($i = $starts[$n - 1]; $i -lt $starts[$n]; $i++) {
            if ($bytes[$i] -eq 123) { $depth++; $seen = $true }
            elseif ($bytes[$i] -eq 125) { $depth-- }
        }
        if ($seen -and $depth -eq 0) { $End = $n; break }
    }
}
$from = $starts[$Start - 1]
$to = $starts[$End]
$header = [Text.Encoding]::ASCII.GetBytes("// Copied unmodified from " + [IO.Path]::GetFileName($Source) + " lines $Start-$End by extract.ps1`r`n")
$fs = [IO.File]::Create($Out)
try {
    if ($bom) { $fs.Write([byte[]](0xEF, 0xBB, 0xBF), 0, 3) }
    $fs.Write($header, 0, $header.Length)
    $fs.Write($bytes, $from, $to - $from)
    if ($bytes[$to - 1] -ne 10) { $fs.Write([byte[]](13, 10), 0, 2) }
}
finally { $fs.Close() }
"extract: $([IO.Path]::GetFileName($Source)) $Start-$End -> $Out"
