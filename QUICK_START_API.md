# 🚀 IstGuide API - Quick Start Guide

## 📋 Hızlı Başlangıç (5 Dakika)

### 1️⃣ API'yi Başlat

```bash
cd src/IstGuide.API
dotnet run
```

✅ `https://localhost:5001` adresinde çalışacak

---

### 2️⃣ Swagger UI ile Test Et

**Tarayıcıda aç:** https://localhost:5001/swagger/ui

Swagger'da tüm endpoints'leri görebilir ve test edebilirsin!

---

### 3️⃣ Postman Collection'u İndir

1. `IstGuide_API_Collection.postman_collection.json` indir
2. Postman'da `Import` yap
3. `base_url` variable'ını set et: `https://localhost:5001`

---

## 🧪 Hızlı Test (Terminal'de)

### Test 1: Rehber Oluştur

```bash
curl -X POST "https://localhost:5001/api/v1/guides/register" \
  -H "Content-Type: application/json" \
  -k -d '{
    "firstName": "Ahmet",
    "lastName": "Yilmaz",
    "email": "ahmet@test.com",
    "phoneNumber": "+905321234567",
    "title": "Licensed Tour Guide",
    "bio": "10 years experience",
    "yearsOfExperience": 10,
    "gender": 0,
    "dateOfBirth": "1990-01-15T00:00:00Z",
    "languageIds": ["00000000-0000-0000-0000-000000000001"],
    "specialtyIds": ["00000000-0000-0000-0000-000000000001"],
    "districtIds": ["00000000-0000-0000-0000-000000000001"]
  }' | jq .
```

**Yanıt:**
```json
{
  "succeeded": true,
  "value": "a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6",
  "errors": []
}
```

**Guide ID'yi kopyala** → `a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6`

---

### Test 2: Profili Görüntüle

```bash
curl -X GET "https://localhost:5001/api/v1/guides/ahmet-yilmaz-istanbul" \
  -k | jq .
```

---

### Test 3: Profili Güncelle

```bash
GUIDE_ID="a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6"
TOKEN="YOUR_JWT_TOKEN_HERE"

curl -X PUT "https://localhost:5001/api/v1/guides/${GUIDE_ID}/profile" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer ${TOKEN}" \
  -k -d '{
    "title": "Senior Tour Guide",
    "bio": "15 years experience",
    "yearsOfExperience": 15,
    "languageIds": ["00000000-0000-0000-0000-000000000001"],
    "specialtyIds": ["00000000-0000-0000-0000-000000000001"],
    "districtIds": ["00000000-0000-0000-0000-000000000001"]
  }' | jq .
```

---

### Test 4: Rehberi Sil

```bash
GUIDE_ID="a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6"
TOKEN="YOUR_JWT_TOKEN_HERE"

curl -X DELETE "https://localhost:5001/api/v1/guides/${GUIDE_ID}" \
  -H "Authorization: Bearer ${TOKEN}" \
  -k -i
```

**Başarılı:** `204 No Content`

---

## 📚 Endpoints Özeti

| Method | Endpoint | Auth | Açıklama |
|--------|----------|------|----------|
| POST | `/api/v1/guides/register` | ❌ | Rehber oluştur |
| PUT | `/api/v1/guides/{id}/profile` | ✅ | Profili güncelle |
| DELETE | `/api/v1/guides/{id}` | ✅ | Rehberi sil |
| GET | `/api/v1/guides/{slug}` | ❌ | Rehber profilini al |
| GET | `/api/v1/guides` | ❌ | Rehberleri ara |
| GET | `/api/admin/guides` | ✅👑 | Tüm rehberleri listele (admin) |
| PUT | `/api/admin/guides/{id}/approve` | ✅👑 | Onay ver (admin) |
| DELETE | `/api/admin/guides/{id}` | ✅👑 | Sil (admin) |

✅ = Kimlik doğrulama gerekli
👑 = Admin rolü gerekli

