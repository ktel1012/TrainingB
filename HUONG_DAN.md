# Hướng Dẫn Sử Dụng Project Sau Khi Cải Thiện

## 📋 Tổng Quan

Project của bạn đã được cải thiện với các tính năng mới:
- ✅ Configuration system (không còn hardcode)
- ✅ Logging system (track errors và activities)
- ✅ Error handling & retry logic (app không crash)
- ✅ Async/await (UI không bị freeze)
- ✅ Resource management (không leak memory)
- ✅ Helper classes (code sạch hơn)

## 🚀 Cách Chạy

### Bước 1: Build Project
```bash
dotnet build
```

### Bước 2: Chạy Application
```bash
dotnet run
```
Hoặc double-click file `.exe` trong folder `bin\Debug\net8.0-windows7.0\`

### Bước 3: Sử dụng
- App sẽ tự động khởi động Chrome browser
- Click các button như bình thường (HN, MN, 4DLO, etc.)
- Kết quả hiển thị trong các textbox tương ứng

## ⚙️ Configuration (appsettings.json)

File `appsettings.json` cho phép bạn thay đổi settings mà không cần compile lại:

### Thay đổi Base URL:
```json
"AppSettings": {
  "BaseUrl": "https://b2one789.net"
}
```

### Thay đổi XPath selectors:
```json
"XPaths": {
  "FirstListPath": "/html/body/...",
  "TablePath": "/html/body/..."
}
```

### Thay đổi threshold values:
```json
"ThresholdValues": {
  "Northern_2d": 1560,
  "Southern_2d": 2265
}
```

### Thay đổi timeouts:
```json
"ImplicitWaitMilliseconds": 2000,
"DefaultSleepMilliseconds": 1000
```

## 📝 Logging

### Xem Logs
Logs được lưu trong folder `logs/`:
- File format: `app-YYYYMMDD.log`
- Ví dụ: `logs/app-20260704.log`

### Log Levels
- **Debug**: Thông tin chi tiết cho debugging
- **Info**: Hoạt động bình thường
- **Warning**: Cảnh báo (không nghiêm trọng)
- **Error**: Lỗi (có exception)
- **Critical**: Lỗi nghiêm trọng

### Auto Cleanup
Logs cũ hơn 7 ngày sẽ tự động bị xóa (có thể thay đổi trong appsettings.json):
```json
"Logging": {
  "RetainDays": 7
}
```

## 🐛 Troubleshooting

### Lỗi: "appsettings.json not found"
**Giải pháp**: 
- Build lại project: `dotnet build`
- File sẽ tự động copy vào output folder

### Lỗi: ChromeDriver version mismatch
**Giải pháp**:
- Update Chrome browser lên phiên bản mới nhất
- Hoặc update Selenium.WebDriver package:
```bash
dotnet add package Selenium.WebDriver --version [latest]
```

### Lỗi: Element not found
**Giải pháp**:
- Website có thể đã thay đổi cấu trúc
- Check logs để xem XPath nào bị lỗi
- Update XPath trong `appsettings.json`

### App bị freeze
**Giải pháp**:
- Đã fix bằng async/await
- Nếu vẫn bị, check logs để xem đang stuck ở đâu

### Memory leak / Chrome processes không tắt
**Giải pháp**:
- Đã fix bằng proper disposal
- Nếu vẫn xảy ra, report trong logs

## 📊 Cấu Trúc Code Mới

```
TrainingB/
├── Configuration/          # Settings & config loader
│   ├── AppSettings.cs
│   └── ConfigurationManager.cs
│
├── Services/              # Utilities & helpers
│   ├── Logger.cs          # Logging system
│   └── WebDriverHelper.cs # Selenium helpers
│
├── Forms/                 # UI forms
│   ├── FormMonitor.cs     # Main form (improved)
│   └── Test/              # Scraper classes
│       ├── FindBase.cs    # Base class
│       ├── HN_*.cs        # Northern scrapers
│       └── MN_*.cs        # Southern scrapers
│
├── appsettings.json       # Configuration
└── logs/                  # Log files
```

## 🔧 Cách Thêm Scraper Mới

### Bước 1: Tạo class mới kế thừa FindBase
```csharp
public class HN_FindNewType(ChromeDriver driver) : FindBase(driver)
{
    public override string URL => "https://...";
    
    public override string GetChangeList()
    {
        return ExecuteWithRetry(() => 
        {
            // Your scraping logic here
            var element = _driver.FindElementSafe(TablePath);
            // ...
            return result;
        }, "HN_FindNewType.GetChangeList");
    }
}
```

### Bước 2: Build & Run
- App sẽ tự động detect class mới qua Reflection
- Không cần thay đổi gì khác!

## 💡 Best Practices

### 1. Luôn dùng ExecuteWithRetry
```csharp
return ExecuteWithRetry(() => 
{
    // Your code
}, "OperationName");
```

### 2. Dùng WebDriverHelper extensions
```csharp
var element = _driver.FindElementSafe(By.XPath("..."));
bool success = _driver.ClickElementSafe(By.Id("button"));
```

### 3. Log quan trọng events
```csharp
Logger.Info("Starting scraping");
Logger.Error("Failed to find element", exception);
```

### 4. Check null trước khi dùng driver
```csharp
if (_driver == null) return "";
```

## 📞 Support

Nếu gặp vấn đề:
1. ✅ Check file logs trong folder `logs/`
2. ✅ Đọc error message trong MessageBox
3. ✅ Verify appsettings.json có đúng format không
4. ✅ Update Chrome browser và ChromeDriver
