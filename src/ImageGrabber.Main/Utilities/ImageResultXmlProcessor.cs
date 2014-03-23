using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ImageGrabber.Common.Config;
using ImageGrabber.Main.ImageSearch.Objects.Results;

namespace ImageGrabber.Main.Utilities
{
    /// <summary>
    /// ImageResultQuery serialziation utility in debugging process to prevent 
    /// wasting allowed query counts by temporarily storing queried images in xml format on disk.
    /// 
    /// These can be retrieved locally instead of making multiple wasted requests to the API
    /// </summary>
    public static class ImageResultXmlProcessor
    {
        /// <summary>
        /// Serialize an image query result list to a xml file.
        /// 
        /// Xml file is internally controlled by global configuration
        /// </summary>
        /// <param name="list"></param>
        public static void Serialize(List<ImageQueryResult> list)
        {
            var ser = new XmlSerializer(typeof(List<ImageQueryResult>));

            using (var fs = new FileStream(GlobalConfig._serialization_tmpImagesList, FileMode.Create))
            {
                ser.Serialize(fs, list);
            }
        }

        /// <summary>
        /// Desirialize an image  query result list from the specified xml file.
        /// 
        /// File path is internally configured in the global configuration.
        /// </summary>
        /// <returns></returns>
        public static List<ImageQueryResult> Desirialize()
        {
            var ser = new XmlSerializer(typeof(List<ImageQueryResult>));
            
            return
                ser.Deserialize(new StreamReader(GlobalConfig._serialization_tmpImagesList)) as List<ImageQueryResult>;
        }
    }
}
