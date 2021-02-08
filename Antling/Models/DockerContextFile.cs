using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Antling.Models
{
    /// <summary>
    /// A file in the dockerfile context
    /// </summary>
    public class DockerContextFile : IValidatableObject
    {
        /// <summary>
        /// Path to the file, relative to the context directory
        /// </summary>
        [Required]
        public string Path { get; set; }
        
        /// <summary>
        /// In-place content of the file
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// URL where the file content could be downloaded
        /// </summary>
        public string Url { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Content == null && Url == null)
                yield return new ValidationResult(
                    "Either the content or the URL has to be specified."
                );
            
            // TODO: validate the path, validate the content, validate the URL
        }
    }
}