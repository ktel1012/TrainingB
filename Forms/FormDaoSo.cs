using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingB.Forms
{
    public partial class FormDaoSo : Form
    {
        public FormDaoSo()
        {
            InitializeComponent();
        }

        private void FromDaoSo_Load(object sender, EventArgs e)
        {

        }

        private void btn22_Click(object sender, EventArgs e)
        {
            Dao2Trong2();
        }
        void Dao2Trong2()
        {
            if (!OnValidateData(2))
            {
                return;
            }
            string sSo = string.Empty;
            sSo = txtSoDao.Text;

            List<string> ls = new List<string>();
            int i = 0;
            int j = 0;

            foreach (var s in sSo)
            {
                i += 1;
                foreach (var s1 in sSo)
                {
                    j += 1;
                    if ((sSo.IndexOf(s) == sSo.IndexOf(s1)) && (i == j))
                        continue;
                    string l = $"{s}{s1}";
                    ls.Add(l);
                }
                j = 0;
            }
            List<string> ls1 = new List<string>();
            foreach (var tm in ls)
            {
                if (!ls1.Contains(tm))
                    ls1.Add(tm);
            }
            string kq = string.Empty;
            foreach (var tm in ls1)
            {
                if (kq == "")
                {
                    kq = $"{tm}";
                }
                else
                {
                    kq = $"{kq},{tm}";
                }
            }
            txtKQ.Text = kq;
            lblStatus.Text = $"{ls1.Count} so";
        }
        void Dao2Trong3()
        {
            if (!OnValidateData(3))
            {
                return;
            }
            string sSo = string.Empty;
            sSo = txtSoDao.Text;

            List<string> ls = new List<string>();
            int i = 0;
            int j = 0;

            foreach (var s in sSo)
            {
                i += 1;
                foreach (var s1 in sSo)
                {
                    j += 1;
                    if ((sSo.IndexOf(s) == sSo.IndexOf(s1)) && (i == j))
                        continue;
                    string l = $"{s}{s1}";
                    ls.Add(l);
                }
                j = 0;
            }
            List<string> ls1 = new List<string>();
            foreach (var tm in ls)
            {
                if (!ls1.Contains(tm))
                    ls1.Add(tm);
            }
            string kq = string.Empty;
            foreach (var tm in ls1)
            {
                if (kq == "")
                {
                    kq = $"{tm}";
                }
                else
                {
                    kq = $"{kq},{tm}";
                }
            }
            txtKQ.Text = kq;
            lblStatus.Text = "{ls1.Count} so";
        }

        void Dao2Trong4()
        {
            if (!OnValidateData(4))
            {
                return;
            }
            string sSo = string.Empty;
            sSo = txtSoDao.Text;

            List<string> ls = new List<string>();
            int i = 0;
            int j = 0;
            foreach (var s in sSo)
            {
                i += 1;
                foreach (var s1 in sSo)
                {
                    j += 1;
                    if ((sSo.IndexOf(s) == sSo.IndexOf(s1) && i == j))
                        continue;
                    string l = $"{s}{s1}";
                    ls.Add(l);
                }
                j = 0;
            }
            List<string> ls1 = new List<string>();
            foreach (var tm in ls)
            {
                if (!ls1.Contains(tm))
                    ls1.Add(tm);
            }
            string kq = string.Empty;
            foreach (var tm in ls1)
            {
                if (kq == "")
                {
                    kq = $"{tm}";
                }
                else
                {
                    kq = $"{kq},{tm}";
                }
            }
            txtKQ.Text = kq;
            lblStatus.Text = $"{ls1.Count} so";
        }

        void Dao3Trong3()
        {
            if (!OnValidateData(3))
            {
                return;
            }
            string sSo = string.Empty;
            sSo = txtSoDao.Text;

            List<string> ls = new List<string>();
            int i = 0;
            int j = 0;
            int k = 0;

            foreach (var s in sSo)
            {
                i += 1;
                foreach (var s1 in sSo)
                {
                    j += 1;
                    foreach (var s2 in sSo)
                    {
                        k += 1;
                        if ((sSo.IndexOf(s) == sSo.IndexOf(s1)) && (i == j))
                            continue;
                        if ((sSo.IndexOf(s1) == sSo.IndexOf(s2)) && (j == k))
                            continue;
                        if ((sSo.IndexOf(s) == sSo.IndexOf(s2)) && (i == k))
                            continue;
                        string l = $"{s}{s1}{s2}";
                        ls.Add(l);
                    }
                    k = 0;
                }
                j = 0;
            }
            List<string> ls1 = new List<string>();
            foreach (var tm in ls)
            {
                if (!ls1.Contains(tm))
                    ls1.Add(tm);
            }
            string kq = string.Empty;
            foreach (var tm in ls1)
            {
                if (kq == "")
                {
                    kq = $"{tm}";
                }
                else
                {
                    kq = $"{kq},{tm}";
                }
            }
            txtKQ.Text = kq;
            lblStatus.Text = $"{ls1.Count} so";
        }

        void Dao3Trong4()
        {
            if (!OnValidateData(4))
            {
                return;
            }
            string sSo = string.Empty;
            sSo = txtSoDao.Text;

            List<string> ls = new List<string>();
            int i = 0;
            int j = 0;
            int k = 0;

            foreach (var s in sSo)
            {
                i += 1;
                foreach (var s1 in sSo)
                {
                    j += 1;
                    foreach (var s2 in sSo)
                    {
                        k += 1;
                        if ((sSo.IndexOf(s) == sSo.IndexOf(s1)) && (i == j))
                            continue;
                        if ((sSo.IndexOf(s1) == sSo.IndexOf(s2)) && (j == k))
                            continue;
                        if ((sSo.IndexOf(s) == sSo.IndexOf(s2)) && (i == k))
                            continue;
                        string l = $"{s}{s1}{s2}";
                        ls.Add(l);
                    }
                    k = 0;
                }
                j = 0;
            }
            List<string> ls1 = new List<string>();
            foreach (var tm in ls)
            {
                if (!ls1.Contains(tm))
                    ls1.Add(tm);
            }
            string kq = string.Empty;
            foreach (var tm in ls1)
            {
                if (kq == "")
                {
                    kq = $"{tm}";
                }
                else
                {
                    kq = $"{kq},{tm}";
                }
            }
            txtKQ.Text = kq;
            lblStatus.Text = $"{ls1.Count} so";
        }

        void Dao3Trong5()
        {
            if (!OnValidateData(5))
            {
                return;
            }
            string sSo = string.Empty;
            sSo = txtSoDao.Text;

            List<string> ls = new List<string>();
            int i = 0;
            int j = 0;
            int k = 0;

            foreach (var s in sSo)
            {
                i += 1;
                foreach (var s1 in sSo)
                {
                    j += 1;
                    foreach (var s2 in sSo)
                    {
                        k += 1;
                        if ((sSo.IndexOf(s) == sSo.IndexOf(s1)) && (i == j))
                            continue;
                        if ((sSo.IndexOf(s1) == sSo.IndexOf(s2)) && (j == k))
                            continue;
                        if ((sSo.IndexOf(s) == sSo.IndexOf(s2)) && (i == k))
                            continue;
                        string l = $"{s}{s1}{s2}";
                        ls.Add(l);
                    }
                    k = 0;
                }
                j = 0;
            }
            List<string> ls1 = new List<string>();
            foreach (var tm in ls)
            {
                if (!ls1.Contains(tm))
                    ls1.Add(tm);
            }
            string kq = string.Empty;
            foreach (var tm in ls1)
            {
                if (kq == "")
                {
                    kq = $"{tm}";
                }
                else
                {
                    kq = $"{kq},{tm}";
                }
            }
            txtKQ.Text = kq;
            lblStatus.Text = $"{ls1.Count} so";
        }

        void Dao4Trong4()
        {
            if (!OnValidateData(4))
            {
                return;
            }
            string sSo = string.Empty;
            sSo = txtSoDao.Text;

            List<string> ls = new List<string>();
            int i = 0;
            int j = 0;
            int k = 0;
            int m = 0;

            foreach (var s in sSo)
            {
                i += 1;
                foreach (var s1 in sSo)
                {
                    j += 1;
                    foreach (var s2 in sSo)
                    {
                        k += 1;
                        foreach (var s3 in sSo)
                        {
                            m += 1;
                            if ((sSo.IndexOf(s) == sSo.IndexOf(s1)) && (i == j))
                                continue;
                            if ((sSo.IndexOf(s) == sSo.IndexOf(s2)) && (i == k))
                                continue;
                            if ((sSo.IndexOf(s) == sSo.IndexOf(s3)) && (i == m))
                                continue;
                            if ((sSo.IndexOf(s1) == sSo.IndexOf(s2)) && (j == k))
                                continue;
                            if ((sSo.IndexOf(s1) == sSo.IndexOf(s3)) && (j == m))
                                continue;
                            if ((sSo.IndexOf(s2) == sSo.IndexOf(s3)) && (k == m))
                                continue;
                            string l = $"{s}{s1}{s2}{s3}";
                            ls.Add(l);
                        }
                        m = 0;
                    }
                    k = 0;
                }
                j = 0;
            }

            List<string> ls1 = new List<string>();
            foreach (var tm in ls)
            {
                if (!ls1.Contains(tm))
                    ls1.Add(tm);
            }
            string kq = string.Empty;
            foreach (var tm in ls1)
            {
                if (kq == "")
                {
                    kq = $"{tm}";
                }
                else
                {
                    kq = $"{kq},{tm}";
                }

            }
            txtKQ.Text = kq;
            lblStatus.Text = $"{ls1.Count} so";
        }
        private bool OnValidateData(int iLength)
        {
            //Label_KQ.Text = "Ket qua:";
            //editor2.Text = "";


            if (string.IsNullOrWhiteSpace(txtKQ.Text))
            {
                lblStatus.Text = "Hay nhap so de dao!";
                return false;
            }
            int n;
            bool isNumeric = int.TryParse(txtSoDao.Text.Trim(), out n);
            if (!isNumeric)
            {
                lblStatus.Text = "So dao chua dung!";
                return false;
            }
            if (txtSoDao.Text.Length != iLength)
            {
                lblStatus.Text = "So dao chua dung!";
                return false;
            }
            return true;
        }

        private void btn23_Click(object sender, EventArgs e)
        {
            Dao2Trong3();
        }

        private void btn33_Click(object sender, EventArgs e)
        {
            Dao3Trong3();
        }

        private void btn44_Click(object sender, EventArgs e)
        {
            Dao4Trong4();
        }

        private void btn34_Click(object sender, EventArgs e)
        {
            Dao3Trong4();
        }

        private void btn35_Click(object sender, EventArgs e)
        {
            Dao3Trong5();
        }

        private void btn24_Click(object sender, EventArgs e)
        {
            Dao2Trong4();
        }
    }
}
