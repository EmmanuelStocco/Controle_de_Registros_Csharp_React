import axios, { AxiosInstance, AxiosError } from 'axios';
import {
  LoginDTO,
  TokenResponseDTO,
  PessoaResponseDTO,
  PessoaCreateDTO,
  PessoaUpdateDTO,
  ApiVersion,
} from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000'; 

const api: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
}); 

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error: AxiosError) => {
    return Promise.reject(error);
  }
);

// ==================== Response Interceptor ====================

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/';
    }
    return Promise.reject(error);
  }
);

// ==================== Auth API ====================

export const authAPI = {
  login: async (credentials: LoginDTO): Promise<TokenResponseDTO> => {
    const response = await api.post<TokenResponseDTO>('/api/auth/login', credentials);
    return response.data;
  },
};

// ==================== Pessoa API ====================

class PessoaAPIService {
  constructor(private version: ApiVersion) {}

  async getAll(): Promise<PessoaResponseDTO[]> {
    const response = await api.get<PessoaResponseDTO[]>(`/api/${this.version}/pessoa`);
    return response.data;
  }

  async getById(id: number): Promise<PessoaResponseDTO> {
    const response = await api.get<PessoaResponseDTO>(`/api/${this.version}/pessoa/${id}`);
    return response.data;
  }

  async create(pessoa: PessoaCreateDTO): Promise<PessoaResponseDTO> {
    const response = await api.post<PessoaResponseDTO>(`/api/${this.version}/pessoa`, pessoa);
    return response.data;
  }

  async update(id: number, pessoa: PessoaUpdateDTO): Promise<void> {
    await api.put(`/api/${this.version}/pessoa/${id}`, pessoa);
  }

  async delete(id: number): Promise<void> {
    await api.delete(`/api/${this.version}/pessoa/${id}`);
  }
}

export const pessoaAPI = {
  v1: new PessoaAPIService('v1'),
  v2: new PessoaAPIService('v2'),
};

export default api;

