$files = Get-ChildItem -Path "TrainTicket.WinForms\Forms" -Filter "*.cs" -Recurse
foreach ($f in $files) {
    $c = Get-Content $f.FullName -Raw
    
    # Clears old grid styles
    $c = $c -replace '(?m)^\s*_grid\.ThemeStyle\..*?;[\r\n]*', ''
    $c = $c -replace '(?m)^\s*_grid\.GridColor.*?;[\r\n]*', ''

    if ($c -match 'public void ApplyTheme\(\)') {
        $c = $c -replace '(?s)(public void ApplyTheme\(\).*?)(\s+\})', "$1
            if (_grid != null) UiTheme.StyleGrid(_grid);$2"
    } 

    Set-Content $f.FullName -Value $c -NoNewline
}
