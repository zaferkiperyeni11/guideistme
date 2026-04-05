# 📖 IstGuide.me Kullanım ve İşletme Rehberi

Bu kılavuz, IstGuide.me portalını yönetmeniz ve iş akışlarını takip etmeniz için hazırlanmıştır.

---

### 1. Sitenin Genel Mantığı
Siteniz, İstanbul'u gezmek isteyen turistler ile uzman rehberleri ve özel turları buluşturan bir köprüdür.
*   **Turistler:** Sitedeki rehberleri bulur, profillerini inceler ve WhatsApp üzerinden doğrudan iletişime geçer.
*   **Rehberler:** Sitenize gelip başvuru yapar, siz onay verirseniz sistemde görünürler. Kendi profillerini ve turlarını yönetebilirler.
*   **Siz (Yönetici):** Kimlerin rehber olacağına karar verir, turları yönetir, yorumları denetler ve site içeriğini kontrol edersiniz.

---

### 2. Yönetim ve İşlem Yöntemleri

#### Seçenek A: Swagger UI (Önerilen)
Admin veya Rehber rolü olan kullanıcılar Swagger arayüzü üzerinden işlemlerini yönetebilir:
1.  Tarayıcıda `http://localhost:5217/swagger` adresine gidin.
2.  Sağ üstteki **Authorize** butonuna tıklayın.
3.  Giriş yaptığınızda aldığınız JWT token'ı `Bearer {token}` formatında yapıştırın.
4.  Artık tüm endpoint'leri arayüz üzerinden test edebilirsiniz.

#### Seçenek B: cURL Komutları
Terminal/Komut satırından API'ye doğrudan istek gönderin:
```bash
# Rehberleri listele
curl -X GET "http://localhost:5217/api/v1/guides"

# JWT token ile korunan işlem (Admin)
curl -X GET "http://localhost:5217/api/admin/guides" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Yeni tur oluştur
curl -X POST "http://localhost:5217/api/v1/tours" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "guideId": "550e8400-e29b-41d4-a716-446655440000",
    "title": "Sultanahmet Yürüyüş Turu",
    "description": "Tarih rehberi ile 4 saatlik tur",
    "price": 99.99,
    "duration": "4 Saat"
  }'
```

#### Seçenek C: Postman Collection
Postman uygulaması içinden önceden hazırlanmış istek şablonlarını kullanın:
1. Postman'i açın ve `Import` seçeneğine tıklayın.
2. `IstGuide_API_Collection.postman_collection.json` dosyasını seçin.
3. Environment ayarlarında API adresi ve token'ı yapılandırın.
4. Hazır şablonları kullanarak istek gönderin.

> **Not:** Admin paneli (Web UI) şu anda geliştirme aşamasındadır. En güvenilir yöntem Swagger UI veya cURL komutlarını kullanmaktır.

---

### 3. Sistemde Giriş Yapmak

Çoğu işlem için JWT token ile yetkilendirme gereklidir. Giriş yapmak için:

```bash
# Admin Giriş
curl -X POST "http://localhost:5217/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "AdminPassword123!"
  }'

# Rehber Giriş
curl -X POST "http://localhost:5217/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "guide@example.com",
    "password": "GuidePassword123!"
  }'
```

**Giriş Yanıtı (başarılı):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "...",
  "expiresIn": 3600,
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "email": "admin@example.com",
  "roles": ["Admin"]
}
```

Bu `accessToken`'ı sonraki tüm isteklerde kullanın:
```bash
curl -X GET "http://localhost:5217/api/admin/guides" \
  -H "Authorization: Bearer {accessToken}"
```

---

### 4. İş Akışları (Adım Adım)

#### A. Yeni Bir Rehber Başvurusu Geldiğinde
Bir rehber sitenizden form doldurduğunda şu yolu izleyin:

**Swagger UI ile:**
1.  `http://localhost:5217/swagger` adresine gidin.
2.  Admin token'ınızla Authorize olun.
3.  `/api/admin/guides` endpoint'ine GET isteği gönderin.
4.  Beklemede olan (`Pending`) rehberleri göreceksiniz.

