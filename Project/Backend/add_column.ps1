$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$dbPath = Join-Path $scriptDir "calendify.db"

Add-Type -Path (Join-Path $scriptDir "bin\Debug\net8.0\Microsoft.Data.Sqlite.dll")

try {
    $connectionString = "Data Source=$dbPath"
    $connection = New-Object Microsoft.Data.Sqlite.SqliteConnection($connectionString)
    $connection.Open()

    $command = $connection.CreateCommand()
    $command.CommandText = "ALTER TABLE Events ADD COLUMN MaxAttendees INTEGER NULL"

    $result = $command.ExecuteNonQuery()

    Write-Host "SUCCESS: MaxAttendees column added to Events table"

    $connection.Close()
}
catch {
    if ($_.Exception.Message -match "duplicate column name") {
        Write-Host "INFO: MaxAttendees column already exists"
    }
    else {
        Write-Host "ERROR: $($_.Exception.Message)"
    }
}

