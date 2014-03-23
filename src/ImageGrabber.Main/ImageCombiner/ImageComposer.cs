using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageGrabber.Common.Config;
using ImageGrabber.Main.ImageCombiner.Interface;
using ImageGrabber.Main.ImageCombiner.Objects;

namespace ImageGrabber.Main.ImageCombiner
{
    public class ImageComposer : IImageComposer
    {
        #region Properties

        private readonly List<string> _imageExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".jpe" };

        private bool _isDebugMode =false;

        #endregion

        #region Methods

        /// <summary>
        /// Combines the images located in the folder path and
        /// returns one large combined image.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        public Image CombineImagesInFolder(string folderPath, bool debugMode = false)
        {
            _isDebugMode = debugMode;

            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Directory " + folderPath + " does not exist");
            }

            var files = Directory.GetFiles(folderPath).ToList();

            if (_isDebugMode)
            {
                Console.WriteLine("Found " + files.Count + " files to compose in folder " + folderPath);
            }

            if (_isDebugMode)
            {
                Console.WriteLine("======== Processing list of files to get images  =======");
            }
            var selectedImages = ProcessListOfFilesForImages(files);

            if (_isDebugMode)
            {
                Console.WriteLine("======== Processing list of image files to get list of ImageObjects =======");
            }
            var imageObjects = ProcessImagePathsToImageObjects(selectedImages);

            if (_isDebugMode)
            {
                Console.WriteLine("======== Processing list of ImageObjects to composed image =======");
            }
            var composedBitmap = CombineImages(imageObjects);

            return composedBitmap;
        }

        #endregion


        #region Privates

        /// <summary>
        /// For a list of image objects which wrap an Image return a composed large image based
        /// on the total count of images.
        /// </summary>
        /// <param name="imageObjects"></param>
        /// <returns></returns>
        private Bitmap CombineImages(List<ImageObject> imageObjects)
        {
            // we are going to roughly here figure out the rows/columns of the squared big image, composed
            // from the smaller default size ( 200x200 ) images. This means some images will not be used in the composition
            // as we will be using a square root of the total number of images
            var imagesPerSection = (int)Math.Floor(Math.Sqrt(imageObjects.Count));

            if (_isDebugMode)
            {
                Console.WriteLine(string.Format("Calculated images per section is {0}", imagesPerSection));
            }

            /*
             * Note that we are drawing a DIMxDIM square image. That is why we are using a fixed default size
             * and only single imagesPerSection variable. We will have equal rows and columns of images in the final image
             */

            var composedWidth = imagesPerSection * GlobalConfig.ComposedImageDimension;
            var composedHeight = imagesPerSection * GlobalConfig.ComposedImageDimension;

            if (_isDebugMode)
            {
                Console.WriteLine(string.Format("Composed Image Width: {0}, Height{1}", composedWidth, composedHeight));
            }

            var composedBitmap = new Bitmap(imagesPerSection * GlobalConfig.ComposedImageDimension,
                imagesPerSection * GlobalConfig.ComposedImageDimension);

            using (var graphics = Graphics.FromImage(composedBitmap))
            {
                graphics.Clear(Color.White);
                var dim = GlobalConfig.ComposedImageDimension;

                for (int i = 0; i < imagesPerSection; i++)
                {
                    for (var k = 0; k < imagesPerSection; k++)
                    {
                        // get the image we are adding by calcualting the index
                        var calculatedImageIndex = (i * imagesPerSection) + k;
                        var imageObject = imageObjects[calculatedImageIndex];


                        graphics.DrawImage(imageObject.Image, new Rectangle()
                        {
                            X = dim * k,
                            Y = dim * i,
                            Width = dim,
                            Height = dim
                        });

                        // once the image is draw we are going to dispoe it as
                        // we no longer need to keep it in memory
                        imageObject.Image.Dispose();

                        if (_isDebugMode)
                        {
                            Console.WriteLine("Added image at index {0} in ImageObjects with name {1} to composed image", calculatedImageIndex, imageObject.ImageName);
                        }
                    }
                }
            }

            return composedBitmap;
        }


        /// <summary>
        /// Take a list of image file names/paths and create a list of ImageObjects that contain the 
        /// processed/resized images and their original sizes.
        ///  </summary>
        /// <param name="imagePathsList"></param>
        /// <returns></returns>
        List<ImageObject> ProcessImagePathsToImageObjects(List<string> imagePathsList)
        {
            // will contain all images, resized to the default size to fit in the combined image
            var imageObjects = new List<ImageObject>();

            foreach (var imagePath in imagePathsList)
            {
                var image = Image.FromFile(imagePath);

                var imageHeight = image.Height;
                var imageWidth = image.Width;

                // create the local image reference for easier access latter on
                // the image is resized to the composed dimension ready for composition
                imageObjects.Add(new ImageObject()
                {
                    Image = (Image)(new Bitmap(image, GlobalConfig.ComposedImageDefaultSize)),
                    OriginalWidth = imageWidth,
                    OriginalHeight = imageHeight,
                    ImageName = Path.GetFileName(imagePath)
                });

                if (_isDebugMode)
                {
                    var message = string.Format("Image {0} has Width: {1}, Height: {2}", Path.GetFileName(imagePath),
                        image.Height, image.Width);

                    Console.WriteLine(message);
                }
            }

            return imageObjects;
        }

        /// <summary>
        /// Go over a list of file names and return only files that are images based on the extension
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        List<string> ProcessListOfFilesForImages(List<string> files)
        {
            // list of processed images
            var selectedImages = new List<string>();

            for (int i = 0; i < files.Count; i++)
            {
                var imagePath = files[i];
                var extension = Path.GetExtension(imagePath);

                if (string.IsNullOrWhiteSpace(extension))
                {
                    if (_isDebugMode)
                    {
                        Console.WriteLine(string.Format("NOT ADDEDD : File {0} has no extension. Skipping adding to processed files", imagePath));
                    }

                    continue;
                }

                string debugMessage = "";
                if (_imageExtensions.Contains(extension.ToLower()))
                {
                    selectedImages.Add(imagePath);
                    debugMessage =
                        string.Format(
                            "ADDED: Path {0} and extension {1}",
                            imagePath, extension);
                }
                else
                {
                    debugMessage =
                        string.Format(
                            "NOT ADDED: Path {0} and extension {1} has not been added to selected images to combine",
                            imagePath, extension);
                }

                if (_isDebugMode)
                {
                    Console.WriteLine(debugMessage);
                }
            }

            if (_isDebugMode)
            {
                Console.WriteLine(string.Format("Total of {0} images selected for combinations", selectedImages.Count));
            }

            return selectedImages;
        }

        #endregion

    }
}