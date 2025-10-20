import React, { useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { toast } from 'react-toastify';
import { authAPI } from '../services/api';
import { LoginDTO } from '../types';

interface LoginProps {
  onLogin: (token: string) => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginDTO>();
  const [loading, setLoading] = useState<boolean>(false);

  const onSubmit: SubmitHandler<LoginDTO> = async (data) => {
    setLoading(true);
    try {
      const response = await authAPI.login(data);
      onLogin(response.token);
      toast.success('Login realizado com sucesso!');
    } catch (error) {
      toast.error('Credenciais inválidas!');
      console.error('Login error:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-form">
        <h2>Login</h2>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="form-group">
            <label htmlFor="username">Usuário:</label>
            <input
              type="text"
              id="username"
              {...register('username', { required: 'Usuário é obrigatório' })}
              disabled={loading}
            />
            {errors.username && (
              <span className="error">{errors.username.message}</span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="password">Senha:</label>
            <input
              type="password"
              id="password"
              {...register('password', { required: 'Senha é obrigatória' })}
              disabled={loading}
            />
            {errors.password && (
              <span className="error">{errors.password.message}</span>
            )}
          </div>

          <button
            type="submit"
            className="btn btn-primary"
            disabled={loading}
            style={{ width: '100%' }}
          >
            {loading ? 'Entrando...' : 'Entrar'}
          </button>
        </form>

        <div style={{ marginTop: '20px', fontSize: '14px', color: '#666' }}>
          <p>
            <strong>Usuários de teste:</strong>
          </p>
          <p>admin / admin123</p>
          <p>user / user123</p>
          <p>test / test123</p>
        </div>
      </div>
    </div>
  );
};

export default Login;

