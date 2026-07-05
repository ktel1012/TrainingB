using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingB.Core.Services;

namespace TrainingB.Core.Scrapers
{
    public class HN_Find2dXIEN(ChromeDriver driver) : FindBase(driver)
    {
        private const string REGION_KEY = "Northern_2d";
        private const string URL_KEY = "HN_2d_Parlay";

        public override string URL => GetUrlFromConfig(URL_KEY);

        public override string GetChangeList()
        {
            return ExecuteWithRetry(() =>
            {
                try
                {
                    var dic = new Dictionary<string, List<int>>();
                    int threshold = GetThresholdFromConfig(REGION_KEY);

                    // Wait for page to load
                    SafeSleep(6000);

                    var tbl = _driver.FindElement(TablePath);
                    var txt = tbl.Text;

                    if (string.IsNullOrWhiteSpace(txt))
                    {
                        Logger.Warning("HN_Find2dXIEN: Table text is empty");
                        return "";
                    }

                    var ls = new List<string>();
                    foreach (var txt2 in txt.Split('\n'))
                    {
                        if (string.IsNullOrEmpty(txt2)) continue;

                        var val = CleanValue(txt2);
                        ls.Add(val);

                        if (ls.Count >= 4)
                        {
                            if (!TryParseInt(ls[0], out int val0, "HN_Find2dXIEN") ||
                                !TryParseInt(ls[1], out int val1, "HN_Find2dXIEN") ||
                                !TryParseInt(ls[2], out int val2, "HN_Find2dXIEN"))
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

                    return ProcessResults(dic, threshold, "2dXienHN");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in HN_Find2dXIEN.GetChangeList", ex);
                    throw;
                }
            }, "HN_Find2dXIEN.GetChangeList");
        }
    }
}
