using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Tesseract;

namespace Common
{
    public static class ImageProcessService
    {
        public static Bitmap ConvertToGrayscale(Bitmap bitmap)
        {
            Bitmap grayScaleBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            using (Graphics graphics = Graphics.FromImage(grayScaleBitmap))
            {
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                    new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                    new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix);

                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttributes);
            }
            return grayScaleBitmap;
        }

        /// <summary>
        /// 对传入的灰度图像进行 Gamma 校正
        /// </summary>
        /// <param name="bitmap">输入的 Bitmap 图像</param>
        /// <returns>处理后的 Bitmap 图像</returns>
        public static Bitmap ApplyGammaCorrection(Bitmap bitmap)
        {

            double gamma = 2.0;

            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);

            // 预计算查找表，避免每次计算重复的 Math.Pow
            byte[] gammaLUT = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                gammaLUT[i] = (byte)Math.Min(255, (int)(255 * Math.Pow(i / 255.0, gamma)));
            }

            // 对图像的每个像素进行 gamma 校正
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color original = bitmap.GetPixel(x, y);
                    byte newVal = gammaLUT[original.R];
                    Color newColor = Color.FromArgb(newVal, newVal, newVal);
                    result.SetPixel(x, y, newColor);
                }
            }
            return result;
        }

        /// <summary>
        /// 对指定路径的图像进行固定阈值二值化处理（假设输入图像已为灰度图），
        /// 并将结果直接覆盖保存到原路径。
        /// 为了避免文件被占用，使用 MemoryStream 保存图像数据到临时文件后替换原图。
        /// </summary>
        /// <param name="imagePath">图像文件的完整路径</param>
        /// <param name="threshold">固定阈值，默认值128</param>
        public static void BinarizeImageInPlace(string imagePath, byte threshold = 128)
        {
            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentNullException(nameof(imagePath));
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("未找到指定的图像文件。", imagePath);

            Bitmap output = null;
            try
            {
                using (Bitmap input = new Bitmap(imagePath))
                {
                    int width = input.Width;
                    int height = input.Height;
                    output = new Bitmap(width, height, input.PixelFormat);
                    Rectangle rect = new Rectangle(0, 0, width, height);

                    // 锁定位图数据，提高处理效率
                    BitmapData dataIn = input.LockBits(rect, ImageLockMode.ReadOnly, input.PixelFormat);
                    BitmapData dataOut = output.LockBits(rect, ImageLockMode.WriteOnly, input.PixelFormat);

                    int pixelSize;
                    if (input.PixelFormat == PixelFormat.Format24bppRgb)
                        pixelSize = 3;
                    else if (input.PixelFormat == PixelFormat.Format32bppArgb ||
                             input.PixelFormat == PixelFormat.Format32bppRgb)
                        pixelSize = 4;
                    else
                    {
                        input.UnlockBits(dataIn);
                        output.UnlockBits(dataOut);
                        throw new NotSupportedException("只支持 24bpp 和 32bpp 图像格式。");
                    }

                    unsafe
                    {
                        byte* ptrIn = (byte*)dataIn.Scan0;
                        byte* ptrOut = (byte*)dataOut.Scan0;
                        int strideIn = dataIn.Stride;
                        int strideOut = dataOut.Stride;

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                int indexIn = y * strideIn + x * pixelSize;
                                int indexOut = y * strideOut + x * pixelSize;
                                // 假设输入图像已经经过灰度处理，任一通道都相同
                                byte pixelValue = ptrIn[indexIn];
                                byte binValue = pixelValue < threshold ? (byte)0 : (byte)255;
                                for (int i = 0; i < pixelSize; i++)
                                {
                                    ptrOut[indexOut + i] = binValue;
                                }
                            }
                        }
                    }
                    input.UnlockBits(dataIn);
                    output.UnlockBits(dataOut);
                }

                // 使用 MemoryStream 保存图像数据到临时文件，确保图像句柄已经关闭
                string tempFilePath = imagePath + ".tmp";
                using (MemoryStream ms = new MemoryStream())
                {
                    // 保存为 PNG 格式，这里也可以根据需要选择其他格式
                    output.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    File.WriteAllBytes(tempFilePath, ms.ToArray());
                }
                output.Dispose();
                output = null;

                // 由于有可能图像文件还被占用，可以尝试调用 GC.WaitForPendingFinalizers
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // 删除原文件，再将临时文件重命名为原文件名
                File.Delete(imagePath);
                File.Move(tempFilePath, imagePath);
            }
            catch (Exception ex)
            {
                // 如果失败，确保释放资源，并抛出异常
                output?.Dispose();
                throw new InvalidOperationException("二值化处理失败: " + ex.Message, ex);
            }
        }

        
        public static Rectangle ExtractAndSaveTableRegion(string imagePath)
        {
            try
            {
                Mat image = new Mat(imagePath, ImreadModes.Grayscale);
                //Mat enhancedImage = ApplyAdaptiveEnhancement(image);
                Rectangle tableRect = DetectTableRegion(image);
                if (tableRect.IsEmpty) return Rectangle.Empty;
                Mat finalResult = ProcessTableRegion(image, tableRect);
                //Mat invertImage = InvertImage(enhancedImage);
                //Mat finalResult = ProcessTableRegion(invertImage, tableRect);
                CvInvoke.Imwrite(imagePath, finalResult);
                return tableRect;
            }
            catch (Exception ex)
            {
                return Rectangle.Empty;
            }
        }

        public static Rectangle ProcessColorfulTableScreenshot(string imagePath)
        {
            try
            {
                // 1. 读取图像并进行颜色增强
                Mat colorImage = new Mat(imagePath, ImreadModes.Color);
                CvInvoke.Imwrite(imagePath, colorImage);

                // 2. 颜色空间转换与饱和度增强（增强彩色文字）
                Mat hsvImage = new Mat();
                CvInvoke.CvtColor(colorImage, hsvImage, ColorConversion.Bgr2Hsv);

                // 分离HSV通道并增强饱和度
                Mat[] hsvChannels = hsvImage.Split();
                Mat saturation = hsvChannels[1];
                CvInvoke.Multiply(saturation, new ScalarArray(2.0), saturation); // 饱和度加倍

                // 合并增强后的通道
                CvInvoke.Merge(new VectorOfMat(hsvChannels), hsvImage);
                CvInvoke.CvtColor(hsvImage, colorImage, ColorConversion.Hsv2Bgr);

                CvInvoke.Imwrite(imagePath, colorImage);        // 这里增强效果挺好的

                // 3. 生成高对比度灰度图像（核心算法）
                Mat grayBasic = new Mat();
                //Mat highContrastGray = GenerateHighContrastGray(colorImage);
                CvInvoke.CvtColor(colorImage, grayBasic, ColorConversion.Bgr2Gray);
                CvInvoke.Imwrite(imagePath, grayBasic);

                // 4. 自适应对比度增强
                Mat enhancedImage = ApplyAdaptiveEnhancement(grayBasic);

                CvInvoke.Imwrite(imagePath, enhancedImage);

                // 5. 表格区域检测与处理（沿用原有逻辑）
                Rectangle tableRect = DetectTableRegion(enhancedImage);
                if (tableRect.IsEmpty) return Rectangle.Empty;

                // 6. 最终处理并保存
                Mat finalResult = ProcessTableRegion(enhancedImage, tableRect);
                CvInvoke.Imwrite(imagePath, finalResult);

                return tableRect;
            }
            catch (Exception ex)
            {
                return Rectangle.Empty;
            }
        }

        // 生成高对比度灰度图像的核心方法
        private static Mat GenerateHighContrastGray(Mat colorImage)
        {
            // 分离颜色通道
            Mat[] channels = colorImage.Split();
            Mat blue = channels[0];
            Mat green = channels[1];
            Mat red = channels[2];

            // 方法一：最大通道差异法
            Mat maxChannel = new Mat();
            CvInvoke.Max(red, green, maxChannel);
            CvInvoke.Max(maxChannel, blue, maxChannel);

            Mat minChannel = new Mat();
            CvInvoke.Min(red, green, minChannel);
            CvInvoke.Min(minChannel, blue, minChannel);

            // 计算通道差异（强化颜色对比）
            Mat channelDiff = new Mat();
            CvInvoke.Subtract(maxChannel, minChannel, channelDiff);

            // 方法二：感知对比度加权
            Mat weightedGray = new Mat();
            float[] weights = { 0.1f, 0.1f, 0.8f }; // 增强冷色系（蓝色）
            CvInvoke.Transform(colorImage, weightedGray, new Matrix<float>(new float[,] { { weights[0], weights[1], weights[2], 0 } }));

            // 融合两种方法
            Mat hybridGray = new Mat();
            CvInvoke.AddWeighted(channelDiff, 0.7, weightedGray, 0.3, 0, hybridGray);

            // 归一化处理
            CvInvoke.Normalize(hybridGray, hybridGray, 0, 255, NormType.MinMax);

            return hybridGray;
        }

        // 自适应对比度增强
        private static Mat ApplyAdaptiveEnhancement(Mat grayImage)
        {
            // 动态Gamma校正（基于图像亮度分布）
            double median = GetImageMedian(grayImage);
            double gamma = median < 100 ? 0.5 : (median > 150 ? 2.0 : 1.0);

            Mat gammaCorrected = new Mat();
            grayImage.ConvertTo(gammaCorrected, DepthType.Cv32F, 1.0 / 255);
            CvInvoke.Pow(gammaCorrected, gamma, gammaCorrected);
            gammaCorrected.ConvertTo(gammaCorrected, DepthType.Cv8U, 255);

            Mat equalized = new Mat();
            CvInvoke.EqualizeHist(gammaCorrected, equalized);

            return equalized;
        }

        // 辅助方法：计算图像中值亮度
        private static double GetImageMedian(Mat grayImage)
        {
            byte[] data = new byte[grayImage.Width * grayImage.Height];
            grayImage.CopyTo(data);
            Array.Sort(data);
            return data[data.Length / 2];
        }

        // 表格区域检测（优化版）
        private static Rectangle DetectTableRegion(Mat enhancedImage)
        {
            // 使用Canny边缘检测
            Mat edges = new Mat();
            CvInvoke.Canny(enhancedImage, edges, 100, 200);

            // 改进的轮廓查找策略
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(edges, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            // 多条件筛选轮廓
            double minArea = 800000;
            double maxArea = 900000;
            double maxAspectRatio = 4;

            Rectangle bestRect = Rectangle.Empty;
            for (int i = 0; i < contours.Size; i++)
            {
                Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);
                double aspect = (double)rect.Width / rect.Height;

                // 计算面积
                int area = rect.Width * rect.Height;
                if (area > minArea && area < maxArea && aspect < maxAspectRatio)
                {
                    if (area > bestRect.Width * bestRect.Height)
                        bestRect = rect;
                }
            }

            return bestRect;
        }

        // 反相处理方法
        public static Mat InvertImage(Mat input)
        {
            Mat output = new Mat();
            CvInvoke.BitwiseNot(input, output);  // 使用位运算取反实现高效反相
            return output;
        }

        // 表格区域后处理
        private static Mat ProcessTableRegion(Mat image, Rectangle rect)
        {
            Mat tableRegion = new Mat(image, rect);

            // 自适应阈值处理
            Mat thresholded = new Mat();
            CvInvoke.AdaptiveThreshold(tableRegion, thresholded,
                255,
                AdaptiveThresholdType.GaussianC,
                ThresholdType.BinaryInv,
                19,    // 根据表格单元格大小调整
                10);  // 增强对比度

            return thresholded;
        }
    }
}