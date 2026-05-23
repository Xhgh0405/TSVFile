using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TSVFile
{
    public partial class frmTSVFile : Form
    {
        /// <summary>
        /// 單字清單。
        /// </summary>
        private readonly WordCollection _WordList = new WordCollection();

        /// <summary>
        /// 關於視窗。
        /// </summary>
        private readonly frmAbout about = new frmAbout();

        /// <summary>
        /// 上一次搜尋的關鍵字。
        /// </summary>
        private string _lastSearchText = string.Empty;

        /// <summary>
        /// 上一次找到的索引。
        /// </summary>
        private int _lastFoundIndex = -1;

        public frmTSVFile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 表單載入時設定預設訊息。
        /// </summary>
        private void frmTSVFile_Load(object sender, EventArgs e)
        {
            tsslMessage.Text = "請開啟檔案；Ctrl+F 可搜尋資料";
        }

        /// <summary>
        /// 更新 ListView 的內容。
        /// </summary>
        private void UpdateListView()
        {
            lvwWord.BeginUpdate();
            lvwWord.Items.Clear();

            foreach (WordItem item in _WordList)
            {
                ListViewItem lvi = new ListViewItem(item.Word);
                lvi.SubItems.Add(item.Phonogram);
                lvi.SubItems.Add(item.SoundPath);
                lvi.SubItems.Add(item.Explain);
                lvwWord.Items.Add(lvi);
            }

            lvwWord.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            if (lvwWord.Columns[0].Width < 100) lvwWord.Columns[0].Width = 100;
            if (lvwWord.Columns[1].Width < 110) lvwWord.Columns[1].Width = 110;
            if (lvwWord.Columns[2].Width < 150) lvwWord.Columns[2].Width = 150;
            if (lvwWord.Columns[3].Width < 250) lvwWord.Columns[3].Width = 250;

            lvwWord.EndUpdate();
        }

        /// <summary>
        /// 讀取文字檔。自動判斷 UTF-8 / UTF-16，避免音標或中文亂碼。
        /// </summary>
        private string[] ReadAllLinesWithBomDetect(string fileName)
        {
            byte[] data = File.ReadAllBytes(fileName);
            Encoding encoding = Encoding.UTF8;

            if (data.Length >= 2 && data[0] == 0xFF && data[1] == 0xFE)
            {
                encoding = Encoding.Unicode;       // UTF-16 Little Endian
            }
            else if (data.Length >= 2 && data[0] == 0xFE && data[1] == 0xFF)
            {
                encoding = Encoding.BigEndianUnicode;
            }
            else if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            {
                encoding = Encoding.UTF8;
            }

            string text = encoding.GetString(data).TrimStart('\uFEFF');
            return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 開啟 TSV 或文字檔。
        /// </summary>
        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // 第一個篩選條件同時顯示 .txt 與 .tsv，避免開啟視窗只看到 .tsv 看不到 WordCards.txt。
                ofd.Filter = "TSV/TXT files (*.tsv;*.txt)|*.tsv;*.txt|TSV files (*.tsv)|*.tsv|Text files (*.txt)|*.txt|All files (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Title = "開啟檔案";
                ofd.InitialDirectory = Application.StartupPath;

                string defaultFile = Path.Combine(Application.StartupPath, "WordCards.txt");
                if (File.Exists(defaultFile))
                {
                    ofd.FileName = "WordCards.txt";
                }

                DialogResult dr = ofd.ShowDialog(this);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        string[] lines = ReadAllLinesWithBomDetect(ofd.FileName);
                        _WordList.LoadFromStringArray(lines);
                        UpdateListView();
                        _lastSearchText = string.Empty;
                        _lastFoundIndex = -1;
                        tsslMessage.Text = $"{_WordList.Count} 筆資料已成功載入";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"讀取檔案失敗：\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tsslMessage.Text = "讀取檔案失敗";
                    }
                }
            }
        }

        /// <summary>
        /// 結束程式。
        /// </summary>
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 顯示 About 表單。
        /// </summary>
        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            about.ShowDialog(this);
        }


        /// <summary>
        /// 顯示搜尋輸入視窗。
        /// </summary>
        private string ShowFindDialog(string defaultText)
        {
            using (Form dialog = new Form())
            using (Label lblKeyword = new Label())
            using (TextBox txtKeyword = new TextBox())
            using (Button btnFind = new Button())
            using (Button btnCancel = new Button())
            {
                dialog.Text = "搜尋";
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ClientSize = new Size(360, 110);
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.ShowInTaskbar = false;

                lblKeyword.Text = "請輸入要搜尋的文字：";
                lblKeyword.AutoSize = true;
                lblKeyword.Location = new Point(12, 15);

                txtKeyword.Location = new Point(15, 40);
                txtKeyword.Size = new Size(330, 23);
                txtKeyword.Text = defaultText;
                txtKeyword.SelectAll();

                btnFind.Text = "搜尋";
                btnFind.DialogResult = DialogResult.OK;
                btnFind.Location = new Point(185, 73);
                btnFind.Size = new Size(75, 25);

                btnCancel.Text = "取消";
                btnCancel.DialogResult = DialogResult.Cancel;
                btnCancel.Location = new Point(270, 73);
                btnCancel.Size = new Size(75, 25);

                dialog.Controls.Add(lblKeyword);
                dialog.Controls.Add(txtKeyword);
                dialog.Controls.Add(btnFind);
                dialog.Controls.Add(btnCancel);
                dialog.AcceptButton = btnFind;
                dialog.CancelButton = btnCancel;

                return dialog.ShowDialog(this) == DialogResult.OK ? txtKeyword.Text.Trim() : null;
            }
        }

        /// <summary>
        /// 判斷一筆資料是否包含搜尋關鍵字。
        /// </summary>
        private bool IsMatch(WordItem item, string keyword)
        {
            if (item == null || string.IsNullOrWhiteSpace(keyword))
            {
                return false;
            }

            return ContainsText(item.Word, keyword)
                || ContainsText(item.Phonogram, keyword)
                || ContainsText(item.SoundPath, keyword)
                || ContainsText(item.Explain, keyword);
        }

        /// <summary>
        /// 不分大小寫比對文字。
        /// </summary>
        private bool ContainsText(string source, string keyword)
        {
            return !string.IsNullOrEmpty(source)
                && source.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// 從指定索引開始搜尋，找不到時會從頭循環。
        /// </summary>
        private bool FindText(string keyword, int startIndex)
        {
            if (_WordList.Count == 0)
            {
                tsslMessage.Text = "尚未載入檔案";
                return false;
            }

            if (string.IsNullOrWhiteSpace(keyword))
            {
                tsslMessage.Text = "請輸入要搜尋的文字";
                return false;
            }

            if (startIndex < 0 || startIndex >= _WordList.Count)
            {
                startIndex = 0;
            }

            for (int offset = 0; offset < _WordList.Count; offset++)
            {
                int index = (startIndex + offset) % _WordList.Count;
                WordItem item = _WordList[index];

                if (IsMatch(item, keyword))
                {
                    lvwWord.SelectedItems.Clear();
                    lvwWord.Items[index].Selected = true;
                    lvwWord.Items[index].Focused = true;
                    lvwWord.EnsureVisible(index);
                    lvwWord.Focus();

                    _lastSearchText = keyword;
                    _lastFoundIndex = index;
                    tsslMessage.Text = $"找到第 {index + 1} 筆資料";
                    return true;
                }
            }

            tsslMessage.Text = $"找不到：{keyword}";
            return false;
        }

        /// <summary>
        /// 功能表：Ctrl+F 搜尋。
        /// </summary>
        private void tsmiFind_Click(object sender, EventArgs e)
        {
            string keyword = ShowFindDialog(_lastSearchText);
            if (keyword == null)
            {
                return;
            }

            int startIndex = 0;
            if (lvwWord.SelectedItems.Count > 0)
            {
                startIndex = lvwWord.SelectedItems[0].Index;
            }

            FindText(keyword, startIndex);
        }

        /// <summary>
        /// 功能表：F3 搜尋下一筆。
        /// </summary>
        private void tsmiFindNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_lastSearchText))
            {
                tsmiFind_Click(sender, e);
                return;
            }

            FindText(_lastSearchText, _lastFoundIndex + 1);
        }

        /// <summary>
        /// 表單關閉前確認是否離開。
        /// </summary>
        private void frmTSVFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("確定要離開嗎?", "離開", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
