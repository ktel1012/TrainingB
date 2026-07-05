using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using TrainingB.Configuration;

namespace TrainingB.Services
{
    public static class WebDriverHelper
    {
        public static IWebElement? FindElementSafe(this IWebDriver driver, By by, int? timeoutSeconds = null)
        {
            try
            {
                timeoutSeconds ??= ConfigurationManager.Config.AppSettings.DefaultTimeoutSeconds;
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds.Value));
                return wait.Until(drv =>
                {
                    try
                    {
                        var element = drv.FindElement(by);
                        return element.Displayed ? element : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Warning($"Element not found within {timeoutSeconds} seconds: {by}");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error finding element: {by}", ex);
                return null;
            }
        }

        public static bool ClickElementSafe(this IWebDriver driver, By by, int? timeoutSeconds = null)
        {
            try
            {
                var element = driver.FindElementSafe(by, timeoutSeconds);
                if (element != null)
                {
                    element.Click();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error clicking element: {by}", ex);
                return false;
            }
        }

        public static string GetTextSafe(this IWebDriver driver, By by, int? timeoutSeconds = null)
        {
            try
            {
                var element = driver.FindElementSafe(by, timeoutSeconds);
                return element?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting text from element: {by}", ex);
                return string.Empty;
            }
        }

        public static void NavigateWithRetry(this ChromeDriver driver, string url, int maxRetries = 3)
        {
            int attempt = 0;
            Exception? lastException = null;
            var config = ConfigurationManager.Config.AppSettings;

            while (attempt < maxRetries)
            {
                try
                {
                    attempt++;
                    Logger.Debug($"Navigating to {url} (Attempt {attempt}/{maxRetries})");
                    driver.Navigate().GoToUrl(url);
                    return;
                }
                catch (WebDriverException ex)
                {
                    lastException = ex;
                    Logger.Warning($"Navigation failed on attempt {attempt}: {ex.Message}");

                    if (attempt < maxRetries)
                    {
                        Thread.Sleep(config.DefaultSleepMilliseconds);
                    }
                }
            }

            Logger.Error($"Navigation to {url} failed after {maxRetries} attempts", lastException);
            throw new InvalidOperationException($"Failed to navigate to {url}", lastException);
        }

        public static ChromeDriver CreateChromeDriver(AppSettings? settings = null)
        {
            try
            {
                settings ??= ConfigurationManager.Config.AppSettings;

                ChromeOptions options = new ChromeOptions();

                if (settings.ChromeDriverSettings.IgnoreCertificateErrors)
                    options.AddArgument("--ignore-certificate-errors");

                if (settings.ChromeDriverSettings.IgnoreSslErrors)
                    options.AddArgument("--ignore-ssl-errors");

                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = settings.ChromeDriverSettings.HideCommandPromptWindow;

                var driver = new ChromeDriver(service, options);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(settings.ImplicitWaitMilliseconds);

                Logger.Info("ChromeDriver created successfully");
                return driver;
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to create ChromeDriver", ex);
                throw;
            }
        }
    }
}
