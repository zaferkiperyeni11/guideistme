import React, { useEffect, useState } from 'react';
import api from '../services/api';

const Dashboard: React.FC = () => {
  const [stats, setStats] = useState<any>(null);

  useEffect(() => {
    api.get('/dashboard/stats')
       .then(res => {
         if (res.data?.succeeded) {
           setStats(res.data.value);
         }
       })
       .catch(err => console.error("Stats yüklenemedi", err));
  }, []);

  return (
    <div>
      <h2 style={{ marginBottom: '2rem' }}>Dashboard</h2>
      
      {stats ? (
        <div className="stat-grid">
          <div className="card" style={{ borderLeft: '4px solid var(--color-success)' }}>
            <h4 style={{ color: 'var(--color-text-secondary)', marginBottom: '0.5rem' }}>Toplam Rehber</h4>
            <div style={{ fontSize: '2rem', fontWeight: 'bold' }}>{stats.totalGuides}</div>
          </div>
          <div className="card" style={{ borderLeft: '4px solid var(--color-warning)' }}>
            <h4 style={{ color: 'var(--color-text-secondary)', marginBottom: '0.5rem' }}>Bekleyen Rehberler</h4>
            <div style={{ fontSize: '2rem', fontWeight: 'bold' }}>{stats.pendingApprovals}</div>
          </div>
          <div className="card" style={{ borderLeft: '4px solid var(--color-primary)' }}>
            <h4 style={{ color: 'var(--color-text-secondary)', marginBottom: '0.5rem' }}>Toplam Yorum</h4>
            <div style={{ fontSize: '2rem', fontWeight: 'bold' }}>{stats.totalReviews}</div>
          </div>
        </div>
      ) : (
        <p>Veriler yükleniyor (Backend çalışır durumda olmalıdır!)...</p>
      )}
    </div>
  );
};

export default Dashboard;
