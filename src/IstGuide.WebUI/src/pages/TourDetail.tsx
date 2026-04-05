import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from '../services/api';

const TourDetail: React.FC = () => {
    const { slug } = useParams();
    const [tour, setTour] = useState<any>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        api.get(`/tours?slug=${slug}`) // Backend'e slug ile filtreleme ekledik varsayarak
            .then(res => {
                const found = res.data.value.find((t: any) => t.slug === slug);
                setTour(found);
                setLoading(false);
            });
    }, [slug]);

    if (loading) return <div style={{ padding: '5rem', textAlign: 'center' }}>Loading Tour Details...</div>;
    if (!tour) return <div style={{ padding: '5rem', textAlign: 'center' }}>Tour Not Found!</div>;

    return (
        <div className="container" style={{ maxWidth: '1000px', margin: '3rem auto', padding: '0 1rem' }}>
            <div style={{ 
                height: '400px', 
                borderRadius: '16px', 
                backgroundImage: `url(${tour.imageUrl || 'https://via.placeholder.com/1000x400?text=Tour+Photo'})`,
                backgroundSize: 'cover',
                backgroundPosition: 'center',
                marginBottom: '2rem'
            }} />
            
            <div style={{ display: 'grid', gridTemplateColumns: '2fr 1fr', gap: '2rem' }}>
                <div>
                    <h1 style={{ fontSize: '2.5rem', marginBottom: '1rem' }}>{tour.title}</h1>
                    <div style={{ display: 'flex', gap: '1.5rem', marginBottom: '2rem', color: 'var(--color-text-secondary)' }}>
                        <span>⏱ <strong>Duration:</strong> {tour.duration}</span>
                        <span>💰 <strong>Starting From:</strong> ${tour.price}</span>
                    </div>
                    <div style={{ lineHeight: '1.8', fontSize: '1.1rem' }}>
                        {tour.description}
                    </div>
                </div>
                
                <div className="card" style={{ padding: '2rem', height: 'fit-content', position: 'sticky', top: '2rem' }}>
                    <h3 style={{ marginBottom: '1.5rem' }}>Book This Tour</h3>
                    <p style={{ color: 'var(--color-text-secondary)', marginBottom: '2rem' }}>Contact us via WhatsApp to book this exclusive Istanbul experience.</p>
                    <a 
                        href={`https://wa.me/905000000000?text=I am interested in ${tour.title}`}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="btn-primary"
                        style={{ display: 'block', textAlign: 'center', padding: '1rem', textDecoration: 'none' }}
                    >
                        Book via WhatsApp
                    </a>
                </div>
            </div>
        </div>
    );
};

export default TourDetail;
