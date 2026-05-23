using System;
using System.Linq;

namespace TSVFile
{
    public class WordItem
    {
        public string Word { get; set; }
        public string Phonogram { get; set; }
        public string SoundPath { get; set; }
        public string Explain { get; set; }

        /// <summary>
        /// 建構子：將一行 TSV 單字資料轉成 WordItem。
        /// 欄位格式：Word \t Phonogram \t SoundPath \t Explain...
        /// </summary>
        /// <param name="str">單行的單字資料</param>
        public WordItem(string str)
        {
            Word = string.Empty;
            Phonogram = string.Empty;
            SoundPath = string.Empty;
            Explain = string.Empty;

            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }

            string[] strLists = str.Split('\t');

            if (strLists.Length >= 1)
            {
                Word = strLists[0].Trim();
            }

            if (strLists.Length >= 2)
            {
                Phonogram = strLists[1].Trim();
            }

            if (strLists.Length >= 3)
            {
                SoundPath = strLists[2].Trim();
            }

            if (strLists.Length >= 4)
            {
                Explain = string.Join(Environment.NewLine, strLists.Skip(3)).Trim();
            }
        }

        /// <summary>
        /// 覆寫 ToString()，ListBox 或除錯時會直接顯示單字。
        /// </summary>
        /// <returns>單字</returns>
        public override string ToString()
        {
            return Word;
        }
    }
}
