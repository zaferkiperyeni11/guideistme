import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Home from './pages/Home';
import GuideList from './pages/GuideList';
import GuideDetail from './pages/GuideDetail';
import RegisterGuide from './pages/RegisterGuide';
import PageDetail from './pages/PageDetail';
import TourDetail from './pages/TourDetail';
import './index.css';

function App() {
  return (
    <Router>
      <div className="app">
        {/* Tüm Sayfalarda Sabit Kalacak Header (Navbar) */}
        <header className="navbar" style={{ height: '80px', backgroundColor: 'transparent', position: 'relative', zIndex: '100' }}>
          <div className="container" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '1rem', maxWidth: '1200px', margin: '0 auto', position: 'relative' }}>
            <Link to="/" style={{ display: 'flex', alignItems: 'center', position: 'relative', width: '300px' }}>
                <img 
                    src="/logo.png" 
                    alt="IstGuide.me Logo" 
                    style={{ 
                        height: '360px', 
                        width: 'auto', 
                        objectFit: 'contain', 
                        marginLeft: '-25px', 
                        marginTop: '-140px', 
                        position: 'absolute', 
                        top: '0',
                        mixBlendMode: 'multiply', // BU SATIR BEYAZLIKLARI SİLER
                        zIndex: 100
                    }} 
                />
            </Link>
            <nav style={{ paddingRight: '1rem' }}>
              <a href="/#tours" style={{ marginRight: '1rem', color: 'var(--color-primary)', textDecoration: 'none', fontWeight: 'bold' }}>Tours</a>
              <Link to="/rehberler" style={{ marginRight: '1rem', color: 'var(--color-text-primary)' }}>Guides</Link>
              <Link to="/sayfa/hakkimizda" style={{ marginRight: '1rem', color: 'var(--color-text-primary)' }}>About Us</Link>
              <Link to="/rehber-ol" className="btn-primary">Become a Guide</Link>
            </nav>
          </div>
        </header>

        {/* Değişken Sayfa İçerikleri */}
        <main>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/rehberler" element={<GuideList />} />
            <Route path="/rehber/:slug" element={<GuideDetail />} />
            <Route path="/rehber-ol" element={<RegisterGuide />} />
            <Route path="/sayfa/:slug" element={<PageDetail />} />
            <Route path="/tour/:slug" element={<TourDetail />} />
          </Routes>
        </main>

        <footer style={{ 
          background: 'var(--color-surface)', 
          padding: '3rem 0', 
          marginTop: '5rem',
          borderTop: '1px solid var(--color-border)'
        }}>
          <div className="container" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', maxWidth: '1200px', margin: '0 auto', padding: '0 1rem' }}>
            <p style={{ color: 'var(--color-text-secondary)' }}>&copy; 2026 IstGuide.me</p>
            <div style={{ display: 'flex', gap: '1.5rem' }}>
              <Link to="/sayfa/hakkimizda" style={{ color: 'var(--color-text-secondary)', textDecoration: 'none' }}>About Us</Link>
              <Link to="/sayfa/gizlilik" style={{ color: 'var(--color-text-secondary)', textDecoration: 'none' }}>Privacy Policy</Link>
              <Link to="/sayfa/iletisim" style={{ color: 'var(--color-text-secondary)', textDecoration: 'none' }}>Contact</Link>
            </div>
          </div>
        </footer>

        {/* Floating WhatsApp Button */}
        <a 
          href="https://wa.me/905000000000?text=Hello, I need assistance with IstGuide.me" 
          target="_blank" 
          rel="noopener noreferrer"
          style={{
            position: 'fixed',
            bottom: '30px',
            right: '30px',
            background: 'linear-gradient(180deg, #25D366 0%, #128C7E 100%)',
            color: 'white',
            width: '90px',
            height: '90px',
            borderRadius: '50%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            boxShadow: '0 10px 25px rgba(18, 140, 126, 0.5), inset 0 2px 4px rgba(255, 255, 255, 0.4), inset 0 -4px 6px rgba(0, 0, 0, 0.2)',
            border: '1px solid rgba(255, 255, 255, 0.2)',
            zIndex: 1000,
            textDecoration: 'none',
            fontSize: '32px',
            transition: 'all 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275)'
          }}
          onMouseOver={e => e.currentTarget.style.transform = 'scale(1.1) translateY(-5px)'}
          onMouseOut={e => e.currentTarget.style.transform = 'scale(1) translateY(0)'}
          title="Contact Us on WhatsApp"
        >
          <svg width="50" height="50" viewBox="0 0 24 24" fill="currentColor">
            <path d="M12.031 6.172c-3.181 0-5.767 2.586-5.768 5.766-.001 1.298.38 2.27 1.019 3.284l-.582 2.128 2.182-.573c.978.58 1.911.928 3.145.929 3.178 0 5.767-2.587 5.768-5.766.001-3.187-2.575-5.768-5.764-5.768zm3.391 8.221c-.144.405-.837.774-1.17.824-.299.045-.677.063-1.092-.069-.252-.08-.575-.187-.988-.365-1.739-.751-2.874-2.502-2.961-2.617-.087-.116-.708-.94-.708-1.793s.448-1.273.607-1.446c.159-.173.346-.217.462-.217h.332c.106 0 .249-.04.39.298.144.347.491 1.2.534 1.287.043.087.072.188.014.304-.058.116-.087.188-.173.289l-.26.304c-.087.086-.177.18-.076.354.101.174.449.741.964 1.201.662.591 1.221.777 1.394.864.174.088.275.073.376-.043.101-.116.433-.506.548-.68.116-.173.231-.144.39-.087s1.011.477 1.184.564.289.13.332.202c.045.072.045.419-.1.824z"/>
          </svg>
        </a>
      </div>
    </Router>
  );
}

export default App;
