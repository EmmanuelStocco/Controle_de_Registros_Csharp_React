#!/bin/bash

echo "========================================"
echo "   Sistema de Cadastro de Pessoas"
echo "========================================"
echo

echo "Iniciando o backend..."
cd backend/PessoaAPI
dotnet run &
BACKEND_PID=$!
cd ../..

echo
echo "Aguardando 5 segundos para o backend inicializar..."
sleep 5

echo
echo "Iniciando o frontend..."
cd frontend
npm start &
FRONTEND_PID=$!

echo
echo "========================================"
echo "   Aplicacao iniciada com sucesso!"
echo "========================================"
echo
echo "Backend: https://localhost:5001"
echo "Frontend: http://localhost:3000"
echo "Swagger: https://localhost:5001/swagger"
echo
echo "Credenciais de teste:"
echo "- admin / admin123"
echo "- user / user123"
echo "- test / test123"
echo
 
cleanup() {
    echo "Parando aplicações..."
    kill $BACKEND_PID 2>/dev/null
    kill $FRONTEND_PID 2>/dev/null
    exit
}
 
trap cleanup SIGINT
 
wait

