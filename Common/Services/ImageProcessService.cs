using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Config;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Tesseract;

namespace Common.Services
{
    public static class ImageProcessService
    {    
        // 表格区域检测（优化版）
        public static Rectangle DetectTableRegion(Mat enhancedImage)
        {
            // 使用Canny边缘检测
            Mat edges = new Mat();
            CvInvoke.Canny(enhancedImage, edges, 100, 200);

            // 改进的轮廓查找策略
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(edges, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            // 多条件筛选轮廓
            var settings = ConfigProvider.Settings.GetConfig();
            double minArea = settings.MinArea;
            double maxArea = settings.MaxArea;
            double maxAspectRatio = settings.MaxAspectRatio;

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

        // 表格区域后处理
        public static Mat ProcessTableRegion(Mat image, Rectangle rect)
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