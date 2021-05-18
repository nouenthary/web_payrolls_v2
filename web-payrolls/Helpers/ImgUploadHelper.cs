using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace web_payrolls.Helpers
{
    public class ImgUploadHelper
    {
        private static Dictionary<string, ImageCodecInfo> _encoders;
        private static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (_encoders == null)
                {
                    _encoders = new Dictionary<string, ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (_encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (var codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        _encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return _encoders;
            }
        }
        public Bitmap ResizeImage(Image image, int width, int height)
        {
            //a holder for the result
            var result = new Bitmap(width, height);

            //use a graphics object to draw the resized image into the bitmap
            using (var graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }
        public string UploadImage(HttpFileCollectionBase sourceFile, string companyCodeTobeFileName, string destinationPath, int MaxWith, int MaxHeight, int resolutionImg)
        {
            HttpPostedFileBase file = sourceFile[0];
            bool isPngFormat = Path.GetExtension(file.FileName.ToLower()) == ".png";
            var miliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string fileName = string.Format("{0}.jpg", companyCodeTobeFileName);
            var image = ResizeImage(Image.FromStream(file.InputStream), MaxWith, MaxHeight);
            SaveJpeg(Path.Combine(destinationPath, fileName), image, resolutionImg, isPngFormat);
      
            return fileName;
        }
        public void SaveJpeg(string path, Image image, int quality, bool isPngFormat)
        {
            //create an encoder parameter for the image quality
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            //get the jpeg codec

            var jpegCodec = isPngFormat ? GetEncoderInfo("image/png") : GetEncoderInfo("image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            var encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters

            image.Save(path, jpegCodec, encoderParams);
        }
        public ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            var lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }
        public void DeleteFileImage(string File_Path_From_Database)
        {
            // Use for ASP server Path
            string Del_Path = HttpContext.Current.Server.MapPath(File_Path_From_Database);
            try
            {
                if (File.Exists(Del_Path) == true)
                {
                    File.Delete(Del_Path);
                }
            }
            catch (Exception) { }
        }
    }
}