import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from '../services/api';

const PageDetail: React.FC = () => {
    const { slug } = useParams<{ slug: string }>();
    const [page, setPage] = useState<any>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        api.get(`/pages/${slug}`)
            .then(res => {
                if (res.data?.succeeded) {
                    setPage(res.data.value);
                }
                setLoading(false);
            })
            .catch(() => setLoading(false));
    }, [slug]);

    if (loading) return <div className="container" style={{padding: '5rem', textAlign: 'center'}}>Yükleniyor...</div>;

    if (!page) return <div className="container" style={{padding: '5rem', textAlign: 'center'}}>Sayfa bulunamadı.</div>;

    return (
        <div className="container" style={{ maxWidth: '800px', margin: '4rem auto', padding: '0 1rem' }}>
            <h1 style={{ color: 'var(--color-primary)', marginBottom: '2rem', borderBottom: '2px solid var(--color-border)', paddingBottom: '1rem' }}>
                {page.title}
            </h1>
            <div 
                className="cms-content" 
                style={{ lineHeight: '1.8', fontSize: '1.1rem', color: 'var(--color-text-primary)' }}
                dangerouslySetInnerHTML={{ __html: page.content }} 
            />
        </div>
    );
};

export default PageDetail;
