# Scraper Refactoring Guide

## 📋 Tổng Quan

Đã refactor 3 scrapers mẫu:
- ✅ `HN_Find2dXIEN.cs` - Pattern cho 2D Northern
- ✅ `MN_Find2dDAU.cs` - Pattern cho 2D Southern
- ✅ `MN_Find2dDUOI.cs` - Pattern cho 2D Southern (duoi)

## 🔄 Pattern Cần Áp Dụng

### Scrapers Còn Lại Cần Update (11 files):

**Northern (HN):**
1. `HN_Find3dDAU.cs`
2. `HN_Find3dDUOI.cs`
3. `HN_Find3dLO.cs`
4. `HN_Find4dDUOI.cs`
5. `HN_Find4dLO.cs`

**Southern (MN):**
6. `MN_Find3dDAU.cs`
7. `MN_Find3dDUOI.cs`
8. `MN_Find3dLO.cs`
9. `MN_Find4dDUOI.cs`
10. `MN_Find4dLO.cs`

---

## 🎯 Refactoring Pattern

### Bước 1: Clean up imports
```csharp
// TRƯỚC
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// SAU
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using TrainingB.Services;
```

### Bước 2: Thêm constants cho region key và URL key
```csharp
public class HN_Find3dDAU(ChromeDriver driver) : FindBase(driver)
{
    // Thêm constants này
    private const string REGION_KEY = "Northern_3d"; // Hoặc Southern_3d, v.v.
    private const string URL_KEY = "HN_3d_Dau";     // Key trong appsettings.json
```

### Bước 3: Update URL property (KHÔNG CÒN HARDCODE!)
```csharp
// TRƯỚC
public override string URL => "https://b2one789.net/traditional/northern-2nd/3d/dau";

// SAU (CÁCH MỚI - 100% CONFIG, ZERO HARDCODE!)
public override string URL => GetUrlFromConfig(URL_KEY);
// URL_KEY: lấy từ appsettings.json
// Nếu key không có → throw exception (fail-fast)
```

### Bước 4: Wrap GetChangeList() với ExecuteWithRetry
```csharp
public override string GetChangeList()
{
    return ExecuteWithRetry(() =>
    {
        try
        {
            // Original logic here
        }
        catch (Exception ex)
        {
            Logger.Error("Error in [ClassName].GetChangeList", ex);
            throw;
        }
    }, "[ClassName].GetChangeList");
}
```

### Bước 5: Replace hardcoded values
```csharp
// TRƯỚC
Thread.Sleep(1000);
int threshold = 670; // hardcoded

// SAU
SafeSleep(_appSettings.DefaultSleepMilliseconds);
int threshold = _config.ThresholdValues.GetValueOrDefault(REGION_KEY, 670);
```

### Bước 6: Add null check cho table text
```csharp
var tbl = _driver.FindElement(TablePath);
var txt = tbl.Text;

// Thêm check này
if (string.IsNullOrWhiteSpace(txt))
{
    Logger.Warning("[ClassName]: Table text is empty");
    return "";
}
```

### Bước 7: Use helper methods
```csharp
// TRƯỚC
var val = txt2.Replace("\r", "");
if (val.Contains(",")) val = val.Replace(",", "");
if (val.Contains(".")) val = val.Replace(".", "");

// SAU
var val = CleanValue(txt2);
```

```csharp
// TRƯỚC
int.Parse(ls[0])

// SAU
if (!TryParseInt(ls[0], out int val0, "[ClassName]"))
{
    ls = new List<string>();
    continue;
}
```

### Bước 8: Replace result processing
```csharp
// TRƯỚC
var f = dic.Where(p => p.Value.Sum() != threshold).Select(p => p).ToList();
string str1 = "";
string str2 = "";
foreach (var s in f)
{
    if (str1 == "") str1 = "3dDau: ";
    str1 += $"{s.Key}({s.Value.Max()}/{threshold}) ; ";
    str2 += $"{s.Key},";
}
var str = str1 + "\r\n" + str2 + "\r\n";
File.WriteAllText(FilePath, str);
return str;

// SAU
return ProcessResults(dic, threshold, "3dDau");
```

---

## 📝 Example: Full Refactored File (3D Pattern)

```csharp
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using TrainingB.Services;

namespace TrainingB.Forms.Test
{
    public class HN_Find3dDAU(ChromeDriver driver) : FindBase(driver)
    {
        private const string REGION_KEY = "Northern_3d";
        
        public override string URL => $"{_config.BaseUrl}/traditional/northern-2nd/3d/dau";
        
        public override string GetChangeList()
        {
            return ExecuteWithRetry(() =>
            {
                try
                {
                    var dic = new Dictionary<string, List<int>>();
                    int threshold = _config.ThresholdValues.GetValueOrDefault(REGION_KEY, 670);
                    
                    var l = _driver.FindElement(FirstListPath);
                    var l2 = l.FindElements(By.XPath("span[@role='button']"));
                    
                    foreach (var l3 in l2)
                    {
                        l3.Click();
                        SafeSleep(_appSettings.DefaultSleepMilliseconds);
                        
                        var tbl = _driver.FindElement(TablePath);
                        var txt = tbl.Text;
                        
                        if (string.IsNullOrWhiteSpace(txt)) continue;
                        
                        var ls = new List<string>();
                        foreach (var txt2 in txt.Split('\n'))
                        {
                            if (string.IsNullOrEmpty(txt2)) continue;
                            
                            var val = CleanValue(txt2);
                            ls.Add(val);
                            
                            if (ls.Count >= 2)
                            {
                                if (!TryParseInt(ls[1], out int sum, "HN_Find3dDAU"))
                                {
                                    ls = new List<string>();
                                    continue;
                                }
                                
                                if (sum == threshold || sum == 0)
                                {
                                    ls = new List<string>();
                                    continue;
                                }
                                
                                dic[ls[0]] = new List<int> { sum };
                                ls = new List<string>();
                            }
                        }
                    }
                    
                    return ProcessResults(dic, threshold, "3dDau");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in HN_Find3dDAU.GetChangeList", ex);
                    throw;
                }
            }, "HN_Find3dDAU.GetChangeList");
        }
    }
}
```

---

## ✅ Checklist Khi Refactor

Cho mỗi scraper, đảm bảo:

- [ ] Clean imports (remove unused)
- [ ] Add REGION_KEY constant
- [ ] Update URL to use `_config.BaseUrl`
- [ ] Wrap with `ExecuteWithRetry()`
- [ ] Replace `Thread.Sleep()` with `SafeSleep()`
- [ ] Get threshold from config
- [ ] Add null check for table text
- [ ] Use `CleanValue()` helper
- [ ] Use `TryParseInt()` instead of `int.Parse()`
- [ ] Use `ProcessResults()` for output
- [ ] Add try-catch with Logger
- [ ] Test build after each file

---

## 🎯 Region Keys Reference

```csharp
Northern_2d = 1560
Northern_3d = 670
Southern_2d = 2265
Southern_3d = 755
```

Các thresholds khác (4d) cần check trong code cũ.

---

## 🚀 After Refactoring

Sau khi refactor tất cả:
1. Build để check errors: `dotnet build`
2. Test run app
3. Check logs trong `logs/` folder
4. Verify kết quả
