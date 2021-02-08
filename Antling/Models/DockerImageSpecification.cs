using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Antling.Models
{
    /// <summary>
    /// Specifies a docker image to be used, in an API request
    /// </summary>
    public class DockerImageSpecification : IValidatableObject
    {
        /// <summary>
        /// Name of the docker image to use.
        /// If not provided, dockerfile is used instead.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Used to create the image when no image name is specified.
        /// </summary>
        public string Dockerfile { get; set; }
        
        /// <summary>
        /// Context files for the dockerfile execution.
        /// </summary>
        public List<DockerContextFile> Context { get; set; }
        
        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext
        )
        {
            if (Name == null && Dockerfile == null)
                yield return new ValidationResult(
                    "Either the image name or the dockerfile must be specified."
                );
        }
    }
}