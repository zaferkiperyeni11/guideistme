# IstGuide API - Guide CRUD Testing Guide

## 🚀 API Başlatma

```bash
cd src/IstGuide.API
dotnet run
```

API `https://localhost:5001` adresinde çalışacak.

Swagger UI: `https://localhost:5001/swagger/ui`

---

## 📝 CRUD İşlemleri Test Etme

### 1️⃣ REHBER OLUŞTURMA (Create)

**Endpoint:** `POST /api/v1/guides/register`

**Request:**
```bash
curl -X POST "https://localhost:5001/api/v1/guides/register" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Ahmet",
    "lastName": "Yilmaz",
    "email": "ahmet@example.com",
    "phoneNumber": "+905321234567",
    "title": "Licensed Tour Guide",
    "bio": "10 years of experience in Istanbul tour guiding",
    "yearsOfExperience": 10,
    "gender": 0,
    "dateOfBirth": "1990-01-01T00:00:00Z",
    "detailedDescription": "Expert in Byzantine history and Ottoman architecture",
    "licenseNumber": "LIC123456",
    "languageIds": ["00000000-0000-0000-0000-000000000001"],
    "specialtyIds": ["00000000-0000-0000-0000-000000000001"],
    "districtIds": ["00000000-0000-0000-0000-000000000001"]
  }'
```

**Başarılı Response (201 Created):**
```json
{
  "succeeded": true,
  "value": "a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6",
  "errors": []
}
```

**Hatalar:**
- ❌ Email zaten kayıtlı: `400 Bad Request`
- ❌ Eksik alanlar: `400 Bad Request` (validation errors)

---

### 2️⃣ REHBER PROFILI GÜNCELLEME (Update)

**Endpoint:** `PUT /api/v1/guides/{id}/profile`

**Gerekli:** Authorization header (Bearer token)

**Request:**
```bash
GUIDE_ID="a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6"

curl -X PUT "https://localhost:5001/api/v1/guides/${GUIDE_ID}/profile" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "title": "Senior Tour Guide",
    "bio": "15 years of professional tour guiding experience",
    "detailedDescription": "Expert in Byzantine history, Ottoman period, and modern Istanbul",
    "yearsOfExperience": 15,
    "licenseNumber": "LIC654321",
    "languageIds": [
      "00000000-0000-0000-0000-000000000001",
      "00000000-0000-0000-0000-000000000002"
    ],
    "specialtyIds": [
      "00000000-0000-0000-0000-000000000001",
      "00000000-0000-0000-0000-000000000003"
    ],
    "districtIds": [
      "00000000-0000-0000-0000-000000000001",
      "00000000-0000-0000-0000-000000000004"
    ]
  }'
```

**Başarılı Response (200 OK):**
```json
{
  "succeeded": true,
  "errors": []
}
```

**Validation Kuralları:**
- ✅ Title: 1-200 karakter
- ✅ Bio: 1-500 karakter
- ✅ YearsOfExperience: 0-60
- ✅ En az 1 Language
- ✅ En az 1 Specialty
- ✅ En az 1 District

**Hatalar:**
- ❌ Guide bulunamadı: `400 Bad Request`
- ❌ Authorization gerekli: `401 Unauthorized`
- ❌ Validation hatası: `400 Bad Request`

---

### 3️⃣ REHBER SİLME (Delete)

**Endpoint:** `DELETE /api/v1/guides/{id}`

**Gerekli:** Authorization header (Bearer token)

**Request:**
```bash
GUIDE_ID="a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6"

curl -X DELETE "https://localhost:5001/api/v1/guides/${GUIDE_ID}" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

**Başarılı Response (204 No Content):**
```
(Boş response)
```

**Özellikler:**
- ✅ Soft delete (veri kaybolmaz)
- ✅ İlişkiler korunur
- ✅ Audit trail güncellenir
- ✅ Geri dönüş mümkün

**Hatalar:**
- ❌ Guide bulunamadı: `400 Bad Request`
- ❌ Authorization gerekli: `401 Unauthorized`

---

## 🔐 Admin Endpoints

### Admin - REHBER LİSTESİ

**Endpoint:** `GET /api/admin/guides`

**Gerekli:** Authorization header + Admin role

```bash
curl -X GET "https://localhost:5001/api/admin/guides?status=0&searchTerm=Ahmet" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN_HERE"
```

**Query Parameters:**
- `status`: GuideStatus (0=Pending, 1=Approved, 2=Rejected, 3=Suspended)
- `searchTerm`: İçin arama

---

### Admin - REHBER ONAYLA

**Endpoint:** `PUT /api/admin/guides/{id}/approve`

```bash
GUIDE_ID="a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6"

curl -X PUT "https://localhost:5001/api/admin/guides/${GUIDE_ID}/approve" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN_HERE"
```

---

### Admin - REHBER SİL

**Endpoint:** `DELETE /api/admin/guides/{id}`

```bash
GUIDE_ID="a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6"

