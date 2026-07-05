# 🎰 TrainingB - Lottery Data Scraper

Web & Mobile Progressive Web App for scraping lottery data.

## 🚀 Features

- **13 Scrapers**: Northern and Southern lottery data
- **Đảo Số Tool**: Number permutation generator (6 modes)
- **PWA Support**: Install as app on mobile/desktop
- **Mobile-Friendly**: Responsive design for all devices
- **Real-time Results**: Live scraping with progress updates

## 🏗️ Architecture

- **Backend**: ASP.NET Core 8.0 Web API
- **Frontend**: Vanilla JavaScript PWA
- **Scraping**: Selenium WebDriver + Chrome
- **Deployment**: Docker containerized

## 📦 Projects

- `TrainingB.Core` - Shared business logic and scrapers
- `TrainingB.Web` - Web API + PWA frontend
- `TrainingB` - WinForms desktop app (legacy)

## 🚀 Quick Start

### Local Development

```bash
cd TrainingB.Web
dotnet run --urls "http://localhost:5555"
```

### Docker

```bash
docker build -t trainingb .
docker run -p 8080:8080 trainingb
```

## 📝 TrainingB - Web Scraping Application

## 📖 Tổng Quan

Ứng dụng Windows Forms scraping dữ liệu từ website b2one789.net, hỗ trợ nhiều loại hình (2D, 3D, 4D) và khu vực (Bắc, Nam).

### Công Nghệ Sử Dụng
- **.NET 8.0** - Framework
- **Windows Forms** - UI
- **Selenium WebDriver 4.35** - Web automation
- **Newtonsoft.Json** - JSON serialization

---

## 🎯 Tính Năng Chính

### ✅ Multi-Region Support
- **HN (Hà Nội)** - Northern region scrapers
- **MN (Miền Nam)** - Southern region scrapers

### ✅ Multi-Game Type Support
- **2D** - Dau, Duoi, Xien
- **3D** - Dau, Duoi, 17Lo
- **4D** - 16Lo, Duoi

### ✅ Advanced Features (v2.0)
- 🔧 Configuration system
- 📝 Comprehensive logging
- 🛡️ Error handling & auto-retry
- ⚡ Async operations (non-blocking UI)
- 💾 Proper resource management
- 🔄 Auto-detection of new scrapers

---

## 🚀 Quick Start

### Yêu Cầu
- Windows 7 trở lên
- .NET 8.0 Runtime
- Google Chrome browser

### Cài Đặt & Chạy

```bash
# Clone hoặc download project
cd TrainingB

# Build
dotnet build

# Run
dotnet run
```

