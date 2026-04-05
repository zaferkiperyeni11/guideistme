import React, { useEffect, useState } from 'react';
import api from '../services/api';

const CmsPages: React.FC = () => {
    const [pages, setPages] = useState<any[]>([]);
    const [selectedPage, setSelectedPage] = useState<any>(null);
    const [content, setContent] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        api.get('/pages')
            .then(res => {
                if (res.data?.succeeded) setPages(res.data.value);
                setLoading(false);
            })
            .catch(() => setLoading(false));
    }, []);

    const handleSave = async () => {
        if (!selectedPage) return;
        try {
            await api.put(`/pages/${selectedPage.id}`, { 
                title: selectedPage.title, 
                content: content 
            });
            alert('Sayfa başarıyla güncellendi!');
        } catch (err) {
            alert('Güncelleme sırasında hata oluştu.');
        }
    };

    if (loading) return <div>Yükleniyor...</div>;

    return (
        <div style={{ display: 'grid', gridTemplateColumns: '250px 1fr', gap: '2rem' }}>
            <div className="card">
                <h3>Sayfalar</h3>
                <ul style={{ listStyle: 'none', padding: 0, marginTop: '1rem' }}>
                    {pages.map(p => (
                        <li key={p.id} 
                            onClick={() => { setSelectedPage(p); setContent(p.content); }}
                            style={{ 
                                padding: '0.75rem', 
                                cursor: 'pointer', 
                                background: selectedPage?.id === p.id ? '#eef2ff' : 'transparent',
                                borderRadius: '4px',
                                marginBottom: '0.25rem',
                                color: selectedPage?.id === p.id ? 'var(--color-primary)' : 'inherit'
                            }}>
                            {p.title}
                        </li>
                    ))}
                </ul>
            </div>

            <div className="card">
                {selectedPage ? (
                    <div>
                        <h2>{selectedPage.title} Düzenle</h2>
                        <div style={{ marginTop: '1.5rem' }}>
                            <label style={{ display: 'block', marginBottom: '0.5rem' }}>İçerik (HTML desteklenir)</label>
                            <textarea 
                                value={content}
                                onChange={e => setContent(e.target.value)}
                                style={{ width: '100%', minHeight: '400px', padding: '1rem', fontFamily: 'monospace' }}
                            />
                            <button 
                                onClick={handleSave}
                                className="btn-primary" 
                                style={{ marginTop: '1rem' }}>
                                Değişiklikleri Kaydet
                            </button>
                        </div>
                    </div>
                ) : (
                    <div style={{ textAlign: 'center', padding: '5rem', color: 'var(--color-text-secondary)' }}>
                        Düzenlemek için soldan bir sayfa seçin.
                    </div>
                )}
            </div>
        </div>
    );
};

export default CmsPages;
