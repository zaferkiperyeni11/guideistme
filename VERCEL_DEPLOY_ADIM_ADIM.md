# 🚀 Vercel'e Deploy Adım Adım

**ÖNEMLİ UYARI:** Vercel statik siteler ve Node.js/Next.js için optimize edilmiştir. .NET ASP.NET Core uygulaması için **Railway.app** veya **Render.com** daha iyidir. Fakat yine de deneyebilirsiniz.

---

## 📋 Ön Gereksinimler

- [ ] GitHub hesabı (https://github.com)
- [ ] Vercel hesabı (https://vercel.com)
- [ ] Projeyi GitHub'a yüklemiş olmalısınız
- [ ] Git komut satırı bilgisi

---

## 1️⃣ Adım 1: Projeyi GitHub'a Yüklemek

Eğer GitHub'a yüklemediyseniz:

```bash
# Proje klasörüne gir
cd c:\Users\Zafer\Downloads\guideistme

# Git başlat
git init

# Tüm dosyaları ekle
git add .

# İlk commit
git commit -m "IstGuide.me Başlangıç"

# GitHub'da yeni repo oluştur → https://github.com/new
# Adı: guideistme
# Açıklaması: Istanbul tour guide platform

# Remote ekle (USERNAME'i değiştir!)
git remote add origin https://github.com/USERNAME/guideistme.git

# Push et
git branch -M main
git push -u origin main
```

---

## 2️⃣ Adım 2: Vercel'e Giriş Yapmak

1. **https://vercel.com** aç
2. **"Sign Up"** butonuna tıkla
3. **"Continue with GitHub"** seç
4. GitHub ile yetkilendir
5. Hesap oluştur ve dashboard'a gel

---

## 3️⃣ Adım 3: Proje Import Etmek

Vercel dashboard'da gördüğün ekranda:

1. **"Add New..."** butonuna tıkla (sağ üst)
2. **"Project"** seç
3. **"Import Git Repository"** seç
4. GitHub'dan **"guideistme"** repo'sunu ara ve seç
5. **"Import"** butonuna bas

---

## 4️⃣ Adım 4: Proje Ayarlamak

Import edildikten sonra açılan sayfada:

### **A. Project Name**
- İsmi `guideistme` yap (veya istediğin isim)
- Alt domaininde bu kullanılacak

### **B. Build Settings**
Bu kısım ÖNEMLİ! .NET için düzenlemen lazım:

1. **Framework Preset:** None seçiliyse bırak
2. **Build Command:**
   ```
   dotnet publish -c Release -o ./out
   ```

3. **Output Directory:**
   ```
   ./out
   ```

4. **Install Command:**
   ```
   dotnet restore
   ```

---

## 5️⃣ Adım 5: Environment Variables Eklemek

Aynı sayfada aşağı kaydığında **"Environment Variables"** bölümü var:

**Eklenecek Değişkenler:**

```
ASPNETCORE_ENVIRONMENT = Production

ConnectionStrings__DefaultConnection = Server=your-db-server;Database=IstGuideDb;User Id=sa;Password=YourPassword;TrustServerCertificate=true;

JWT_SECRET = sizin-cok-guclu-gizli-anahtar-en-az-32-karakter

ASPNETCORE_URLS = http://+:80
```

> **Not:** `ConnectionStrings__DefaultConnection` database bağlantı string'ini güncelle!

---

## 6️⃣ Adım 6: Deploy Etmek

1. Tüm ayarları kontrol et
2. **"Deploy"** butonuna bas
3. Siyah ekranda logs akacak
4. 5-15 dakika bekle

---

## ⏳ Logs Ekranında Ne Görmelisin?

Başarılı deploy için şu adımları görmelisin:

```
✓ Downloaded files
✓ Restored packages with dotnet restore
✓ Building with dotnet publish -c Release
✓ Installing dependencies
✓ Starting application
✓ Deployment complete!

URL: https://guideistme.vercel.app
```

---

## ❌ Sorun Yaşarsanız

### Problem 1: "Build failed - Unexpected token"
**Sebep:** Vercel Node.js için tasarlanmıştır, .NET'i tanımıyor
**Çözüm:**
- `vercel.json` dosyası oluştur:

```json
{
  "buildCommand": "dotnet publish -c Release -o ./out",
  "outputDirectory": "./out",
  "framework": null,
  "installCommand": "dotnet restore"
}
```

### Problem 2: "502 Bad Gateway"
**Sebep:** Uygulama başlayamıyor
**Çözüm:**
- Logs'u kontrol et (Deployments → Log)
- Veritabanı bağlantı string'ini kontrol et
- Environment variables eksik mi kontrol et

### Problem 3: "Cannot find module"
**Sebep:** Dependency yüklenmedi
**Çözüm:**
```bash
# .csproj dosyasını kontrol et
# Vercel dashboard → Redeploy butonuna bas
```

---

## 🎯 Alternatif: Railway Kullanmak (DAHA İYİ)

Vercel sorun yaşarsan **Railway.app** kullan:

1. **https://railway.app** aç
2. **"Login with GitHub"** seç
3. **"New Project"** → **"Deploy from GitHub Repo"**
4. **guideistme** seç
5. 3 dakika sonra canlı! ✅

**Railway avantajları:**
- ✅ .NET tam destek
- ✅ PostgreSQL database built-in
- ✅ Daha güvenilir
- ✅ Türkiye'den hızlı

---

## ✅ Başarılı Deploy Belirtileri

Url açtığında:

- ✅ Swagger UI açılıyor → `https://guideistme.vercel.app/swagger`
- ✅ API endpoints çalışıyor → `https://guideistme.vercel.app/api/v1/guides`
- ✅ Login sayfası yükleniyor
- ✅ Veritabanı bağlantısı var

---

## 🚀 Özet Komutlar

```bash
# GitHub'a push et
git add .
git commit -m "Deploy hazırlığı"
git push origin main

# Vercel otomatik deploy eder!
# Vercel Dashboard → Deployments → Canlı URL'i gör
```

---

## 💡 Tavsiye

**Vercel yerine Railway kullan:**
- Daha kolay
- Daha hızlı
- Daha güvenilir
- Database dahil
- Fiyat: ₺100-250/ay (Vercel'le aynı)

**Railway Deploy:**
```
1. https://railway.app → Sign in with GitHub
2. "New Project" → "Deploy from GitHub repo"
3. Repo seç
4. Deploy et
5. Bitti! 🎉
```

---

*Sorularınız varsa bizimle iletişime geçin!* 😊
