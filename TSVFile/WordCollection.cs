using System.Collections.ObjectModel;

namespace TSVFile
{
    public class WordCollection : Collection<WordItem>
    {
        /// <summary>
        /// 從字串陣列載入資料。
        /// </summary>
        /// <param name="lines">輸入的單字字串陣列</param>
        public void LoadFromStringArray(string[] lines)
        {
            this.Clear();

            if (lines == null)
            {
                return;
            }

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                WordItem item = new WordItem(line);

                if (!string.IsNullOrWhiteSpace(item.Word))
                {
                    this.Add(item);
                }
            }
        }
    }
}
