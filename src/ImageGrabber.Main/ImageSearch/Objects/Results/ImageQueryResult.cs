using System;

namespace ImageGrabber.Main.ImageSearch.Objects.Results
{
    /// <summary>
    /// The class used to return query results from the ImageUrlGrabber.
    /// 
    /// Provides an abstraction over the Bing Search Container Image result
    ///
    /// Internall image grabber app query result mapping to the Bing Search Container image result
    /// </summary>
    public class ImageQueryResult
    {
        public Guid ID { get; set; }

        public String Title { get; set; }

        public String MediaUrl { get; set; }

        public String SourceUrl { get; set; }

        public String DisplayUrl { get; set; }

        public Int32? Width { get; set; }

        public Int32? Height { get; set; }

        public Int64? FileSize { get; set; }

        public String ContentType { get; set; }

        public ImageQueryThumbnailResult ThumbnailResult { get; set; }
    }
}
