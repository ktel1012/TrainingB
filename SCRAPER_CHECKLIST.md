# Scraper Refactoring Checklist

## Progress Tracker

### ✅ Completed (3/14)
- [x] HN_Find2dXIEN.cs
- [x] MN_Find2dDAU.cs  
- [x] MN_Find2dDUOI.cs

### ⏳ Northern (HN) - 5 remaining
- [ ] HN_Find3dDAU.cs - Region: Northern_3d, URL: HN_3d_Dau, Threshold: 670
- [ ] HN_Find3dDUOI.cs - Region: Northern_3d, URL: HN_3d_Duoi, Threshold: 670
- [ ] HN_Find3dLO.cs - Region: Northern_3d, URL: HN_3d_17Lo, Threshold: 670
- [ ] HN_Find4dDUOI.cs - Region: Northern_4d, URL: HN_4d_Duoi, Threshold: 670
- [ ] HN_Find4dLO.cs - Region: Northern_4d, URL: HN_4d_16Lo, Threshold: 670

### ⏳ Southern (MN) - 6 remaining
- [ ] MN_Find3dDAU.cs - Region: Southern_3d, URL: MN_3d_Dau, Threshold: 755
- [ ] MN_Find3dDUOI.cs - Region: Southern_3d, URL: MN_3d_Duoi, Threshold: 755
- [ ] MN_Find3dLO.cs - Region: Southern_3d, URL: MN_3d_17Lo, Threshold: 755
- [ ] MN_Find4dDUOI.cs - Region: Southern_4d, URL: MN_4d_Duoi, Threshold: 2010
- [ ] MN_Find4dLO.cs - Region: Southern_4d, URL: MN_4d_16Lo, Threshold: 2010

---

## Quick Refactoring Steps

### For each file:

1. **Open file**
   ```bash
   code Forms/Test/[filename].cs
   ```

2. **Clean imports** (remove unused, add TrainingB.Services)
   ```csharp
   using OpenQA.Selenium;
   using OpenQA.Selenium.Chrome;
   using System.Collections.Generic;
   using System.Linq;
   using TrainingB.Services;
   ```

3. **Add constants**
   ```csharp
   private const string REGION_KEY = "Northern_3d"; // or Southern_3d, etc.
   private const string URL_KEY = "HN_3d_Dau";     // See appsettings.json Urls section
   ```

4. **Update URL (100% config-driven, ZERO HARDCODE)**
   ```csharp
   public override string URL => GetUrlFromConfig(URL_KEY);
   // URL_KEY: từ appsettings.json
   // Nếu thiếu key → throw exception (fail-fast)
   ```

5. **Wrap GetChangeList()**
   ```csharp
   public override string GetChangeList()
   {
       return ExecuteWithRetry(() =>
       {
           try
           {
               // existing code
           }
           catch (Exception ex)
           {
               Logger.Error("Error in [ClassName].GetChangeList", ex);
               throw;
           }
       }, "[ClassName].GetChangeList");
   }
   ```

6. **Replace hardcoded values**
   - `Thread.Sleep(X)` → `SafeSleep(_appSettings.DefaultSleepMilliseconds)`
   - `[number]` → `_config.ThresholdValues.GetValueOrDefault(REGION_KEY, [default])`

7. **Add null check after FindElement**
   ```csharp
   var txt = tbl.Text;
   if (string.IsNullOrWhiteSpace(txt))
   {
       Logger.Warning("[ClassName]: Table text is empty");
       return "";
   }
   ```

8. **Use helpers**
   - `.Replace("\r", "").Replace(",", "")...` → `CleanValue(txt2)`
   - `int.Parse(ls[0])` → `TryParseInt(ls[0], out int val0, "[ClassName]")`

9. **Replace output logic**
   ```csharp
   return ProcessResults(dic, threshold, "[prefix]");
   ```

10. **Build & test**
    ```bash
    dotnet build
    ```

---

## Threshold Reference

Add these to `appsettings.json` if missing:

```json
"ThresholdValues": {
  "Northern_2d": 1560,
  "Northern_3d": 670,
  "Northern_4d": 670,
  "Southern_2d": 2265,
  "Southern_3d": 755,
  "Southern_4d": 2010
}
```

---

## Time Estimate

- Per file: ~10-15 minutes
- Total for 11 files: **2-3 hours**

---

## Batch Commands

### Build after each file:
```bash
dotnet build
```

### Build and run:
```bash
dotnet run
```

### Check errors:
```bash
dotnet build 2>&1 | grep error
```

---

## Common Mistakes to Avoid

1. ❌ Don't forget to change class name in Logger.Error()
2. ❌ Don't forget to change REGION_KEY constant
3. ❌ Don't use `_config.ScraperSettings` - use `_config` directly
4. ❌ Don't forget to import `TrainingB.Services`
5. ❌ Don't use `int.Parse()` - use `TryParseInt()`

---

## Testing After Refactoring

1. Build successfully ✅
2. Run app ✅
3. Click each region button (HN, MN) ✅
4. Check logs in `logs/` folder ✅
5. Verify results in textboxes ✅

---

## Quick Copy-Paste Template

### For 3D scrapers (with buttons):

```csharp
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using TrainingB.Services;

namespace TrainingB.Forms.Test
{
    public class XX_FindXdYYY(ChromeDriver driver) : FindBase(driver)
    {
        private const string REGION_KEY = "REGION_KEY_HERE";  // e.g., "Northern_3d"
        private const string URL_KEY = "URL_KEY_HERE";        // e.g., "HN_3d_Dau"

        public override string URL => GetUrlFromConfig(URL_KEY);  // NO HARDCODE!
        
        public override string GetChangeList()
        {
            return ExecuteWithRetry(() =>
            {
                try
                {
                    var dic = new Dictionary<string, List<int>>();
                    int threshold = _config.ThresholdValues.GetValueOrDefault(REGION_KEY, DEFAULT_THRESHOLD);
                    
                    var l = _driver.FindElement(FirstListPath);
                    var l2 = l.FindElements(By.XPath("span[@role='button']"));
                    
                    foreach (var l3 in l2)
                    {
                        l3.Click();
                        SafeSleep(_appSettings.DefaultSleepMilliseconds);
                        
                        var tbl = _driver.FindElement(TablePath);
                        var txt = tbl.Text;
                        
                        if (string.IsNullOrWhiteSpace(txt)) continue;
                        
                        // PARSING LOGIC HERE
                    }
                    
                    return ProcessResults(dic, threshold, "PREFIX");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in XX_FindXdYYY.GetChangeList", ex);
                    throw;
                }
            }, "XX_FindXdYYY.GetChangeList");
        }
    }
}
```

---

## Progress Tracking

Mark files as you complete them above. 

**Goal:** All 14 scrapers refactored! 🎯
