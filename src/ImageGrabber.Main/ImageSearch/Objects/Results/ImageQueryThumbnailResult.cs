using System;

namespace ImageGrabber.Main.ImageSearch.Objects.Results
{
    /// <summary>
    /// The internal reference to the thumbnail
    /// </summary>
    public class ImageQueryThumbnailResult
    {
        public String MediaUrl{get;set;}
        
        public String ContentType{get;set;}
        
        public Int32? Width{get;set;}
        
        public Int32? Height{get;set;}
        
        public Int64? FileSize{get;set;}
    }
}