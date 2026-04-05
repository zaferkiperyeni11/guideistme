import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import api from '../services/api';

const GuideDetail: React.FC = () => {
    const { slug } = useParams<{ slug: string }>();
    const [guide, setGuide] = useState<any>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        api.get(`/guides/${slug}`)
            .then(res => {
                if (res.data?.succeeded) {
                    setGuide(res.data.value);
                }
                setLoading(false);
            })
            .catch(() => setLoading(false));
    }, [slug]);

    if (loading) return <div className="container" style={{padding: '5rem', textAlign: 'center'}}>Yükleniyor...</div>;

    if (!guide) return (
      <div className="container" style={{padding: '5rem', textAlign: 'center'}}>
        <h2 style={{color: 'var(--color-error)'}}>Rehber Bulunamadı!</h2>
        <Link to="/rehberler" className="btn-secondary">Geri Dön</Link>
      </div>
    );

    return (
        <div className="container" style={{ maxWidth: '1200px', margin: '3rem auto', padding: '0 1.5rem' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '350px 1fr', gap: '3rem', alignItems: 'flex-start' }}>
                {/* Sol Taraf: Fotoğraf ve İletişim */}
                <div style={{ position: 'sticky', top: '2rem' }}>
                    <div style={{ 
                        height: '420px', 
                        width: '100%', 
                        background: '#eee', 
                        borderRadius: 'var(--radius-lg)', 
                        marginBottom: '1.5rem',
                        backgroundImage: `url(${guide.profilePhotoUrl || 'https://via.placeholder.com/350x420?text=Rehber'})`,
                        backgroundSize: 'cover'
                    }} />
                    
                    <a href={guide.whatsAppUrl} target="_blank" rel="noopener noreferrer" 
                       className="btn-primary" 
                       style={{ 
                         width: '100%', 
                         textAlign: 'center', 
                         display: 'block', 
                         background: 'var(--color-whatsapp)', 
                         fontSize: '1.1rem',
                         padding: '1rem'
                       }}>
                        WhatsApp ile İletişime Geç
                    </a>
                </div>

                {/* Sağ Taraf: Bilgiler */}
                <div>
                   <h1 style={{ fontSize: '2.5rem', marginBottom: '0.5rem', color: 'var(--color-primary)' }}>{guide.fullName}</h1>
                   <p style={{ fontSize: '1.25rem', color: 'var(--color-text-secondary)', marginBottom: '1.5rem' }}>{guide.title}</p>
                   
                   <div style={{ display: 'flex', gap: '2rem', marginBottom: '2rem', borderBottom: '1px solid var(--color-border)', paddingBottom: '1.5rem' }}>
                        <div><strong style={{display:'block'}}>Deneyim</strong> {guide.yearsOfExperience} Yıl</div>
                        <div><strong style={{display:'block'}}>Puan</strong> ★ {guide.rating || '0.0'}</div>
                        <div><strong style={{display:'block'}}>Bölgeler</strong> {guide.districts?.join(', ') || 'İstanbul'}</div>
                   </div>

                   <section style={{ marginBottom: '2.5rem' }}>
                        <h3 style={{ marginBottom: '1rem' }}>Hakkımda</h3>
                        <p style={{ lineHeight: '1.8', color: 'var(--color-text-primary)' }}>{guide.bio}</p>
                   </section>

                   <section>
                        <h3 style={{ marginBottom: '1rem' }}>Uzmanlık ve Diller</h3>
                        <div style={{ display: 'flex', gap: '0.5rem', flexWrap: 'wrap' }}>
                            {guide.specialties?.map((s: any) => (
                                <span key={s} style={{ padding: '0.4rem 1rem', background: '#eef2ff', borderRadius: '20px', fontSize: '0.9rem', color:'#3b82f6' }}>{s}</span>
                            ))}
                            {guide.languages?.map((l: any) => (
                                <span key={l} style={{ padding: '0.4rem 1rem', background: '#fef2f2', borderRadius: '20px', fontSize: '0.9rem', color:'#ef4444' }}>{l}</span>
                            ))}
                        </div>
                   </section>

                   <hr style={{margin: '3rem 0', borderColor: 'var(--color-border)' }} />

                   {/* Yorum Yapma Bölümü */}
                   <section style={{ maxWidth: '600px' }}>
                       <h3 style={{ marginBottom: '1.5rem' }}>Deneyiminizi Paylaşın</h3>
                       <form onSubmit={(e) => {
                           e.preventDefault();
                           const fd = new FormData(e.currentTarget);
                           const data = {
                               guideId: guide.id,
                               reviewerName: fd.get('name'),
                               comment: fd.get('comment'),
                               rating: Number(fd.get('rating'))
                           };
                           api.post('/reviews', data)
                               .then(() => {
                                   alert('Yorumunuz onay için gönderildi!');
                                   window.location.reload();
                               })
                               .catch(() => alert('Hata oluştu!'));
                       }}>
                           <input name="name" type="text" placeholder="Adınız Soyadınız" required />
                           <select name="rating" required style={{ width: '100%', padding: '0.5rem', marginBottom: '1rem', border: '1px solid var(--color-border)', borderRadius: 'var(--radius-md)' }}>
                               <option value="5">★★★★★ (5 Yıldız)</option>
                               <option value="4">★★★★☆ (4 Yıldız)</option>
                               <option value="3">★★★☆☆ (3 Yıldız)</option>
                               <option value="2">★★☆☆☆ (2 Yıldız)</option>
                               <option value="1">★☆☆☆☆ (1 Yıldız)</option>
                           </select>
                           <textarea name="comment" rows={3} placeholder="Deneyiminiz nasıldı?" required style={{ width: '100%', padding: '0.5rem', marginBottom: '1rem', border: '1px solid var(--color-border)', borderRadius: 'var(--radius-md)' }} />
                           <button type="submit" className="btn-primary">Yorumu Gönder</button>
                       </form>
                   </section>
                </div>
            </div>
        </div>
    );
};

export default GuideDetail;
