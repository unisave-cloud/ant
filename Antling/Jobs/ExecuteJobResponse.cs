using System.Collections.Generic;

namespace Antling.Jobs
{
    public class ExecuteJobResponse
    {
        /// <summary>
        /// Log of the docker image build
        /// </summary>
        public string ImageBuildLog { get; set; }
        
        /// <summary>
        /// Log of the container that ran the job
        /// </summary>
        public string ContainerLog { get; set; }
        
        /// <summary>
        /// Result files (paths within the container) and their contents
        /// </summary>
        public Dictionary<string, string> Results { get; set; }
    }
}