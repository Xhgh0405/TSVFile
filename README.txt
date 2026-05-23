上課練習(7) - TSV資料檔讀取程式

完成內容：
1. 專案名稱：TSVFile
2. 主表單：frmTSVFile
3. 功能表：File / Open / Exit、Search / Find / Find Next、Help / About
4. ListView 欄位：單字、音標、音檔路徑、解釋
5. 可讀取 .tsv、.txt 檔案
6. 已放入 WordCards.txt，並設定為建置時複製到輸出目錄
7. 支援 UTF-8 與有 BOM 的 Unicode 文字檔，例如 WordCards.txt
8. 關閉程式前會詢問是否離開
9. 新增 Ctrl+F 搜尋功能，可搜尋目前載入資料中的文字

使用方式：
1. 用 Visual Studio 開啟 TSVFile.sln
2. 按 F5 執行
3. 點選 File -> Open
4. 選擇 .txt 或 .tsv 檔案
5. 按 Ctrl+F 輸入關鍵字搜尋
6. 按 F3 可以搜尋下一筆符合的資料

注意：
如果 Visual Studio 顯示 .NET Framework 4.8 未安裝，請安裝 .NET Framework 4.8 Developer Pack，或在專案屬性中改成你電腦已安裝的 .NET Framework 版本。

修正版說明：
- File > Open 預設會同時顯示 .txt 與 .tsv。
- 若 WordCards.txt 已複製到輸出目錄，開啟視窗會預設選到 WordCards.txt。
- 讀檔會自動判斷 UTF-8 / UTF-16，避免 Unicode 文字檔音標亂碼。
- 這版只新增 Ctrl+F 搜尋功能，沒有加入播放聲音功能，避免一般 txt 檔不適用。
