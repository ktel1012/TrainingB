using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using TrainingB.Core.Services;

namespace TrainingB.Core.Scrapers
{
    public class MN_Find3dDAU(ChromeDriver driver) : FindBase(driver)
    {
        private const string REGION_KEY = "Southern_3d_Alt";
        private const string URL_KEY = "MN_3d_Dau";

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

                    Logger.Info($"MN_Find3dDAU: Found {l2.Count} buttons to process");

                    int buttonIndex = 0;
                    foreach (var l3 in l2)
                    {
                        buttonIndex++;
                        Logger.Debug($"MN_Find3dDAU: Processing button {buttonIndex}/{l2.Count}");

                        l3.Click();

                        // Wait for table data to load and stabilize (important for Docker/headless)
                        WaitForTableData(TablePath, maxWaitSeconds: 10);

                        var tbl = _driver.FindElement(TablePath);
                        var txt = tbl.Text;

                        int itemsInThisButton = string.IsNullOrWhiteSpace(txt) ? 0 : txt.Split('\n').Length;
                        Logger.Debug($"MN_Find3dDAU: Button {buttonIndex} returned {itemsInThisButton} lines");

                        if (string.IsNullOrWhiteSpace(txt)) continue;

                        var ls = new List<string>();
                        foreach (var txt2 in txt.Split('\n'))
                        {
                            if (string.IsNullOrEmpty(txt2)) continue;

                            var val = CleanValue(txt2);
                            ls.Add(val);

                            if (ls.Count >= 4)
                            {
                                if (!TryParseInt(ls[0], out int val0, "MN_Find3dDAU") ||
                                    !TryParseInt(ls[1], out int val1, "MN_Find3dDAU") ||
                                    !TryParseInt(ls[2], out int val2, "MN_Find3dDAU"))
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

                    Logger.Info($"MN_Find3dDAU: Processed {l2.Count} buttons, found {dic.Count} total items");
                    return ProcessResults(dic, threshold, "3dDau");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in MN_Find3dDAU.GetChangeList", ex);
                    throw;
                }
            }, "MN_Find3dDAU.GetChangeList");
        }
    }
}
