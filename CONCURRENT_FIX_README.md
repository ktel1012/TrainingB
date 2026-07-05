# Fix: Concurrent Requests Issue

## Problem
When 2+ users click scraper buttons simultaneously, server crashes due to:
- Multiple ChromeDriver instances created (each ~200-300MB RAM)
- Free tier Render has only 512MB RAM
- No request locking mechanism → Race condition

## Solution Implemented

### 1. Request Locking (ScraperController.cs)
Added `SemaphoreSlim` to allow only 1 scraper to run at a time:

```csharp
private static readonly SemaphoreSlim _scraperLock = new SemaphoreSlim(1, 1);
private static bool _isScraperRunning = false;
private static string _currentScraperName = "";
```

When a new request arrives:
1. Check if another scraper is running → Return 409 if busy
2. Try to acquire lock (non-blocking) → Return 409 if failed
3. Set `_isScraperRunning = true` and run scraper
4. Finally: Release lock and reset state

### 2. Frontend Error Handling (app.js)
Updated to display clear messages when server is busy:

```javascript
else if (response.status === 409) {
    showStatus(`⏳ ${name}: Server đang bận`, 'error');
    const message = data.message || `Scraper khác đang chạy`;
    resultsDiv.textContent = `${message}\n\n` + resultsDiv.textContent;
}
```

## New Behavior

**When 2 users click buttons simultaneously:**

- **User 1** (clicks first): ✅ Request accepted, scraper runs normally
- **User 2** (clicks 5s later): ⏳ Gets 409 status with message "Please wait for 'MN_4D_LO' to complete"
- **User 2** (clicks again after User 1 finishes): ✅ Request accepted

## Files Changed

1. `TrainingB.Web/Controllers/ScraperController.cs`
   - Added locking mechanism
   - Return 409 when busy

2. `TrainingB.Web/wwwroot/app.js`
   - Handle 409 status code
   - Display user-friendly messages

3. `HUONG_DAN_FIX_VA_DEPLOY.html`
   - Added section 6.0 explaining concurrent requests issue

## Testing

### Local Test:
```bash
dotnet run --project TrainingB.Web/TrainingB.Web.csproj --urls "http://localhost:5555"
```

Then open 2 browser tabs and try clicking buttons simultaneously.

### Expected Results:
- First request: ✅ Runs successfully
- Second request: ⏳ Returns 409 with clear message
- Second request (retry after first completes): ✅ Runs successfully

## Deploy to Production

1. Commit changes:
```bash
git add .
git commit -m "Fix: Add request locking to prevent concurrent scraper execution"
git push origin main
```

2. Render auto-deploys in ~5-10 minutes

3. Test on production URL with 2 devices (mobile + desktop)

## Benefits

✅ No more server crashes  
✅ Clear user feedback when busy  
✅ Better resource management  
✅ Predictable behavior under load  

## Date
2026-07-05
