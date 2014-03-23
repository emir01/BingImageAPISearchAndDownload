using ImageGrabber.Main.ImageSearch.Objects.Registries.SearchOptionRegistires;

namespace ImageGrabber.Main.ImageSearch.Objects.QueryObjects
{
    /// <summary>
    /// Contains a collection of bing image search api common options.
    /// 
    /// Contains the functionality to dynamicly build Image query options.
    /// 
    /// Immutable options object, which once created cannot be internally changed, unless you recreate the object
    /// </summary>
    public class ImageSearchOptions
    {

        #region Properties

        /// <summary>
        /// Adult has a private internal setter as it will be set only
        /// in the static builder and then can be accessed as such from outside.
        /// </summary>
        public string Adult { get; private set; }

        private string _imageFilterAspectValue;

        private string _imageFilterColor;

        private string _imageFilterFace;

        private string _imageFilterSize;

        private string _imageFilterStyle;

        /// <summary>
        /// The public image filter property that is built using the internal
        /// separate properties
        /// </summary>
        public string ImageFilter
        {
            get
            {
                return GetImageFilter();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///  Private constructor
        /// </summary>
        private ImageSearchOptions()
        {

        }

        #endregion

        #region Builders

        /// <summary>
        /// Construct an image search options object.
        /// </summary>
        /// <returns></returns>
        public static ImageSearchOptions Build(
            string adult = "",
            string imageFilterAspectValue = "",
            string imageFilterColor = "",
            string imageFilterFace = "",
            string imageFilterSize = "",
            string imageFilterStyle = "")
        {
            return new ImageSearchOptions()
                   {
                       // default adult to off
                       Adult = string.IsNullOrWhiteSpace(adult) ? QueryAdultValues.OFF : adult,

                       // default to nothing for the image filter properties

                       _imageFilterAspectValue = imageFilterAspectValue,

                       _imageFilterColor = imageFilterColor,

                       _imageFilterFace = imageFilterFace,

                       _imageFilterSize = imageFilterSize,

                       _imageFilterStyle = imageFilterStyle
                   };
        }

        #endregion

        #region Image Filter construction

        /// <summary>
        /// Builds the image filter option from all separate image filter properties
        /// </summary>
        /// <returns></returns>
        private string GetImageFilter()
        {
            var imageFilter = "";
            /*
                string imageFilterAspectValue = "",
                string imageFilterColor = "",
                string imageFilterFace = "",
                string imageFilterSize = "",
                string imageFilterStyle = "")
            */

            // indicate if an option is already added. 
            // Used to check for '+' character to combine options
            var flagOptionAdded = false;

            if (!string.IsNullOrWhiteSpace(_imageFilterAspectValue))
            {
                // check the {0} argument using inline bool checking
                imageFilter += string.Format("{0}Aspect:{1}", flagOptionAdded ? "+" : "", _imageFilterAspectValue);
                flagOptionAdded = true;
            }

            if (!string.IsNullOrWhiteSpace(_imageFilterColor))
            {
                imageFilter += string.Format("{0}Color:{1}", flagOptionAdded ? "+" : "", _imageFilterColor);
                flagOptionAdded = true;
            }

            if (!string.IsNullOrWhiteSpace(_imageFilterFace))
            {
                imageFilter += string.Format("{0}Face:{1}", flagOptionAdded ? "+" : "", _imageFilterFace);
                flagOptionAdded = true;
            }

            // we will only allow size values of Small Medium Large - no specification for custom sizes for now
            if (!string.IsNullOrWhiteSpace(_imageFilterSize))
            {
                imageFilter += string.Format("{0}Size:{1}", flagOptionAdded ? "+" : "", _imageFilterSize);
                flagOptionAdded = true;
            }

            if (!string.IsNullOrWhiteSpace(_imageFilterStyle))
            {
                imageFilter += string.Format("{0}Style:{1}", flagOptionAdded ? "+" : "", _imageFilterStyle);
                flagOptionAdded = true;
            }
         
            return imageFilter;
        }

        #endregion
    }
}
