import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { pessoaAPI } from '../services/api';
import { PessoaResponseDTO, ApiVersion } from '../types';

interface PessoaListProps {
  apiVersion: ApiVersion;
}

const PessoaList: React.FC<PessoaListProps> = ({ apiVersion }) => {
  const [pessoas, setPessoas] = useState<PessoaResponseDTO[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadPessoas();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [apiVersion]);

  const loadPessoas = async (): Promise<void> => {
    try {
      setLoading(true);
      const data = await pessoaAPI[apiVersion].getAll();
      setPessoas(data);
    } catch (error) {
      toast.error('Erro ao carregar pessoas');
      console.error('Load error:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number): Promise<void> => {
    if (window.confirm('Tem certeza que deseja excluir esta pessoa?')) {
      try {
        await pessoaAPI[apiVersion].delete(id);
        toast.success('Pessoa excluída com sucesso!');
        loadPessoas();
      } catch (error) {
        toast.error('Erro ao excluir pessoa');
        console.error('Delete error:', error);
      }
    }
  };

  const formatDate = (dateString: string): string => { 
    const [ano, mes, dia] = dateString.split('T')[0].split('-');
    return `${dia}/${mes}/${ano}`;
  };

  if (loading) {
    return <div className="loading">Carregando...</div>;
  }

  return (
    <div className="pessoa-list">
      <div className="list-header">
        <h2>Lista de Pessoas</h2>
        <button
          className="btn btn-primary"
          onClick={() => navigate('/pessoa/new')}
        >
          Nova Pessoa
        </button>
      </div>

      {pessoas.length === 0 ? (
        <div className="empty-state">
          <p>Nenhuma pessoa cadastrada.</p>
          <button
            className="btn btn-primary"
            onClick={() => navigate('/pessoa/new')}
          >
            Cadastrar primeira pessoa
          </button>
        </div>
      ) : (
        <div className="table-container">
          <table className="pessoa-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nome</th>
                <th>CPF</th>
                <th>Sexo</th>
                <th>Email</th>
                <th>Data Nascimento</th>
                <th>Naturalidade</th>
                <th>Nacionalidade</th>
                <th>Endereço</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {pessoas.map((pessoa) => (
                <tr key={pessoa.id}>
                  <td>{pessoa.id}</td>
                  <td>{pessoa.nome}</td>
                  <td>{pessoa.cpf}</td>
                  <td>{pessoa.sexo || '-'}</td>
                  <td>{pessoa.email || '-'}</td>
                  <td>{formatDate(pessoa.dataNascimento)}</td>
                  <td>{pessoa.naturalidade || '-'}</td>
                  <td>{pessoa.nacionalidade || '-'}</td>
                  <td>{pessoa.endereco || '-'}</td>
                  <td>
                    <button
                      className="btn btn-secondary btn-sm"
                      onClick={() => navigate(`/pessoa/edit/${pessoa.id}`)}
                    >
                      Editar
                    </button>
                    <button
                      className="btn btn-danger btn-sm"
                      onClick={() => handleDelete(pessoa.id)}
                      style={{ marginLeft: '5px' }}
                    >
                      Excluir
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default PessoaList;

