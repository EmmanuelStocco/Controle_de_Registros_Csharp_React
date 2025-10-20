@echo off
echo ========================================
echo   Sistema de Cadastro de Pessoas
echo ========================================
echo.

echo Iniciando o backend...
cd backend\PessoaAPI
start "Backend" cmd /k "dotnet run"
cd ..\..

echo.
echo Aguardando 5 segundos para o backend inicializar...
timeout /t 5 /nobreak > nul

echo.
echo Iniciando o frontend...
cd frontend
start "Frontend" cmd /k "npm start"

echo.
echo ========================================
echo   Aplicacao iniciada com sucesso!
echo ========================================
echo.
echo Backend: https://localhost:5001
echo Frontend: http://localhost:3000
echo Swagger: https://localhost:5001/swagger
echo.
echo Credenciais de teste:
echo - admin / admin123
echo - user / user123
echo - test / test123
echo.
pause

