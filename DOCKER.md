# 🐳 Docker - Guia de Deploy

Este guia mostra como rodar o projeto usando Docker

## 📦 Pré-requisitos

- Docker instalado ([Download](https://www.docker.com/))
- Docker Compose instalado (geralmente vem com Docker Desktop)

## 🚀 Rodar Localmente com Docker

### Opção 1: Docker Compose (Recomendado)

```bash
# Na raiz do projeto
docker-compose up --build
```

Isso vai:
- Buildar a API backend (porta 5000)
- Buildar o frontend React (porta 3000)
- Criar uma rede entre os containers

Acesse:
- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger

### Opção 2: Rodar Containers Individualmente

**Backend:**
```bash
cd backend/PessoaAPI
docker build -t pessoa-api .
docker run -p 5000:80 pessoa-api
```

**Frontend:**
```bash
cd frontend
docker build -t pessoa-frontend .
docker run -p 3000:80 pessoa-frontend
```

## 🌐 Deploy na Render

### Backend (API)

1. Crie um novo **Web Service** na Render
2. Conecte seu repositório GitHub
3. Configure:
   - **Name**: `pessoa-api`
   - **Environment**: `Docker`
   - **Dockerfile Path**: `backend/PessoaAPI/Dockerfile`
   - **Docker Build Context**: `backend/PessoaAPI`
   - **Port**: `80`

4. Variáveis de ambiente:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:80
   ```

5. Deploy!

### Frontend

1. Crie um novo **Static Site** na Render
2. Conecte seu repositório GitHub
3. Configure:
   - **Name**: `pessoa-frontend`
   - **Build Command**: `cd frontend && npm install && npm run build`
   - **Publish Directory**: `frontend/build`

4. Variáveis de ambiente:
   ```
   REACT_APP_API_URL=https://pessoa-api.onrender.com
   ```
   ⚠️ **Substitua pela URL do seu backend!**

5. Deploy!

### Alternativa: Frontend com Docker na Render

Se preferir usar Docker para o frontend também:

1. Crie um novo **Web Service** na Render
2. Configure:
   - **Environment**: `Docker`
   - **Dockerfile Path**: `frontend/Dockerfile`
   - **Docker Build Context**: `frontend`
   - **Port**: `80`

3. Variáveis de ambiente:
   ```
   REACT_APP_API_URL=https://pessoa-api.onrender.com
   ```

## 🔧 Configurações Importantes

### CORS no Backend

Certifique-se de que o backend aceita requisições do frontend em produção.

Em `Program.cs`, atualize o CORS:

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "https://pessoa-frontend.onrender.com"  // Adicione sua URL
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
```

### URL da API no Frontend

Crie um arquivo `.env.production` no frontend:

```
REACT_APP_API_URL=https://pessoa-api.onrender.com
```

## 📝 Comandos Úteis

```bash
# Ver logs dos containers
docker-compose logs -f

# Parar os containers
docker-compose down

# Parar e remover volumes
docker-compose down -v

# Reconstruir containers
docker-compose up --build --force-recreate

# Ver containers rodando
docker ps

# Limpar imagens não usadas
docker system prune -a
``` 
  

## 📚 Recursos

- [Documentação Docker](https://docs.docker.com/)
- [Render Docs](https://render.com/docs)
- [.NET Docker](https://docs.microsoft.com/en-us/dotnet/core/docker/)