curl -X DELETE "https://localhost:5001/api/admin/guides/${GUIDE_ID}" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN_HERE"
```

---

## 🧪 Test Senaryoları

### Senaryo 1: Tam CRUD Akışı

```bash
#!/bin/bash

# 1. Rehber oluştur
echo "1. Creating new guide..."
CREATE_RESPONSE=$(curl -s -X POST "https://localhost:5001/api/v1/guides/register" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Mehmet",
    "lastName": "Kaya",
    "email": "mehmet@example.com",
    "phoneNumber": "+905401234567",
    "title": "Tour Guide",
    "bio": "5 years experience",
    "yearsOfExperience": 5,
    "gender": 0,
    "dateOfBirth": "1995-06-15T00:00:00Z",
    "languageIds": ["00000000-0000-0000-0000-000000000001"],
    "specialtyIds": ["00000000-0000-0000-0000-000000000001"],
    "districtIds": ["00000000-0000-0000-0000-000000000001"]
  }')

GUIDE_ID=$(echo $CREATE_RESPONSE | jq -r '.value')
echo "✅ Guide created: $GUIDE_ID"

# 2. Profili güncelle
echo "2. Updating guide profile..."
curl -s -X PUT "https://localhost:5001/api/v1/guides/${GUIDE_ID}/profile" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "title": "Senior Tour Guide",
    "bio": "Updated bio",
    "yearsOfExperience": 7,
    "languageIds": ["00000000-0000-0000-0000-000000000001"],
    "specialtyIds": ["00000000-0000-0000-0000-000000000001"],
    "districtIds": ["00000000-0000-0000-0000-000000000001"]
  }' | jq .
echo "✅ Profile updated"

# 3. Profili görüntüle
echo "3. Fetching guide profile..."
curl -s -X GET "https://localhost:5001/api/v1/guides/mehmet-kaya-istanbul" | jq .
echo "✅ Profile retrieved"

# 4. Sil
echo "4. Deleting guide..."
curl -s -X DELETE "https://localhost:5001/api/v1/guides/${GUIDE_ID}" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
echo "✅ Guide deleted (soft delete)"

echo "All tests completed!"
```

---

## 📊 Response Örnekleri

### Success Response
```json
{
  "succeeded": true,
  "value": "a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6",
  "errors": []
}
```

### Error Response
```json
{
  "succeeded": false,
  "value": null,
  "errors": [
    "Bu email adresi zaten kayıtlı.",
    "Title alanı 1-200 karakter olmalıdır."
  ]
}
```

---

## 🔗 Postman Collection

Şu link'te Postman collection indirebilirsin:

```json
{
  "info": {
    "name": "IstGuide API - Guide CRUD",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Create Guide",
      "request": {
        "method": "POST",
        "url": "{{base_url}}/api/v1/guides/register",
        "body": {
          "mode": "raw",
          "raw": "{ ... }"
        }
      }
    },
    {
      "name": "Update Guide Profile",
      "request": {
        "method": "PUT",
        "url": "{{base_url}}/api/v1/guides/{{guide_id}}/profile",
        "header": [
          {
            "key": "Authorization",
            "value": "Bearer {{token}}"
          }
        ]
      }
    },
    {
      "name": "Delete Guide",
      "request": {
        "method": "DELETE",
        "url": "{{base_url}}/api/v1/guides/{{guide_id}}",
        "header": [
          {
            "key": "Authorization",
            "value": "Bearer {{token}}"
          }
        ]
      }
    }
  ]
}
```

---

## ⚙️ Environment Variables (Postman)

```
base_url = https://localhost:5001
token = YOUR_JWT_TOKEN_HERE
guide_id = a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6
```

---

## 🛠️ Troubleshooting

### HTTPS Certificate Hatası
```bash
curl -k -X POST "https://localhost:5001/api/v1/guides/register" ...
```

### Token Alınamıyor
1. `/api/auth/login` endpoint'ine POST yap
2. JWT token'ı kopyala
3. Header'a `Authorization: Bearer {token}` ekle

### Validation Hatası
- Title uzunluğunu kontrol et (1-200)
- Bio uzunluğunu kontrol et (1-500)
- En az 1 Language, Specialty, District seç
- YearsOfExperience: 0-60 arasında olmalı

---

## 📋 Test Checklist

- [ ] Rehber oluştur (Create) ✅
- [ ] Email duplicate kontrolü ✅
- [ ] Profil güncelle (Update) ✅
- [ ] Validation kuralları test et ✅
- [ ] Rehber sil (Delete) ✅
- [ ] Admin endpoints test et ✅
- [ ] Error handling kontrol et ✅

---

**Son Güncelleme:** 2026-04-05
**API Versiyonu:** v1
**Status:** 🎉 ÜRETIM HAZIR
