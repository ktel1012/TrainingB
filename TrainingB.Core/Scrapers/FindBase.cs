using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingB.Core.Configuration;
using TrainingB.Core.Services;

namespace TrainingB.Core.Scrapers
{
    public abstract class FindBase
    {
        protected readonly ChromeDriver _driver;
        protected readonly ScraperSettings _config;
        protected readonly AppSettings _appSettings;

        public FindBase(ChromeDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _config = ConfigurationManager.Config.ScraperSettings;
            _appSettings = ConfigurationManager.Config.AppSettings;
        }

        /// <summary>
        /// URL must be overridden by derived classes using GetUrlFromConfig().
        /// No default hardcoded value to ensure all scrapers are properly configured.
        /// </summary>
        public abstract string URL { get; }

        public virtual string FilePath
        {
            get
            {
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";
                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }
                return directory + this.GetType().Name + ".json";
            }
        }

        public virtual By FirstListPath => By.XPath(_config.XPaths.FirstListPath);
        public virtual By TablePath => By.XPath(_config.XPaths.TablePath);
        public virtual By TablePath2 => By.XPath(_config.XPaths.TablePath2);

        public virtual string GetChangeList()
        {
            return "";
        }

        protected T ExecuteWithRetry<T>(Func<T> action, string operationName)
        {
            int attempt = 0;
            Exception? lastException = null;
            int maxRetries = _appSettings.MaxRetries;

            while (attempt < maxRetries)
            {
                try
                {
                    attempt++;
                    Logger.Debug($"{operationName} - Attempt {attempt}/{maxRetries}");
                    return action();
                }
                catch (NoSuchElementException ex)
                {
                    lastException = ex;
                    Logger.Warning($"{operationName} - Element not found on attempt {attempt}: {ex.Message}");
                }
                catch (StaleElementReferenceException ex)
                {
                    lastException = ex;
                    Logger.Warning($"{operationName} - Stale element on attempt {attempt}: {ex.Message}");
                }
                catch (WebDriverException ex)
                {
                    lastException = ex;
                    Logger.Warning($"{operationName} - WebDriver error on attempt {attempt}: {ex.Message}");
                }

                if (attempt < maxRetries)
                {
                    Thread.Sleep(_appSettings.RetryDelayMilliseconds);
                }
            }

            Logger.Error($"{operationName} - Failed after {maxRetries} attempts", lastException);
            throw new InvalidOperationException($"{operationName} failed after {maxRetries} attempts", lastException);
        }

        protected void SafeSleep(int milliseconds)
        {
            try
            {
                Thread.Sleep(milliseconds);
            }
            catch (Exception ex)
            {
                Logger.Warning($"Sleep interrupted: {ex.Message}");
            }
        }

        /// <summary>
        /// Wait until table has stable content (no longer changing).
        /// Useful after clicking buttons that trigger AJAX/dynamic content loading.
        /// Ensures ALL data is loaded, not just partial data.
        /// </summary>
        protected void WaitForTableData(By tablePath, int maxWaitSeconds = 10)
        {
            try
            {
                int waitedMs = 0;
                int checkIntervalMs = 300;
                int maxWaitMs = maxWaitSeconds * 1000;
                string previousText = "";
                int stableCount = 0;
                int requiredStableChecks = 3; // Need 3 consecutive same results

                while (waitedMs < maxWaitMs)
                {
                    try
                    {
                        var tbl = _driver.FindElement(tablePath);
                        var currentText = tbl.Text ?? "";

                        if (!string.IsNullOrWhiteSpace(currentText))
                        {
                            // Check if content is stable (same as previous check)
                            if (currentText == previousText)
                            {
                                stableCount++;

                                // If stable for required checks, data is complete
                                if (stableCount >= requiredStableChecks)
                                {
                                    Logger.Debug($"Table data stable after {waitedMs}ms ({currentText.Length} chars, {stableCount} stable checks)");
                                    return;
                                }
                            }
                            else
                            {
                                // Content changed, reset counter
                                stableCount = 0;
                                previousText = currentText;
                                Logger.Debug($"Table data changing... ({currentText.Length} chars)");
                            }
                        }
                    }
                    catch
                    {
                        // Element not found yet, continue waiting
                        stableCount = 0;
                        previousText = "";
                    }

                    Thread.Sleep(checkIntervalMs);
                    waitedMs += checkIntervalMs;
                }

                Logger.Warning($"Table data not stable after {maxWaitSeconds}s wait (last length: {previousText.Length})");
            }
            catch (Exception ex)
            {
                Logger.Warning($"Error in WaitForTableData: {ex.Message}");
            }
        }

