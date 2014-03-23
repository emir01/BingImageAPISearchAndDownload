using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Net;
using ImageGrabber.Common.Config;
using ImageGrabber.Main.ImageSearch.Interface;
using ImageGrabber.Main.ImageSearch.Objects.QueryObjects;
using ImageGrabber.Main.ImageSearch.Objects.Results;
using ImageGrabber.Main.ImageSearch.Utilities;
using ImageGrabber.Main.Utilities;

namespace ImageGrabber.Main.ImageSearch
{
    public class ImageUrlGrabber : IImageUrlGrabber
    {
        #region Properties

        private StreamWriter debugWritter;

        /// <summary>
        /// Contain current debug information
        /// </summary>
        private bool _debugFlag = false;

        #endregion

        public List<ImageQueryResult> GetImageUrlList(ImageQueryConfig config, bool isDebug = false)
        {
            //set the debug state
            _debugFlag = isDebug;

            _debug_OpenFileDebug();

            // create the bing search container
            var bingContainer = new BingSearchContainer(new Uri(GlobalConfig._search_baseUrl));
            bingContainer.Credentials = new NetworkCredential(GlobalConfig._search_accountKey, GlobalConfig._search_accountKey);

            var executionFlowObject = QueryFlowConfig.Build(config.Count);

            var total_requested = 0;

            var images = new List<ImageQueryResult>();

            for (int i = 0; i < executionFlowObject.NumberOfFullExecutions + 1; i++)
            {
                // crate a new query for each request as for now I dont know how to actually
                // modify query parameters
                var query = bingContainer.Image(config.Query, null, null, config.SearchOptions.Adult, null, null, config.SearchOptions.ImageFilter);

                // i will be checked to determine $top. if its reqced the total number of executions
                // we will set $top to the remainder
                if (i == executionFlowObject.NumberOfFullExecutions)
                {
                    query = query.AddQueryOption("$top", executionFlowObject.PartialExecutionCount);
                }
                else
                {
                    query = query.AddQueryOption("$top", GlobalConfig._search_maxQueryCount + (i * GlobalConfig._search_maxQueryCount));
                }

                query = query.AddQueryOption("$skip", total_requested);

                _debug_PrintConsoleQuery(query);

                var results = query.Execute();

                // if we got nothing in this query we will just
                // return what we have so far
                if (results == null)
                {
                    return images;
                }

                var listResults = results.ToList();

                _debug_WriteImageResults(listResults, total_requested);

                total_requested += listResults.Count();

                images.AddRange(listResults.Select(ImageResultMappings.MapImageQueryResultFromImageResult));
            }

            _debug_CloseFileDebug();

            return images;
        }

        #region Private Utilities Debug

        /// <summary>
        /// Open the file debug session
        /// </summary>
        private void _debug_OpenFileDebug()
        {
            if (_debugFlag)
            {
                debugWritter = new StreamWriter(GlobalConfig._search_imageUrlGrabberDebugFile, false);
            }
        }

        /// <summary>
        /// Close the file debug session
        /// </summary>
        private void _debug_CloseFileDebug()
        {
            if (_debugFlag)
            {
                debugWritter.Close();
            }
        }

        private void _debug_PrintConsoleQuery(DataServiceQuery<ImageResult> imageQuery)
        {
            if (_debugFlag)
            {
                Console.WriteLine("####################################################");
                Console.WriteLine(imageQuery.RequestUri);
                Console.WriteLine("####################################################");
            }
        }

        private void _debug_WriteImageResults(IEnumerable<ImageResult> results, int startingCount)
        {
            if (_debugFlag)
            {
                int curretCount = startingCount;
                foreach (var result in results)
                {
                    // increase the starting count
                    curretCount++;

                    Console.WriteLine(curretCount + ". " + result.Title);

                    debugWritter.WriteLine(curretCount + "# " + result.MediaUrl);
                    debugWritter.WriteLine("--------------------------------------------------------------------");

                }
            }
        }

        #endregion

        #region Private Utilities Query

        /// <summary>
        /// Used to go around the fact that we cannot return more than 50 results
        /// at a given time. Possibly a constraint or misconfiguration.
        /// 
        /// This will split the requested count to a NumberOfFullExecutions * 50 and a
        /// final execution with PartialExecutionCount
        /// </summary>
        private class QueryFlowConfig
        {
            /// <summary>
            /// No public construction. Only via the static factory method
            /// </summary>
            private QueryFlowConfig()
            { }

            public int NumberOfFullExecutions { get; set; }

            public int PartialExecutionCount { get; set; }

            /// <summary>
            /// Builds the QueryFlowConfig object based on the requested image count
            /// </summary>
            /// <param name="requestedCount"></param>
            /// <returns></returns>
            public static QueryFlowConfig Build(int requestedCount)
            {
                var fullExecutions = Math.Floor(((double)requestedCount) / GlobalConfig._search_maxQueryCount);
                var partialExecutionCount = requestedCount % GlobalConfig._search_maxQueryCount;

                return new QueryFlowConfig()
                       {
                           NumberOfFullExecutions = (int)fullExecutions,
                           PartialExecutionCount = partialExecutionCount
                       };
            }
        }

        #endregion
    }
}
