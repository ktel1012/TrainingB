using Microsoft.AspNetCore.Mvc;
using TrainingB.Core.Scrapers;
using TrainingB.Core.Services;
using OpenQA.Selenium.Chrome;

namespace TrainingB.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScraperController : ControllerBase
    {
        // QUAN TRỌNG: Lock để chỉ cho phép 1 scraper chạy tại 1 thời điểm
        private static readonly SemaphoreSlim _scraperLock = new SemaphoreSlim(1, 1);
        private static bool _isScraperRunning = false;
        private static string _currentScraperName = "";

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                status = "running",
                message = "TrainingB Web API is running",
                timestamp = DateTime.UtcNow,
                scraperRunning = _isScraperRunning,
                currentScraper = _currentScraperName
            });
        }

        [HttpPost("run/{scraperName}")]
        public async Task<IActionResult> RunScraper(string scraperName)
        {
            // Check nếu đang có scraper khác chạy
            if (_isScraperRunning)
            {
                Logger.Warning($"API: Scraper {scraperName} rejected - another scraper is running: {_currentScraperName}");
                return StatusCode(409, new
                {
                    error = "Another scraper is currently running",
                    currentScraper = _currentScraperName,
                    message = $"Please wait for '{_currentScraperName}' to complete"
                });
            }

            // Thử acquire lock (không đợi)
            if (!await _scraperLock.WaitAsync(0))
            {
                Logger.Warning($"API: Scraper {scraperName} rejected - lock busy");
                return StatusCode(409, new { error = "Server is busy, please try again" });
            }

            ChromeDriver? driver = null;

            try
            {
                _isScraperRunning = true;
                _currentScraperName = scraperName;

                Logger.Info($"API: Starting scraper: {scraperName}");

                driver = WebDriverHelper.CreateChromeDriver();

                FindBase? scraper = scraperName.ToUpper() switch
                {
                    "HN_2D_XIEN" => new HN_Find2dXIEN(driver),
                    "HN_3D_DAU" => new HN_Find3dDAU(driver),
                    "HN_3D_DUOI" => new HN_Find3dDUOI(driver),
                    "HN_3D_LO" => new HN_Find3dLO(driver),
                    "HN_4D_DUOI" => new HN_Find4dDUOI(driver),
                    "HN_4D_LO" => new HN_Find4dLO(driver),
                    "MN_2D_DAU" => new MN_Find2dDAU(driver),
                    "MN_2D_DUOI" => new MN_Find2dDUOI(driver),
                    "MN_3D_DAU" => new MN_Find3dDAU(driver),
                    "MN_3D_DUOI" => new MN_Find3dDUOI(driver),
                    "MN_3D_LO" => new MN_Find3dLO(driver),
                    "MN_4D_DUOI" => new MN_Find4dDUOI(driver),
                    "MN_4D_LO" => new MN_Find4dLO(driver),
                    _ => null
                };

                if (scraper == null)
                {
                    return BadRequest(new { error = $"Unknown scraper: {scraperName}" });
                }

                // Navigate to URL
                Logger.Debug($"Navigating to: {scraper.URL}");
                driver.NavigateWithRetry(scraper.URL);

                // Run scraper with timeout
                // 4D scrapers need more time (100 buttons × 3s = 5min, actual 10-12min needed)
                // 2D/3D scrapers complete in 1-3 minutes
                int timeoutMinutes = scraperName.Contains("4D") ? 12 : 5;
                using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(timeoutMinutes));
                var task = Task.Run(() => scraper.GetChangeList(), cts.Token);

                string result;
                try
                {
                    result = await task;
                }
                catch (TaskCanceledException)
                {
                    Logger.Warning($"API: Scraper {scraperName} timed out after {timeoutMinutes} minutes");
                    return StatusCode(408, new { error = $"Scraper timed out after {timeoutMinutes} minutes" });
                }

                Logger.Info($"API: Scraper {scraperName} completed successfully");

                return Ok(new
                {
                    scraperName,
                    result,
                    success = true,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"API: Error running scraper {scraperName}", ex);
                return StatusCode(500, new
                {
                    scraperName,
                    error = ex.Message,
                    type = ex.GetType().Name,
                    success = false,
                    timestamp = DateTime.UtcNow
                });
            }
            finally
            {
                try
                {
                    driver?.Quit();
                    driver?.Dispose();
                    Logger.Debug($"ChromeDriver disposed for {scraperName}");
                }
                catch (Exception ex)
                {
                    Logger.Warning($"Error disposing ChromeDriver for {scraperName}: {ex.Message}");
                }
                finally
                {
                    // QUAN TRỌNG: Release lock và reset state
                    _isScraperRunning = false;
                    _currentScraperName = "";
                    _scraperLock.Release();
                    Logger.Info($"API: Released lock for {scraperName}");
                }
            }
        }

        [HttpGet("scrapers")]
        public IActionResult GetAvailableScrapers()
        {
            var scrapers = new[]
            {
                new { name = "HN_2D_XIEN", description = "Northern 2D Parlay", region = "Northern" },
                new { name = "HN_3D_DAU", description = "Northern 3D First", region = "Northern" },
                new { name = "HN_3D_DUOI", description = "Northern 3D Last", region = "Northern" },
                new { name = "HN_3D_LO", description = "Northern 3D Lo", region = "Northern" },
                new { name = "HN_4D_DUOI", description = "Northern 4D Last", region = "Northern" },
                new { name = "HN_4D_LO", description = "Northern 4D Lo", region = "Northern" },
                new { name = "MN_2D_DAU", description = "Southern 2D First", region = "Southern" },
                new { name = "MN_2D_DUOI", description = "Southern 2D Last", region = "Southern" },
                new { name = "MN_3D_DAU", description = "Southern 3D First", region = "Southern" },
                new { name = "MN_3D_DUOI", description = "Southern 3D Last", region = "Southern" },
                new { name = "MN_3D_LO", description = "Southern 3D Lo", region = "Southern" },
                new { name = "MN_4D_DUOI", description = "Southern 4D Last", region = "Southern" },
                new { name = "MN_4D_LO", description = "Southern 4D Lo", region = "Southern" }
            };

            return Ok(scrapers);
        }

        [HttpPost("run-all")]
        public async Task<IActionResult> RunAllScrapers()
        {
            try
            {
                Logger.Info("API: Starting all scrapers");

                var results = new List<object>();
                ChromeDriver? driver = null;

                try
                {
                    driver = WebDriverHelper.CreateChromeDriver();

                    var scrapers = new FindBase[]
                    {
                        new HN_Find2dXIEN(driver),
                        new HN_Find3dDAU(driver),
                        new HN_Find3dDUOI(driver),
                        new HN_Find3dLO(driver),
                        new HN_Find4dDUOI(driver),
                        new HN_Find4dLO(driver),
                        new MN_Find2dDAU(driver),
                        new MN_Find2dDUOI(driver),
                        new MN_Find3dDAU(driver),
                        new MN_Find3dDUOI(driver),
                        new MN_Find3dLO(driver),
                        new MN_Find4dDUOI(driver),
                        new MN_Find4dLO(driver)
                    };

                    foreach (var scraper in scrapers)
                    {
                        try
                        {
                            driver.NavigateWithRetry(scraper.URL);
                            var result = await Task.Run(() => scraper.GetChangeList());
                            results.Add(new 
                            { 
                                scraper = scraper.GetType().Name,
                                result,
                                success = true
                            });
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error with {scraper.GetType().Name}", ex);
                            results.Add(new { scraper = scraper.GetType().Name, error = ex.Message, success = false });
                        }
                    }
                }
                finally
                {
                    driver?.Quit();
                    driver?.Dispose();
                }

                return Ok(new { results, timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                Logger.Error("API: Error running all scrapers", ex);
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
