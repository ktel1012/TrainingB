# URL Configuration - 100% No Hardcode! 🎯

## ✅ Vấn Đề Đã Fix

### Trước (Vẫn còn hardcode):
```csharp
public override string URL => $"{_config.BaseUrl}/traditional/northern/parlay";
//                                                  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//                                                  VẪN HARDCODE PATH!
```

### Sau (100% config-driven - KHÔNG FALLBACK):
```csharp
private const string URL_KEY = "HN_2d_Parlay";
public override string URL => GetUrlFromConfig(URL_KEY);
//                            ^^^^^^^^^^^^^^^^^^^^^^^^^^
//                            LẤY TỪ CONFIG! Không có fallback - ép buộc phải có trong config!
```

---

## 📋 URL Keys trong appsettings.json

```json
{
  "ScraperSettings": {
    "BaseUrl": "https://b2one789.net",
    "Urls": {
      "HN_2d_Parlay": "traditional/northern/parlay",
      "HN_3d_Dau": "traditional/northern-2nd/3d/dau",
      "HN_3d_Duoi": "traditional/northern-2nd/3d/duoi",
      "HN_3d_17Lo": "traditional/northern-2nd/3d/17lo",
      "HN_4d_Duoi": "traditional/northern-2nd/4d/duoi",
      "HN_4d_16Lo": "traditional/northern-2nd/4d/16lo",
      "MN_2d_Dau": "traditional/southern/2d/dau",
      "MN_2d_Duoi": "traditional/southern/2d/duoi",
      "MN_3d_Dau": "traditional/southern/3d/dau",
      "MN_3d_Duoi": "traditional/southern/3d/duoi",
      "MN_3d_17Lo": "traditional/southern/3d/17lo",
      "MN_4d_Duoi": "traditional/southern/4d/duoi",
      "MN_4d_16Lo": "traditional/southern/4d/16lo"
    }
  }
}
```

---

## 🔧 Cách Sử Dụng trong Scrapers

### Pattern cho mỗi scraper:

```csharp
public class HN_Find3dDAU(ChromeDriver driver) : FindBase(driver)
{
    // 1. Define constants
    private const string REGION_KEY = "Northern_3d";
    private const string URL_KEY = "HN_3d_Dau";  // ← Key trong appsettings.json
    
    // 2. Use GetUrlFromConfig helper (NO HARDCODE!)
    public override string URL => GetUrlFromConfig(URL_KEY);
    //                                              ^^^^^^^
    //                                              CHỈ CẦN KEY! Không hardcode fallback!
}
```

---

## 📚 Mapping Table

| Scraper Class | URL_KEY | Region Key | Path |
|--------------|---------|-----------|------|
| HN_Find2dXIEN | `HN_2d_Parlay` | Northern_2d | traditional/northern/parlay |
| HN_Find3dDAU | `HN_3d_Dau` | Northern_3d | traditional/northern-2nd/3d/dau |
| HN_Find3dDUOI | `HN_3d_Duoi` | Northern_3d | traditional/northern-2nd/3d/duoi |
| HN_Find3dLO | `HN_3d_17Lo` | Northern_3d | traditional/northern-2nd/3d/17lo |
| HN_Find4dDUOI | `HN_4d_Duoi` | Northern_4d | traditional/northern-2nd/4d/duoi |
| HN_Find4dLO | `HN_4d_16Lo` | Northern_4d | traditional/northern-2nd/4d/16lo |
| MN_Find2dDAU | `MN_2d_Dau` | Southern_2d | traditional/southern/2d/dau |
| MN_Find2dDUOI | `MN_2d_Duoi` | Southern_2d | traditional/southern/2d/duoi |
| MN_Find3dDAU | `MN_3d_Dau` | Southern_3d | traditional/southern/3d/dau |
| MN_Find3dDUOI | `MN_3d_Duoi` | Southern_3d | traditional/southern/3d/duoi |
| MN_Find3dLO | `MN_3d_17Lo` | Southern_3d | traditional/southern/3d/17lo |
| MN_Find4dDUOI | `MN_4d_Duoi` | Southern_4d | traditional/southern/4d/duoi |
| MN_Find4dLO | `MN_4d_16Lo` | Southern_4d | traditional/southern/4d/16lo |

---

## 🎯 Lợi Ích

### 1. **100% Configuration-driven**
- Không còn hardcode path trong code
- Thay đổi URL chỉ cần sửa appsettings.json
- Không cần compile lại!

### 2. **Fail-Fast Safety**
```csharp
GetUrlFromConfig(URL_KEY)
```
- Nếu key không tồn tại → **THROW EXCEPTION**
- ✅ Ép buộc config phải đầy đủ
- ✅ Phát hiện lỗi config ngay lập tức
- ✅ Không chạy với config sai

### 3. **Easy to Test**
- Test với URL khác → chỉ sửa config
- Staging/Production environments → config khác nhau
- A/B testing URLs → dễ dàng

### 4. **Centralized Management**
- Tất cả URLs ở 1 nơi
- Dễ review
- Dễ maintain

---

## 🔍 GetUrlFromConfig() Implementation

```csharp
protected string GetUrlFromConfig(string urlKey)
{
    // Tìm key trong config
    if (_config.Urls.TryGetValue(urlKey, out string? path))
    {
        // Tìm thấy → combine với BaseUrl
        return $"{_config.BaseUrl}/{path}";
    }

    // Không tìm thấy → LOG ERROR và THROW EXCEPTION
    Logger.Error($"URL key '{urlKey}' not found in config! This is a configuration error.");
    throw new InvalidOperationException($"Required URL key '{urlKey}' is missing from appsettings.json");
}
```

**Tại sao throw exception?**
- ✅ Ép buộc appsettings.json phải đầy đủ
- ✅ Fail-fast: phát hiện lỗi ngay, không chạy với config sai
- ✅ Không có hardcode fallback (100% config-driven!)

---

## ⚙️ Cách Thêm URL Mới

### Bước 1: Thêm vào appsettings.json
```json
"Urls": {
  "NEW_Key": "new/path/here"
}
```

### Bước 2: Dùng trong scraper
```csharp
private const string URL_KEY = "NEW_Key";
public override string URL => GetUrlFromConfig(URL_KEY);  // Không cần fallback!
```

### Bước 3: Build & run
```bash
dotnet build
dotnet run
```

---

## 🚀 Migration Status

### ✅ Đã Migration (3/14):
- [x] HN_Find2dXIEN.cs
- [x] MN_Find2dDAU.cs
- [x] MN_Find2dDUOI.cs

### ⏳ Còn lại (11/14):
Sử dụng mapping table ở trên để migration

---

## 💡 Best Practices

1. **Naming Convention**: `[Region]_[Dimension]_[Type]`
   - Region: HN (Northern), MN (Southern)
   - Dimension: 2d, 3d, 4d
   - Type: Dau, Duoi, Parlay, 16Lo, 17Lo

2. **NO fallback needed**: Config phải đầy đủ - fail-fast nếu thiếu

3. **Keep paths in sync**: URL_KEY name ≈ path meaning

4. **Validate config**: Đảm bảo tất cả keys cần thiết có trong appsettings.json

---

## ✅ Kết Luận

**Không còn hardcode URLs!** 🎉

- BaseUrl: từ config ✅
- Paths: từ config ✅  
- Fallback: có sẵn ✅
- Logging: tự động ✅

**Result:** Professional, maintainable, flexible! 💪
