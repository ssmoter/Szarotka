# Skrypt przywracania serwera .NET po aktualizacji bazy danych
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force
$ErrorActionPreference = "Stop"

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "Uruchamiam serwer .NET z nowa baza danych..." -ForegroundColor Yellow
Write-Host "==========================================================" -ForegroundColor Cyan

# Przywracamy Twoj obraz .NET i podpinamy zaktualizowana baze danych
gcloud run deploy serwer-aot `
  --image="gcr.io/szarotka-505112/serwer-aot:latest" `
  --region="europe-west1" `
  --add-volume="name=dysk-sqlite,type=cloud-storage,bucket=szarotka-db" `
  --add-volume-mount="volume=dysk-sqlite,mount-path=/data" `
  --set-env-vars="PERSISTENT_DB_PATH=/data" `
  --set-env-vars="EmailConfiguration__From=Szarotka,EmailConfiguration__SmtpServer=smtp.ethereal.email,EmailConfiguration__Port=587,EmailConfiguration__UserName=marianna.will@ethereal.email,EmailConfiguration__Password=X4CnjUgfKwHYQKbJny,EmailConfiguration__ExpireDateMinutes=15" `
  --execution-environment="gen2" `
  --max-instances=1 `
  --allow-unauthenticated `
  --project="szarotka-505112"

Write-Host ""
Write-Host "SUCCESS: Serwer .NET wstal i pobral nowa baze danych!" -ForegroundColor Green

[Console]::Beep(523, 500)
