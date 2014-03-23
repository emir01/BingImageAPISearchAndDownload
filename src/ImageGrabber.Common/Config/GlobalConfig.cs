using System.Drawing;

namespace ImageGrabber.Common.Config
{
    public static class GlobalConfig
    {
        #region Global Flags

        public static bool _global_useSearchApiSource = true;

        #endregion

        #region Image Search Configuration

        public static string _search_baseUrl = "https://api.datamarket.azure.com/Bing/Search";

        // Must enter Bing Search Account Key Here!
        public static string _search_accountKey = "YOUR_ACCOUNT_KEY_HERE";

        public static string _search_imageUrlGrabberDebugFile = @"C:\tmp\image-query.txt";

        /// <summary>
        /// Constained by the image service as I was not able to get more than 50 results even 
        /// with the correct parameters and value for $top
        /// 
        /// </summary>
        public static int _search_maxQueryCount = 50;

        #endregion

        #region Image Download Configuration

        public static string _download_imageDownloadPath = @"C:\tmp\images\";

        public static string _download_imageThumbDownloadPath = @"C:\tmp\images\thumb\";

        public static string _download_default_save_extension = ".png";

        public static bool _download_run_download = true;

        #endregion

        #region Image Composition Configuration

        /// <summary>
        /// The combined image will be a square, so we use a single fixed composed image dimension
        /// </summary>
        public static int ComposedImageDimension = 400;

        /// <summary>
        /// The default size to which downloaded images will be resized before composing them 
        /// in the large image.
        /// </summary>
        public static Size ComposedImageDefaultSize = new Size(ComposedImageDimension, ComposedImageDimension);

        public static string ComposedImagePath = @"C:\tmp\composed.png";

        #endregion

        #region Serialization

        public static string _serialization_tmpImagesList = @"C:\tmp\images-list-tmp.xml";

        #endregion
    }
}