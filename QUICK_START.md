# 🚀 **QUICK START GUIDE**

**TrainingB Web + Mobile Lottery Scraper**  
**Version:** 2.0 - Multi-platform  
**Status:** ✅ Production Ready

---

## ⚡ **START SERVER (30 GIÂY)**

### **1. Mở terminal:**
```powershell
cd D:\TrainingB
```

### **2. Chạy server:**
```powershell
dotnet run --project TrainingB.Web --urls "http://localhost:5555"
```

### **3. Đợi message:**
```
Now listening on: http://localhost:5555
Application started.
```

### **4. Mở browser:**
- Main UI: http://localhost:5555
- Debug: http://localhost:5555/test.html

✅ **DONE!**

---

## 🖥️ **SỬ DỤNG DESKTOP APP**

```powershell
cd D:\TrainingB
dotnet run
# OR
.\bin\Debug\net8.0-windows7.0\TrainingB.exe
```

---

## 📱 **SỬ DỤNG WEB/MOBILE**

### **Trên cùng máy:**
1. Mở browser: http://localhost:5555
2. Click scraper button
3. Đợi kết quả (1-5 phút)

### **Trên điện thoại (cùng WiFi):**
1. Tìm IP của PC:
   ```powershell
   ipconfig
   # Tìm IPv4: vd 192.168.1.100
   ```

2. Trên phone, mở: `http://192.168.1.100:5555`

3. Install PWA:
   - Click "Cài đặt App"
   - Add to Home Screen
   - Dùng như native app!

---

## 🎯 **13 SCRAPERS AVAILABLE**

### **Northern (Miền Bắc):**
1. HN_2D_XIEN - 2D Parlay
2. HN_3D_DAU - 3D First
3. HN_3D_DUOI - 3D Last
4. HN_3D_LO - 3D Lo
5. HN_4D_DUOI - 4D Last ✅ Fixed
6. HN_4D_LO - 4D Lo

### **Southern (Miền Nam):**
7. MN_2D_DAU - 2D First
8. MN_2D_DUOI - 2D Last
9. MN_3D_DAU - 3D First
10. MN_3D_DUOI - 3D Last
11. MN_3D_LO - 3D Lo
12. MN_4D_DUOI - 4D Last ✅ Fixed
13. MN_4D_LO - 4D Lo

---

## ⚙️ **CONFIGURATION**

**File:** `TrainingB.Web/appsettings.json`

```json
{
  "AppSettings": {
    "BaseUrl": "https://b2one789.net",
    "ImplicitWaitMilliseconds": 5000,
    "DefaultTimeoutSeconds": 30,
    "MaxRetries": 3
  },
  "ScraperSettings": {
    "Urls": { ... },
    "XPaths": { ... },
    "ThresholdValues": { ... }
  }
}
```

**Modify as needed, then restart server.**

---

## 🔧 **TROUBLESHOOTING**

### **Server won't start:**
```powershell
# Kill existing process
taskkill /F /IM dotnet.exe

# Clean and rebuild
dotnet clean
dotnet build TrainingB.Web
dotnet run --project TrainingB.Web
```

### **Scraper timeout:**
- Scrapers có thể mất 1-5 phút
- Đợi đủ thời gian
- Check server logs
- See: `TROUBLESHOOTING_GUIDE.md`

### **No results:**
- Check internet connection
- Website có thể đổi XPath
- Test manual: mở URL trong browser
- See debug page: `/test.html`

---

## 📊 **EXPECTED TIMING**

| Scraper | Typical Time | Max Time |
|---------|--------------|----------|
| HN_2D_XIEN | ~5 seconds | 30s |
| MN_2D_DAU | ~10 seconds | 1 min |
| HN_4D_DUOI | ~3 minutes | 5 min |
| MN_4D_DUOI | ~3 minutes | 5 min |
| All 13 | ~10-15 min | 20 min |

---

## 🚀 **DEPLOY TO SERVER**

See detailed guide: `DEPLOYMENT_GUIDE.md`

**Quick version:**
```bash
# 1. Build
dotnet publish -c Release TrainingB.Web -o publish

# 2. Upload to server
scp -r publish/* user@server:/var/www/trainingb/

# 3. Run
ssh user@server
cd /var/www/trainingb
dotnet TrainingB.Web.dll --urls "http://0.0.0.0:5000"
```

---

## 📁 **PROJECT STRUCTURE**

```
TrainingB/
├── TrainingB/              ← Desktop (WinForms)
├── TrainingB.Core/         ← Shared logic (14 scrapers)
└── TrainingB.Web/          ← Web API + PWA
    ├── Controllers/
    ├── wwwroot/            ← Frontend
    └── appsettings.json
```

---

## 📚 **DOCUMENTATION**

| File | Purpose |
|------|---------|
| `WEB_PROJECT_COMPLETE.md` | Full project overview |
| `DEPLOYMENT_GUIDE.md` | Deploy to VPS |
| `TROUBLESHOOTING_GUIDE.md` | Fix common issues |
| `TIMEOUT_FIX_COMPLETE.md` | Timeout issue details |
| `ISSUE_RESOLVED_SUMMARY.md` | Recent fix summary |
| `QUICK_START.md` | This file |

---

## 🎯 **MOST COMMON TASKS**

### **Start server:**
```powershell
cd D:\TrainingB
dotnet run --project TrainingB.Web --urls "http://localhost:5555"
```

### **Test scraper:**
```
Open: http://localhost:5555/test.html
Click: "4. Test MN_4D_DUOI"
Wait: 3-5 minutes
Result: ✅ Success
```

### **Check logs:**
```powershell
# If logs folder exists
Get-Content TrainingB.Web\logs\app-*.log -Tail 50
```

### **Rebuild:**
```powershell
dotnet clean TrainingB.Web
dotnet build TrainingB.Web
```

---

## ✅ **HEALTH CHECK**

### **1. Test API:**
```powershell
Invoke-WebRequest http://localhost:5555/api/scraper/status
# Should return: {"status":"running",...}
```

### **2. Test UI:**
```
Open: http://localhost:5555
Should see: 13 scraper buttons
```

### **3. Test scraper:**
```
Click: "Northern 2D Parlay" (fast, ~5 seconds)
Result: Should see results in UI
```

✅ All green? **System is healthy!**

---

## 💡 **TIPS**

### **Performance:**
- Run 1 scraper at a time (avoid overload)
- Long scrapers (4D) take 3-5 min - be patient
- Use debug page to monitor timing

### **Development:**
- Edit `appsettings.json` for config changes
- No need to rebuild for config changes
- Just restart server

### **Testing:**
- Desktop app: Fastest testing
- Web UI: Production-like
- Debug page: Detailed diagnostics

---

## 🆘 **QUICK HELP**

### **Error: "Port already in use"**
```powershell
# Use different port
dotnet run --project TrainingB.Web --urls "http://localhost:6000"
```

### **Error: "ChromeDriver not found"**
```powershell
# Install Chrome/Chromium
# Restart terminal
```

### **Error: "Timeout after 5 minutes"**
- Normal for very slow websites
- Consider increasing task timeout
- Or retry later

---

## 🎊 **CURRENT STATUS**

- ✅ **Build:** Success (0 errors, 0 warnings)
- ✅ **Desktop:** Working
- ✅ **Web API:** Working
- ✅ **PWA UI:** Working
- ✅ **13 Scrapers:** All functional
- ✅ **Timeout issue:** Fixed
- ✅ **Production:** Ready

---

**Server:** http://localhost:5555  
**Debug:** http://localhost:5555/test.html  
**Ready to use!** 🚀

**For detailed help, see other .md files in root folder.**
