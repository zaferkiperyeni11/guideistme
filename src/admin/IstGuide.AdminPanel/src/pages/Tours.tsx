import { useEffect, useState } from 'react';
import api from '../services/api';
import axios from 'axios';

const Tours: React.FC = () => {
    const [tours, setTours] = useState<any[]>([]);
    const [guides, setGuides] = useState<any[]>([]);
    const [_loading, setLoading] = useState(true);
    const [newTour, setNewTour] = useState({ title: '', description: '', price: 0, duration: '', guideId: '' });

    const token = localStorage.getItem('admin_token');
    const authHeaders = { headers: { Authorization: `Bearer ${token}` } };

    useEffect(() => {
        api.get('/tours').then(res => {
            if (res.data) setTours(Array.isArray(res.data) ? res.data : (res.data.value || []));
            setLoading(false);
        }).catch(() => setLoading(false));

        axios.get('https://istguideme.runasp.net/api/admin/guides', authHeaders).then(res => {
            if (res.data) setGuides(Array.isArray(res.data) ? res.data : []);
        }).catch(() => {});
    }, []);

    const handleAddTour = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await api.post('/tours', newTour);
            alert('Tour added successfully!');
            window.location.reload();
        } catch (err) {
            alert('Error adding tour');
        }
    };

    return (
        <div>
            <h1>Tour Management</h1>

            <section className="card" style={{ marginBottom: '2rem' }}>
                <h3>Add New Tour</h3>
                <form onSubmit={handleAddTour} style={{ display: 'grid', gap: '1rem', marginTop: '1rem' }}>
                    <select value={newTour.guideId} onChange={e => setNewTour({...newTour, guideId: e.target.value})} required>
                        <option value="">-- Select Guide --</option>
                        {guides.map((g: any) => (
                            <option key={g.id} value={g.id}>{g.fullName}</option>
                        ))}
                    </select>
                    <input type="text" placeholder="Tour Title" onChange={e => setNewTour({...newTour, title: e.target.value})} required />
                    <textarea placeholder="Description" onChange={e => setNewTour({...newTour, description: e.target.value})} required />
                    <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
                        <input type="number" placeholder="Price ($)" onChange={e => setNewTour({...newTour, price: Number(e.target.value)})} required />
                        <input type="text" placeholder="Duration (e.g. 4 Hours)" onChange={e => setNewTour({...newTour, duration: e.target.value})} required />
                    </div>
                    <button type="submit" className="btn-primary">Save Tour</button>
                </form>
            </section>

            <section className="card">
                <h3>Existing Tours</h3>
                <table style={{ width: '100%', borderCollapse: 'collapse', marginTop: '1rem' }}>
                    <thead>
                        <tr style={{ textAlign: 'left', borderBottom: '1px solid #eee' }}>
                            <th style={{ padding: '0.5rem' }}>Title</th>
                            <th>Duration</th>
                            <th>Price</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tours.map(tour => (
                            <tr key={tour.id} style={{ borderBottom: '1px solid #fafafa' }}>
                                <td style={{ padding: '0.5rem' }}>{tour.title}</td>
                                <td>{tour.duration}</td>
                                <td>${tour.price}</td>
                                <td style={{ color: tour.isActive ? 'green' : 'red' }}>{tour.isActive ? 'Active' : 'Inactive'}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </section>
        </div>
    );
};

export default Tours;