**cURL ile:**
```bash
# Tüm rehberleri listele
curl -X GET "http://localhost:5217/api/admin/guides" \
  -H "Authorization: Bearer {accessToken}"

# Pending durumundaki rehberleri filtrele
curl -X GET "http://localhost:5217/api/admin/guides?status=Pending" \
  -H "Authorization: Bearer {accessToken}"

# Rehberi onayla
curl -X PUT "http://localhost:5217/api/admin/guides/{guideId}/approve" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json"

# Rehberi reddet (sebep belirtilerek)
curl -X PUT "http://localhost:5217/api/admin/guides/{guideId}/reject" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{"reason": "Yetersiz belgeler sağlanmıştır."}'

# Rehberi askıya al
curl -X PUT "http://localhost:5217/api/admin/guides/{guideId}/suspend" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json"

# Rehberi öne çıkar
curl -X PUT "http://localhost:5217/api/admin/guides/{guideId}/toggle-featured" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json"
```

**Rehber Durumları:**
- **Pending:** Yeni başvuru, onay beklemede
- **Approved:** Onaylı, sitede görebilir
- **Rejected:** Reddedilmiş
- **Suspended:** Geçici olarak devre dışı bırakılmış

#### B. Yeni Bir Tur Paketi Eklemek ve Yönetmek
Müşterilerinize özel turlar sunmak için:

**API ile (Swagger üzerinden):**

**Tur Oluşturma:**
1.  `POST /api/v1/tours` — Yeni tur oluştur
    *   **GuideId:** Turu verecek rehberin ID'si (ZORUNLU)
    *   **Title:** Turun adı (Örn: Sultanahmet Yürüyüş Turu) - Max 200 karakter
    *   **Description:** Turun içeriğini anlatan açıklama - Max 2000 karakter
    *   **Price:** Turun fiyatı (₺) - 0'dan büyük olmalıdır
    *   **Duration:** Ne kadar süreceği (Örn: 4 Saat)
    *   **ImageUrl:** (Opsiyonel) Tur görseli
    *   **DistrictId:** (Opsiyonel) Turun yapıldığı bölge

    Örnek istek:
    ```json
    {
      "guideId": "550e8400-e29b-41d4-a716-446655440000",
      "title": "Sultanahmet Sarayları Turu",
      "description": "Topkapı Sarayı ve Dolmabahçe Sarayını içeren dönem rehberli tur.",
      "price": 99.99,
      "duration": "4 Saat"
    }
    ```

**Tur Listeleme ve Detay:**
2.  `GET /api/v1/tours` — Tüm turları listele
3.  `GET /api/v1/tours/{slug}` — Tur detaylarını getir
4.  `GET /api/v1/tours/guide/{guideId}` — Belirli rehberin turlarını listele
5.  `GET /api/v1/tours/featured` — Öne çıkan turları listele
6.  `GET /api/v1/tours/active` — Aktif turları listele

**Tur Güncelleme:**
7.  `PUT /api/v1/tours/{id}` — Tur bilgilerini güncelle
    *   **Title, Description, Price, Duration, ImageUrl, DistrictId** güncellenir
    *   **IsActive:** Turun aktif olup olmadığını kontrol eder

**Tur Silme:**
8.  `DELETE /api/v1/tours/{id}` — Turu sil (soft delete)

> **Önemli:**
> - Her tur bir rehberle ilişkilendirilmek zorundadır. Tur oluştururken `GuideId` alanını doldurmayı unutmayın.
> - Turlar otomatik olarak `IsActive: true` durumda oluşturulur.
> - Slug otomatik olarak tur başlığından oluşturulur.
> - Soft delete kullanılır (turlar tamamen silinmez, sadece IsDeleted bayrağı işaretlenir).

#### C. Gelen Yorumları Denetlemek
Turistler rehberler hakkında yorum yapabilir. Kötü içerikli yorumları engellemek için:

**cURL ile:**
```bash
# Bekleyen yorumları getir
curl -X GET "http://localhost:5217/api/admin/reviews/pending" \
  -H "Authorization: Bearer {accessToken}"

# Yorumu onayla
curl -X PUT "http://localhost:5217/api/admin/reviews/{reviewId}/approve" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json"

# Yorumu reddet
curl -X PUT "http://localhost:5217/api/admin/reviews/{reviewId}/reject" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json"

# Yoruma yönetici cevabı ver
curl -X PUT "http://localhost:5217/api/admin/reviews/{reviewId}/reply" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{"replyText": "Teşekkür ederiz, geribildiriminiz bizim için çok değerli."}'
```

