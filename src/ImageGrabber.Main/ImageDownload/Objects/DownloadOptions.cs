namespace ImageGrabber.Main.ImageDownload.Objects
{
    /// <summary>
    /// Configuration object for the image download process.
    /// </summary>
    public class DownloadOptions
    {
        #region Configuration Options

        public bool DownloadThumbnails { get; private set; }

        public string ThumbnailPrefix { get; private set; }

        #endregion

        #region Constructor

        private DownloadOptions()
        {

        }

        #endregion

        #region Build

        public static DownloadOptions Build(bool downloadThumbnails = false, string thumbnailPrefix = "t_")
        {
            return new DownloadOptions()
                   {
                       DownloadThumbnails = downloadThumbnails,
                       ThumbnailPrefix = string.IsNullOrWhiteSpace(thumbnailPrefix) ? "t_" : thumbnailPrefix
                   };
        }

        #endregion

    }
}
