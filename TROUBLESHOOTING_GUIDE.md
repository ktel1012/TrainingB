# 🔧 **TROUBLESHOOTING GUIDE**

## ❌ **VẤN ĐỀ: Scraper bị crash giữa chừng**

### **Triệu chứng:**
- Scraper chạy được 1 lúc
- Đột ngột tắt/không phản hồi
- Không có kết quả hiển thị
- Browser đóng đột ngột

### **Nguyên nhân có thể:**
1. **ChromeDriver crash** - Do memory/CPU quá tải
2. **Timeout** - Website load chậm, scraper timeout
3. **Anti-bot detection** - Website chặn bot
4. **Network issues** - Mất kết nối internet
5. **XPath thay đổi** - Website đổi cấu trúc

---

## 🛠️ **GIẢI PHÁP ĐÃ IMPLEMENT**

### **1. Increased Timeouts** ✅

**File:** `appsettings.json`

```json
{
  "AppSettings": {
    "ImplicitWaitMilliseconds": 5000,      // ↑ 2000 → 5000
    "DefaultSleepMilliseconds": 1500,      // ↑ 1000 → 1500
    "RetryDelayMilliseconds": 2000,        // ↑ 1000 → 2000
    "DefaultTimeoutSeconds": 30            // ↑ 10 → 30
  }
}
```

**Effect:**
- Chờ lâu hơn cho elements load
- Nhiều thời gian hơn cho retry
- Ít bị timeout false positive

---

### **2. Task Timeout Protection** ✅

**Added:** 5-minute max timeout per scraper

```csharp
// Run scraper with 5-minute timeout
using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
var task = Task.Run(() => scraper.GetChangeList(), cts.Token);

try
{
    result = await task;
}
catch (TaskCanceledException)
{
    Logger.Warning($"Scraper {scraperName} timed out after 5 minutes");
    return StatusCode(408, new { error = "Scraper timed out" });
}
```

**Effect:**
- Nếu scraper chạy quá 5 phút → auto cancel
- Trả về lỗi rõ ràng thay vì hang

---

### **3. Better Error Handling** ✅

**Before:**
```csharp
driver?.Quit();
driver?.Dispose();
```

**After:**
```csharp
try
{
    driver?.Quit();
    driver?.Dispose();
    Logger.Debug($"ChromeDriver disposed for {scraperName}");
}
catch (Exception ex)
{
    Logger.Warning($"Error disposing driver: {ex.Message}");
}
```

**Effect:**
- Cleanup driver ngay cả khi có lỗi
- Log rõ ràng khi dispose fail

---

### **4. Detailed Error Response** ✅

**Before:**
```json
{
  "error": "Object reference not set..."
}
```

**After:**
```json
{
  "scraperName": "HN_4D_DUOI",
  "error": "Element not found",
  "type": "NoSuchElementException",
  "success": false,
  "timestamp": "2026-07-04T..."
}
```

**Effect:**
- Dễ debug hơn
- Biết chính xác scraper nào lỗi
- Biết loại lỗi

---

## 🔍 **CÁCH DEBUG**

### **Bước 1: Check Logs**

**Windows:**
```powershell
# Check recent logs
Get-Content TrainingB.Web\logs\app-*.log -Tail 50
```

**Server:**
```bash
# Realtime logs
sudo journalctl -u trainingb -f
```

**Tìm gì:**
- `[Error]` - Lỗi scraping
- `[Warning]` - Timeout, retry
- `NoSuchElementException` - XPath sai
- `TimeoutException` - Website chậm

---

### **Bước 2: Test Scraper Riêng**

**Test 1 scraper thôi:**
```javascript
// Trong browser console (F12)
fetch('/api/scraper/run/HN_4D_DUOI', {
    method: 'POST'
})
.then(r => r.json())
.then(d => console.log(d));
```

**Watch response:**
- Success? → Scraper OK
- Timeout? → Tăng timeout
- Error? → Check XPath

---

### **Bước 3: Increase Timeout Thêm**

**Nếu vẫn timeout, edit `appsettings.json`:**

```json
{
  "AppSettings": {
    "ImplicitWaitMilliseconds": 10000,     // 10 seconds
    "DefaultSleepMilliseconds": 3000,      // 3 seconds
    "DefaultTimeoutSeconds": 60            // 1 minute
  }
}
```

