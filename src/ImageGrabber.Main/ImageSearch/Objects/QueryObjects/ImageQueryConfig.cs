namespace ImageGrabber.Main.ImageSearch.Objects.QueryObjects
{
    /// <summary>
    /// Image query config contains general query parameters like 
    /// the query and the number of results as well as marketplace specifications and
    /// regional options
    /// </summary>
    public class ImageQueryConfig
    {
        #region Properties

        public string Query { get; set; }

        public int Count { get; set; }

        public ImageSearchOptions SearchOptions { get; set; }

        #endregion

        #region Constructor

        private ImageQueryConfig()
        {
        
        }

        #endregion

        #region Factory Methods

        public static ImageQueryConfig Get(string query, int count,  ImageSearchOptions imageSearchOptions)
        {
            return new ImageQueryConfig
                   {
                       Query = query,
                       Count = count,
                       SearchOptions =  imageSearchOptions
                   };
        }

        #endregion
    }
}
