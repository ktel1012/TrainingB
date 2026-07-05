using System.Collections.Generic;

namespace TrainingB.Configuration
{
    public class AppSettings
    {
        public string BaseUrl { get; set; } = "https://b2one789.net";
        public int ImplicitWaitMilliseconds { get; set; } = 2000;
        public int DefaultSleepMilliseconds { get; set; } = 1000;
        public int MaxRetries { get; set; } = 3;
        public int RetryDelayMilliseconds { get; set; } = 1000;
        public int DefaultTimeoutSeconds { get; set; } = 10;
        public ChromeDriverSettings ChromeDriverSettings { get; set; } = new();
    }

    public class ChromeDriverSettings
    {
        public bool HideCommandPromptWindow { get; set; } = true;
        public bool IgnoreCertificateErrors { get; set; } = true;
        public bool IgnoreSslErrors { get; set; } = true;
    }

    public class ScraperSettings
    {
        public string BaseUrl { get; set; } = "https://b2one789.net";
        public XPathSettings XPaths { get; set; } = new();
        public Dictionary<string, string> Urls { get; set; } = new();
        public Dictionary<string, int> ThresholdValues { get; set; } = new();
    }

    public class XPathSettings
    {
        public string FirstListPath { get; set; } = "/html/body/div[1]/div/md-content/div[1]/div[2]/div/div[1]/div[2]/div/div[1]/div/div/div[1]";
        public string TablePath { get; set; } = "/html/body/div[1]/div/md-content/div[1]/div[2]/div/div[1]/div[2]/div/div[1]/div/table";
        public string TablePath2 { get; set; } = "/html/body/div[9]/div[2]/div/div[2]";
    }

    public class LoggingSettings
    {
        public string MinimumLevel { get; set; } = "Information";
        public string LogFilePath { get; set; } = "logs/app-.log";
        public int RetainDays { get; set; } = 7;
    }

    public class RootConfiguration
    {
        public AppSettings AppSettings { get; set; } = new();
        public ScraperSettings ScraperSettings { get; set; } = new();
        public LoggingSettings Logging { get; set; } = new();
    }
}
