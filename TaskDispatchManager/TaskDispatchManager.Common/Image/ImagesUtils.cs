using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// 图片处理工具类
    /// </summary>
    /// <remarks>冯瑞 2011-11-14 11:16:10</remarks>
    public class ImagesUtils
    {
        #region 缩放图
        /// <summary>
        /// 生成缩放图
        /// </summary>
        /// <param name="imageWidth">宽度</param>
        /// <param name="imageHeight">高度</param>
        /// <param name="filePath">源文件路径</param>
        /// <param name="rndNum"></param>
        /// <returns></returns>
        public static string MakeImageZoomIN(int imageWidth, int imageHeight, string filePath, int rndNum)
        {
            try
            {
                Image newImage = ToImageZoom(imageWidth, imageHeight, Image.FromFile(filePath));

                string filename = filePath.Substring(0, filePath.LastIndexOf("\\") + 1) + rndNum.ToString();
                string exename = Path.GetExtension(filePath);
                string savename = filename + exename;

                newImage.Save(savename, GetCodecInfo("image/jpeg"), GetParameters("100"));

                newImage.Dispose();
                return savename;
            }
            catch { return ""; }
        }

        /// <summary>
        /// 生成缩放图
        /// </summary>
        /// <param name="imageWidth">宽度</param>
        /// <param name="imageHeight">高度</param>
        /// <param name="filePath">源文件路径</param>
        /// <param name="rndNum"></param>
        /// <returns></returns>
        public static string MakeImageZoom(int imageWidth, int imageHeight, string filePath, int rndNum)
        {
            try
            {
                Image srcImage = Image.FromFile(filePath);
                if (srcImage.Width != imageWidth || srcImage.Height != imageHeight)
                {
                    Image newImage = ToImageZoom(imageWidth, imageHeight, Image.FromFile(filePath));

                    string filename = filePath.Substring(0, filePath.LastIndexOf("\\") + 1) + rndNum.ToString();
                    string exename = Path.GetExtension(filePath);
                    string savename = filename + exename;
                    EncoderParameters parameters = new EncoderParameters(1);
                    parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ic = null;
                    foreach (ImageCodecInfo codec in codecs)
                    {
                        if (codec.MimeType == "image/jpeg")
                        {
                            ic = codec;
                        }
                    }

                    newImage.Save(savename, ic, parameters);
                    //newImage.Save(savename, GetCodecInfo("image/jpeg"), GetParameters("100"));

                    newImage.Dispose();
                    return savename;
                }
                else
                {
                    string filename = filePath.Substring(0, filePath.LastIndexOf("\\") + 1) + rndNum.ToString();
                    string exename = Path.GetExtension(filePath);
                    string savename = filename + exename;
                    srcImage.Save(savename);
                    return savename;
                }
            }
            catch { return ""; }
        }

        /// <summary>
        /// 生成缩放图
        /// </summary>
        /// <param name="imageWidth">宽度</param>
        /// <param name="imageHeight">高度</param>
        /// <param name="filePath">源文件路径</param>
        /// <param name="rndNum"></param>
        /// <returns></returns>
        public static string MakeImageZoomFlag(int imageWidth, int imageHeight, string filePath, int rndNum, bool flag)
        {
            try
            {
                Image srcImage = Image.FromFile(filePath);
                if (srcImage.Width != imageWidth || srcImage.Height != imageHeight)
                {
                    Image newImage = ToImageZoom(imageWidth, imageHeight, Image.FromFile(filePath));

                    string filename = filePath.Substring(0, filePath.LastIndexOf("\\") + 1) + rndNum.ToString();
                    string exename = Path.GetExtension(filePath);
                    string savename = filename + exename;
                    EncoderParameters parameters = new EncoderParameters(1);
                    parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)80);
                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ic = null;
                    foreach (ImageCodecInfo codec in codecs)
                    {
                        if (codec.MimeType == "image/jpeg")
                        {
                            ic = codec;
                        }
                    }

                    newImage.Save(savename, ic, parameters);
                    //newImage.Save(savename, GetCodecInfo("image/jpeg"), GetParameters("100"));

                    newImage.Dispose();

                    return savename;
                }
                else
                {
                    string filename = filePath.Substring(0, filePath.LastIndexOf("\\") + 1) + rndNum.ToString();
                    string exename = Path.GetExtension(filePath);
                    string savename = filename + exename;
                    srcImage.Save(savename);
                    return savename;
                }
            }
            catch { return ""; }
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        private static Image ToImageZoom(int imageWidth, int imageHeight, Image image)
        {
            System.Drawing.Image newImage = new Bitmap(imageWidth, imageHeight, PixelFormat.Format32bppPArgb);
            Graphics g = Graphics.FromImage(newImage);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.High;

            g.DrawImage(image, new RectangleF(0, 0, imageWidth, imageHeight),
                new RectangleF(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            g.Dispose();

            return newImage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strQulity"></param>
        /// <returns></returns>
        private static EncoderParameters GetParameters(string strQulity)
        {
            System.Drawing.Imaging.Encoder myEncoder;
            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters;
            myEncoderParameters = new EncoderParameters(1);

            long x = long.Parse(strQulity);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, x);
            myEncoderParameters.Param[0] = myEncoderParameter;
            return myEncoderParameters;
        }

        #endregion

        #region  生成缩略图

        ///<summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param>     
        public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, out string outthumbnailPath)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
             new Rectangle(x, y, ow, oh),
             GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                outthumbnailPath = thumbnailPath;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        #endregion

        #region 转换

        /// <summary>
        /// 得到图像解码参数
        /// </summary>
        /// <param name="qulity">图片质量（1-100）</param>
        /// <returns></returns>
        /// <remarks>2013-2-25 BY 田小岐</remarks>
        public static EncoderParameters GetEncoderParameters(long qulity)
        {
            System.Drawing.Imaging.Encoder myEncoder;
            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters;
            myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, qulity);
            myEncoderParameters.Param[0] = myEncoderParameter;
            return myEncoderParameters;
        }

        /// <summary>
        /// 得到图片信息代码
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        /// <remarks>2013-2-25 BY 田小岐</remarks>
        public static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        ///// <summary>
        ///// Image转换成byte[]数组
        ///// </summary>
        ///// <param name="imageIn"></param>
        ///// <returns></returns>
        //public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        //{
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //    if (imageIn.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
        //    {
        //        imageIn.Save(ms, GetImageCodecInfo("image/jpeg"), GetEncoderParameters(100));
        //    }
        //    else if (imageIn.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
        //    {
        //        imageIn.Save(ms, GetImageCodecInfo("image/png"), GetEncoderParameters(100));
        //    }
        //    else if (imageIn.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid)
        //    {
        //        imageIn.Save(ms, GetImageCodecInfo("image/gif"), GetEncoderParameters(100));
        //    }
        //    else
        //    {
        //        imageIn.Save(ms, GetImageCodecInfo("image/png"), GetEncoderParameters(100));
        //    }

        //    return ms.ToArray();
        //}

        /// <summary>
        /// Image转换成byte[]数组
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, imageIn.RawFormat);
            return ms.ToArray();
        }

        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn);
            System.Drawing.Image outImage = System.Drawing.Image.FromStream(ms);
            return outImage;
        }

        //
        /// <summary>
        /// 根据图片路径返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>返回的字节流</returns>
        public static byte[] GetImageByte(string imagePath)
        {
            using (FileStream files = new FileStream(imagePath, FileMode.Open))
            {
                byte[] imgByte = new byte[files.Length];
                files.Read(imgByte, 0, imgByte.Length);
                files.Close();
                return imgByte;
            }
        }

        /// <summary>
        /// 根据图片路径返回Image对象
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns></returns>
        /// <remarks>Ralf 2013-01-16</remarks>
        public static Image GetImageByPath(string imagePath)
        {
            byte[] img = GetImageByte(imagePath);
            Image outImg = ByteArrayToImage(img);
            return outImg;
        }

        #endregion

        #region 旋转

        #endregion

        #region 图片加水印

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="watermarkText"></param>
        /// <param name="savePath"></param>
        public static void AddImageSignPic(System.Drawing.Image img, string watermarkText, string savePath)
        {
            using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(img))
            {
                System.Drawing.Font fontWater = new Font("黑体", 10);
                System.Drawing.Brush brushWater = new SolidBrush(Color.Black);
                gWater.DrawString(watermarkText, fontWater, brushWater, (img.Width / 2) + 10, (img.Height / 2) + 10);
                gWater.Dispose();
                //img.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                img.Save(savePath);
            }
        }
        /// <summary>
        /// 图片加水印
        /// </summary>
        /// <param name="img">原始图片</param>
        /// <param name="fileName">原始图片文件名(文件名+扩展名 eg:123.jpg)</param>
        /// <param name="waterMark">水印图片</param>
        /// <param name="waterMarkPosition">水印位置（九宫格，可接受1-9的数字，对应水印位置）</param>
        /// <param name="quality">质量（1-100的数字）</param>
        /// <param name="waterMarkTransparency">水印透明度（1-10的数字）</param>
        /// <returns>加水印后的图片流</returns>
        /// <remarks>Ralf 2012-03-31</remarks>
        public static byte[] AddImageSignPic(Image img, string fileName, Image waterMark, int waterMarkPosition, int quality, int waterMarkTransparency)
        {
            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (waterMarkTransparency >= 1 && waterMarkTransparency <= 10)
            {
                transparency = (waterMarkTransparency / 10.0F);
            }

            float[][] colorMatrixElements = {
                                                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 0.0f, 0.0f, transparency, 0.0f},
                                                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                                            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (waterMarkPosition)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (waterMark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (waterMark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (waterMark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (waterMark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (waterMark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (waterMark.Width));
                    ypos = (int)((img.Height * (float).50) - (waterMark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - waterMark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (waterMark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - waterMark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (waterMark.Width));
                    ypos = (int)((img.Height * (float).99) - waterMark.Height);
                    break;
            }

            g.DrawImage(waterMark, new Rectangle(xpos, ypos, waterMark.Width, waterMark.Height), 0, 0, waterMark.Width, waterMark.Height, GraphicsUnit.Pixel, imageAttributes);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                {
                    ici = codec;
                }
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 80;
            }
            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            MemoryStream ms = new MemoryStream();

            if (ici != null)
            {
                img.Save(ms, ici, encoderParams);
            }
            else
            {
                img.Save(ms, GetFormat(fileName));
            }

            byte[] buffer = ms.ToArray();

            g.Dispose();
            g = null;
            imageAttributes.Dispose();
            imageAttributes = null;
            ms.Close();
            ms.Dispose();
            ms = null;

            return buffer;
        }

        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        private static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                    goto case "jpeg";
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        #endregion

        #region 正方型裁剪并缩放 (http://www.cnblogs.com/wu-jian/archive/2011/02/21/1959382.html)
        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary>
        /// <remarks>吴剑 2010-11-23</remarks>
        /// <param name="postedFile">原图HttpPostedFile对象</param>
        /// <param name="fileSaveUrl">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(System.Web.HttpPostedFile postedFile, string fileSaveUrl, int side, int quality)
        {
            //创建目录
            string dir = Path.GetDirectoryName(fileSaveUrl);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(postedFile.InputStream, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= side && initImage.Height <= side)
            {
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //原始图片的宽、高
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                //非正方型先裁剪为正方型
                if (initWidth != initHeight)
                {
                    //截图对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //宽大于高的横图
                    if (initWidth > initHeight)
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                        Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                        //画图
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽
                        initWidth = initHeight;
                    }
                    //高大于宽的竖图
                    else
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                        Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                        //画图
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置高
                        initHeight = initWidth;
                    }

                    //将截图对象赋给原图
                    initImage = (System.Drawing.Image)pickedImage.Clone();
                    //释放截图资源
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }

                //缩略图对象
                System.Drawing.Image resultImage = new System.Drawing.Bitmap(side, side);
                System.Drawing.Graphics resultG = System.Drawing.Graphics.FromImage(resultImage);
                //设置质量
                resultG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                resultG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //用指定背景色清空画布
                resultG.Clear(Color.White);
                //绘制缩略图
                resultG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initWidth, initHeight), System.Drawing.GraphicsUnit.Pixel);

                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                EncoderParameters ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                //保存缩略图
                resultImage.Save(fileSaveUrl, ici, ep);

                //释放关键质量控制所用资源
                ep.Dispose();

                //释放缩略图资源
                resultG.Dispose();
                resultImage.Dispose();

                //释放原始图片资源
                initImage.Dispose();
            }
        }

        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary>
        /// <remarks>吴剑 2010-11-23</remarks>
        /// <param name="postedFile">原图HttpPostedFile对象</param>
        /// <param name="fileSaveUrl">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(System.IO.Stream fromFile, string fileSaveUrl, int side, int quality)
        {
            //创建目录
            string dir = Path.GetDirectoryName(fileSaveUrl);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= side && initImage.Height <= side)
            {
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //原始图片的宽、高
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                //非正方型先裁剪为正方型
                if (initWidth != initHeight)
                {
                    //截图对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //宽大于高的横图
                    if (initWidth > initHeight)
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                        Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                        //画图
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽
                        initWidth = initHeight;
                    }
                    //高大于宽的竖图
                    else
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                        Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                        //画图
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置高
                        initHeight = initWidth;
                    }

                    //将截图对象赋给原图
                    initImage = (System.Drawing.Image)pickedImage.Clone();
                    //释放截图资源
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }

                //缩略图对象
                System.Drawing.Image resultImage = new System.Drawing.Bitmap(side, side);
                System.Drawing.Graphics resultG = System.Drawing.Graphics.FromImage(resultImage);
                //设置质量
                resultG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                resultG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //用指定背景色清空画布
                resultG.Clear(Color.White);
                //绘制缩略图
                resultG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initWidth, initHeight), System.Drawing.GraphicsUnit.Pixel);

                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                EncoderParameters ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                //保存缩略图
                resultImage.Save(fileSaveUrl, ici, ep);

                //释放关键质量控制所用资源
                ep.Dispose();

                //释放缩略图资源
                resultG.Dispose();
                resultImage.Dispose();

                //释放原始图片资源
                initImage.Dispose();
            }
        }
        #endregion

        #region 固定模版裁剪并缩放 (http://www.cnblogs.com/wu-jian/archive/2011/02/21/1959382.html)
        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <remarks>吴剑 2010-11-15</remarks>
        /// <param name="postedFile">原图HttpPostedFile对象</param>
        /// <param name="fileSaveUrl">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(System.Web.HttpPostedFile postedFile, string fileSaveUrl, int maxWidth, int maxHeight, int quality)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(postedFile.InputStream, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //模版的宽高比例
                double templateRate = (double)maxWidth / maxHeight;
                //原图片的宽高比例
                double initRate = (double)initImage.Width / initImage.Height;

                //原图与模版比例相等，直接缩放
                if (templateRate == initRate)
                {
                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //定位
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                    //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象实例化
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        //裁剪源定位
                        fromR.X = 0;
                        fromR.Y = (int)Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)Math.Floor(initImage.Width / templateRate);

                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)Math.Floor(initImage.Width / templateRate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        fromR.X = (int)Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //裁剪
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                    //保存缩略图
                    templateImage.Save(fileSaveUrl, ici, ep);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    templateG.Dispose();
                    templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
            }

            //释放资源
            initImage.Dispose();
        }
        #endregion

        #region 等比缩放 (http://www.cnblogs.com/wu-jian/archive/2011/02/21/1959382.html)
        /// <summary>
        /// 图片等比缩放
        /// </summary>
        /// <remarks>吴剑 2011-01-21</remarks>
        /// <param name="postedFile">原图HttpPostedFile对象</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度</param>
        /// <param name="targetHeight">指定的最大高度</param>
        /// <param name="watermarkText">水印文字(为""表示不使用水印)</param>
        /// <param name="watermarkImage">水印图片路径(为""表示不使用水印)</param>
        public static void ZoomAuto(System.Web.HttpPostedFile postedFile, string savePath, System.Double targetWidth, System.Double targetHeight, string watermarkText, string watermarkImage)
        {
            //创建目录
            string dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(postedFile.InputStream, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= targetWidth && initImage.Height <= targetHeight)
            {
                //文字水印
                if (watermarkText != "")
                {
                    using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(initImage))
                    {
                        System.Drawing.Font fontWater = new Font("黑体", 10);
                        System.Drawing.Brush brushWater = new SolidBrush(Color.White);
                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                        gWater.Dispose();
                    }
                }

                //透明图片水印
                if (watermarkImage != "")
                {
                    if (File.Exists(watermarkImage))
                    {
                        //获取水印图片
                        using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (initImage.Width >= wrImage.Width && initImage.Height >= wrImage.Height)
                            {
                                Graphics gWater = Graphics.FromImage(initImage);

                                //透明属性
                                ImageAttributes imgAttributes = new ImageAttributes();
                                ColorMap colorMap = new ColorMap();
                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remapTable = { colorMap };
                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                float[][] colorMatrixElements = {
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };

                                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                gWater.DrawImage(wrImage, new Rectangle(initImage.Width - wrImage.Width, initImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);

                                gWater.Dispose();
                            }
                            wrImage.Dispose();
                        }
                    }
                }

                //保存
                initImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //缩略图宽、高计算
                double newWidth = initImage.Width;
                double newHeight = initImage.Height;

                //宽大于高或宽等于高（横图或正方）
                if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
                {
                    //如果宽大于模版
                    if (initImage.Width > targetWidth)
                    {
                        //宽按模版，高按比例缩放
                        newWidth = targetWidth;
                        newHeight = initImage.Height * (targetWidth / initImage.Width);
                    }
                }
                //高大于宽（竖图）
                else
                {
                    //如果高大于模版
                    if (initImage.Height > targetHeight)
                    {
                        //高按模版，宽按比例缩放
                        newHeight = targetHeight;
                        newWidth = initImage.Width * (targetHeight / initImage.Height);
                    }
                }

                //生成新图
                //新建一个bmp图片
                System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                //新建一个画板
                System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);

                //设置质量
                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //置背景色
                newG.Clear(Color.White);
                //画图
                newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);

                //文字水印
                if (watermarkText != "")
                {
                    using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(newImage))
                    {
                        System.Drawing.Font fontWater = new Font("宋体", 10);
                        System.Drawing.Brush brushWater = new SolidBrush(Color.White);
                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                        gWater.Dispose();
                    }
                }

                //透明图片水印
                if (watermarkImage != "")
                {
                    if (File.Exists(watermarkImage))
                    {
                        //获取水印图片
                        using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (newImage.Width >= wrImage.Width && newImage.Height >= wrImage.Height)
                            {
                                Graphics gWater = Graphics.FromImage(newImage);

                                //透明属性
                                ImageAttributes imgAttributes = new ImageAttributes();
                                ColorMap colorMap = new ColorMap();
                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remapTable = { colorMap };
                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                float[][] colorMatrixElements = {
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };

                                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                gWater.DrawImage(wrImage, new Rectangle(newImage.Width - wrImage.Width, newImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                                gWater.Dispose();
                            }
                            wrImage.Dispose();
                        }
                    }
                }

                //保存缩略图
                newImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                //释放资源
                newG.Dispose();
                newImage.Dispose();
                initImage.Dispose();
            }
        }

        #endregion

        #region 其它 (http://www.cnblogs.com/wu-jian/archive/2011/02/21/1959382.html)
        /// <summary>
        /// 判断文件类型是否为WEB格式图片
        /// (注：JPG,GIF,BMP,PNG)
        /// </summary>
        /// <param name="contentType">HttpPostedFile.ContentType</param>
        /// <returns></returns>
        public static bool IsWebImage(string contentType)
        {
            if (contentType == "image/pjpeg" || contentType == "image/jpeg" || contentType == "image/gif" || contentType == "image/bmp" || contentType == "image/png")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        //最近由于需要C#图片处理的相关应用，遂在网上参考一下，写出如下代码作为总结。
        //通常需要将图片缩放到指定宽度与高度的缩略图，若只对原图片进行缩放，当图片前后高宽比例不同时，缩放后的图片就会拉伸变形。可以采取的办法是先按照目标图片的高宽比进行裁剪，然后缩放到目标图片的高宽就ok了。
        //原理很简单：
        //1）首先根据目标图片的高宽比计算原图片的裁剪矩形区域
        //2）然后将该矩形区域内的图像拷贝到目标高宽的矩形区域中，下面上代码：
        /// <summary>  
        /// 图片转换（裁剪并缩放）  
        /// </summary>  
        /// <param name="ASrcFileName">源文件名称</param>  
        /// <param name="ADestFileName">目标文件名称</param>  
        /// <param name="AWidth">转换后的宽度（像素）</param>  
        /// <param name="AHeight">转换后的高度（像素）</param>  
        /// <param name="AQuality">保存质量（取值在1-100之间）</param>  
        public static void DoConvert(string ASrcFileName, string ADestFileName, int AWidth, int AHeight, int AQuality)
        {
            Image ASrcImg = Image.FromFile(ASrcFileName);
            if (ASrcImg.Width <= AWidth && ASrcImg.Height <= AHeight)
            {//图片的高宽均小于目标高宽，直接保存  
                ASrcImg.Save(ADestFileName);
                return;
            }
            double ADestRate = AWidth * 1.0 / AHeight;
            double ASrcRate = ASrcImg.Width * 1.0 / ASrcImg.Height;
            //裁剪后的宽度  
            double ACutWidth = ASrcRate > ADestRate ? (ASrcImg.Height * ADestRate) : ASrcImg.Width;
            //裁剪后的高度  
            double ACutHeight = ASrcRate > ADestRate ? ASrcImg.Height : (ASrcImg.Width / ADestRate);
            //待裁剪的矩形区域，根据原图片的中心进行裁剪  
            Rectangle AFromRect = new Rectangle(Convert.ToInt32((ASrcImg.Width - ACutWidth) / 2), Convert.ToInt32((ASrcImg.Height - ACutHeight) / 2), (int)ACutWidth, (int)ACutHeight);
            //目标矩形区域  
            Rectangle AToRect = new Rectangle(0, 0, AWidth, AHeight);

            Image ADestImg = new Bitmap(AWidth, AHeight);
            Graphics ADestGraph = Graphics.FromImage(ADestImg);
            ADestGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            ADestGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            ADestGraph.DrawImage(ASrcImg, AToRect, AFromRect, GraphicsUnit.Pixel);

            //获取系统image/jpeg编码信息  
            ImageCodecInfo[] AInfos = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo AInfo = null;
            foreach (ImageCodecInfo i in AInfos)
            {
                if (i.MimeType == "image/jpeg")
                {
                    AInfo = i;
                    break;
                }
            }
            //设置转换后图片质量参数  
            EncoderParameters AParams = new EncoderParameters(1);
            AParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)AQuality);
            //保存  
            ADestImg.Save(ADestFileName, AInfo, AParams);
        }

        #region GetPicThumbnail


        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="newHeight">高度</param>
        /// <param name="newWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string dFile, int newHeight = 0, int newWidth = 0, int flag = 100)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size sourceSize = new Size(iSource.Width, iSource.Height);

            if (sourceSize.Width > newHeight || sourceSize.Width > newWidth) //将**改成c#中的或者操作符号
            {
                if ((sourceSize.Width * newHeight) > (sourceSize.Height * newWidth))
                {
                    sW = newWidth;
                    sH = (newWidth * sourceSize.Height) / sourceSize.Width;
                }
                else
                {
                    sH = newHeight;
                    sW = (sourceSize.Width * newHeight) / sourceSize.Height;
                }
            }

            else
            {
                sW = sourceSize.Width;
                sH = sourceSize.Height;
            }

            if (newHeight == 0 || newWidth == 0)
            {
                newHeight = sourceSize.Height * newHeight;
                newWidth = sourceSize.Width * newWidth;
            }


            Bitmap ob = new Bitmap(newWidth, newHeight);

            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);

            g.CompositingQuality = CompositingQuality.HighQuality;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((newWidth - sW) / 2, (newHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            iSource.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }

                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                //iSource.Dispose();
                ob.Dispose();

            }
        }



        public static bool GetPicThumbnail2(string sourceFile, string targetFile, decimal proportion = 1, int flag = 100)
        {

            int newHeight = 0; int newWidth = 0;

            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sourceFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size sourceSize = new Size(iSource.Width, iSource.Height);

            //if (sourceSize.Width > newHeight || sourceSize.Width > newWidth) //将**改成c#中的或者操作符号
            //{
            //    if ((sourceSize.Width * newHeight) > (sourceSize.Height * newWidth))
            //    {
            //        sW = newWidth;
            //        sH = (newWidth * sourceSize.Height) / sourceSize.Width;
            //    }
            //    else
            //    {
            //        sH = newHeight;
            //        sW = (sourceSize.Width * newHeight) / sourceSize.Height;
            //    }
            //}

            //else
            //{
            //    sW = sourceSize.Width;
            //    sH = sourceSize.Height;
            //}

            sW = sourceSize.Width;
            sH = sourceSize.Height;

            newHeight = Convert.ToInt32(sourceSize.Height * proportion);
            newWidth = Convert.ToInt32(sourceSize.Width * proportion);


            Bitmap ob = new Bitmap(newWidth, newHeight);

            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);

            g.CompositingQuality = CompositingQuality.HighQuality;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((newWidth - sW) / 2, (newHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            iSource.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }

                if (jpegICIinfo != null)
                {
                    ob.Save(targetFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(targetFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                //iSource.Dispose();
                ob.Dispose();

            }
        }

        #endregion

        public static void CropImage(int originWidth, int originHeight, int startX, int startY, int width, int height, int finalWidth, int finalHeight, string tempImageFilePath, string saveFilePath, bool allowTransparent)
        {

            var sourceImage = Image.FromFile(tempImageFilePath);
            var ratio = (double)sourceImage.Width / originWidth;
            var srcRect = new Rectangle((int)(startX * ratio), (int)(startY * ratio), (int)(width * ratio), (int)(height * ratio));
            var destRect = new Rectangle(0, 0, finalWidth, finalHeight);

            var finalImage = new Bitmap(finalWidth, finalHeight);
            var graphics = Graphics.FromImage(finalImage);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; //设置高质量插值法
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; //设置高质量,低速度呈现平滑程度
            graphics.Clear(Color.Transparent); //清空画布并以透明背景色填充
            graphics.DrawImage(sourceImage, destRect, srcRect, GraphicsUnit.Pixel);

            if (allowTransparent)
            {
                finalImage.Save(saveFilePath, ImageFormat.Png);
            }
            else
            {
                var qualityValue = 100; //图像质量 1-100的范围
                var codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo encoder = null;
                foreach (var codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                    {
                        encoder = codec;
                    }
                }
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)qualityValue);

                finalImage.Save(saveFilePath, encoder, encoderParameters);
            }

            finalImage.Dispose();
            sourceImage.Dispose();

            if (File.Exists(tempImageFilePath))
            {
                try
                {
                    File.Delete(tempImageFilePath);
                }
                catch { }
            }
        }


        public static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;

            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(outPath, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    iSource.Save(outPath, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
            }
        }


        public static void ChangeImageSize(double maxWidth, double maxHeight, string originFilePath, string saveFilePath, bool allowTransparent)
        {
            var sourceImage = Image.FromFile(originFilePath);

            var finalWidth = maxWidth;
            var finalHeight = maxHeight;
            var ratio = (double)sourceImage.Width / (double)sourceImage.Height;
            if (maxWidth / maxHeight > ratio)
            {
                finalWidth = ratio * maxHeight;
            }
            else
            {
                finalHeight = maxWidth / ratio;
            }

            if (sourceImage.Width < maxWidth && sourceImage.Height < maxHeight)
            {
                finalHeight = sourceImage.Height;
                finalWidth = sourceImage.Width;
            }

            var srcRect = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
            var destRect = new Rectangle(0, 0, (int)finalWidth, (int)finalHeight);

            var finalImage = new Bitmap((int)finalWidth, (int)finalHeight);
            var graphics = Graphics.FromImage(finalImage);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; //设置高质量插值法
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; //设置高质量,低速度呈现平滑程度
            graphics.Clear(Color.Transparent); //清空画布并以透明背景色填充
            graphics.DrawImage(sourceImage, destRect, srcRect, GraphicsUnit.Pixel);

            sourceImage.Dispose();

            if (allowTransparent)
            {
                finalImage.Save(saveFilePath, ImageFormat.Png);
            }
            else
            {
                var qualityValue = 100; //图像质量 1-100的范围
                var codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo encoder = null;
                foreach (var codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                    {
                        encoder = codec;
                    }
                }
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)qualityValue);

                finalImage.Save(saveFilePath, encoder, encoderParameters);
            }

            finalImage.Dispose();
            sourceImage.Dispose();

            //if (File.Exists(originFilePath))
            //{
            //    try
            //    {
            //        File.Delete(originFilePath);
            //    }
            //    catch
            //    {

            //    }
            //}
        }

    }
}
