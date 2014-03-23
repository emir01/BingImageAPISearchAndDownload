namespace ImageGrabber.Main.ImageSearch.Objects.Registries.SearchOptionRegistires
{
    /// <summary>
    /// Registry class for possible image size values for the Image Filters filters
    /// option in the query parameters
    /// </summary>
    public static class ImageFilterSizeValues
    {
        public static string SMALL = "Small";

        public static string MEDIUM = "Medium";
        
        public static string LARGE = "Small";

        /// <summary>
        /// Used when setting actual height in pixel values (unasigned int value)
        /// </summary>
        public static string SPEC_HEIGHT = "Height";

        /// <summary>
        /// Used when setting actual height in pixel values (unasigned int value)
        /// </summary>
        public static string SPEC_WIDTH = "Width";
    }
}
