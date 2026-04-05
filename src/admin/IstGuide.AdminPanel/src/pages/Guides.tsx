import React, { useEffect, useState } from 'react';
import api from '../services/api';

const Guides: React.FC = () => {
    const [guides, setGuides] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);
    const [newGuide, setNewGuide] = useState({ 
        firstName: '', 
        lastName: '', 
        email: '', 
        phoneNumber: '', 
        title: '', 
        bio: '',
        yearsOfExperience: 0
    });

    const fetchGuides = () => {
        setLoading(true);
        api.get('https://istguideme.runasp.net/api/admin/guides').then(res => {
            if (res.data) {
                setGuides(res.data);
            }
            setLoading(false);
        }).catch(() => setLoading(false));
    };

    useEffect(() => {
        fetchGuides();
    }, []);

    const handleAddGuide = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            // Using Register endpoints as a proxy for 'Add Guide' in admin
            // In a real app, admin might have a direct 'Create' endpoint
            await api.post('/guides/register', newGuide);
            alert('Guide added successfully!');
            fetchGuides();
            setNewGuide({ firstName: '', lastName: '', email: '', phoneNumber: '', title: '', bio: '', yearsOfExperience: 0 });
        } catch (err) {
            alert('Error adding guide');
        }
    };

    const handleDeleteGuide = async (id: string) => {
        if (!confirm('Are you sure you want to delete this guide?')) return;
        try {
            await api.delete(`https://istguideme.runasp.net/api/admin/guides/${id}`);
            alert('Guide deleted successfully!');
            fetchGuides();
        } catch (err) {
            alert('Error deleting guide');
        }
    };

    if (loading) return <div>Loading...</div>;

    return (
        <div>
            <h1>Guide Management</h1>
            
            <section className="card" style={{ marginBottom: '2rem' }}>
                <h3>Add New Guide</h3>
                <form onSubmit={handleAddGuide} style={{ display: 'grid', gap: '1rem', marginTop: '1rem' }}>
                    <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
                        <input type="text" placeholder="First Name" value={newGuide.firstName} onChange={e => setNewGuide({...newGuide, firstName: e.target.value})} required />
                        <input type="text" placeholder="Last Name" value={newGuide.lastName} onChange={e => setNewGuide({...newGuide, lastName: e.target.value})} required />
                    </div>
                    <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
                        <input type="email" placeholder="Email" value={newGuide.email} onChange={e => setNewGuide({...newGuide, email: e.target.value})} required />
                        <input type="text" placeholder="Phone" value={newGuide.phoneNumber} onChange={e => setNewGuide({...newGuide, phoneNumber: e.target.value})} required />
                    </div>
                    <input type="text" placeholder="Professional Title" value={newGuide.title} onChange={e => setNewGuide({...newGuide, title: e.target.value})} required />
                    <textarea placeholder="Bio" value={newGuide.bio} onChange={e => setNewGuide({...newGuide, bio: e.target.value})} required />
                    <input type="number" placeholder="Years of Experience" value={newGuide.yearsOfExperience} onChange={e => setNewGuide({...newGuide, yearsOfExperience: Number(e.target.value)})} required />
                    
                    <button type="submit" className="btn-primary">Save Guide</button>
                </form>
            </section>

            <section className="card">
                <h3>Existing Guides</h3>
                <table style={{ width: '100%', borderCollapse: 'collapse', marginTop: '1rem' }}>
                    <thead>
                        <tr style={{ textAlign: 'left', borderBottom: '1px solid #eee' }}>
                            <th style={{ padding: '0.5rem' }}>Name</th>
                            <th>Title</th>
                            <th>Email</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {guides.map(guide => (
                            <tr key={guide.id} style={{ borderBottom: '1px solid #fafafa' }}>
                                <td style={{ padding: '0.5rem' }}>{guide.fullName}</td>
                                <td>{guide.title}</td>
                                <td>{guide.email}</td>
                                <td>{guide.status}</td>
                                <td>
                                    <button onClick={() => handleDeleteGuide(guide.id)} style={{ color: 'red', background: 'none', border: 'none', cursor: 'pointer' }}>Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </section>
        </div>
    );
};

export default Guides;
