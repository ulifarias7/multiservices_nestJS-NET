# ===============================
# Docker Volumes Backup Script
# ===============================

$BackupRoot = "C:\Users\ufarias.ENCODELABS\Desktop\multiservices_nestJS-NET\docker-volumes-backup"

$Volumes = @(
    @{ Name = "multiservices_nestjs-net_pgdata";        Folder = "pgdata" },
    @{ Name = "multiservices_nestjs-net_keycloak_data"; Folder = "keycloak_data" },
    @{ Name = "multiservices_nestjs-net_minio-data";    Folder = "minio-data" },
    @{ Name = "multiservices_nestjs-net_rabbitmq-data"; Folder = "rabbitmq-data" }
)

foreach ($v in $Volumes) {

    $TargetPath = Join-Path $BackupRoot $v.Folder

    Write-Host "Backing up volume: $($v.Name)"

    if (Test-Path $TargetPath) {
        Remove-Item -Recurse -Force $TargetPath
    }

    New-Item -ItemType Directory -Path $TargetPath | Out-Null

    docker run --rm `
        -v "$($v.Name):/volume" `
        -v "$($TargetPath):/backup" `
        alpine `
        sh -c "cp -a /volume/. /backup/"
}

Write-Host "Backup completed"
