using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingB.Forms
{
    public partial class FormWinTabs : Form
    {
        private ChromeDriver? _driver;
        public FormWinTabs()
        {
            InitializeComponent();
        }
        public FormWinTabs(ChromeDriver driver)
        {
            InitializeComponent();
            _driver = driver;
        }
        private Dictionary<string, string> _windows = new Dictionary<string, string>();
        private void FormWinTabs_Load(object sender, EventArgs e)
        {
            RefreshItem();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            RefreshItem();
        }
        private void RefreshItem()
        {
            if (_driver == null) return;
            Text = $"Title : {_driver.Title} URL {_driver.Url}";
            listBox1.Items.Clear();
            _windows = new Dictionary<string, string>();
            string currentWindow = _driver.CurrentWindowHandle;
            foreach (string id in _driver.WindowHandles)
            {
                string switchedWindowTitle = _driver.SwitchTo().Window(id).Title;
                switchedWindowTitle = $"{switchedWindowTitle}{_driver.Url}";
                _windows[switchedWindowTitle] = id;
                listBox1.Items.Add(switchedWindowTitle);
            }
            _driver.SwitchTo().Window(currentWindow);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_driver == null || listBox1.SelectedItem == null) return;
            if (!_windows.ContainsKey($"{listBox1.SelectedItem}")) return;
            string id = _windows[$"{listBox1.SelectedItem}"];
            try
            {
                _driver.SwitchTo().Window(id);
            }
            catch { }
            
            Text = $"Title : {_driver.Title} URL {_driver.Url}";
        }
    }
}
