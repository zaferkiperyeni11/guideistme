import React, { useEffect, useState } from 'react';
import api from '../services/api';

const Approvals: React.FC = () => {
  const [guides, setGuides] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchPending = () => {
    api.get('/guides/admin?status=0') // 1 = Pending (Örneğin)
      .then(res => {
        if (res.data) {
          setGuides(res.data);
        }
        setLoading(false);
      })
      .catch(() => setLoading(false));
  };

  useEffect(() => { fetchPending(); }, []);

  const handleApprove = async (id: string) => {
    try {
      await api.put(`/guides/${id}/approve`);
      alert("Guide approved successfully!");
      fetchPending();
    } catch (err) {
      alert("Error approving guide.");
    }
  };

  if (loading) return <div>Yükleniyor...</div>;

  return (
    <div>
      <h2 style={{ marginBottom: '2rem' }}>Rehber Onay Bekleyenler</h2>
      
      <div className="card">
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid var(--color-border)', textAlign: 'left' }}>
              <th style={{ padding: '1rem' }}>Ad Soyad</th>
              <th style={{ padding: '1rem' }}>Başlık</th>
              <th style={{ padding: '1rem' }}>Email</th>
              <th style={{ padding: '1rem' }}>Aksiyon</th>
            </tr>
          </thead>
          <tbody>
            {guides.map(g => (
              <tr key={g.id} style={{ borderBottom: '1px solid var(--color-border)' }}>
                <td style={{ padding: '1rem' }}>{g.fullName}</td>
                <td style={{ padding: '1rem' }}>{g.title}</td>
                <td style={{ padding: '1rem' }}>{g.email}</td>
                <td style={{ padding: '1rem' }}>
                  <button 
                    onClick={() => handleApprove(g.id)}
                    style={{ background: 'var(--color-success)', color: 'white', border: 'none', padding: '0.5rem 1rem', borderRadius: '4px', cursor: 'pointer' }}>
                    Onayla
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {guides.length === 0 && <p style={{ padding: '2rem', textAlign: 'center' }}>Bekleyen başvuru bulunmamaktadır.</p>}
      </div>
    </div>
  );
};

export default Approvals;
