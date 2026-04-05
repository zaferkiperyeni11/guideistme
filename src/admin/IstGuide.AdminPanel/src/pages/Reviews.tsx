import React, { useEffect, useState } from 'react';
import api from '../services/api';

const Reviews: React.FC = () => {
    const [reviews, setReviews] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);

    const fetchReviews = () => {
        api.get('/reviews') // Backend'de tüm yorumları çeken uç nokta
            .then(res => {
                if (res.data?.succeeded) setReviews(res.data.value);
                setLoading(false);
            })
            .catch(() => setLoading(false));
    };

    useEffect(() => { fetchReviews(); }, []);

    const handleAction = async (id: string, approve: boolean) => {
        try {
            if (approve) {
                await api.post(`/reviews/${id}/approve`);
            } else {
                await api.delete(`/reviews/${id}`);
            }
            alert(approve ? 'Yorum onaylandı!' : 'Yorum silindi!');
            fetchReviews();
        } catch (err) {
            alert('İşlem başarısız.');
        }
    };

    if (loading) return <div>Yükleniyor...</div>;

    return (
        <div>
            <h2 style={{ marginBottom: '2rem' }}>Yorum Moderasyonu</h2>
            <div className="card">
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                    <thead>
                        <tr style={{ borderBottom: '2px solid var(--color-border)', textAlign: 'left' }}>
                            <th style={{ padding: '1rem' }}>Rehber</th>
                            <th style={{ padding: '1rem' }}>Yorum Yapan</th>
                            <th style={{ padding: '1rem' }}>Puan</th>
                            <th style={{ padding: '1rem' }}>Yorum Mesajı</th>
                            <th style={{ padding: '1rem' }}>İşlem</th>
                        </tr>
                    </thead>
                    <tbody>
                        {reviews.map(r => (
                            <tr key={r.id} style={{ borderBottom: '1px solid var(--color-border)' }}>
                                <td style={{ padding: '1rem' }}>{r.guideName}</td>
                                <td style={{ padding: '1rem' }}>{r.reviewerName}</td>
                                <td style={{ padding: '1rem', color: '#FFB400' }}>★ {r.rating}</td>
                                <td style={{ padding: '1rem', fontSize: '0.9rem' }}>{r.comment}</td>
                                <td style={{ padding: '1rem', display: 'flex', gap: '0.5rem' }}>
                                    {!r.isApproved && (
                                        <button 
                                            onClick={() => handleAction(r.id, true)}
                                            style={{ background: 'var(--color-success)', color: 'white', border: 'none', padding: '0.4rem 0.8rem', borderRadius: '4px', cursor: 'pointer' }}>
                                            Onayla
                                        </button>
                                    )}
                                    <button 
                                        onClick={() => handleAction(r.id, false)}
                                        style={{ background: 'var(--color-error)', color: 'white', border: 'none', padding: '0.4rem 0.8rem', borderRadius: '4px', cursor: 'pointer' }}>
                                        Sil
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                {reviews.length === 0 && <p style={{ padding: '2rem', textAlign: 'center' }}>Henüz yorum bulunmuyor.</p>}
            </div>
        </div>
    );
};

export default Reviews;
