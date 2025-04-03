using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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

            double gamma = 2.5;

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

    }
}