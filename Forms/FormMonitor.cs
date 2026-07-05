using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.ComponentModel;
using TrainingB.Core.Scrapers;
using System.Reflection;
using TrainingB.Core.Configuration;
using TrainingB.Core.Services;

namespace TrainingB.Forms
{
    public partial class FormMonitor : Form
    {
        private ChromeDriver? driver;
        private readonly AppSettings _appSettings;
        private bool _isProcessing = false;

        public FormMonitor()
        {
            InitializeComponent();
            _appSettings = ConfigurationManager.Config.AppSettings;

            try
            {
                InitializeWebDriver();
                Logger.Info("FormMonitor initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to initialize FormMonitor", ex);
                MessageBox.Show($"Failed to initialize: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeWebDriver()
        {
            try
            {
                ChromeOptions options = new ChromeOptions();

                if (_appSettings.ChromeDriverSettings.IgnoreCertificateErrors)
                    options.AddArgument("--ignore-certificate-errors");

                if (_appSettings.ChromeDriverSettings.IgnoreSslErrors)
                    options.AddArgument("--ignore-ssl-errors");

                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = _appSettings.ChromeDriverSettings.HideCommandPromptWindow;

                driver = new ChromeDriver(service, options);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(_appSettings.ImplicitWaitMilliseconds);

                Logger.Info("WebDriver initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to initialize WebDriver", ex);
                throw;
            }
        }

        private void FormMonitor_Load(object sender, EventArgs e)
        {
            //var ls = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsSubclassOf(typeof(FindBase))).ToList();
            //foreach (var l in ls)
            //{
            //    listBox1.Items.Add(l.Name);
            //}
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Closing WebDriver");
                driver?.Quit();
            }
            catch (Exception ex)
            {
                Logger.Error("Error closing WebDriver", ex);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (_isProcessing)
            {
                MessageBox.Show("Processing is already in progress", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _isProcessing = true;
                Cursor = Cursors.WaitCursor;

                await Task.Run(() => ProcessAllScrapers());
            }
            catch (Exception ex)
            {
                Logger.Error("Error in button2_Click", ex);
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isProcessing = false;
                Cursor = Cursors.Default;
            }
        }

        private void ProcessAllScrapers()
        {
            try
            {
                var ls = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsSubclassOf(typeof(FindBase))).ToList();

                foreach (var l in ls)
                {
                    try
                    {
                        if (driver == null)
                        {
                            Logger.Error("Driver is null");
                            return;
                        }

                        FindBase? instance = (FindBase?)Activator.CreateInstance(l, driver);
                        if (instance == null) continue;

                        this.Invoke((Action)(() => txtURL.Text = instance.URL));
                        driver.Url = instance.URL;
                        instance.GetChangeList();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error processing scraper {l.Name}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error in ProcessAllScrapers", ex);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                Logger.Info("Form closing, cleaning up resources");
                driver?.Quit();
                driver?.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error("Error during form closing", ex);
            }

            base.OnClosing(e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (driver == null)
                {
                    MessageBox.Show("WebDriver is not initialized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var frm = new FormWinTabs(driver);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error("Error in btnRefresh_Click", ex);
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task MN_Load()
        {
            await Task.Run(() =>
            {
                try
                {
                    var ls = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsSubclassOf(typeof(FindBase))).ToList();

                    foreach (var l in ls)
                    {
                        if (!l.Name.StartsWith("MN_")) continue;

                        if ((l.Name.Contains("4dLO") && !chk4dloMN.Checked) ||
                            (l.Name.Contains("4dDUOI") && !chk4dduoiMN.Checked))
                            continue;

                        try
                        {
                            if (driver == null) return;

                            this.Invoke((Action)(() => listBox1.Items.Add(l.Name)));

                            FindBase? instance = (FindBase?)Activator.CreateInstance(l, driver);
                            if (instance == null) continue;

                            this.Invoke((Action)(() => txtURL.Text = instance.URL));
                            driver.Url = instance.URL;

                            var ret = instance.GetChangeList();

                            this.Invoke((Action)(() =>
                            {
                                if (ret.StartsWith("2d")) txt2D.Text += ret;
                                if (ret.StartsWith("3d")) txt3D.Text += ret;
                                if (ret.StartsWith("4d")) txt4D.Text += ret;
                            }));

                            Logger.Info($"Completed scraper: {l.Name}");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error in MN_Load for {l.Name}", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in MN_Load", ex);
                }
            });
        }
        private async Task HN_Load()
        {
            await Task.Run(() =>
            {
                try
                {
                    var ls = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsSubclassOf(typeof(FindBase))).ToList();

                    foreach (var l in ls)
                    {
                        if (!l.Name.StartsWith("HN_")) continue;

                        if (l.Name.Contains("4dLO") && !chk4dlo.Checked) continue;

                        try
                        {
                            if (driver == null) return;

                            this.Invoke((Action)(() => listBox1.Items.Add(l.Name)));

                            FindBase? instance = (FindBase?)Activator.CreateInstance(l, driver);
                            if (instance == null) continue;

                            this.Invoke((Action)(() => txtURL.Text = instance.URL));
                            driver.Url = instance.URL;

                            var ret = instance.GetChangeList();

                            this.Invoke((Action)(() =>
                            {
                                if (ret.StartsWith("2d")) txt2D.Text += ret;
                                if (ret.StartsWith("3d")) txt3D.Text += ret;
                                if (ret.StartsWith("4d")) txt4D.Text += ret;
                            }));

                            Logger.Info($"Completed scraper: {l.Name}");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error in HN_Load for {l.Name}", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in HN_Load", ex);
                }
            });
        }

        private async void btnMN_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            try
            {
                _isProcessing = true;
                Cursor = Cursors.WaitCursor;
                await MN_Load();
            }
            catch (Exception ex)
            {
                Logger.Error("Error in btnMN_Click", ex);
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isProcessing = false;
                Cursor = Cursors.Default;
            }
        }

        private async void btnHN_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            try
            {
                _isProcessing = true;
                Cursor = Cursors.WaitCursor;
                await HN_Load();
            }
            catch (Exception ex)
            {
                Logger.Error("Error in btnHN_Click", ex);
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isProcessing = false;
                Cursor = Cursors.Default;
            }
        }

        private void txt4D_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDaoSo_Click(object sender, EventArgs e)
        {
            var frm = new FormDaoSo();
            frm.ShowDialog();
        }

        private async Task LoadScrapersByFilter(string regionPrefix, string filterKeyword)
        {
            await Task.Run(() =>
            {
                try
                {
                    var ls = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(a => a.IsSubclassOf(typeof(FindBase)))
                        .ToList();

                    foreach (var l in ls)
                    {
                        if (!l.Name.StartsWith(regionPrefix)) continue;
                        if (!l.Name.Contains(filterKeyword)) continue;

                        try
                        {
                            if (driver == null) return;

                            this.Invoke((Action)(() => listBox1.Items.Add(l.Name)));

                            FindBase? instance = (FindBase?)Activator.CreateInstance(l, driver);
                            if (instance == null) continue;

                            this.Invoke((Action)(() => txtURL.Text = instance.URL));
                            driver.Url = instance.URL;

                            var ret = instance.GetChangeList();

                            this.Invoke((Action)(() =>
                            {
                                if (ret.StartsWith("4d")) txt4D.Text += ret;
                            }));

                            Logger.Info($"Completed scraper: {l.Name}");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error processing {l.Name}", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error in LoadScrapersByFilter ({regionPrefix}, {filterKeyword})", ex);
                }
            });
        }

        private async void btn4dlo_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            try
            {
                _isProcessing = true;
                Cursor = Cursors.WaitCursor;

                if (chk4dlo.Checked)
                {
                    await LoadScrapersByFilter("HN_", "4dLO");
                }
                if (chk4dloMN.Checked)
                {
                    await LoadScrapersByFilter("MN_", "4dLO");
                }
                if (chk4dduoiMN.Checked)
                {
                    await LoadScrapersByFilter("MN_", "4dDUOI");
                }
                if (chk4DDuoiHN.Checked)
                {
                    await LoadScrapersByFilter("HN_", "4dDUOI");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error in btn4dlo_Click", ex);
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isProcessing = false;
                Cursor = Cursors.Default;
            }
        }
    }
}
