using System.Collections.Generic;
using System.Security.AccessControl;

namespace Antling.Docker
{
    /// <summary>
    /// Options for starting a new docker container
    /// </summary>
    public class DockerRunOptions
    {
        /// <summary>
        /// Detach the container from the starting process
        /// </summary>
        public bool Detach { get; set; }
        
        /// <summary>
        /// Remove the container when it exists
        /// </summary>
        public bool Rm { get; set; }
        
        /// <summary>
        /// Memory limit in megabytes
        /// </summary>
        public int? MemoryMegabytes { get; set; }

        /// <summary>
        /// Port mapping (external:internal)
        /// </summary>
        public Dictionary<int, int> PortMapping { get; set; }

        /// <summary>
        /// Name to give to the new container
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Name (tag) of the docker image to use
        /// </summary>
        public string Image { get; set; }
        
        /// <summary>
        /// Additional arguments to pass to the container on startup
        /// </summary>
        public List<string> Arguments { get; set; }

        public IEnumerable<string> GetArguments()
        {
            if (Detach)
                yield return "-d";
            
            if (Rm)
                yield return "--rm";
            
            if (MemoryMegabytes != null)
            {
                yield return "-m";
                yield return $"{MemoryMegabytes}m";
            }

            if (PortMapping != null)
            {
                foreach (var pair in PortMapping)
                {
                    yield return "-p";
                    yield return $"{pair.Key}:{pair.Value}";
                }
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                yield return "--name";
                yield return Name;
            }
            
            yield return Image;

            if (Arguments != null)
            {
                foreach (string a in Arguments)
                    yield return a;
            }
        }
    }
}