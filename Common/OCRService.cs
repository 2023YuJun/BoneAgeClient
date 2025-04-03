using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace Common
{
    public static class OCRService
    {
        // 定义事件委托
        public delegate void OcrCompletedEventHandler(object sender, OcrCompletedEventArgs e);

        // 定义事件
        public static event OcrCompletedEventHandler OcrCompleted;

        private static string tessDataPath;

        static OCRService()
        {
            // 获取类库项目的 App.config 文件路径
            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            tessDataPath = Path.Combine(solutionPath, "Common", "Tesseract-OCR", "tessdata");
        }

        public static async void OCRSercive(string screenshotPath, Point mousePosition, int screenHeight)
        {
            try
            {
                string ocrResult = await Task.Run(() => PerformOcr(screenshotPath));
                List<string[]> tableData = ParseToTable(ocrResult);

                if (tableData.Count > 0)
                {
                    string ifzValue, RegularExpressions;
                    ConfigHelper.GetSetting("IFZ", out ifzValue);
                    ConfigHelper.GetSetting("RE", out RegularExpressions);
                    int ifz = int.Parse(ifzValue);

                    string rowData = ExtractCenterRowData(tableData, mousePosition, screenHeight, ifz, RegularExpressions);
                    ConfigHelper.SetSetting("DetectData", rowData);
                    // 触发事件并传递结果
                    OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Result = rowData });
                }
                else
                {
                    // 触发事件并传递错误信息
                    OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Error = "未识别到表格数据" });
                }

            }
            catch (Exception ex)
            {
                // 触发事件并传递错误信息
                OcrCompleted?.Invoke(null, new OcrCompletedEventArgs { Error = $"处理过程中发生错误: {ex.Message}" });
            }
        }

        public static string PerformOcr(string imagePath)
        {
            try
            {
                using (var engine = new TesseractEngine(tessDataPath, "chi_sim", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
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


        public static string ExtractCenterRowData(List<string[]> tableData, Point mousePosition, int screenHeight, int ifz, string regexPattern)
        {
            try
            {
                if (tableData.Count == 0)
                {
                    return "未识别到表格数据";
                }
                // 计算每行的高度（假设每行高度相等）
                int lineHeight = screenHeight / tableData.Count;

                // 确定鼠标所在行
                int rowIndex = mousePosition.Y / lineHeight;
                if (rowIndex < 0 || rowIndex >= tableData.Count)
                {
                    return "鼠标位置超出表格范围";
                }

                string[] row = tableData[rowIndex];

                if (row.Length <= ifz)
                {
                    return "列索引超出范围";
                }

                string columnData = row[ifz];

                // 使用正则表达式处理列数据
                Match match = Regex.Match(columnData, regexPattern);
                if (match.Success)
                {
                    return match.Value;
                }
                return columnData;
                //return "未找到匹配的列数据";
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
        public string Error { get; set; }
    }
}
