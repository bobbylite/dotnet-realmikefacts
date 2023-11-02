dotnet restore

$output = dotnet list package --vulnerable
Write-Output $output

if ($output -match "Project ``bobbylite.realmikefacts.web`` has the following vulnerable packages") {
    exit 1
}
else {
    Write-Output "Project ``bobbylite.realmikefacts.web`` has no vulnerable packages"
}