Hoặc chạy file `.exe` trong folder `bin\Debug\net8.0-windows7.0\`

---

## 📂 Cấu Trúc Project

```
TrainingB/
│
├── Configuration/              # Configuration management
│   ├── AppSettings.cs          # Config models
│   └── ConfigurationManager.cs # Config loader
│
├── Services/                   # Utility services
│   ├── Logger.cs               # Logging system
│   └── WebDriverHelper.cs      # Selenium helpers
│
├── Forms/                      # UI Forms
│   ├── FormMonitor.cs          # Main monitoring form
│   ├── FormDaoSo.cs            # Number reversal form
│   ├── FormInput.cs            # Input form
│   ├── FormWinTabs.cs          # Tab management
│   │
│   └── Test/                   # Scraper implementations
│       ├── FindBase.cs         # Base scraper class
│       │
│       ├── HN_Find2dXIEN.cs    # Northern 2D Parlay
│       ├── HN_Find3dDAU.cs     # Northern 3D Head
│       ├── HN_Find3dDUOI.cs    # Northern 3D Tail
│       ├── HN_Find3dLO.cs      # Northern 3D Lo
│       ├── HN_Find4dDUOI.cs    # Northern 4D Tail
│       ├── HN_Find4dLO.cs      # Northern 4D Lo
│       │
│       ├── MN_Find2dDAU.cs     # Southern 2D Head
│       ├── MN_Find2dDUOI.cs    # Southern 2D Tail
│       ├── MN_Find3dDAU.cs     # Southern 3D Head
│       ├── MN_Find3dDUOI.cs    # Southern 3D Tail
│       ├── MN_Find3dLO.cs      # Southern 3D Lo
│       ├── MN_Find4dDUOI.cs    # Southern 4D Tail
│       └── MN_Find4dLO.cs      # Southern 4D Lo
│
├── appsettings.json            # Configuration file
├── logs/                       # Log files (auto-generated)
│
├── HUONG_DAN.md               # Vietnamese user guide
├── IMPROVEMENTS.md            # Technical improvements
├── CHANGELOG.md               # Version history
└── README.md                  # This file
```

---

## ⚙️ Configuration

Chỉnh sửa file `appsettings.json`:

```json
{
  "AppSettings": {
    "BaseUrl": "https://b2one789.net",
    "ImplicitWaitMilliseconds": 2000,
    "DefaultSleepMilliseconds": 1000
  },
  "ScraperSettings": {
    "XPaths": {
      "FirstListPath": "...",
      "TablePath": "..."
    },
    "ThresholdValues": {
      "Northern_2d": 1560,
      "Southern_2d": 2265
    }
  },
  "Logging": {
    "RetainDays": 7
  }
}
```

---

## 📝 Logging

Logs được lưu tự động trong folder `logs/`:
- Format: `app-YYYYMMDD.log`
- Auto cleanup sau N ngày (config trong `appsettings.json`)
- Levels: Debug, Info, Warning, Error, Critical

---

## 🔧 Cách Thêm Scraper Mới

1. Tạo class mới kế thừa `FindBase`
2. Override `URL` property
3. Override `GetChangeList()` method
4. Build & run (auto-detect qua Reflection)

```csharp
public class HN_FindNewType(ChromeDriver driver) : FindBase(driver)
{
    public override string URL => "https://...";
    
    public override string GetChangeList()
    {
        return ExecuteWithRetry(() => 
        {
            // Scraping logic
            return result;
        }, "OperationName");
    }
}
```

---

## 📚 Documentation

- **[HUONG_DAN.md](HUONG_DAN.md)** - Hướng dẫn sử dụng chi tiết (Tiếng Việt)
- **[IMPROVEMENTS.md](IMPROVEMENTS.md)** - Chi tiết cải tiến kỹ thuật
- **[CHANGELOG.md](CHANGELOG.md)** - Lịch sử thay đổi

---

## 🐛 Troubleshooting

### App không chạy?
1. Check .NET 8.0 đã cài chưa
2. Check Chrome browser version
3. Xem logs trong folder `logs/`

### Element not found?
1. Website có thể đã thay đổi
2. Update XPath trong `appsettings.json`
3. Check logs để xem XPath nào fail

### UI bị freeze?
- Đã fix trong v2.0 bằng async/await
- Nếu vẫn bị, report issue

---

## 📊 Version History

### v2.0 (2026-07-04) - Major Refactoring
- ✅ Configuration system
- ✅ Logging system
- ✅ Error handling & retry logic
- ✅ Async/await for UI
- ✅ Resource management
- ✅ Code refactoring

### v1.0 (Original)
- Basic scraping functionality
- Multiple scrapers
- Windows Forms UI

---

## 🤝 Contributing

Để contribute:
1. Fork project
2. Create feature branch
3. Make changes
4. Test thoroughly
5. Submit pull request

---

## 📄 License

This is a private project. All rights reserved.

---

## ✨ Credits

- **Framework**: .NET 8.0
- **UI**: Windows Forms
- **Automation**: Selenium WebDriver
- **JSON**: Newtonsoft.Json

---

**Build Status**: ✅ Passing

**Last Updated**: 2026-07-04
