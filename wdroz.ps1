# Skrypt automatycznego wdrożenia serwera .NET Native AOT do Google Cloud
$ErrorActionPreference = "Stop"

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "1/2: Rozpoczynam kompilację Native AOT w Google Cloud..." -ForegroundColor Yellow
Write-Host "==========================================================" -ForegroundColor Cyan

# Krok 1: Budowanie obrazu w chmurze
gcloud builds submit --tag gcr.io/szarotka-505112/serwer-aot:latest --project=szarotka-505112 .

Write-Host ""
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "2/2: Publikuję skompilowany kontener na Cloud Run..." -ForegroundColor Yellow
Write-Host "==========================================================" -ForegroundColor Cyan

# Krok 2: Wdrożenie na Cloud Run i podpięcie bazy SQLite
gcloud run deploy serwer-aot `
  --image="gcr.io/szarotka-505112/serwer-aot:latest" `
  --region="europe-west1" `
  --add-volume="name=dysk-sqlite,type=cloud-storage,bucket=szarotka-db" `
  --add-volume-mount="volume=dysk-sqlite,mount-path=/data" `
  --set-env-vars="PERSISTENT_DB_PATH=/data" `
  --execution-environment="gen2" `
  --max-instances=1 `
  --allow-unauthenticated `
  --project="szarotka-505112"

Write-Host ""
Write-Host "✔ SUKCES: Twój serwer Native AOT został pomyślnie zaktualizowany!" -ForegroundColor Green
[System.Media.SystemSounds]::Asterisk.Play() # Dźwięk powiadomienia systemowego Windows
