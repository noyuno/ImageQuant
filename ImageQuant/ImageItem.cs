﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuant
{
    public class ImageItem
    {
        public string DestPath;
        public string SourcePath;
        public Image Thumbnail;

        public ImageItem()
        {

        }

        public ImageItem(string destpath,string sourcepath)
        {
            DestPath = destpath;
            SourcePath = sourcepath;
            Thumbnail = FileImaging.GetThumbnail(destpath);
        }

        public ImageItem(string destpath, string sourcepath, Bitmap thumbnail)
        {
            DestPath = destpath;
            SourcePath = sourcepath;
            Thumbnail = thumbnail;
        }
    }
}