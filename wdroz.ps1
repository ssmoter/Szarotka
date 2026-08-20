# Skrypt automatycznego wdrozenia serwera .NET Native AOT do Google Cloud
# Samoczynne odblokowanie uprawnien dla obecnego uzytkownika systemu Windows
Write-Host "⚙ Konfiguruje uprawnienia terminala..." -ForegroundColor Gray
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force

$ErrorActionPreference = "Stop"

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "1/2: Rozpoczynam kompilacje Native AOT w Google Cloud..." -ForegroundColor Yellow
Write-Host "==========================================================" -ForegroundColor Cyan

# Krok 1: Budowanie obrazu w chmurze
gcloud builds submit --tag gcr.io/szarotka-505112/serwer-aot:latest --project=szarotka-505112 .

Write-Host ""
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "2/2: Publikuje skompilowany kontener na Cloud Run..." -ForegroundColor Yellow
Write-Host "==========================================================" -ForegroundColor Cyan

# Krok 2: Wdrozenie na Cloud Run i podpiecie bazy SQLite wraz ze zmiennymi srodowiskowymi poczty
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
Write-Host "SUCCESS: Serwer Native AOT zostal pomyslnie zaktualizowany!" -ForegroundColor Green

# Prosty, niezawodny sygnal dzwiekowy na zakonczenie kompilacji
[Console]::Beep(440, 500)