> **Not:** Siz onay vermedikçe hiçbir yorum sitede görünmez. Sistem tarafından otomatik olarak `Pending` durumunda tutulurlar.

#### D. İletişim Taleplerini Yönetmek
Ziyaretçiler rehberlere mesaj gönderebilir:

**cURL ile:**
```bash
# Tüm talepleri listele
curl -X GET "http://localhost:5217/api/admin/contact-requests" \
  -H "Authorization: Bearer {accessToken}"

# İstatistikleri gör
curl -X GET "http://localhost:5217/api/admin/contact-requests/stats" \
  -H "Authorization: Bearer {accessToken}"
```

#### E. Bilgi Sayfalarını Düzenlemek (CMS & Pages)
"Hakkımızda" veya "Gizlilik Politikası" gibi sayfaları güncellemek için:

**cURL ile:**
```bash
# Mevcut sayfa içeriğini getir
curl -X GET "http://localhost:5217/api/admin/pages/about-us" \
  -H "Authorization: Bearer {accessToken}"

# Sayfa içeriğini güncelle
curl -X PUT "http://localhost:5217/api/admin/pages/about-us" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "IstGuide.me, İstanbul turizminin kalbindedir...",
    "isPublished": true
  }'
```

**Varsayılan Sayfa Anahtarları:**
- `about-us` — Hakkımızda
- `privacy` — Gizlilik Politikası
- `terms` — Kullanım Şartları
- `contact` — İletişim Bilgileri

#### F. Bölgeleri ve Uzmanlık Alanlarını Yönetmek

**cURL ile:**
```bash
# Tüm bölgeleri listele
curl -X GET "http://localhost:5217/api/v1/districts"

# Yeni bölge ekle
curl -X POST "http://localhost:5217/api/admin/districts" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{"name": "Beyoğlu", "description": "Modern İstanbul"}'

# Bölgeyi güncelle
curl -X PUT "http://localhost:5217/api/admin/districts/{districtId}" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{"name": "Beyoğlu", "description": "Güncel açıklama"}'

# Tüm uzmanlık alanlarını listele
curl -X GET "http://localhost:5217/api/v1/specialties"

# Yeni uzmanlık alanı ekle
curl -X POST "http://localhost:5217/api/admin/specialties" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{"name": "Osmanlı Mimarisi", "description": "Osmanlı dönemi yapıları"}'

# Uzmanlık alanını güncelle
curl -X PUT "http://localhost:5217/api/admin/specialties/{specialtyId}" \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{"name": "Osmanlı Mimarisi", "description": "Güncel açıklama"}'
```

---

### 5. Dashboard İstatistikleri
Genel durumu bir bakışta görmek için:

**cURL ile:**
```bash
curl -X GET "http://localhost:5217/api/admin/dashboard/stats" \
  -H "Authorization: Bearer {accessToken}"
```

**Yanıt Örneği:**
```json
{
  "totalGuides": 45,
  "pendingGuidesCount": 5,
  "activeGuidesCount": 40,
  "totalToursCount": 120,
  "activeToursCount": 115,
  "pendingReviewsCount": 8,
  "approvedReviewsCount": 245,
  "totalContactRequests": 150,
  "pendingContactRequests": 12
}
```

---

### 6. Sitedeki Butonlar Ne İşe Yarar? (Ziyaretçi Gözüyle)

*   **Guides:** Tüm kayıtlı ve onaylı rehberleri gösteren katalog sayfasıdır. Dil, bölge ve uzmanlık alanına göre filtreleme yapılabilir.
*   **Become a Guide:** Yeni rehberlerin başvuru yapması için tasarlanan formdur.
*   **WhatsApp Butonu:** Ziyaretçinin doğrudan rehberle veya destek hattıyla konuşmasını sağlar.
*   **Tours:** Rehberlere ait tur paketlerini listeler.

---

### 7. Veritabanı ve Teknik Bilgiler

*   **Veritabanı:** SQL Server 2022 (localhost)
*   **Veritabanı Adı:** IstGuideDb
*   **API Adresi:** `http://localhost:5217`
*   **Swagger:** `http://localhost:5217/swagger`
*   **Seed Data:** Sistem ilk açıldığında otomatik olarak 12 dil, 10 uzmanlık alanı, 15 İstanbul ilçesi ve 1 örnek rehber eklenir.

---

### 8. API Endpoint Özeti (Referans)

