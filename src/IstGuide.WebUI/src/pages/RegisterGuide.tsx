import React, { useState } from 'react';
import api from '../services/api';

const RegisterGuide: React.FC = () => {
    const [formData, setFormData] = useState({
        firstName: '', lastName: '', email: '', phoneNumber: '', bio: '', title: 'Profesyonel Rehber'
    });
    const [photo, setPhoto] = useState<File | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setSubmitting(true);
        try {
            // 1. Önce rehber bilgilerini kaydet
            const res = await api.post('/guides/register', formData);
            
            if (res.data?.succeeded && photo) {
                const guideId = res.data.value.id;
                // 2. Eğer kayıt başarılıysa ve fotoğraf seçildiyse fotoğrafı yükle
                const fd = new FormData();
                fd.append('file', photo);
                await api.post(`/guides/${guideId}/upload-photo`, fd, {
                    headers: { 'Content-Type': 'multipart/form-data' }
                });
            }
            
            alert("Başvurunuz ve fotoğrafınız başarıyla alındı! Admin onayı bekleniyor.");
            setSubmitting(false);
        } catch (error) {
            console.error(error);
            alert("Kayıt sırasında bir hata oluştu.");
            setSubmitting(false);
        }
    };

    return (
        <div className="auth-container" style={{ maxWidth: '600px', margin: '3rem auto' }}>
            <h2 style={{ textAlign: 'center', marginBottom: '1.5rem', color: 'var(--color-primary)' }}>Guide Application Form</h2>
            <form onSubmit={handleSubmit} className="card" style={{ padding: '2rem' }}>
                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1rem' }}>
                    <input type="text" placeholder="First Name" value={formData.firstName} onChange={e => setFormData({...formData, firstName: e.target.value})} required />
                    <input type="text" placeholder="Last Name" value={formData.lastName} onChange={e => setFormData({...formData, lastName: e.target.value})} required />
                </div>
                
                <input type="email" placeholder="Email Address" value={formData.email} onChange={e => setFormData({...formData, email: e.target.value})} required style={{ marginBottom: '1rem', width: '100%' }} />
                <input type="text" placeholder="WhatsApp Number (e.g. +90...)" value={formData.phoneNumber} onChange={e => setFormData({...formData, phoneNumber: e.target.value})} required style={{ marginBottom: '1rem', width: '100%' }} />
                
                <div style={{ marginBottom: '1.5rem' }}>
                    <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: 'bold' }}>Profile Photo</label>
                    <input 
                        type="file" 
                        accept="image/*"
                        onChange={e => setPhoto(e.target.files ? e.target.files[0] : null)}
                        style={{ padding: '0.5rem', border: '1px dashed var(--color-border)', width: '100%' }}
                    />
                </div>

                <textarea 
                    placeholder="Tell us about your experiences and expertise..." 
                    rows={5}
                    value={formData.bio}
                    onChange={e => setFormData({...formData, bio: e.target.value})}
                    required
                    style={{ width: '100%', padding: '0.8rem', marginBottom: '1.5rem', borderRadius: 'var(--radius-md)', border: '1px solid var(--color-border)' }}>
                </textarea>

                <button type="submit" disabled={submitting} className="btn-primary" style={{ width: '100%', padding: '1rem' }}>
                    {submitting ? 'Submitting...' : 'Complete Application'}
                </button>
            </form>
        </div>
    );
};

export default RegisterGuide;
