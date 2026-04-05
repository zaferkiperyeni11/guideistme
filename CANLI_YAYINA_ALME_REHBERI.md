# 🚀 IstGuide.me Canlı Yayına Alma Rehberi

Bu rehber, IstGuide.me projesini internette canlı hale getirmek için adım adım talimatlar içerir.

---

## 📋 1. Gerekli Şeyler (Kontrol Listesi)

Başlamadan önce bunları hazır edin:

- [ ] Projeyi GitHub'a yüklemiş olmalısınız
- [ ] .NET 8 yüklü olmalı
- [ ] SQL Server veya PostgreSQL hazır olmalı
- [ ] Bir hosting hizmeti seçmeli
- [ ] Domain adı (bedava veya ücretli)
- [ ] SSL Sertifikası (çoğu hosting'de ücretsiz)

---

## 🌐 2. Domain Seçenekleri

### A. BEDAVA Subdomain Seçenekleri (İlk Başta Ideal)

#### 1. **Vercel** (Tam Bedava - En Kolay)
- **Özellikleri:**
  - Bedava domain: `istguide.vercel.app`
  - Hosted Git üzerinden otomatik deploy
  - GitHub ile bağlantı
  - SSL sertifikası dahil
  - 100 GB bandwidth/ayda

- **Adımlar:**
  1. https://vercel.com → Ücretsiz hesap oluştur
  2. GitHub hesabını bağla
  3. Projeyi seç
  4. Deploy et

- **Uyarı:** Vercel statik siteler ve Node.js için ideal. .NET için kısıtlı destek vardır.

---

#### 2. **Railway.app** (Bedava Deneme - .NET Uyumlu)
- **Özellikleri:**
  - İlk 5$ bedava kredi (yaklaşık 1 ay)
  - .NET ve SQL Server desteği
  - Otomatik deploy
  - Veritabanı incluided
  - URL: `istguide.up.railway.app`

- **Adımlar:**
  1. https://railway.app → Ücretsiz hesap oluştur
  2. GitHub ile bağlantı kur
  3. New Project → GitHub Repo seç
  4. Ayarları yapılandır ve deploy et

- **Ücret:** Bedava kredi bitiş sonrası aylık ~$5-20 arası

---

#### 3. **Render** (Bedava Tier - .NET Uyumlu)
- **Özellikleri:**
  - Bedava seviye: 750 saatlik compute/ay
  - .NET 8 desteği
  - PostgreSQL database dahil
  - Subdomain: `istguide.onrender.com`

- **Adımlar:**
  1. https://render.com → Hesap oluştur
  2. GitHub'ı bağla
  3. Web Service → Repo seçip deploy et

- **Uyarı:** Bedava seviyede ~15 dakika inaktivite sonrası site "uyur". İlk ziyarette 30 saniye yükleme olabilir.

---

#### 4. **Heroku Alternatifi - Railway + Render Kombinasyonu**
- Heroku artık bedava tier'ı kaldırdı
- **Railway veya Render öneriliyor**

---

#### 5. **Netlify** (Statik İçin - Frontend Parçası)
- Frontend'i Netlify'da, Backend'i başka yerde host edebilirsiniz
- Bedava: `istguide.netlify.app`
- Ama API backend gerekir (Vercel/Railway/Render'de)

---

### B. UCUZ Domain Seçenekleri (Sonra Satın Alabilirsiniz)

| Sağlayıcı | Fiyat | Özellikleri |
|-----------|-------|-----------|
| **Namecheap** | ₺15-30/yıl | Türkiye'de popüler, çok ucuz |
| **GoDaddy** | ₺20-50/yıl | Kolay UI, Türkçe destek |
| **.com.tr** | ₺150/yıl | Türk domain |
| **Porkbun** | ₺10-25/yıl | Çok ucuz, hoşlanıp UI |
| **Freenom** | **Bedava** | `.tk .ml .ga` (çok sınırlı) |

---

## 🏗️ 3. Hosting Seçenekleri (Backend)

### Önerilen: Railway.app (En İyi Fiyat/Performans)

**Neden Railway?**
- ✅ .NET tam destek
- ✅ PostgreSQL/SQL Server destek
- ✅ Bedava $5 kredi (1 ay yeterli)
- ✅ Sonrası ucuz ($5-20/ay)
- ✅ GitHub otomasyonu

**Railway'e Deploy Adımları:**

1. **GitHub'a Yüklemek:**
   ```bash
   git remote add origin https://github.com/your-username/guideistme.git
   git push -u origin main
   ```

2. **Railway'e Gitmek:**
   - https://railway.app → Sign in with GitHub
   - "New Project" → "Deploy from GitHub repo"
   - Repoyu seç

3. **Environment Variables Ayarlamak:**
   - Railway dashboard → Variables sekmesi
   - Ekle:
     ```
     ASPNETCORE_ENVIRONMENT=Production
     ConnectionStrings__DefaultConnection=postgresql://...
     JWT_SECRET=sizin-gizli-anahtariniz
     ```

4. **Database Oluşturmak:**
   - Railway → Add → PostgreSQL
   - Bağlantı string'i otomatik Environment'a eklenir

5. **Deploy Etmek:**
   - Railway otomatik olarak .csproj dosyasını bulur
   - `dotnet publish` çalıştırır
   - URL size verilir: `istguide.up.railway.app`

---

### Alternatif: Render.com

**Adımlar:**
1. https://render.com → Sign up
2. "New +" → "Web Service"
3. GitHub repoyu bağla
4. Build command: `dotnet publish -c Release`
5. Start command: `./bin/Release/net8.0/publish/IstGuide.API`
6. Environment variables ekle

---

## 📱 4. Frontend Hosting (React/Vue varsa)

Frontend ayrıca host edilebilir:

### Vercel (En Kolay - React için)
```bash
npm install -g vercel
vercel
```

### Netlify
```bash
npm install -g netlify-cli
netlify deploy
```

---

## 🔐 5. SSL Sertifikası (HTTPS)

**İyi haber:** Tüm hosting'ler bedava SSL verir!

- Railway → Otomatik SSL
- Render → Otomatik SSL
- Vercel → Otomatik SSL

Kendi domain'iniz varsa:
- Let's Encrypt (bedava) - Çoğu hosting'e integrated
- Cloudflare (bedava) - DNS üzerinden SSL

---

## 🌍 6. Custom Domain Bağlamak

Bedava subdomain yerine `istguide.com` gibi bir domain almak istiyorsanız:

### Adım Adım:

1. **Domain Satın Al:**
   - Namecheap, GoDaddy, Porkbun vb. sitelerden
   - Maliyeti: ₺15-50/yıl

2. **Railway/Render'e Bağla:**
   - Railway Dashboard → Settings → Custom Domain
   - `istguide.com` yazın
   - Verilen DNS records'ları kopyalayın

3. **Domain Sağlayıcıda DNS Ayarlaması:**
   - Namecheap → Domain → Manage → DNS
   - Railway'in verdiği CNAME ve A records'ı ekle
   - 5-30 dakika sonra aktif olur

4. **Sonuç:**
   - https://istguide.com → Railway'deki uygulamaya yönlendirilir

---

## 📊 7. Fiyat Karşılaştırması (Aylık)

| Çözüm | Fiyat | Notlar |
|-------|-------|--------|
| **Railway Bedava Kredi** | $0 | 1 ay |
| **Railway Standart** | $5-15 | 1-100 ziyaretçi/gün |
| **Render Bedava** | $0 | Uyku modu var |
| **Heroku Alternatifi** | $7+ | Artık yok |
| **Domain (Namecheap)** | ₺15/yıl | İsteğe bağlı |
| **Cloudflare DNS** | $0 | SSL + WAF |

**Toplam Başlangıç Maliyeti:** ₺0 - ₺50 (sadece domain istiyorsanız)

---

## 🚀 8. Hızlı Başlangıç (15 Dakika)

### En Kolay Yol: Render + Freenom

1. **Freenom'da Bedava Domain:**
   - https://www.freenom.com
   - `.tk` domain al (bedava)
   - Örn: `istguide.tk`

2. **Render'e Deploy:**
   - https://render.com
   - GitHub repoyu bağla
   - Deploy et (5 dakika)
   - URL: `istguide.onrender.com`

3. **Custom Domain:**
   - Render → Custom Domain → `istguide.tk` ekle
   - Freenom → Nameservers → Render'in verdiğini ekle
   - 10 dakika bekle

4. **Bitti!** 🎉
   - https://istguide.tk canlı!

---

## 📝 9. Pre-Launch Kontrol Listesi

Canlıya almadan önce:

- [ ] Veritabanı migration'ları çalıştırıldı
- [ ] Seed data yüklendi (örnek rehberler, diller vb.)
- [ ] Environment variables doğru set edildi
- [ ] JWT secret güçlü şifre mi?
- [ ] Email konfigürasyonu var mı?
- [ ] Hata logging'i aktif mı?
- [ ] SSL sertifikası aktif mı?
- [ ] Database backup ayarlandı mı?
- [ ] Test admin hesabı oluşturuldu mu?
- [ ] Sitede admin giriş test edildi mi?

---

## 🔄 10. Deployment Workflow (Sonraki Güncellemeler)

Kodu güncelledikten sonra:

```bash
# Yerel'de test et
dotnet test

# GitHub'a push et
git add .
git commit -m "Yeni feature"
git push origin main

# Railway otomatik deploy eder! 🚀
# Veya manuel olarak:
# Railway Dashboard → Redeploy
```

---

## 🛠️ 11. Sorunlar ve Çözümleri

### Problem: Database Connection Hatası
**Çözüm:**
```
Railway Dashboard → Variables → ConnectionString kontrol et
PostgreSQL service'in çalışıp çalışmadığını kontrol et
```

### Problem: Site Yüklenmediğinde
**Çözüm:**
```
Logs → Hata mesajını oku
dotnet publish çalışıp çalışmadığını kontrol et
Start command doğru mu kontrol et
```

### Problem: CORS Hatası
**Çözüm:**
```csharp
// Program.cs'de:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

app.UseCors("AllowAll");
```

### Problem: 502 Bad Gateway
**Çözüm:**
```
Uygulama başlamıyor demektir
Logs'ları kontrol et
Environment variables eksik olabilir
```

---

## 💡 12. İleri Seviye Seçenekler

### Kendi Sunucu (Biraz Pahalı)

| Sağlayıcı | Fiyat | Özellikleri |
|-----------|-------|-----------|
| **DigitalOcean** | $5/ay | VPS, full kontrol |
| **Linode** | $5/ay | Kolay UI |
| **AWS** | $0-100/ay | Karmaşık ama güçlü |
| **Azure** | $0-50/ay | Microsoft'tan |
| **Hetzner** | €3/ay | Avrupa'da ucuz |

**Avantajlar:** Tam kontrol, sınırsız özellik
**Dezavantajlar:** Kurulum zor, sistem yönetimi gerekli

---

## 📞 13. Canlıya Almadan Eğitim

Canlı almadan önce test edin:

```bash
# 1. Production build
dotnet build -c Release

# 2. Veritabanını production'a set et
set ASPNETCORE_ENVIRONMENT=Production

# 3. Lokal'de başlat
dotnet run --configuration Release

# 4. https://localhost:5001'de test et
```

---

## ✅ Tavsiye: Başında Yapılması Gerekenler

**Eğer hiç hosting bilgisi yoksa:**
1. ✅ Railway.app + Freenom.com'u dene (bedava)
2. ✅ 2 hafta ücretsiz test et
3. ✅ İşe yaradığını gördükten sonra domain satın al
4. ✅ Sonra Railway premium'a upgrade et (~$5-20/ay)

**Bütçe varsa:**
1. ✅ Namecheap'ten domain al (₺15/yıl)
2. ✅ Railway'e deploy et (~$5-20/ay)
3. ✅ Cloudflare'e bağla (bedava DNS + SSL)
4. ✅ Toplam: ₺80-300/ay

---

## 🎯 Ozet

| Seçenek | Bedava? | Zorluk | Tavsiye |
|---------|---------|---------|----------|
| **Railway + Freenom** | Evet (1 ay) | Kolay | ⭐ Başlanıç için |
| **Render** | Kısmen | Kolay | ⭐ Uzun vadede |
| **Vercel** | Evet | Orta | İyi ama .NET'e sınırlı |
| **DigitalOcean** | Hayır (~$60/yıl) | Zor | Profesyonel |
| **AWS/Azure** | Kısmen | Çok Zor | Enterprise |

---

## 🚀 Şimdi Başlamaya Hazır mısınız?

**30 saniyede başlayın:**
1. https://railway.app → GitHub ile kayıt ol
2. Repoyu seç
3. Deploy butonuna bas
4. 5 dakika sonra canlı! 🎉

**Sorularınız varsa:** admin@istguide.me

---

*Son Güncelleme: Nisan 2026*
*Hazırlayan: Antigravity AI Assistant*