#### 8.1. Public Endpoints (Yönetim Gerekmez)

| Endpoint | Yöntem | Açıklama |
|----------|--------|----------|
| `/api/v1/guides` | GET | Rehber ara (filtreleme + sayfalama) |
| `/api/v1/guides/{slug}` | GET | Rehber detay profili |
| `/api/v1/guides/featured` | GET | Öne çıkan rehberler |
| `/api/v1/guides/register` | POST | Yeni rehber başvurusu |
| `/api/v1/guides/{guideId}/reviews` | GET | Rehber yorumları |
| `/api/v1/guides/{guideId}/reviews` | POST | Rehbere yorum gönder |
| `/api/v1/tours` | GET | Tüm turları listele |
| `/api/v1/tours/{slug}` | GET | Tur detaylarını getir |
| `/api/v1/tours/guide/{guideId}` | GET | Rehberin turlarını listele |
| `/api/v1/tours/featured` | GET | Öne çıkan turları listele |
| `/api/v1/tours/active` | GET | Aktif turları listele |
| `/api/v1/contact-requests` | POST | İletişim talebi oluştur |
| `/api/v1/districts` | GET | Tüm ilçeler |
| `/api/v1/languages` | GET | Tüm diller |
| `/api/v1/specialties` | GET | Tüm uzmanlık alanları |
| `/api/v1/pages/{key}` | GET | Statik sayfa içeriği |

#### 8.2. Rehber Yönetim Endpoints (Rehber Giriş Gerekli)

| Endpoint | Yöntem | Açıklama |
|----------|--------|----------|
| `/api/v1/guides/{id}` | PUT | Rehber profilini güncelle |
| `/api/v1/guides/{id}` | DELETE | Rehber profilini sil (soft delete) |
| `/api/v1/tours` | POST | Yeni tur oluştur |
| `/api/v1/tours/{id}` | PUT | Tur bilgilerini güncelle |
| `/api/v1/tours/{id}` | DELETE | Turu sil (soft delete) |

#### 8.3. Admin Endpoints (Admin Giriş Gerekli)

| Endpoint | Yöntem | Açıklama |
|----------|--------|----------|
| `/api/admin/guides` | GET | Tüm rehberleri listele (status filtresi) |
| `/api/admin/guides/{id}/approve` | PUT | Rehberi onayla |
| `/api/admin/guides/{id}/reject` | PUT | Rehberi reddet |
| `/api/admin/guides/{id}/suspend` | PUT | Rehberi askıya al |
| `/api/admin/guides/{id}/toggle-featured` | PUT | Rehberi öne çıkar/kaldır |
| `/api/admin/guides/{id}` | DELETE | Rehberi sil |
| `/api/admin/reviews/pending` | GET | Bekleyen yorumları getir |
| `/api/admin/reviews/{id}/approve` | PUT | Yorumu onayla |
| `/api/admin/reviews/{id}/reject` | PUT | Yorumu reddet |
| `/api/admin/reviews/{id}/reply` | PUT | Yoruma yönetici cevabı ver |
| `/api/admin/contact-requests` | GET | Tüm talepleri listele |
| `/api/admin/contact-requests/stats` | GET | İstatistikleri gör |
| `/api/admin/pages/{key}` | GET | Sayfa içeriğini getir |
| `/api/admin/pages/{key}` | PUT | Sayfa içeriğini güncelle |
| `/api/admin/districts` | POST | Yeni ilçe ekle |
| `/api/admin/districts/{id}` | PUT | İlçeyi güncelle |
| `/api/admin/specialties` | POST | Yeni uzmanlık alanı ekle |
| `/api/admin/specialties/{id}` | PUT | Uzmanlık alanını güncelle |
| `/api/admin/dashboard/stats` | GET | Dashboard istatistikleri |

---

### 9. Testler ve Kalite Güvencesi
Proje xUnit test framework'ü ve NSubstitute mocking kütüphanesi ile kapsamlı bir şekilde test edilmektedir.

**Test Sayıları:**
- **Domain Tests:** 12 test (Rehber ve Tur entity yaşam döngüsü)
- **Application Tests:** 57 test (CQRS command handler ve validator testleri)
- **Toplam:** 69 test

