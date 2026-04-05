import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../services/api';

interface Guide {
  id: string;
  fullName: string;
  title: string;
  slug: string;
  profilePhotoUrl?: string;
  rating: number;
}

const GuideList: React.FC = () => {
  const [guides, setGuides] = useState<Guide[]>([]);
  const [districts, setDistricts] = useState<any[]>([]);
  const [languages, setLanguages] = useState<any[]>([]);
  
  const [loading, setLoading] = useState(true);
  const [filters, setFilters] = useState({ districtId: '', languageId: '', searchTerm: '' });

  const fetchData = () => {
    setLoading(true);
    // Filtreleri query string olarak hazırla
    const query = new URLSearchParams(filters as any).toString();
    api.get(`/guides?${query}`)
      .then(res => {
        if (res.data?.succeeded) setGuides(res.data.value);
        setLoading(false);
      })
      .catch(() => setLoading(false));
  };

  // İlk yüklemede bölgeleri ve dilleri seçenekler için çek
  useEffect(() => {
    api.get('/lookups/districts').then(res => setDistricts(res.data.value || []));
    api.get('/lookups/languages').then(res => setLanguages(res.data.value || []));
  }, []);

  // Filtreler her değiştiğinde listeyi güncelle
  useEffect(() => {
    const timer = setTimeout(() => fetchData(), 300);
    return () => clearTimeout(timer);
  }, [filters]);

  return (
    <div className="container" style={{ maxWidth: '1200px', margin: '2rem auto', padding: '0 1rem' }}>
      <header style={{ marginBottom: '3rem' }}>
        <h1 style={{ color: 'var(--color-primary)' }}>Explore Our Guides</h1>
        
        {/* Filter Bar */}
        <div className="card" style={{ 
            display: 'flex', 
            gap: '1rem', 
            marginTop: '1.5rem', 
            padding: '1.5rem',
            flexWrap: 'wrap',
            alignItems: 'center',
            backgroundColor: '#f8fafc'
        }}>
            <div style={{ flex: 1, minWidth: '200px' }}>
                <label style={{ fontSize: '0.8rem', fontWeight: 'bold', display: 'block', marginBottom: '0.3rem' }}>Search</label>
                <input 
                    type="text" 
                    placeholder="Name or title..." 
                    value={filters.searchTerm}
                    onChange={e => setFilters({...filters, searchTerm: e.target.value})}
                    style={{ margin: 0, width: '100%' }} 
                />
            </div>
            
            <div style={{ width: '200px' }}>
                <label style={{ fontSize: '0.8rem', fontWeight: 'bold', display: 'block', marginBottom: '0.3rem' }}>District</label>
                <select 
                    value={filters.districtId}
                    onChange={e => setFilters({...filters, districtId: e.target.value})}
                    style={{ width: '100%', padding: '0.5rem', borderRadius: '4px', border: '1px solid var(--color-border)' }}>
                    <option value="">All Districts</option>
                    {districts.map(d => <option key={d.id} value={d.id}>{d.name}</option>)}
                </select>
            </div>

            <div style={{ width: '200px' }}>
                <label style={{ fontSize: '0.8rem', fontWeight: 'bold', display: 'block', marginBottom: '0.3rem' }}>Language</label>
                <select 
                    value={filters.languageId}
                    onChange={e => setFilters({...filters, languageId: e.target.value})}
                    style={{ width: '100%', padding: '0.5rem', borderRadius: '4px', border: '1px solid var(--color-border)' }}>
                    <option value="">All Languages</option>
                    {languages.map(l => <option key={l.id} value={l.id}>{l.name}</option>)}
                </select>
            </div>
             
            <button 
                onClick={() => setFilters({ districtId: '', languageId: '', searchTerm: '' })}
                style={{ background: 'none', border: 'none', color: 'var(--color-error)', cursor: 'pointer', fontSize: '0.9rem' }}>
                Clear
            </button>
        </div>
      </header>

      {loading ? (
        <div style={{ textAlign: 'center', padding: '5rem' }}>Loading results...</div>
      ) : (
        <div className="grid-guides" style={{ 
          display: 'grid', 
          gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))', 
          gap: '1.5rem' 
        }}>
          {guides.map(guide => (
            <Link key={guide.id} to={`/rehber/${guide.slug}`} style={{ textDecoration: 'none' }}>
              <div className="card" style={{ padding: '1rem', transition: 'transform 0.2s' }}>
                <div style={{ 
                  height: '240px', 
                  background: '#eee', 
                  borderRadius: '8px', 
                  marginBottom: '1rem',
                  backgroundImage: `url(${guide.profilePhotoUrl || 'https://via.placeholder.com/280x240?text=Guide'})`,
                  backgroundSize: 'cover'
                }} />
                <h3 style={{ margin: '0.25rem 0', color: 'var(--color-text-primary)' }}>{guide.fullName}</h3>
                <p style={{ fontSize: '0.9rem', color: 'var(--color-text-secondary)', marginBottom: '0.5rem' }}>{guide.title}</p>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                  <span style={{ color: '#FFB400' }}>★ {guide.rating || '0.0'}</span>
                </div>
              </div>
            </Link>
          ))}
        </div>
      )}
      
      {!loading && guides.length === 0 && (
        <div style={{ textAlign: 'center', padding: '5rem', border: '1px dashed var(--color-border)' }}>
          No guides found matching the criteria.
        </div>
      )}
    </div>
  );
};

export default GuideList;
