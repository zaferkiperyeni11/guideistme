# 🚀 Projeyi GitHub'a Yükleme Rehberi

Bu rehber, IstGuide.me projesini GitHub hesabınıza yüklemek için adım adım talimatlar içerir.

---

## 📋 Ön Gereksinimler

- [ ] GitHub hesabı (https://github.com)
- [ ] Git yüklü bilgisayarda (https://git-scm.com)
- [ ] Git ayarlanmış (isim ve email)

---

## 1️⃣ Adım 1: GitHub'da Yeni Repository Oluşturmak

1. **https://github.com** aç
2. Sağ üst köşede **"+"** butonuna tıkla
3. **"New repository"** seç
4. Açılan sayfada:

### Repository Ayarları:

**Repository name (Depo Adı):**
```
guideistme
```

**Description (Açıklama):**
```
Istanbul tour guide platform - CQRS architecture with .NET 8 and React
```

**Public/Private (Herkese Açık/Gizli):**
- Public seçin (bedava ve herkese açık)

**Initialize repository:**
- ✋ **SEÇMEYIN** - Boş başlayacağız

5. **"Create repository"** butonuna bas

---

## 2️⃣ Adım 2: Git'i Projeye Ayarlamak

Terminal/Command Prompt'u aç ve proje klasörüne git:

```bash
cd c:\Users\Zafer\Downloads\guideistme
```

Eğer zaten git başlatılmışsa (`.git` klasörü varsa), başlayabilirsin. Yoksa başlat:

```bash
git init
```

---

## 3️⃣ Adım 3: Git Konfigürasyonu (İlk Sefer)

Eğer daha önce yapmadıysan:

```bash
# Kendi adını ve e-postanı yaz
git config --global user.name "Zafer Kiperenyi"
git config --global user.email "senin@email.com"

# Kontrol et
git config --global user.name
git config --global user.email
```

---

## 4️⃣ Adım 4: Dosyaları Hazırlamak

### .gitignore Dosyası Oluşturmak (Önemli!)

Proje klasöründe `.gitignore` dosyası oluştur:

```bash
# Proje klasöründe
type nul > .gitignore
```

`.gitignore` dosyasına şunları ekle:

```
# .NET
bin/
obj/
.vs/
*.user
*.userprefs
*.log
appsettings.Development.json
appsettings.Local.json

# Node/React
node_modules/
dist/
.env
.env.local

# IDE
.vscode/
.idea/
*.swp
*.swo

# OS
.DS_Store
Thumbs.db

# Build
*.exe
*.dll
*.so
*.dylib
```

---

## 5️⃣ Adım 5: Dosyaları Staging'e Eklemek

```bash
# Tüm dosyaları ekle (gitignore tarafından hariç tutulacaklar haricinde)
git add .

# Veya sadece belirli dosyaları eklemek için:
git add src/
git add tests/
git add .gitignore
git add README.md
```

Neler eklenecek kontrol et:

```bash
git status
```

Yeşil renkteki dosyalar eklenecek, kırmızı renktekiler eklenmeyecek.

---

## 6️⃣ Adım 6: İlk Commit Yapma

```bash
git commit -m "Initial commit: IstGuide.me project setup"
```

Veya Türkçe:

```bash
git commit -m "İlk commit: IstGuide.me proje kurulumu"
```

---

## 7️⃣ Adım 7: GitHub'a Remote Bağlantısı Eklemek

GitHub'daki repository sayfasında kopyaladığın URL'i kullan:

```bash
# USERNAME'i kendi GitHub kullanıcı adınla değiştir
git remote add origin https://github.com/USERNAME/guideistme.git

# Kontrol et
git remote -v
```

---

## 8️⃣ Adım 8: Branch İsmini Değiştirmek (Opsiyonel)

GitHub artık `main` kullanıyor, eski `master` yerine:

```bash
git branch -M main
```

---

## 9️⃣ Adım 9: GitHub'a Push Etmek (Yükleme)

```bash
# İlk sefer -u flag'ı kullan (upstream bağlantısı kur)
git push -u origin main
```

**İlk sefer açılacak pencere:**
- GitHub giriş sayfası açılır
- Tarayıcıda "Authorize" butonuna bas
- Otomatik olarak tamamlanacak

**Eğer hata alırsan:** Personal Access Token kullan:
1. GitHub → Settings → Developer settings → Personal access tokens
2. Tokens (classic) → Generate new token
3. `repo` iznini seç
4. Token'ı kopyala
5. Terminal'de sorulan şifre yerine token'ı yapıştır

---

## 🔄 10: Sonraki Push'lar (Kolay)

Artık her değişiklikten sonra:

```bash
# Dosyaları kontrol et
git status

# Değişiklikleri ekle
git add .

# Commit yap
git commit -m "Açıklamanızı yazın"

# Push et
git push
```

---

## ✅ Başarılı Push Belirtileri

GitHub'daki sayfayı yenile, şunları göreceksin:
- ✅ Proje dosyaları görünecek
- ✅ Commit geçmişi görünecek
- ✅ Eğer README.md varsa, sayfada gösterilecek

---

## 📝 README.md Oluşturmak (Tavsiye)

Proje kök klasöründe `README.md` dosyası oluştur:

```markdown
# 🏛️ IstGuide.me

Istanbul tour guide booking platform with CQRS architecture.

## 🏗️ Architecture

- **Backend:** .NET 8 with ASP.NET Core
- **Frontend:** React with TypeScript
- **Database:** SQL Server
- **Pattern:** CQRS with MediatR
- **Testing:** xUnit and NSubstitute

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- SQL Server
- Node.js 18+

### Backend
```bash
cd src/IstGuide.API
dotnet run
```

### Frontend
```bash
cd frontend
npm install
npm start
```

## 📚 Documentation

- [Architecture Plan](./istguide-architecture-plan-UPDATED.md)
- [API Testing Guide](./API_TESTING_GUIDE.md)
- [Usage Guide (Turkish)](./kullanim_kilavuzu.md)
- [Simple Guide (Turkish)](./basitkilavuz.md)

## 🧪 Tests

```bash
# Run all tests
dotnet test

# Domain tests
dotnet test tests/IstGuide.Domain.Tests

# Application tests
dotnet test tests/IstGuide.Application.Tests
```

## 📄 License

MIT License

## ✉️ Contact

admin@istguide.me
```

---

## 🛑 Sorun Giderme

### Hata: "fatal: not a git repository"
```bash
# Çözüm: Git'i başlat
git init
```

### Hata: "fatal: could not read Username"
```bash
# Çözüm: SSH key ayarla veya Personal Access Token kullan
# SSH: https://docs.github.com/en/authentication/connecting-to-github-with-ssh
```

### Hata: "Updates were rejected"
```bash
# Çözüm: Remote'u kontrol et
git remote -v

# Yanlışsa düzelt
git remote remove origin
git remote add origin https://github.com/USERNAME/guideistme.git
```

### Dosyaları Silmek İstemiyorum Ama .gitignore'a Eklemek İstiyorum
```bash
# Dosyayı local'de tut ama Git'ten çıkar
git rm --cached appsettings.Local.json
git add .gitignore
git commit -m "Add local settings to gitignore"
git push
```

---

## 🎯 Özet Komutlar (Hızlı Referans)

```bash
# İlk kurulum
git init
git config --global user.name "Ad Soyad"
git config --global user.email "email@example.com"
git add .
git commit -m "Initial commit"
git branch -M main
git remote add origin https://github.com/USERNAME/guideistme.git
git push -u origin main

# Sonraki güncellemeler
git add .
git commit -m "Açıklamanız"
git push
```

---

## 📖 Faydalı Bağlantılar

- **Git Kurulumu:** https://git-scm.com/downloads
- **Git Türkçe Rehberi:** https://git-scm.com/book/tr/v2
- **GitHub Başlangıç:** https://docs.github.com/en/get-started
- **SSH Anahtarları:** https://docs.github.com/en/authentication/connecting-to-github-with-ssh
- **Personal Access Token:** https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token

---

*Son Güncelleme: Nisan 2026*
*Hazırlayan: Antigravity AI Assistant*
