using Common.Config;
using Emgu.CV;
using Emgu.CV.Reg;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using Tesseract;

namespace Common.Services
{
    public static class OCRService
    {
        // 定义事件委托
        public delegate void OcrCompletedEventHandler(object sender, OcrCompletedEventArgs e);

        // 定义事件
        public static event OcrCompletedEventHandler OcrCompleted;

        private static string tessDataPath;

        // 静态Tesseract引擎（线程安全通过锁保证）
        private static TesseractEngine _engine;
        private static readonly object _engineLock = new object();
        static OCRService()
        {
            // 获取类库项目的 App.config 文件路径
            tessDataPath = Path.Combine(ConfigProvider.solutionRoot, "Common", "Tesseract-OCR", "tessdata");
            lock (_engineLock)
            {
                _engine = new TesseractEngine(tessDataPath, "chi_sim", EngineMode.Default);
                _engine.SetVariable("tessedit_char_whitelist", "0123456789 ");
            }
        }
        public static void PerformDirectOcr(Mat image)
        {
            try
            {
                string rawText = PerformOcr(image);
                var settings = ConfigProvider.Settings.GetConfig();

                Match match = Regex.Match(rawText, settings.RE);
                string result = String.Empty;
                if (match.Success)
                {
                    result = match.Value.ToString();
                    ConfigProvider.Settings.UpdateConfig(s =>{ s.DetectData = result; s.DetectStatus = true; });
                    OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Result = result });
                }
                else
                {
                    result = "未匹配到内容";
                    ConfigProvider.Settings.UpdateConfig(s => { s.DetectData = result; s.DetectStatus = false; });
                    OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Tip = result });
                }
            }
            catch (Exception ex)
            {
                ConfigProvider.Settings.UpdateConfig(s => { s.DetectStatus = false; });
                OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Error = $"处理过程中发生错误: {ex.Message}" });
                throw;
            }
        }

        public static async void OCRServiceProcessing(Mat image, Point mousePosition, int screenHeight, Rectangle tableRegion)
        {
            try
            {
                string ocrResult = await Task.Run(() => PerformOcr(image));
                List<string[]> tableData = ParseToTable(ocrResult);

                if (tableData.Count > 0)
                {
                    // 计算鼠标位置相对于裁剪后的表格区域的位置
                    Point relativeMousePosition = new Point(
                        mousePosition.X - tableRegion.X,
                        mousePosition.Y - tableRegion.Y
                    );

                    string rowData = ExtractCenterRowData(tableData, relativeMousePosition, screenHeight);
                    var settings = ConfigProvider.Settings.GetConfig(); 
                    Match match = Regex.Match(rowData, settings.RE);
                    ConfigProvider.Settings.UpdateConfig(s => { s.DetectData = rowData; });
                    if (string.IsNullOrEmpty(rowData))
                    {
                        ConfigProvider.Settings.UpdateConfig(s => { s.DetectStatus = false; });
                    }
                    // 触发事件并传递结果
                    if (match.Success) {
                        ConfigProvider.Settings.UpdateConfig(s => { s.DetectStatus = true; });
                        OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Result = rowData });
                    }
                    else
                    {
                        ConfigProvider.Settings.UpdateConfig(s => { s.DetectStatus = false; });
                        OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Tip = rowData });
                    }
                }
                else
                {
                    // 触发事件并传递错误信息
                    ConfigProvider.Settings.UpdateConfig(s => { s.DetectStatus = false; });
                    OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Tip = "未识别到表格数据" });
                }

            }
            catch (Exception ex)
            {
                // 触发事件并传递错误信息
                ConfigProvider.Settings.UpdateConfig(s => { s.DetectStatus = false; });
                OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Error = $"处理过程中发生错误: {ex.Message}" });
                throw;
            }
        }

        // OCR处理最耗时函数
        public static string PerformOcr(Mat image)
        {
            try
            {
                lock (_engineLock)
                {
                    CvInvoke.Imwrite("temp.png", image);
                    using (var img = Pix.LoadFromFile("temp.png"))
                    {
                        using (var page = _engine.Process(img))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"OCR处理发生错误: {ex.Message}", ex);
            }
        }
        public static List<string[]> ParseToTable(string text)
        {
            List<string[]> tableData = new List<string[]>();
            string[] lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // 使用正则表达式分割多种分隔符（空格、制表符、竖线等）
                    string[] columns = Regex.Split(line.Trim(), @"\s+|\t+|\|+");
                    columns = columns.Where(c => !string.IsNullOrWhiteSpace(c)).ToArray();
                    if (columns.Length > 0)
                    {
                        tableData.Add(columns);
                    }
                }
            }

            return tableData;
        }


        public static string ExtractCenterRowData(List<string[]> tableData, Point mousePosition, int screenHeight)
        {
            try
            {
                var settings = ConfigProvider.Settings.GetConfig();
                int columnToTable = settings.ColumnToTable;
                string regexPattern = settings.RE;
                if (tableData.Count == 0 || screenHeight == 0)
                {
                    return "未识别到表格数据";
                }
                // 计算每行的高度（假设每行高度相等）
                int lineHeight = screenHeight / tableData.Count - 1;

                // 确定鼠标所在行
                int rowIndex = mousePosition.Y / lineHeight;
                if (rowIndex < 0 || rowIndex >= tableData.Count)
                {
                    return "鼠标位置超出表格范围";
                }

                string[] row = tableData[rowIndex];

                if (row.Length <= columnToTable)
                {
                    return "列索引超出范围";
                }

                string columnData = row[columnToTable];
                string result = string.Empty;
                MatchCollection matches = Regex.Matches(columnData, regexPattern);
                if (matches.Count != 0)
                {
                    foreach (Match match in matches)
                    {
                        result += match.Value;
                    }
                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"提取行数据时发生错误: {ex.Message}", ex);
            }
        }
    }

    // 定义事件参数类
    public class OcrCompletedEventArgs : EventArgs
    {
        public string Result { get; set; }
        public string Tip { get; set; }
        public string Error { get; set; }
    }
}
