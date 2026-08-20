# Skrypt wylaczania serwera .NET i przygotowania do aktualizacji bazy danych
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force
$ErrorActionPreference = "Stop"

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "Zamykam serwer .NET i zamrazam usluge Cloud Run..." -ForegroundColor Yellow
Write-Host "==========================================================" -ForegroundColor Cyan

# Podmieniamy kontener na puste Hello World, co wymusi bezpieczne zamkniecie serwera .NET
gcloud run deploy serwer-aot `
  --image="us-docker.pkg.dev/cloudrun/container/hello:latest" `
  --region="europe-west1" `
  --clear-volumes `
  --clear-env-vars `
  --max-instances=0 `
  --project="szarotka-505112"

Write-Host ""
Write-Host "SUCCESS: Serwer .NET zostal bezpiecznie wylaczony!" -ForegroundColor Green
Write-Host "MOŻESZ TERAZ PODMIENIĆ PLIK .DB W CLOUD STORAGE." -ForegroundColor Yellow

[Console]::Beep(440, 500)