        protected bool TryParseInt(string value, out int result, string context = "")
        {
            if (int.TryParse(value, out result))
                return true;

            Logger.Warning($"Failed to parse '{value}' as integer{(string.IsNullOrEmpty(context) ? "" : $" in {context}")}");
            result = 0;
            return false;
        }

        protected string CleanValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace("\r", "")
                        .Replace(",", "")
                        .Replace(".", "");
        }

        protected string GetUrlFromConfig(string urlKey)
        {
            if (_config.Urls.TryGetValue(urlKey, out string? path))
            {
                return $"{_config.BaseUrl}/{path}";
            }

            Logger.Error($"URL key '{urlKey}' not found in config! This is a configuration error.");
            throw new InvalidOperationException($"Required URL key '{urlKey}' is missing from appsettings.json");
        }

        protected int GetThresholdFromConfig(string regionKey)
        {
            if (_config.ThresholdValues.TryGetValue(regionKey, out int threshold))
            {
                return threshold;
            }

            Logger.Error($"Threshold key '{regionKey}' not found in config! This is a configuration error.");
            throw new InvalidOperationException($"Required threshold key '{regionKey}' is missing from appsettings.json");
        }

        protected string ProcessResults4Items(Dictionary<string, List<int>> results, int threshold, string prefix)
        {
            try
            {
                var filtered = results.Where(p => p.Value.Sum() != threshold).Select(p => p).ToList();

                if (filtered.Count == 0)
                {
                    Logger.Info($"{prefix}: No changes detected");
                    // Return informative message instead of empty string
                    return $"{prefix}: No changes detected (threshold: {threshold}, total items: {results.Count})";
                }

                string str1 = $"{prefix}: ";
                string str2 = "";

                foreach (var s in filtered)
                {
                    str1 += $"{s.Key}({s.Value.Max()}/{threshold}) ; ";
                    str2 += $"{s.Key},";
                }

                var result = str1 + "\r\n" + str2 + "\r\n";

                try
                {
                    File.WriteAllText(FilePath, result);
                }
                catch (Exception ex)
                {
                    Logger.Warning($"Failed to write result to file: {ex.Message}");
                }

                Logger.Info($"{prefix} completed: {filtered.Count} results found");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error processing results for {prefix}", ex);
                return "";
            }
        }

        protected string ProcessResults(Dictionary<string, List<int>> results, int threshold, string prefix)
        {
            try
            {
                var filtered = results.Where(p => p.Value.Sum() != threshold).Select(p => p).ToList();

                if (filtered.Count == 0)
                {
                    Logger.Info($"{prefix}: No changes detected");
                    // Return informative message instead of empty string
                    return $"{prefix}: No changes detected (threshold: {threshold}, total items: {results.Count})";
                }

                string str1 = $"{prefix}: ";
                string str2 = "";

                foreach (var s in filtered)
                {
                    var values = s.Value;
                    if (values.Count > 1)
                    {
                        str1 += $"{s.Key}({values.Max()}/{values.Min()}) ; ";
                    }
                    else
                    {
                        str1 += $"{s.Key}({values[0]}/{threshold}) ; ";
                    }
                    str2 += $"{s.Key},";
                }

                var result = str1 + "\r\n" + str2 + "\r\n";

                try
                {
                    File.WriteAllText(FilePath, result);
                }
                catch (Exception ex)
                {
                    Logger.Warning($"Failed to write result to file: {ex.Message}");
                }

                Logger.Info($"{prefix} completed: {filtered.Count} results found");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error processing results for {prefix}", ex);
                return "";
            }
        }
    }
}
