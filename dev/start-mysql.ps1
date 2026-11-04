# Local MySQL for development - no installer, no configuration needed.
# First run: downloads a portable MySQL 8, creates the database and user the
# backend expects (port 3308, erp_mmtech_20032025, Admin / NaZe3X5V).
# Later runs: just starts it if it is not already running.
# Everything lives in %LOCALAPPDATA%\SmarterSRP\mysql.

$ErrorActionPreference = 'Stop'

$Version  = '8.0.46'
$Port     = 3308
$Database = 'erp_mmtech_20032025'
$DbUser   = 'Admin'
$DbPass   = 'NaZe3X5V'

$Root    = Join-Path $env:LOCALAPPDATA 'SmarterSRP\mysql'
$BaseDir = Join-Path $Root "mysql-$Version-winx64"
$DataDir = Join-Path $Root 'data'
$Ini     = Join-Path $Root 'my.ini'
$Bin     = Join-Path $BaseDir 'bin'
$Mysqld  = Join-Path $Bin 'mysqld.exe'
$Mysql   = Join-Path $Bin 'mysql.exe'

function Write-Step($msg) { Write-Host "==> $msg" -ForegroundColor Cyan }

function Test-MySqlUp {
    try {
        $client = New-Object System.Net.Sockets.TcpClient
        $client.Connect('127.0.0.1', $Port)
        $client.Close()
        return $true
    } catch { return $false }
}

function Wait-MySqlUp {
    for ($i = 0; $i -lt 60; $i++) {
        if (Test-MySqlUp) { return }
        Start-Sleep -Seconds 1
    }
    throw "MySQL did not start. Check the log: $DataDir\*.err"
}

New-Item -ItemType Directory -Force -Path $Root | Out-Null

# 1) Download and extract MySQL (first run only)
if (-not (Test-Path $Mysqld)) {
    $zip = Join-Path $Root "mysql-$Version-winx64.zip"
    if (-not (Test-Path $zip)) {
        Write-Step "Downloading MySQL $Version (about 250 MB, first run only)..."
        $ProgressPreference = 'SilentlyContinue'
        Invoke-WebRequest -Uri "https://cdn.mysql.com/Downloads/MySQL-8.0/mysql-$Version-winx64.zip" -OutFile $zip
    }
    Write-Step 'Extracting...'
    Expand-Archive -Path $zip -DestinationPath $Root -Force
    Remove-Item $zip
}

# 2) Config file
$baseFwd = $BaseDir -replace '\\', '/'
$dataFwd = $DataDir -replace '\\', '/'
@"
[mysqld]
basedir=$baseFwd
datadir=$dataFwd
port=$Port
mysqlx=OFF
lower_case_table_names=1
character-set-server=utf8mb4
collation-server=utf8mb4_unicode_ci
"@ | Set-Content -Path $Ini -Encoding ASCII

# 3) Initialise the data folder (first run only)
$firstRun = -not (Test-Path $DataDir)
if ($firstRun) {
    Write-Step 'Initialising database files...'
    & $Mysqld "--defaults-file=$Ini" --initialize-insecure --console
    if ($LASTEXITCODE -ne 0) {
        throw 'MySQL initialisation failed. Install the Microsoft Visual C++ Redistributable (x64) and try again.'
    }
}

# 4) Start the server in the background
if (Test-MySqlUp) {
    Write-Step "MySQL is already running on port $Port."
} else {
    Write-Step "Starting MySQL on port $Port..."
    Start-Process -FilePath $Mysqld -ArgumentList "--defaults-file=`"$Ini`"" -WindowStyle Hidden
    Wait-MySqlUp
}

# 5) Create the database and user the backend expects (safe to repeat)
$sql = @"
CREATE DATABASE IF NOT EXISTS ``$Database`` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER IF NOT EXISTS '$DbUser'@'%' IDENTIFIED BY '$DbPass';
CREATE USER IF NOT EXISTS '$DbUser'@'localhost' IDENTIFIED BY '$DbPass';
GRANT ALL PRIVILEGES ON *.* TO '$DbUser'@'%';
GRANT ALL PRIVILEGES ON *.* TO '$DbUser'@'localhost';
FLUSH PRIVILEGES;
"@
$sql | & $Mysql -uroot -h 127.0.0.1 -P $Port
if ($LASTEXITCODE -ne 0) { throw 'Could not create the database or user.' }

Write-Host ''
Write-Host "MySQL is ready on port $Port." -ForegroundColor Green
Write-Host "Database: $Database   User: $DbUser   Password: $DbPass"
Write-Host 'Now run Fly.WebApi and Leapay.Client from Visual Studio.'
Write-Host "To stop MySQL later, run dev\stop-mysql.cmd"
