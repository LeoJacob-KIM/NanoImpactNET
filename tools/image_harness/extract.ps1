# Copy original source lines byte-for-byte (CP949) into an include file. Keep ASCII.
#   extract.ps1 <source> <out> <startLine> [<endLine>]
# Without endLine: from startLine to the brace that closes the function starting there.
param([string]$Source, [string]$Out, [int]$Start, [int]$End = 0)
$enc = [Text.Encoding]::GetEncoding(949)
$lines = [IO.File]::ReadAllText($Source, $enc).Replace("`r`n", "`n").Split("`n")
if ($End -le 0) {
    $depth = 0; $seen = $false
    for ($i = $Start - 1; $i -lt $lines.Count; $i++) {
        foreach ($c in $lines[$i].ToCharArray()) {
            if ($c -eq '{') { $depth++; $seen = $true }
            elseif ($c -eq '}') { $depth-- }
        }
        if ($seen -and $depth -eq 0) { $End = $i + 1; break }
    }
}
$body = $lines[($Start - 1)..($End - 1)] -join "`r`n"
$header = "// Copied unmodified from " + [IO.Path]::GetFileName($Source) + " lines $Start-$End by extract.ps1`r`n"
[IO.File]::WriteAllText($Out, $header + $body + "`r`n", $enc)
"extract: $([IO.Path]::GetFileName($Source)) $Start-$End -> $Out"
