import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import Dashboard from './pages/Dashboard';
import Approvals from './pages/Approvals';
import CmsPages from './pages/CmsPages';
import Tours from './pages/Tours';
import Reviews from './pages/Reviews';
import Guides from './pages/Guides';
import Login from './pages/Login';
import './index.css';

function App() {
  const [token, setToken] = useState(localStorage.getItem('admin_token'));

  const handleLogin = (newToken: string) => {
    localStorage.setItem('admin_token', newToken);
    setToken(newToken);
  };

  const handleLogout = () => {
    localStorage.removeItem('admin_token');
    setToken(null);
  };

  if (!token) {
    return <Login onLogin={handleLogin} />;
  }

  return (
    <Router>
      <div className="admin-layout">
        <aside className="sidebar">
          <div className="sidebar-header">
            IstGuide Admin
          </div>
          <nav className="sidebar-nav">
            <Link to="/">Dashboard</Link>
            <Link to="/onaylar">Guide Approvals</Link>
            <Link to="/guides">Guide Management</Link>
            <Link to="/tours">Tour Management</Link>
            <Link to="/yorumlar">Review Management</Link>
            <a href="#">Messages</a>
            <Link to="/cms">CMS & Pages</Link>
            
            <button 
              onClick={handleLogout}
              style={{ 
                marginTop: 'auto', 
                background: 'transparent', 
                color: '#ef4444', 
                border: 'none', 
                padding: '1.5rem', 
                cursor: 'pointer',
                textAlign: 'left'
              }}>
              Logout
            </button>
          </nav>
        </aside>

        <main className="main-content">
          <header className="topbar">
            <span>Admin Control Panel</span>
          </header>
          
          <div className="content-area">
            <Routes>
              <Route path="/" element={<Dashboard />} />
              <Route path="/onaylar" element={<Approvals />} />
              <Route path="/yorumlar" element={<Reviews />} />
              <Route path="/cms" element={<CmsPages />} />
            <Route path="/tours" element={<Tours />} />
            <Route path="/guides" element={<Guides />} />
              <Route path="*" element={<Navigate to="/" />} />
            </Routes>
          </div>
        </main>
      </div>
    </Router>
  );
}

export default App;
