using System.Collections.Generic;
using ImageGrabber.Main.ImageSearch.Objects.QueryObjects;
using ImageGrabber.Main.ImageSearch.Objects.Results;

namespace ImageGrabber.Main.ImageSearch.Interface
{
    /// <summary>
    /// Descirbes the query functionality for returning a list of image urls for a given topic
    /// </summary>
    public interface IImageUrlGrabber
    {
        List<ImageQueryResult> GetImageUrlList(ImageQueryConfig config, bool isDebug = false);
    }
}