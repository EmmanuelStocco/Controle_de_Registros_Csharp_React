// ==================== DTOs ====================

export interface PessoaCreateDTO {
  nome: string;
  sexo?: string;
  email?: string;
  dataNascimento: string; // ISO string format
  naturalidade?: string;
  nacionalidade?: string;
  cpf: string;
}

export interface PessoaUpdateDTO {
  nome: string;
  sexo?: string;
  email?: string;
  dataNascimento: string;
  naturalidade?: string;
  nacionalidade?: string;
  cpf: string;
}

export interface PessoaResponseDTO {
  id: number;
  nome: string;
  sexo?: string;
  email?: string;
  dataNascimento: string;
  naturalidade?: string;
  nacionalidade?: string;
  cpf: string;
  endereco?: string; // Opcional: preenchido na v1 opcionalmente, obrigatório na v2
  dataCadastro: string;
  dataAtualizacao: string;
}

export interface LoginDTO {
  username: string;
  password: string;
}

export interface TokenResponseDTO {
  token: string;
  expiresAt: string;
}

// ==================== API Version ====================

export type ApiVersion = 'v1' | 'v2';

// ==================== Error Response ====================

export interface ErrorResponse {
  message: string;
  errors?: Record<string, string[]>;
}

