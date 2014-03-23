using ImageGrabber.Main.ImageSearch.Objects.Results;
using ImageGrabber.Main.Utilities;

namespace ImageGrabber.Main.ImageSearch.Utilities
{
    /// <summary>
    /// Maps  bing utility image ressults to internal image grabber results
    /// </summary>
    public class ImageResultMappings
    {
        /// <summary>
        /// Map an ImageResult from the Bing Search Utilities to a internal Image Grabber
        /// ImageQueryResult object
        /// </summary>
        /// <param name="imageResult"></param>
        /// <returns></returns>
        public static ImageQueryResult MapImageQueryResultFromImageResult(ImageResult imageResult)
        {
            var imageQueryResult = new ImageQueryResult()
                                   {
                                       ID = imageResult.ID,

                                       Title = imageResult.Title,

                                       MediaUrl = imageResult.MediaUrl,

                                       SourceUrl = imageResult.SourceUrl,

                                       DisplayUrl = imageResult.DisplayUrl,

                                       Width = imageResult.Width,

                                       Height = imageResult.Height,

                                       FileSize = imageResult.FileSize,

                                       ContentType = imageResult.ContentType,

                                       ThumbnailResult = GetThumbnailResultFromThumbnail(imageResult.Thumbnail)
                                   };

            return imageQueryResult;
        }

        /// <summary>
        /// Map an ImageResult Thumbnail object to the internal Image Grabber ImageThumbnail
        /// </summary>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        private static ImageQueryThumbnailResult GetThumbnailResultFromThumbnail(Thumbnail thumbnail)
        {
            var imageQueryThumbnailResult = new ImageQueryThumbnailResult()
            {
                MediaUrl = thumbnail.MediaUrl,
                ContentType = thumbnail.ContentType,
                Width = thumbnail.Width,
                Height = thumbnail.Height,
                FileSize = thumbnail.FileSize
            };

            return imageQueryThumbnailResult;
        }
    }
}
