namespace Antling.Docker
{
    /// <summary>
    /// Options for building a Docker image
    /// </summary>
    public class DockerBuildOptions
    {
        /// <summary>
        /// --file option
        /// </summary>
        public string DockerfilePath { get; set; }
        
        /// <summary>
        /// The only argument of the docker command - the context directory
        /// </summary>
        public string ContextPath { get; set; }
        
        /// <summary>
        /// Tag to be given to the resulting image
        /// </summary>
        public string Tag { get; set; }
    }
}