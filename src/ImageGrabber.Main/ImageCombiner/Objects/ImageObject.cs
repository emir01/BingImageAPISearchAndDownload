using System.Drawing;

namespace ImageGrabber.Main.ImageCombiner.Objects
{
    /// <summary>
    /// Contains a temporary workable in memory reference of the image
    /// on disk
    /// </summary>
    public class ImageObject
    {
        /// <summary>
        /// Reference to the actual image in memory created from the image file name/path
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Reference to the image file name from which this image object was created
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Reference to the original width as the Image in the ImageObject is resized to the Composed Image Dimensions
        /// </summary>
        public int OriginalWidth { get; set; }

        /// <summary>
        /// Reference to the original height as the Image in the ImageObject is resized to the Composed Image Dimensions
        /// </summary>
        public int OriginalHeight { get; set; }
    }
}