---

## 🔑 Kimlik Doğrulama

### JWT Token Alma

```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -k -d '{
    "email": "user@example.com",
    "password": "password123"
  }' | jq .
```

**Yanıt:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600
}
```

Token'ı her request'te header'a ekle:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

---

## 🧮 Validation Kuralları

### Create Guide
- ✅ Email unique olmalı
- ✅ FirstName/LastName zorunlu
- ✅ PhoneNumber zorunlu
- ✅ Title, Bio zorunlu
- ✅ En az 1 Language, Specialty, District

### Update Profile
- ✅ Title: 1-200 karakter
- ✅ Bio: 1-500 karakter
- ✅ YearsOfExperience: 0-60
- ✅ En az 1 Language, Specialty, District

---

## 🐛 Sık Hatalar

### HTTPS Certificate Hatası
```bash
# -k flag'ı ekle
curl -k https://localhost:5001/...
```

### 401 Unauthorized
```
❌ Token eksik veya geçersiz
✅ Authorization header'a token ekle
Authorization: Bearer YOUR_TOKEN
```

### 400 Bad Request
```
❌ Validation hatası
✅ Request body'yi kontrol et
✅ Gerekli alanları kontrol et
```

### 404 Not Found
```
❌ Guide bulunamadı
✅ Guide ID'ni kontrol et
```

---

## 📊 Test Otomasyonu

### Bash Script ile Test

```bash
#!/bin/bash

BASE_URL="https://localhost:5001"
TOKEN="YOUR_TOKEN"

echo "1️⃣ Creating guide..."
CREATE=$(curl -s -k -X POST "$BASE_URL/api/v1/guides/register" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Test",
    "lastName": "User",
    "email": "test@test.com",
    "phoneNumber": "+905001234567",
    "title": "Guide",
    "bio": "Bio",
    "yearsOfExperience": 5,
    "gender": 0,
    "dateOfBirth": "1995-01-01T00:00:00Z",
    "languageIds": ["00000000-0000-0000-0000-000000000001"],
    "specialtyIds": ["00000000-0000-0000-0000-000000000001"],
    "districtIds": ["00000000-0000-0000-0000-000000000001"]
  }')

GUIDE_ID=$(echo $CREATE | jq -r '.value')
echo "✅ Created: $GUIDE_ID"

echo "2️⃣ Updating profile..."
curl -s -k -X PUT "$BASE_URL/api/v1/guides/$GUIDE_ID/profile" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Senior Guide",
    "bio": "Updated",
    "yearsOfExperience": 10,
    "languageIds": ["00000000-0000-0000-0000-000000000001"],
    "specialtyIds": ["00000000-0000-0000-0000-000000000001"],
    "districtIds": ["00000000-0000-0000-0000-000000000001"]
  }' | jq .
echo "✅ Updated"

echo "3️⃣ Deleting guide..."
curl -s -k -X DELETE "$BASE_URL/api/v1/guides/$GUIDE_ID" \
  -H "Authorization: Bearer $TOKEN" \
  -o /dev/null -w "Status: %{http_code}\n"
echo "✅ Deleted"

echo "All tests completed!"
```

---

## 📁 Dosyalar

- `API_TESTING_GUIDE.md` - Detaylı test guide
- `IstGuide_API_Collection.postman_collection.json` - Postman collection
- `GUIDE_CRUD_IMPLEMENTATION.md` - Teknik detaylar
- `TEST_SONUÇLARI.txt` - Test sonuçları

---

## ✨ Sonraki Adımlar

1. **Login endpoint'i test et** - Token al
2. **Swagger UI'da tüm endpoints'leri gözden geçir**
3. **Postman Collection'ı import et**
4. **Kendi verilerinle test et**

---

**Status:** 🎉 ÜRETIM HAZIR
**Tarih:** 2026-04-05
**API Versiyonu:** v1
