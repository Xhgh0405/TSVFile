using System;
using System.Reflection;
using System.Windows.Forms;

namespace TSVFile
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();

            lblProductName.Text = GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title, "TSV資料檔讀取程式");
            lblVersion.Text = $"版本：{assemblyName.Version}";
            lblCopyright.Text = GetAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright, "Copyright © 2026");
            lblCompany.Text = GetAssemblyAttribute<AssemblyCompanyAttribute>(a => a.Company, "元智大學");
            txtDescription.Text = GetAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description, "上課練習(7) - TSV資料檔讀取程式");
        }

        private string GetAssemblyAttribute<T>(Func<T, string> value, string defaultValue) where T : Attribute
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(T), false);

            if (attributes.Length > 0)
            {
                string text = value((T)attributes[0]);

                if (!string.IsNullOrWhiteSpace(text))
                {
                    return text;
                }
            }

            return defaultValue;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
