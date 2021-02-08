using System.Threading.Tasks;
using Antling.Models;

namespace Antling.Common
{
    public interface IImageBuilder
    {
        /// <summary>
        /// Initializes the image builder
        /// </summary>
        void Initialize();

        /// <summary>
        /// Builds a docker image based on the given specification
        /// </summary>
        /// <param name="spec">Image specification</param>
        /// <param name="tag">Tag to assign to the image</param>
        /// <returns>Build log</returns>
        Task<string> Build(DockerImageSpecification spec, string tag);
    }
}