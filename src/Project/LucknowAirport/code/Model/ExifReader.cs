using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sitecore.LucknowAirport.Website.Model
{
    public class ExifReader
    {
        public ExifReader()
        {
        }

        private void BlankMetaInfo(string query, BitmapMetadata metaData)
        {
            ulong dummy;
            object obj = metaData.GetQuery(query);
            if (obj != null)
            {
                if (obj is string)
                {
                    metaData.SetQuery(query, string.Empty);
                    return;
                }
                if (ulong.TryParse(obj.ToString(), out dummy))
                {
                    metaData.SetQuery(query, 0);
                }
            }
        }

        public byte[] SetUpMetadataOnImage(Stream file, string filename)
        {
            byte[] data;
            BitmapDecoder original = BitmapDecoder.Create(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);
            BitmapEncoder output = null;
            string ext = Path.GetExtension(filename);
            if (ext == ".png")
            {
                output = new PngBitmapEncoder();
            }
            else if (ext == ".jpg")
            {
                output = new JpegBitmapEncoder();
            }
            else if (ext == ".jpeg")
            {
                output = new JpegBitmapEncoder();
            }
            if (original.Frames[0] != null && original.Frames[0].Metadata != null)
            {
                BitmapFrame frameCopy = (BitmapFrame)original.Frames[0].Clone();
                BitmapMetadata metadata = original.Frames[0].Metadata.Clone() as BitmapMetadata;
                this.StripMeta(metadata);
                output.Frames.Add(BitmapFrame.Create(frameCopy, frameCopy.Thumbnail, metadata, frameCopy.ColorContexts));
            }
            using (MemoryStream ms = new MemoryStream())
            {
                output.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public void StripMeta(BitmapMetadata metaData)
        {
            for (int i = 270; i < 42016; i++)
            {
                if (i != 274 && i != 277 && i != 284 && i != 530 && i != 531 && i != 282 && i != 283 && i != 296)
                {
                    string query = string.Concat("/app1/ifd/exif:{uint=", i, "}");
                    this.BlankMetaInfo(query, metaData);
                    query = string.Concat("/app1/ifd/exif/subifd:{uint=", i, "}");
                    this.BlankMetaInfo(query, metaData);
                    query = string.Concat("/ifd/exif:{uint=", i, "}");
                    this.BlankMetaInfo(query, metaData);
                    query = string.Concat("/ifd/exif/subifd:{uint=", i, "}");
                    this.BlankMetaInfo(query, metaData);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                string query = string.Concat("/app1/ifd/gps/{ulong=", i, "}");
                this.BlankMetaInfo(query, metaData);
                query = string.Concat("/ifd/gps/{ulong=", i, "}");
                this.BlankMetaInfo(query, metaData);
            }
        }
    }
}