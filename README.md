# Sistema de Cadastro de Pessoas

Sistema completo de cadastro de pessoas desenvolvido com .NET 6 e React.

## 🚀 Funcionalidades

### Backend (.NET 6)
- ✅ API REST completa com CRUD de pessoas
- ✅ Validação de CPF com algoritmo oficial
- ✅ Validação de email
- ✅ Validação de data de nascimento
- ✅ Autenticação JWT
- ✅ Duas versões da API (v1 e v2)
- ✅ Documentação Swagger
- ✅ Testes automatizados com XUnit (cobertura > 80%)
- ✅ Banco de dados InMemory (equivalente ao H2)

### Frontend (React 18)
- ✅ Interface moderna e responsiva
- ✅ Autenticação JWT
- ✅ CRUD completo de pessoas
- ✅ Suporte a ambas as versões da API
- ✅ Validações em tempo real
- ✅ Notificações toast

## 📋 Requisitos Atendidos

### Obrigatórios
1. **Backend .NET 6+** ✅
2. **API REST com CRUD** ✅
3. **Validações obrigatórias** ✅
4. **Frontend React 17+** ✅
5. **Repositório público** ✅

### Extras
1. **Documentação Swagger** ✅
2. **Banco H2 (InMemory)** ✅
3. **API v2 com endereço** ✅
4. **Autenticação JWT** ✅
5. **Testes automatizados** ✅

## 🛠️ Tecnologias Utilizadas

### Backend
- .NET 6
- Entity Framework Core
- JWT Authentication
- Swagger/OpenAPI
- XUnit (Testes)
- FluentAssertions
- Docker
- Moq

### Frontend
- React 18
- React Router DOM
- React Hook Form
- Axios
- React Toastify

## 🚀 Como Executar

### Pré-requisitos
- .NET 6 SDK
- Node.js 16+
- npm ou yarn

### Backend
```bash
# Navegar para o diretório do backend
cd backend/PessoaAPI

# Restaurar dependências
dotnet restore

# Executar a aplicação
dotnet run
```

A API estará disponível em:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger: `https://localhost:5001/swagger`

### Frontend
```bash
# Navegar para o diretório do frontend
cd frontend

# Instalar dependências
npm install

# Executar a aplicação
npm start
```

O frontend estará disponível em: `http://localhost:3000`

### 🐳 Docker (Recomendado para Deploy)

Para rodar com Docker:

```bash
# Na raiz do projeto
docker-compose up --build
```

Acesse:
- Frontend: `http://localhost:3000`
- Backend: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`

📖 **Guia completo de Docker e Deploy**: Veja [DOCKER.md](./DOCKER.md)

### 🚀 Script Rápido

**Windows:**
```bash
run.bat
```

**Linux/Mac:**
```bash
chmod +x run.sh
./run.sh
```

## 🔐 Credenciais de Teste

- **admin** / admin123
- **user** / user123
- **test** / test123

## 📚 Endpoints da API

### Autenticação
- `POST /api/auth/login` - Login

### API v1 (Pessoas)
- `GET /api/v1/pessoa` - Listar pessoas
- `GET /api/v1/pessoa/{id}` - Buscar pessoa por ID
- `POST /api/v1/pessoa` - Criar pessoa
- `PUT /api/v1/pessoa/{id}` - Atualizar pessoa
- `DELETE /api/v1/pessoa/{id}` - Excluir pessoa

### API v2 (Pessoas com Endereço)
- `GET /api/v2/pessoa` - Listar pessoas
- `GET /api/v2/pessoa/{id}` - Buscar pessoa por ID
- `POST /api/v2/pessoa` - Criar pessoa
- `PUT /api/v2/pessoa/{id}` - Atualizar pessoa
- `DELETE /api/v2/pessoa/{id}` - Excluir pessoa

## 🧪 Testes

### Executar Testes
```bash
# Executar todos os testes
dotnet test

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### Cobertura de Código
O projeto possui cobertura de código superior a 80% nos testes automatizados.

## 📝 Modelo de Dados

### Pessoa (v1)
- Id (int, PK)
- Nome (string, obrigatório)
- Sexo (string, opcional)
- Email (string, opcional, único)
- DataNascimento (DateTime, obrigatório)
- Naturalidade (string, opcional)
- Nacionalidade (string, opcional)
- CPF (string, obrigatório, único)
- DataCadastro (DateTime)
- DataAtualizacao (DateTime)

### PessoaV2 (v2)
- Todos os campos da v1 +
- Endereco (string, obrigatório)

## 🔒 Segurança

- Autenticação JWT obrigatória para todas as operações
- Validação de CPF com algoritmo oficial
- Validação de email
- Validação de unicidade de CPF e email
- CORS configurado para o frontend

## 📊 Validações

### CPF
- Formato correto (11 dígitos)
- Algoritmo de validação oficial
- Unicidade no banco de dados

### Email
- Formato válido
- Unicidade no banco de dados (quando preenchido)

### Data de Nascimento
- Campo obrigatório
- Validação de formato

## 🌐 Deploy

Para deploy em nuvem, recomenda-se:
1. Azure App Service
2. AWS Elastic Beanstalk
3. Heroku
4. DigitalOcean App Platform

### Variáveis de Ambiente
- `Jwt__Key`: Chave secreta para JWT
- `Jwt__Issuer`: Emissor do token
- `Jwt__Audience`: Audiência do token

## 👨‍💻 Desenvolvedor

Sistema desenvolvido, demonstrando conhecimento em:
- .NET 6 e C#
- React e JavaScript
- Entity Framework Core
- JWT Authentication
- Testes automatizados
- API REST
- Swagger/OpenAPI

## Deploy
O projeto foi hospedado na Railway, você pode testa-lo em:
- API: `https://pessoa-api-production.up.railway.app/swagger/index.html`
- Front: `https://pessoa-frontend-production.up.railway.app/`
