import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import PessoaList from './components/PessoaList';
import PessoaForm from './components/PessoaForm';
import { ToastContainer } from 'react-toastify';
import { ApiVersion } from './types';
import 'react-toastify/dist/ReactToastify.css';
import './App.css';

const App: React.FC = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [apiVersion, setApiVersion] = useState<ApiVersion>('v1');

  useEffect(() => {
    const savedToken = localStorage.getItem('token');
    if (savedToken) {
      setIsAuthenticated(true);
    }
  }, []);

  const handleLogin = (newToken: string): void => {
    setIsAuthenticated(true);
    localStorage.setItem('token', newToken);
  };

  const handleLogout = (): void => {
    setIsAuthenticated(false);
    localStorage.removeItem('token');
  };

  if (!isAuthenticated) {
    return (
      <div className="App">
        <Login onLogin={handleLogin} />
        <ToastContainer />
      </div>
    );
  }

  return (
    <Router>
      <div className="App">
        <header className="header">
          <div className="container">
            <h1>Sistema de Cadastro de Pessoas</h1>
            <div className="version-selector">
              <button
                className={apiVersion === 'v1' ? 'active' : ''}
                onClick={() => setApiVersion('v1')}
              >
                API v1
              </button>
              <button
                className={apiVersion === 'v2' ? 'active' : ''}
                onClick={() => setApiVersion('v2')}
              >
                API v2 (com endereço)
              </button>
              <button
                className="btn btn-secondary"
                onClick={handleLogout}
                style={{ marginLeft: '20px' }}
              >
                Logout
              </button>
            </div>
          </div>
        </header>

        <div className="container">
          <Routes>
            <Route path="/" element={<PessoaList apiVersion={apiVersion} />} />
            <Route
              path="/pessoa/new"
              element={<PessoaForm apiVersion={apiVersion} />}
            />
            <Route
              path="/pessoa/edit/:id"
              element={<PessoaForm apiVersion={apiVersion} />}
            />
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
        </div>

        <ToastContainer />
      </div>
    </Router>
  );
};

export default App;

