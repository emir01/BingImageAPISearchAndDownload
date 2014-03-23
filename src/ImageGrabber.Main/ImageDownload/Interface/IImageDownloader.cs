using System.Collections.Generic;
using ImageGrabber.Main.ImageDownload.Objects;
using ImageGrabber.Main.ImageSearch.Objects.Results;

namespace ImageGrabber.Main.ImageDownload.Interface
{
    internal interface IImageDownloader
    {
        bool DownloadImages(List<ImageQueryResult> imageList, DownloadOptions downloadOptions, bool debugMode);
    }
}