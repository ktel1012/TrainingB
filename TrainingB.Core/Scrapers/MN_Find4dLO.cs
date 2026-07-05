using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using TrainingB.Core.Services;

namespace TrainingB.Core.Scrapers
{
    public class MN_Find4dLO(ChromeDriver driver) : FindBase(driver)
    {
        private const string REGION_KEY = "Southern_4d";
        private const string URL_KEY = "MN_4d_Lo";

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
                        SafeSleep(_appSettings.DefaultSleepMilliseconds);

                        var tbl1 = _driver.FindElement(TablePath);
                        var txt = tbl1.Text;

                        if (string.IsNullOrWhiteSpace(txt)) continue;

                        var ls = new List<string>();
                        foreach (var txt2 in txt.Split('\n'))
                        {
                            if (string.IsNullOrEmpty(txt2)) continue;

                            var val = CleanValue(txt2);
                            ls.Add(val);

                            if (ls.Count >= 4)
                            {
                                if (!TryParseInt(ls[0], out int val0, "MN_Find4dLO") ||
                                    !TryParseInt(ls[1], out int val1, "MN_Find4dLO") ||
                                    !TryParseInt(ls[2], out int val2, "MN_Find4dLO"))
                                {
                                    ls = new List<string>();
                                    continue;
                                }

                                int sum = val0 + val1 + val2;
                                if (sum == threshold || (val0 == 0 && val1 == 0 && val2 == 0))
                                {
                                    ls = new List<string>();
                                    continue;
                                }

                                dic[ls[3]] = new List<int> { val0, val1, val2 };
                                ls = new List<string>();
                            }
                        }
                    }

                    return ProcessResults4Items(dic, threshold, "4dLoMN");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in MN_Find4dLO.GetChangeList", ex);
                    throw;
                }
            }, "MN_Find4dLO.GetChangeList");
        }
    }
}
