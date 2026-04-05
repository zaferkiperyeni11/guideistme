import axios from 'axios';

const api = axios.create({
  baseURL: 'https://istguideme.runasp.net/api/v1',
  headers: {
    'Content-Type': 'application/json'
  }
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('admin_token');
  // Demo token ise sunucuya Bearer olarak geçme (sunucu reddedebilir)
  if (token && token !== 'demo_token_123') {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
