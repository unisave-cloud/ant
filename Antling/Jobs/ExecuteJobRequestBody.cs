using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Antling.Models;

namespace Antling.Jobs
{
    public class ExecuteJobRequestBody
    {
        /// <summary>
        /// Docker image of the container that will execute the job
        /// </summary>
        [Required]
        public DockerImageSpecification Image { get; set; }

        /// <summary>
        /// List of files (paths inside the container) to return
        /// as the result of the job
        /// </summary>
        [Required]
        public List<string> Results { get; set; }
    }
}