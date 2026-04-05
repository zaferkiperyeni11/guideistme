import { useState, useEffect } from 'react';
import api from '../services/api';

interface SelectOption {
    id: string;
    name: string;
}

const RegisterGuide: React.FC = () => {
    const [formData, setFormData] = useState({
        firstName: '', lastName: '', email: '', phoneNumber: '', bio: '', title: '',
        yearsOfExperience: 0, dateOfBirth: '',
        languageIds: [] as string[], specialtyIds: [] as string[], districtIds: [] as string[]
    });
    const [photo, setPhoto] = useState<File | null>(null);
    const [submitting, setSubmitting] = useState(false);
    const [languages, setLanguages] = useState<SelectOption[]>([]);
    const [specialties, setSpecialties] = useState<SelectOption[]>([]);
    const [districts, setDistricts] = useState<SelectOption[]>([]);

    useEffect(() => {
        const fetchOptions = async () => {
            try {
                const [langRes, specRes, distRes] = await Promise.all([
                    api.get('/languages'),
                    api.get('/specialties'),
                    api.get('/districts')
                ]);
                setLanguages(langRes.data || []);
                setSpecialties(specRes.data || []);
                setDistricts(distRes.data || []);
            } catch (err) {
                console.error('Seçenekler yüklenemedi:', err);
            }
        };
        fetchOptions();
    }, []);

    const toggleSelection = (field: 'languageIds' | 'specialtyIds' | 'districtIds', id: string) => {
        setFormData(prev => ({
            ...prev,
            [field]: prev[field].includes(id)
                ? prev[field].filter((x: string) => x !== id)
                : [...prev[field], id]
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setSubmitting(true);
        try {
            const payload = {
                ...formData,
                yearsOfExperience: Number(formData.yearsOfExperience),
                dateOfBirth: formData.dateOfBirth ? new Date(formData.dateOfBirth).toISOString() : null
            };
            const res = await api.post('/guides/register', payload);

            if (res.data?.succeeded && photo) {
                const guideId = res.data.value?.id || res.data.value;
                const fd = new FormData();
                fd.append('file', photo);
                await api.post(`/guides/${guideId}/upload-photo`, fd, {
                    headers: { 'Content-Type': 'multipart/form-data' }
                });
            }

            if (res.data?.succeeded) {
                alert("Başvurunuz başarıyla alındı! Admin onayı bekleniyor.");
                setFormData({
                    firstName: '', lastName: '', email: '', phoneNumber: '', bio: '', title: '',
                    yearsOfExperience: 0, dateOfBirth: '',
                    languageIds: [], specialtyIds: [], districtIds: []
                });
                setPhoto(null);
            } else {
                alert("Hata: " + (res.data?.errors?.join(', ') || 'Bilinmeyen hata'));
            }
        } catch (error: any) {
            console.error(error);
            const msg = error.response?.data?.errors?.join(', ') || "Kayıt sırasında bir hata oluştu.";
            alert(msg);
        } finally {
            setSubmitting(false);
        }
    };

    const chipStyle = (selected: boolean) => ({
        display: 'inline-block',
        padding: '0.4rem 0.8rem',
        margin: '0.2rem',
        borderRadius: '20px',
        border: selected ? '2px solid #2563eb' : '1px solid #d1d5db',
        backgroundColor: selected ? '#dbeafe' : '#f9fafb',
        color: selected ? '#1d4ed8' : '#374151',
        cursor: 'pointer',
        fontSize: '0.85rem',
        fontWeight: selected ? '600' : '400',
        transition: 'all 0.2s'
    });

    return (
        <div className="auth-container" style={{ maxWidth: '650px', margin: '3rem auto', padding: '0 1rem' }}>
            <h2 style={{ textAlign: 'center', marginBottom: '1.5rem', color: 'var(--color-primary, #2563eb)' }}>
                Rehber Başvuru Formu
            </h2>
            <form onSubmit={handleSubmit} className="card" style={{ padding: '2rem' }}>

                {/* Ad Soyad */}
                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1rem' }}>
                    <input type="text" placeholder="Adınız" value={formData.firstName}
                        onChange={e => setFormData({...formData, firstName: e.target.value})} required />
                    <input type="text" placeholder="Soyadınız" value={formData.lastName}
                        onChange={e => setFormData({...formData, lastName: e.target.value})} required />
                </div>

                {/* Email */}
                <input type="email" placeholder="Email Adresi" value={formData.email}
                    onChange={e => setFormData({...formData, email: e.target.value})} required
                    style={{ marginBottom: '1rem', width: '100%' }} />

                {/* Telefon */}
                <input type="text" placeholder="WhatsApp Numarası (+90...)" value={formData.phoneNumber}
                    onChange={e => setFormData({...formData, phoneNumber: e.target.value})} required
                    style={{ marginBottom: '1rem', width: '100%' }} />

                {/* Başlık */}
                <input type="text" placeholder="Profesyonel Ünvan (ör: Tarihi Yarımada Uzmanı)" value={formData.title}
                    onChange={e => setFormData({...formData, title: e.target.value})} required
                    style={{ marginBottom: '1rem', width: '100%' }} />

                {/* Doğum Tarihi ve Deneyim */}
                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1rem' }}>
                    <div>
                        <label style={{ display: 'block', marginBottom: '0.3rem', fontSize: '0.85rem', color: '#6b7280' }}>
                            Doğum Tarihi
                        </label>
                        <input type="date" value={formData.dateOfBirth}
                            onChange={e => setFormData({...formData, dateOfBirth: e.target.value})} required
                            style={{ width: '100%' }} />
                    </div>
                    <div>
                        <label style={{ display: 'block', marginBottom: '0.3rem', fontSize: '0.85rem', color: '#6b7280' }}>
                            Deneyim (Yıl)
                        </label>
                        <input type="number" min="0" max="60" value={formData.yearsOfExperience}
                            onChange={e => setFormData({...formData, yearsOfExperience: parseInt(e.target.value) || 0})}
                            style={{ width: '100%' }} />
                    </div>
                </div>

                {/* Dil Seçimi */}
                <div style={{ marginBottom: '1rem' }}>
                    <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: 'bold', fontSize: '0.9rem' }}>
                        Konuştuğunuz Diller *
                    </label>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '0.2rem' }}>
                        {languages.map(lang => (
                            <span key={lang.id} onClick={() => toggleSelection('languageIds', lang.id)}
                                style={chipStyle(formData.languageIds.includes(lang.id))}>
                                {lang.name}
                            </span>
                        ))}
                    </div>
                    {formData.languageIds.length === 0 && <small style={{ color: '#ef4444' }}>En az bir dil seçin</small>}
                </div>

                {/* Uzmanlık Seçimi */}
                <div style={{ marginBottom: '1rem' }}>
                    <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: 'bold', fontSize: '0.9rem' }}>
                        Uzmanlık Alanları *
                    </label>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '0.2rem' }}>
                        {specialties.map(spec => (
                            <span key={spec.id} onClick={() => toggleSelection('specialtyIds', spec.id)}
                                style={chipStyle(formData.specialtyIds.includes(spec.id))}>
                                {spec.name}
                            </span>
                        ))}
                    </div>
                    {formData.specialtyIds.length === 0 && <small style={{ color: '#ef4444' }}>En az bir uzmanlık seçin</small>}
                </div>

                {/* Bölge Seçimi */}
                <div style={{ marginBottom: '1rem' }}>
                    <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: 'bold', fontSize: '0.9rem' }}>
                        Hizmet Bölgeleri *
                    </label>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '0.2rem' }}>
                        {districts.map(dist => (
                            <span key={dist.id} onClick={() => toggleSelection('districtIds', dist.id)}
                                style={chipStyle(formData.districtIds.includes(dist.id))}>
                                {dist.name}
                            </span>
                        ))}
                    </div>
                    {formData.districtIds.length === 0 && <small style={{ color: '#ef4444' }}>En az bir bölge seçin</small>}
                </div>

                {/* Fotoğraf */}
                <div style={{ marginBottom: '1.5rem' }}>
                    <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: 'bold', fontSize: '0.9rem' }}>
                        Profil Fotoğrafı
                    </label>
                    <input type="file" accept="image/*"
                        onChange={e => setPhoto(e.target.files ? e.target.files[0] : null)}
                        style={{ padding: '0.5rem', border: '1px dashed #d1d5db', width: '100%', borderRadius: '8px' }} />
                </div>

                {/* Bio */}
                <textarea placeholder="Kendinizi ve deneyimlerinizi anlatın..."
                    rows={4} value={formData.bio}
                    onChange={e => setFormData({...formData, bio: e.target.value})} required
                    style={{ width: '100%', padding: '0.8rem', marginBottom: '1.5rem', borderRadius: '8px', border: '1px solid #d1d5db' }}>
                </textarea>

                {/* Gönder */}
                <button type="submit" disabled={submitting} className="btn-primary"
                    style={{ width: '100%', padding: '1rem', fontSize: '1rem' }}>
                    {submitting ? 'Gönderiliyor...' : 'Başvuruyu Tamamla'}
                </button>
            </form>
        </div>
    );
};

export default RegisterGuide;
