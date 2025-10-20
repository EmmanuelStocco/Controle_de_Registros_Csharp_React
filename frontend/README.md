# Frontend TypeScript - Sistema de Cadastro de Pessoas

Frontend em **React 18 + TypeScript** para o sistema de cadastro de pessoas.

## 🚀 Tecnologias

- **React 18** - Framework JavaScript
- **TypeScript** - Tipagem estática
- **React Router v6** - Roteamento
- **React Hook Form** - Formulários com validação
- **Axios** - Cliente HTTP
- **React Toastify** - Notificações

## 📁 Estrutura

```
frontend2/
├── public/
│   ├── index.html
│   └── manifest.json
├── src/
│   ├── components/
│   │   ├── Login.tsx
│   │   ├── PessoaList.tsx
│   │   └── PessoaForm.tsx
│   ├── services/
│   │   └── api.ts           # Cliente HTTP tipado
│   ├── types/
│   │   └── index.ts          # Tipos/Interfaces do backend
│   ├── App.tsx
│   ├── App.css
│   ├── index.tsx
│   └── index.css
├── package.json
└── tsconfig.json
```

## 🎯 Destaques TypeScript

### 1. **Tipos Fortemente Tipados**
Todos os DTOs do backend são representados como interfaces TypeScript:

```typescript
interface PessoaResponseDTO {
  id: number;
  nome: string;
  cpf: string;
  dataNascimento: string;
  email?: string;
  // ...
}
```

### 2. **API Cliente Tipado**
Axios configurado com tipos genéricos:

```typescript
async getAll(): Promise<PessoaResponseDTO[]> {
  const response = await api.get<PessoaResponseDTO[]>(`/api/${this.version}/pessoa`);
  return response.data;
}
```

### 3. **Componentes Funcionais Tipados**
Props e estados com tipos explícitos:

```typescript
interface LoginProps {
  onLogin: (token: string) => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
  const [loading, setLoading] = useState<boolean>(false);
  // ...
}
```

### 4. **Formulários Tipados com React Hook Form**
```typescript
const { register, handleSubmit, formState: { errors } } = useForm<PessoaFormData>();
```

## 🔧 Como Executar

### 1. Instalar dependências:
```bash
npm install
```

### 2. Configurar variável de ambiente (opcional):
Crie `.env` na raiz (padrão já está em `http://localhost:5000`):

**Para HTTP (sem SSL - recomendado em dev):**
```env
REACT_APP_API_URL=http://localhost:5000
```

**Para HTTPS (requer certificado confiável):**
```env
REACT_APP_API_URL=https://localhost:5001
```

💡 **Dica:** Use HTTP em desenvolvimento para evitar problemas com certificados SSL auto-assinados.

### 3. Executar em desenvolvimento:
```bash
npm start
```

Acesse: http://localhost:3000

## 📦 Build para Produção

```bash
npm run build
```

Gera pasta `build/` otimizada para deploy.

## 🌐 Deploy

### Vercel (Recomendado)
```bash
npm install -g vercel
vercel
```

### Netlify
```bash
npm install -g netlify-cli
netlify deploy --prod --dir=build
```

## 🔑 Credenciais de Teste

- **admin** / admin123
- **user** / user123
- **test** / test123

## 🎨 Diferenças do Frontend JavaScript

| Aspecto | JavaScript | TypeScript |
|---------|------------|------------|
| Tipagem | Dinâmica | Estática |
| Autocomplete | Limitado | Completo |
| Erros | Runtime | Compile-time |
| Refatoração | Arriscada | Segura |
| Documentação | Manual | Automática (via tipos) |

## 📝 Versionamento da API

O frontend suporta 2 versões da API:

- **v1**: Endereço opcional
- **v2**: Endereço obrigatório

Altere dinamicamente entre as versões usando os botões no header.

## 🛠️ Funcionalidades

✅ Autenticação JWT  
✅ CRUD completo de pessoas  
✅ Validação de CPF  
✅ Suporte a API v1 e v2  
✅ Formulários com validação  
✅ Notificações (toast)  
✅ Design responsivo  
✅ Type-safe em 100% do código  

## 📄 Licença

MIT