**Then restart:**
```bash
# Stop server
Ctrl+C

# Restart
dotnet run --project TrainingB.Web
```

---

### **Bước 4: Check Website**

**Manual test:**
1. Mở https://b2one789.net/traditional/northern-2nd/4d/duoi
2. Kiểm tra:
   - Website có load được không?
   - Có CAPTCHA không?
   - Có thay đổi layout không?
   - Có message "Bot detected" không?

**Nếu có CAPTCHA/Bot detection:**
- Cần thêm delay giữa requests
- Cần user-agent rotation
- Cần proxy (nếu IP bị ban)

---

### **Bước 5: Test XPath**

**Trong browser:**
1. Mở URL của scraper
2. F12 → Console
3. Test XPath:

```javascript
// Test FirstListPath
$x("/html/body/div[1]/div/md-content/div[1]/div[2]/div/div[1]/div[2]/div/div[1]/div/div/div[1]")

// Test TablePath
$x("/html/body/div[1]/div/md-content/div[1]/div[2]/div/div[1]/div[2]/div/div[1]/div/table")

// Test TablePath2
$x("/html/body/div[9]/div[2]/div/div[2]")
```

**Nếu return empty []:**
→ XPath sai, cần update trong appsettings.json

---

## 🚨 **COMMON ERRORS**

### **Error 1: NoSuchElementException**

**Meaning:** Không tìm thấy element (XPath sai)

**Fix:**
1. Check XPath (F12 → Inspect)
2. Update `appsettings.json` → `XPaths`
3. Restart server

---

### **Error 2: TimeoutException**

**Meaning:** Website load chậm quá timeout

**Fix:**
1. Tăng `ImplicitWaitMilliseconds`
2. Tăng `DefaultTimeoutSeconds`
3. Check internet connection

---

### **Error 3: WebDriverException**

**Meaning:** ChromeDriver crashed/failed

**Fix:**
1. Restart server
2. Check Chrome/Chromium installed
3. Update chromedriver version

---

### **Error 4: 408 Request Timeout**

**Meaning:** Scraper chạy quá 5 phút

**Fix:**
1. Tăng timeout trong Controller (từ 5 min → 10 min)
2. Optimize scraper logic
3. Split thành smaller tasks

---

## 💡 **BEST PRACTICES**

### **1. Run 1 scraper at a time**
- Tránh overload server/browser
- Dễ debug khi có lỗi

### **2. Monitor logs realtime**
```bash
tail -f TrainingB.Web/logs/app-*.log
```

### **3. Test trên Desktop trước**
- Desktop app stable hơn
- Nếu Desktop lỗi → code issue
- Nếu Web lỗi → deployment issue

### **4. Gradual timeout increase**
- Bắt đầu: 5s, 1.5s, 30s
- Nếu timeout: 10s, 3s, 60s
- Tối đa: 30s, 5s, 120s

### **5. Add delays between scrapers**
```javascript
// Trong app.js, thêm delay
await new Promise(r => setTimeout(r, 3000)); // Wait 3s
```

---

## 📝 **CURRENT SETTINGS (IMPROVED)**

```json
{
  "AppSettings": {
    "ImplicitWaitMilliseconds": 5000,      ✅ Increased
    "DefaultSleepMilliseconds": 1500,      ✅ Increased
    "RetryDelayMilliseconds": 2000,        ✅ Increased
    "DefaultTimeoutSeconds": 30,           ✅ Increased
    "MaxRetries": 3                        ✅ OK
  }
}
```

**+ Task timeout: 5 minutes** ✅  
**+ Better error handling** ✅  
**+ Detailed logging** ✅

---

## 🎯 **NEXT STEPS**

### **Nếu vẫn lỗi:**

1. **Check logs** (step-by-step above)
2. **Test manual** (open URL in browser)
3. **Increase timeout** thêm
4. **Contact me** với error details

### **Send me:**
- Log file content (last 100 lines)
- Scraper name (HN_4D_DUOI)
- Error message
- Screenshot (nếu có)

---

**Server đã restart với settings mới!**  
**Thử chạy HN_4D_DUOI lại xem!** 🔄
