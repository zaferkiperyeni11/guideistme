import axios from 'axios';

// Geliştirme ortamında (localhost) API'ye giden base url
const api = axios.create({
  baseURL: 'http://istguideme.runasp.net/api/v1',
  headers: {
    'Content-Type': 'application/json'
  }
});

// Eğer JWT Token kullanıyorsak isteklerin içine authorization header'ını otomatik yerleştiren kanca (interceptor)
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
