import React, { useState } from 'react';
import api from '../services/api';

interface LoginProps {
  onLogin: (token: string) => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await api.post('/auth/login', { email, password });
      if (res.data?.succeeded) {
        onLogin(res.data.value.token);
      } else {
        setError('Giriş bilgileri hatalı!');
      }
    } catch (err) {
      setError('Sistem hatası veya geçersiz bilgiler.');
    }
  };

  return (
    <div style={{ 
      height: '100vh', 
      display: 'flex', 
      alignItems: 'center', 
      justifyContent: 'center', 
      background: 'var(--color-bg)' 
    }}>
      <form onSubmit={handleSubmit} className="card" style={{ width: '100%', maxWidth: '400px' }}>
        <h2 style={{ textAlign: 'center', marginBottom: '2rem', color: 'var(--color-primary)' }}>Admin Login</h2>
        
        {error && <p style={{ color: 'var(--color-error)', marginBottom: '1rem', textAlign: 'center' }}>{error}</p>}
        
        <div style={{ marginBottom: '1rem' }}>
          <label style={{ display: 'block', marginBottom: '0.5rem' }}>Email</label>
          <input 
            type="email" 
            value={email} 
            onChange={e => setEmail(e.target.value)} 
            required 
            style={{ width: '100%' }}
          />
        </div>
        
        <div style={{ marginBottom: '2rem' }}>
          <label style={{ display: 'block', marginBottom: '0.5rem' }}>Password</label>
          <input 
            type="password" 
            value={password} 
            onChange={e => setPassword(e.target.value)} 
            required 
            style={{ width: '100%' }}
          />
        </div>
        
        <button type="submit" className="btn-primary" style={{ width: '100%', padding: '1rem' }}>Login</button>

        <div style={{ margin: '1.5rem 0', textAlign: 'center', color: '#64748b', fontSize: '0.9rem' }}>--- OR ---</div>

        <button 
          type="button" 
          onClick={() => onLogin('demo_token_123')}
          style={{ 
            width: '100%', 
            padding: '1rem', 
            background: '#f8fafc', 
            color: 'var(--color-primary)', 
            border: '1.5px dashed var(--color-primary)', 
            fontWeight: 'bold', 
            cursor: 'pointer',
            borderRadius: '8px'
          }}>
          🚀 DEMO LOGIN (NO PASSWORD)
        </button>
      </form>
    </div>
  );
};

export default Login;
