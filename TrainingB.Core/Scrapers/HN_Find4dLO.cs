using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using TrainingB.Core.Services;

namespace TrainingB.Core.Scrapers
{
    public class HN_Find4dLO(ChromeDriver driver) : FindBase(driver)
    {
        private const string REGION_KEY = "Northern_3d";
        private const string URL_KEY = "HN_4d_Lo";

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
                    var b = l.FindElements(By.XPath("button[@type='button']"));
                    SafeSleep(300);

                    var tbl = _driver.FindElement(TablePath2);
                    var bb = tbl.FindElements(By.XPath("button[@type='button']"));

                    foreach (var bb1 in bb)
                    {
                        b[5].Click();
                        bb1.Click();
                        WaitForTableData(TablePath, maxWaitSeconds: 5);

                        var tbl1 = _driver.FindElement(TablePath);
                        var txt = tbl1.Text;

                        if (string.IsNullOrWhiteSpace(txt)) continue;

                        var ls = new List<string>();
                        foreach (var txt2 in txt.Split('\n'))
                        {
                            if (string.IsNullOrEmpty(txt2)) continue;

                            var val = CleanValue(txt2);
                            ls.Add(val);

                            if (ls.Count >= 2)
                            {
                                if (!TryParseInt(ls[1], out int sum, "HN_Find4dLO"))
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

                    return ProcessResults(dic, threshold, "4dLo");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in HN_Find4dLO.GetChangeList", ex);
                    throw;
                }
            }, "HN_Find4dLO.GetChangeList");
        }
    }
}
