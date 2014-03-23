using System.Drawing;

namespace ImageGrabber.Main.ImageCombiner.Interface
{
    public interface IImageComposer
    {
        /// <summary>
        /// Combines the images located in the folder path and
        /// returns one large combined image.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        Image CombineImagesInFolder(string folderPath, bool debugMode = false);
    }
}