**Test Komutu:**
```bash
# Domain testlerini çalıştır
dotnet test tests/IstGuide.Domain.Tests

# Application testlerini çalıştır
dotnet test tests/IstGuide.Application.Tests

# Tüm testleri çalıştır ve detaylı rapor göster
dotnet test --verbosity normal

# Test kapsamını göster
dotnet test /p:CollectCoverage=true
```

**Rehber İşlemlerine Ait Testler:**
- ✅ Rehber oluşturma (4 test)
- ✅ Rehber silme (4 test)
- ✅ Rehber güncelleme (2 test)
- ✅ Rehber validasyon (10 test)
- ✅ Rehber entity yaşam döngüsü (12 test)

**Tur İşlemlerine Ait Testler:**
- ✅ Tur oluşturma (3 test)
- ✅ Tur güncelleme (3 test)
- ✅ Tur silme (3 test)
- ✅ Tur validasyon (8 test)

---

### 10. Rehber Profili Yönetimi (Rehberler için)

Rehberler giriş yaptıktan sonra kendi profillerini yönetebilirler:

**Profil Güncelleme:**
1.  `PUT /api/v1/guides/{id}` — Profil bilgilerini güncelle
    *   **FirstName:** Ad
    *   **LastName:** Soyadı
    *   **Email:** E-posta adresi
    *   **PhoneNumber:** Telefon numarası
    *   **Title:** Rehber unvanı (Örn: "Tarih Rehberi")
    *   **Bio:** Özgeçmiş/Hakkında (Max 500 karakter)
    *   **DateOfBirth:** Doğum tarihi
    *   **Gender:** Cinsiyet
    *   **Image:** Profil fotoğrafı URL'si
    *   **LanguageIds:** Konuştuğu diller (array)
    *   **SpecialtyIds:** Uzmanlık alanları (array)
    *   **DistrictIds:** Turlar verdiği bölgeler (array)

**Profil Silme:**
2.  `DELETE /api/v1/guides/{id}` — Profil tamamen sil (soft delete)

> **Not:** Rehberler sadece kendi profillerini güncelleyebilir. Admin onay almadan sitenin rehber listesinde görünmezler.

---

### 11. Veritabanı Yapısı Özeti

Proje 15 ana entity ile tasarlanmıştır:

**Çekirdek Entityler:**
- **Guide:** Rehber bilgileri
- **Tour:** Turun detayları
- **Review:** Rehberlere yazılan yorumlar
- **ContactRequest:** Ziyaretçilerin iletişim talepleri
- **PageContent:** CMS için statik sayfa içeriği

**Yönetim Entityleri:**
- **Language:** Rehberlerin konuştuğu diller
- **Specialty:** Rehberlerin uzmanlık alanları
- **District:** İstanbul'un ilçeleri
- **AppRole:** Sistem rolleri (User, Guide, Admin)
- **AppUser:** Sistem kullanıcıları

**İlişkiler:**
- 1 Rehber → N Tur (Guide.Id → Tour.GuideId)
- 1 Rehber → N Yorum (Guide.Id → Review.GuideId)
- Rehber ↔ Dil (Many-to-Many)
- Rehber ↔ Uzmanlık (Many-to-Many)
- Rehber ↔ Bölge (Many-to-Many)

---

### 💡 Özet Tavsiye

**Günlük/Haftalık İş Sırası:**
1. Haftada birkaç kez **bekleyen rehber başvurularını** kontrol edin ve onaylayın/reddettinin.
2. **Yorum yönetimini** gözden geçirin ve uygunsuz yorumları reddedin.
3. Yeni turlar ekleyerek ve rehber profillerini zenginleştirerek sitenizi canlı tutun.
4. **İletişim taleplerini** kontrol edin ve rehberlere yönlendirin.

**En Pratik Yöntemler:**
- Swagger arayüzü (`/swagger`) tüm işlemlerinizi test etmek için kullanın.
- API'ye direkt cURL komutları ile çalışabilirsiniz (bkz. API_TESTING_GUIDE.md).
- Postman collection'ını import ederek hazır istek şablonlarını kullanabilirsiniz.

**Sistem Güvenliği:**
- Tüm veri değişiklikleri JWT token ile korunur.
- Rehberler kendi verilerine, adminler tüm veriye erişebilir.
- Soft delete kullanıldığı için hiçbir veri kalıcı olarak silinmez.

---
*Son Güncelleme: Nisan 2026*
*Hazırlayan: Antigravity AI Assistant*
