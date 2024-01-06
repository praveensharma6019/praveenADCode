using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class ExifReader
    {
        public byte[] SetUpMetadataOnImage(Stream file, string filename)
        {
            BitmapDecoder original = BitmapDecoder.Create(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);
            // create an encoder for the output file
            BitmapEncoder output = null;
            string ext = Path.GetExtension(filename);
            switch (ext)
            {
                case ".png":
                    output = new PngBitmapEncoder();
                    break;
                case ".jpg":
                    output = new JpegBitmapEncoder();
                    break;
                case ".jpeg":
                    output = new JpegBitmapEncoder();
                    break;
            }

            if (original.Frames[0] != null && original.Frames[0].Metadata != null)
            {
                // So, we clone the object since it's frozen.
                BitmapFrame frameCopy = (BitmapFrame)original.Frames[0].Clone();
                BitmapMetadata metadata = original.Frames[0].Metadata.Clone() as BitmapMetadata;

                StripMeta(metadata);

                // finally, we create a new frame that has all of this new metadata, along with the data that was in the original message
                output.Frames.Add(BitmapFrame.Create(frameCopy, frameCopy.Thumbnail, metadata, frameCopy.ColorContexts));
            }

            byte[] data;

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
                if (i == 274 || i == 277 || i == 284 || i == 530 || i == 531 || i == 282 || i == 283 || i == 296) continue;

                string query = "/app1/ifd/exif:{uint=" + i + "}";
                BlankMetaInfo(query, metaData);

                query = "/app1/ifd/exif/subifd:{uint=" + i + "}";
                BlankMetaInfo(query, metaData);

                query = "/ifd/exif:{uint=" + i + "}";
                BlankMetaInfo(query, metaData);

                query = "/ifd/exif/subifd:{uint=" + i + "}";
                BlankMetaInfo(query, metaData);
            }

            for (int i = 0; i < 4; i++)
            {
                string query = "/app1/ifd/gps/{ulong=" + i + "}";
                BlankMetaInfo(query, metaData);
                query = "/ifd/gps/{ulong=" + i + "}";
                BlankMetaInfo(query, metaData);
            }
        }

        private void BlankMetaInfo(string query, BitmapMetadata metaData)
        {
            object obj = metaData.GetQuery(query);
            if (obj != null)
            {
                if (obj is string)
                    metaData.SetQuery(query, string.Empty);
                else
                {
                    ulong dummy;
                    if (ulong.TryParse(obj.ToString(), out dummy))
                    {
                        metaData.SetQuery(query, 0);
                    }

                }
            }
        }
    }
}