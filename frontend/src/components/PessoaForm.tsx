import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { toast } from 'react-toastify';
import { pessoaAPI } from '../services/api';
import { ApiVersion } from '../types';
import ReactDatePicker, { registerLocale } from 'react-datepicker';
import { ptBR } from 'date-fns/locale';
import 'react-datepicker/dist/react-datepicker.css';

// Registra locale pt-BR
registerLocale('pt-BR', ptBR);

interface PessoaFormProps {
  apiVersion: ApiVersion;
}

interface PessoaFormData {
  nome: string;
  sexo?: string;
  email?: string;
  dataNascimento: Date; // Objeto Date
  naturalidade?: string;
  nacionalidade?: string;
  cpf: string;
  endereco?: string;
}

const PessoaForm: React.FC<PessoaFormProps> = ({ apiVersion }) => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [loading, setLoading] = useState<boolean>(false);
  const isEditMode = Boolean(id);

  const {
    register,
    handleSubmit,
    setValue,
    control,
    formState: { errors },
  } = useForm<PessoaFormData>();

  useEffect(() => {
    if (isEditMode && id) {
      loadPessoa(parseInt(id));
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const loadPessoa = async (pessoaId: number): Promise<void> => {
    try {
      const pessoa = await pessoaAPI[apiVersion].getById(pessoaId);
      setValue('nome', pessoa.nome);
      setValue('sexo', pessoa.sexo || '');
      setValue('email', pessoa.email || '');

      // Data já vem no formato ISO, converte para objeto Date
      const dataISO = pessoa.dataNascimento.split('T')[0];
      setValue('dataNascimento', new Date(dataISO + 'T00:00:00'));

      setValue('naturalidade', pessoa.naturalidade || '');
      setValue('nacionalidade', pessoa.nacionalidade || '');
      setValue('cpf', pessoa.cpf); // CPF já vem formatado do backend

      if (pessoa.endereco) {
        setValue('endereco', pessoa.endereco);
      }
    } catch (error) {
      toast.error('Erro ao carregar pessoa');
      console.error('Load error:', error);
      navigate('/');
    }
  };

  const onSubmit: SubmitHandler<PessoaFormData> = async (data) => {
    setLoading(true);
    try {
      // Remove formatação do CPF
      const cpfLimpo = data.cpf.replace(/\D/g, '');

      // Converte Date para yyyy-MM-dd
      const year = data.dataNascimento.getFullYear();
      const month = String(data.dataNascimento.getMonth() + 1).padStart(2, '0');
      const day = String(data.dataNascimento.getDate()).padStart(2, '0');
      const dataISO = `${year}-${month}-${day}`;

      console.log('Data enviada:', dataISO);

      const pessoaData: any = {
        nome: data.nome,
        sexo: data.sexo || undefined,
        email: data.email || undefined,
        dataNascimento: dataISO, // Converte para formato ISO
        naturalidade: data.naturalidade || undefined,
        nacionalidade: data.nacionalidade || undefined,
        cpf: cpfLimpo,
        endereco: data.endereco || undefined, // Incluir endereço se fornecido
      };

      if (isEditMode && id) {
        await pessoaAPI[apiVersion].update(parseInt(id), pessoaData);
        toast.success('Pessoa atualizada com sucesso!');
      } else {
        await pessoaAPI[apiVersion].create(pessoaData);
        toast.success('Pessoa cadastrada com sucesso!');
      }

      navigate('/');
    } catch (error: any) {
      const errorMessage =
        error.response?.data?.message || 'Erro ao salvar pessoa';
      toast.error(errorMessage);
      console.error('Save error:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="pessoa-form">
      <h2>{isEditMode ? 'Editar Pessoa' : 'Nova Pessoa'}</h2>

      <form onSubmit={handleSubmit(onSubmit)}>
        {/* Dados Pessoais */}
        <fieldset>
          <legend>Dados Pessoais</legend>

          <div className="form-group">
            <label htmlFor="nome">Nome Completo: *</label>
            <input
              type="text"
              id="nome"
              {...register('nome', { required: 'Nome é obrigatório' })}
              disabled={loading}
            />
            {errors.nome && <span className="error">{errors.nome.message}</span>}
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="cpf">CPF: *</label>
              <input
                type="text"
                id="cpf"
                placeholder="000.000.000-00"
                maxLength={14}
                {...register('cpf', {
                  required: 'CPF é obrigatório',
                  pattern: {
                    value: /^\d{3}\.\d{3}\.\d{3}-\d{2}$/,
                    message: 'CPF inválido',
                  },
                })}
                onInput={(e: React.ChangeEvent<HTMLInputElement>) => {
                  let value = e.target.value.replace(/\D/g, '');
                  if (value.length >= 3) {
                    value = value.slice(0, 3) + '.' + value.slice(3);
                  }
                  if (value.length >= 7) {
                    value = value.slice(0, 7) + '.' + value.slice(7);
                  }
                  if (value.length >= 11) {
                    value = value.slice(0, 11) + '-' + value.slice(11, 13);
                  }
                  e.target.value = value;
                }}
                disabled={loading}
              />
              {errors.cpf && <span className="error">{errors.cpf.message}</span>}
            </div>

            <div className="form-group">
              <label htmlFor="sexo">Sexo:</label>
              <select id="sexo" {...register('sexo')} disabled={loading}>
                <option value="">Selecione...</option>
                <option value="M">Masculino</option>
                <option value="F">Feminino</option>
              </select>
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="dataNascimento">Data de Nascimento: *</label>
              <Controller
                control={control}
                name="dataNascimento"
                rules={{ required: 'Data de nascimento é obrigatória' }}
                render={({ field }) => (
                  <ReactDatePicker
                    selected={field.value}
                    onChange={(date: Date | null) => field.onChange(date)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="dd/mm/aaaa"
                    className="date-picker-input"
                    disabled={loading}
                    showYearDropdown
                    scrollableYearDropdown
                    yearDropdownItemNumber={100}
                    maxDate={new Date()}
                    locale="pt-BR"
                  />
                )}
              />
              {errors.dataNascimento && (
                <span className="error">{errors.dataNascimento.message}</span>
              )}
            </div>



            <div className="form-group">
              <label htmlFor="email">Email:</label>
              <input
                type="email"
                id="email"
                {...register('email')}
                disabled={loading}
              />
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="naturalidade">Naturalidade:</label>
              <input
                type="text"
                id="naturalidade"
                placeholder="Ex: São Paulo"
                {...register('naturalidade')}
                disabled={loading}
              />
            </div>

            <div className="form-group">
              <label htmlFor="nacionalidade">Nacionalidade:</label>
              <input
                type="text"
                id="nacionalidade"
                placeholder="Ex: Brasileira"
                {...register('nacionalidade')}
                disabled={loading}
              />
            </div>
          </div>
        </fieldset>

        {/* Endereço */}
        <fieldset>
          <legend>
            Endereço
            {apiVersion === 'v2' && <span style={{ color: 'red' }}> *</span>}
            {apiVersion === 'v1' && <span style={{ color: '#666', fontSize: '0.9em' }}> (opcional)</span>}
          </legend>

          <div className="form-group">
            <label htmlFor="endereco">Endereço Completo:</label>
            <textarea
              id="endereco"
              rows={3}
              placeholder="Ex: Rua das Flores, 123 - Centro - São Paulo/SP - CEP 01000-000"
              {...register('endereco')}
              disabled={loading}
            />
          </div>
        </fieldset>

        {/* Botões */}
        <div className="form-actions">
          <button
            type="button"
            className="btn btn-secondary"
            onClick={() => navigate('/')}
            disabled={loading}
          >
            Cancelar
          </button>
          <button type="submit" className="btn btn-primary" disabled={loading}>
            {loading ? 'Salvando...' : isEditMode ? 'Atualizar' : 'Cadastrar'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default PessoaForm;

