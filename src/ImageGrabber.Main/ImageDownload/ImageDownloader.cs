using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ImageGrabber.Common.Config;
using ImageGrabber.Main.ImageDownload.Interface;
using ImageGrabber.Main.ImageDownload.Objects;
using ImageGrabber.Main.ImageSearch.Objects.Results;

namespace ImageGrabber.Main.ImageDownload
{
    public class ImageDownloader : IImageDownloader
    {
        #region Properties

        private bool _isDebugMode = false;

        #endregion

        public bool DownloadImages(List<ImageQueryResult> imageList, DownloadOptions options, bool debugMode)
        {
            _isDebugMode = debugMode;

            if (_isDebugMode)
            {
                Console.WriteLine("===");
                Console.WriteLine("=== Starting image download process");
            }

            DoDirectoryCheck(GlobalConfig._download_imageDownloadPath);

            if (options.DownloadThumbnails)
            {
                DoDirectoryCheck(GlobalConfig._download_imageThumbDownloadPath);
            }

            var image_count = 1;

            foreach (var imageQueryResult in imageList)
            {
                var url = imageQueryResult.MediaUrl;


                var mainImageSavePath = GetImageSavePath(imageQueryResult.MediaUrl, GlobalConfig._download_imageDownloadPath, image_count);

                //download the actual image
                DownloadRemoteImageFile(url, mainImageSavePath);

                // check if we should download the thumbnail
                if (options.DownloadThumbnails)
                {
                    var thumbnailPath = GetImageSavePath(imageQueryResult.ThumbnailResult.MediaUrl,
                        GlobalConfig._download_imageThumbDownloadPath, image_count, options.ThumbnailPrefix);

                    DownloadRemoteImageFile(imageQueryResult.ThumbnailResult.MediaUrl, thumbnailPath);
                }

                if (_isDebugMode)
                {
                    // this will be out of debug mode for now
                    Console.WriteLine("Saved as: " + mainImageSavePath);
                    Console.WriteLine("======================================================");
                }


                image_count++;
            }

            return true;
        }


        #region  Image Storage Utilities

        /// <summary>
        /// Return a image save path on disk, based on the image url,
        /// the download order and the path on disk where to save the image
        /// 
        /// Returns a raw download path in the format: {path}\{i}.{extension}
        ///  </summary>
        /// <param name="folder">The folder where the image will be stored.</param>
        /// <param name="imageOrder"></param>
        /// <param name="url">The image web url from where it can be downloaded. Contains the extension information</param>
        /// <param name="imageSavePrefix">Optional image prefix parameter. Prepended to the image</param>
        /// <returns></returns>
        private string GetImageSavePath(string url, string folder, int imageOrder, string imageSavePrefix = "")
        {
            // get the url witout the query string
            var cleanUrl = new Uri(url).GetLeftPart(UriPartial.Path);

            var fileName = Path.GetFileName(cleanUrl);
            var extension = Path.GetExtension(cleanUrl);

            // check if extension is missing, if so revert to default extension during download/saving
            // process

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = GlobalConfig._download_default_save_extension;
            }

            var savePath = folder + imageSavePrefix + imageOrder + extension;

            if (_isDebugMode)
            {
                Console.WriteLine("Image: " + fileName);
                Console.WriteLine("Extension: " + extension);
            }

            return savePath;
        }

        #endregion

        #region Downloading

        private void DownloadRemoteImageFile(string uri, string fileName)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Check that the remote file was found. The ContentType
                // check is performed since a request for a non-existent
                // image file might be redirected to a 404-page, which would
                // yield the StatusCode "OK", even though the image was not
                // found.
                if ((response.StatusCode == HttpStatusCode.OK ||
                     response.StatusCode == HttpStatusCode.Moved ||
                     response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    // if the remote file was found, download it
                    using (Stream inputStream = response.GetResponseStream())
                    using (Stream outputStream = File.OpenWrite(fileName))
                    {

                        byte[] buffer = new byte[4096];
                        int bytesRead;

                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Directory Checks

        /// <summary>
        /// Check if the given directory exists. If it does not creates the directory.
        /// 
        /// Clear any content inside the directory
        /// </summary>
        /// <param name="directoryPath"></param>
        private void DoDirectoryCheck(string directoryPath)
        {
            // if exists we are cleaning all files
            if (Directory.Exists(directoryPath))
            {
                var files = Directory.GetFiles(directoryPath);

                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        #endregion
    }
}
