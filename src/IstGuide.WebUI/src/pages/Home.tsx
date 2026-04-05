import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../services/api';

const Home: React.FC = () => {
  const [popularDistricts, setPopularDistricts] = useState<any[]>([]);

  useEffect(() => {
    // Component yüklendiğinde popüler bölgeleri çek
    api.get('/districts/popular')
      .then(res => {
         if (res.data?.succeeded) {
            setPopularDistricts(res.data.value);
         }
      })
      .catch(err => console.error("Popüler bölgeler alınamadı", err));
  }, []);

  return (
    <div>
      {/* Hero Section */}
      <section className="hero" style={{ backgroundColor: '#fff', padding: '5rem 1rem', textAlign: 'center' }}>
        <h1 style={{ marginBottom: '1.2rem', lineHeight: '1.2', color: '#1e293b' }}>
          <span style={{ 
            fontSize: '1.8rem', 
            fontWeight: '300', 
            color: '#64748b', 
            display: 'block', 
            marginBottom: '0.3rem'
          }}>Don’t Just Visit Istanbul</span>
          <span style={{ 
            fontSize: '2.8rem', 
            fontWeight: '800', 
            color: 'var(--color-primary)', 
            letterSpacing: '-1px',
            textTransform: 'uppercase'
          }}>Live It !...</span>
        </h1>
        <p style={{ color: '#64748b', fontSize: '1.05rem', marginBottom: '2.5rem', maxWidth: '600px', margin: '0 auto 2.5rem auto', lineHeight: '1.6' }}>
          Skip the lines and discover hidden stories with the city’s most elite storytellers <br />
          Your journey curated by local experts
        </p>
        
        <div className="search-bar" style={{ maxWidth: '600px', margin: '0 auto', display: 'flex', gap: '0.5rem' }}>
          <input type="text" placeholder="Search by District, Expertise or Name..." style={{ margin: 0, flex: 1 }} />
          <Link to="/rehberler">
            <button className="btn-primary">Search</button>
          </Link>
        </div>
        
        {popularDistricts.length > 0 && (
          <div style={{ marginTop: '1.5rem', display: 'flex', gap: '1rem', justifyContent: 'center', flexWrap: 'wrap' }}>
             {popularDistricts.map(d => (
                <span key={d.slug} style={{ color: 'var(--color-primary)', cursor: 'pointer' }}>#{d.name}</span>
             ))}
          </div>
        )}
      </section>

      {/* Why Choose Us Section */}
      <section style={{ backgroundColor: '#fff', padding: '5rem 0' }}>
        <div className="container" style={{ maxWidth: '1200px', margin: '0 auto', textAlign: 'center', padding: '0 1rem' }}>
          <h2 style={{ fontSize: '2.5rem', marginBottom: '3rem', color: '#1e293b', fontWeight: '700' }}>Why to choose IstGuide</h2>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))', gap: '2rem' }}>
            <div className="card" style={{ padding: '2rem' }}>
              <div style={{ fontSize: '3rem', marginBottom: '1.5rem' }}>🏆</div>
              <h3>Certified Experts</h3>
              <p style={{ color: 'var(--color-text-secondary)' }}>All our guides are ministry-certified professionals with years of experience.</p>
            </div>
            <div className="card" style={{ padding: '2rem' }}>
              <div style={{ fontSize: '3rem', marginBottom: '1.5rem' }}>💬</div>
              <h3>Fluent Communication</h3>
              <p style={{ color: 'var(--color-text-secondary)' }}>Find guides speaking English, Arabic, Spanish, French, and many more.</p>
            </div>
            <div className="card" style={{ padding: '2rem' }}>
              <div style={{ fontSize: '3rem', marginBottom: '1.5rem' }}>🛡️</div>
              <h3>Secure & Trusted</h3>
              <p style={{ color: 'var(--color-text-secondary)' }}>We verify every guide to ensure you have a safe and wonderful experience.</p>
            </div>
          </div>
        </div>
      </section>

      {/* Tour Packages Section */}
      <section id="tours" style={{ backgroundColor: 'var(--color-surface)', padding: '5rem 1rem' }}>
        <div className="container" style={{ maxWidth: '1200px', margin: '0 auto' }}>
          <h2 style={{ fontSize: '2.5rem', marginBottom: '3rem', color: 'var(--color-text-primary)' }}>Best Experiences in Istanbul</h2>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))', gap: '2rem' }}>
             {[
               { id: 1, title: 'Old City & Hagia Sophia Heritage', slug: 'hagia-sophia-blue-mosque', duration: '4 Hours', price: 55, image: 'https://images.unsplash.com/photo-1541432901042-2d8bd64b4a9b?auto=format&fit=crop&w=600&q=80' },
               { id: 2, title: 'Private Bosphorus Sunset Yacht', slug: 'bosphorus-yacht-dolmabahce', duration: '3 Hours', price: 85, image: 'https://images.unsplash.com/photo-1524231757912-21f4fe3a7200?auto=format&fit=crop&w=600&q=80' },
               { id: 3, title: 'Grand Bazaar & Spice Market Adventure', slug: 'grand-bazaar-spice-market', duration: '3 Hours', price: 40, image: 'https://images.unsplash.com/photo-1615802266155-aa517904e908?auto=format&fit=crop&w=600&q=80' }
             ].map(tour => (
               <Link key={tour.id} to={`/tour/${tour.slug}`} style={{ textDecoration: 'none' }}>
                 <div className="card" style={{ padding: 0, overflow: 'hidden', transition: 'transform 0.3s' }} onMouseOver={e => e.currentTarget.style.transform = 'translateY(-10px)'} onMouseOut={e => e.currentTarget.style.transform = 'translateY(0)'}>
                    <div style={{ 
                        height: '200px', 
                        backgroundImage: `url(${tour.image})`,
                        backgroundSize: 'cover',
                        backgroundPosition: 'center'
                    }} />
                    <div style={{ padding: '1.5rem' }}>
                        <h3 style={{ marginBottom: '0.5rem', color: 'var(--color-text-primary)' }}>{tour.title}</h3>
                        <div style={{ display: 'flex', justifyContent: 'space-between', color: 'var(--color-text-secondary)', fontSize: '0.9rem' }}>
                            <span>⏱ {tour.duration}</span>
                            <span style={{ fontWeight: 'bold', color: 'var(--color-primary)' }}>From ${tour.price}</span>
                        </div>
                    </div>
                 </div>
               </Link>
             ))}
          </div>
        </div>
      </section>

      {/* Featured Guides Section */}
      <section className="featured-guides" style={{ padding: '4rem 1rem', maxWidth: '1200px', margin: '0 auto' }}>
        <h3 style={{ fontSize: 'var(--font-size-2xl)', marginBottom: '2rem' }}>Featured Guides</h3>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '2rem' }}>
          {/* Mock Guide */}
          <div className="guide-card" style={{ backgroundColor: 'var(--color-bg)', border: '1px solid var(--color-border)', borderRadius: 'var(--radius-lg)', padding: '1.5rem', boxShadow: 'var(--shadow-sm)' }}>
             <div style={{ display: 'flex', alignItems: 'center', marginBottom: '1rem' }}>
              <div style={{ width: '60px', height: '60px', borderRadius: '50%', backgroundColor: '#eee', marginRight: '1rem' }}></div>
              <div>
                <h4 style={{ margin: 0 }}>John Doe</h4>
                <p style={{ margin: 0, fontSize: 'var(--font-size-sm)', color: 'var(--color-text-muted)' }}>History Turlar, Architecture</p>
              </div>
            </div>
            <p style={{ color: 'var(--color-text-secondary)', fontSize: 'var(--font-size-sm)', marginBottom: '1rem' }}>Explore Istanbul's hidden gems with me.</p>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <span style={{ color: 'var(--color-star)', fontWeight: 'bold' }}>⭐ 4.9 (120 Reviews)</span>
              <Link to={`/rehberler`}>
                  <button style={{ padding: '0.5rem 1rem', background: 'transparent', color: 'var(--color-primary)', border: '1px solid var(--color-primary)', borderRadius: 'var(--radius-md)', cursor: 'pointer' }}>View Profile</button>
              </Link>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Home;
