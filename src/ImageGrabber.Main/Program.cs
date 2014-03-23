using System;
using System.Collections.Generic;
using ImageGrabber.Common.Config;
using ImageGrabber.Main.ImageCombiner;
using ImageGrabber.Main.ImageDownload;
using ImageGrabber.Main.ImageDownload.Objects;
using ImageGrabber.Main.ImageSearch;
using ImageGrabber.Main.ImageSearch.Objects.QueryObjects;
using ImageGrabber.Main.ImageSearch.Objects.Registries.SearchOptionRegistires;
using ImageGrabber.Main.ImageSearch.Objects.Results;
using ImageGrabber.Main.Utilities;

namespace ImageGrabber.Main
{
    public class Program
    {
        static void Main(string[] args)
        {
            // This is the query - or you could get it from args.
            string query = "human form abstract";

            var count = 100;
            var debug = true;

            List<ImageQueryResult> imageList = new List<ImageQueryResult>();

            if (GlobalConfig._global_useSearchApiSource)
            {
                if (debug)
                {
                    Console.WriteLine("Getting ImageQueryResults from BING SEARCH API");
                }

                // get the image list from actual source
                var grabber = new ImageUrlGrabber();
                imageList = grabber.GetImageUrlList(
                    ImageQueryConfig.Get(
                        query,
                        count,
                        ImageSearchOptions.Build(QueryAdultValues.OFF, imageFilterColor: ImageFilterColorValues.MONOCHROME, imageFilterAspectValue: ImageFilterAspectValues.SQUARE)),
                    debug);

                // serialize it for temporary storage for any usage
                // mostly to prevent extra queries when debugging.
                ImageResultXmlProcessor.Serialize(imageList);
            }
            else
            {
                Console.WriteLine("Getting ImageQueryResults from XML");
                imageList = ImageResultXmlProcessor.Desirialize();
            }

            if (GlobalConfig._download_run_download)
            {
                Console.WriteLine("Redownloading images");
                // Try and download images
                var imageDownloader = new ImageDownloader();

                imageDownloader.DownloadImages(imageList, DownloadOptions.Build(true, "thumb_"), debug);
            }
            else
            {
                Console.WriteLine("Using Predownloaded images");
                // do not download image and run the composer from what is
                // already in the folder
            }

            var imageComposer = new ImageComposer();
            var composedImage = imageComposer.CombineImagesInFolder(GlobalConfig._download_imageDownloadPath, debug);

            composedImage.Save(GlobalConfig.ComposedImagePath);
            composedImage.Dispose();

            Console.WriteLine("DONE!");
            Console.ReadLine();
        }
    }
}
