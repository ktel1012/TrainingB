using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using TrainingB.Core.Services;

namespace TrainingB.Core.Scrapers
{
    public class HN_Find3dDAU(ChromeDriver driver) : FindBase(driver)
    {
        private const string REGION_KEY = "Northern_3d";
        private const string URL_KEY = "HN_3d_Dau";

        public override string URL => GetUrlFromConfig(URL_KEY);

        public override string GetChangeList()
        {
            return ExecuteWithRetry(() =>
            {
                try
                {
                    var dic = new Dictionary<string, List<int>>();
                    int threshold = GetThresholdFromConfig(REGION_KEY);

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
