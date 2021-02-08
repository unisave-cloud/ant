using System.Threading.Tasks;

namespace Antling.Docker
{
    /// <summary>
    /// Interface to control the docker daemon
    /// </summary>
    public interface IDocker
    {
        /// <summary>
        /// Builds a new docker image from a dockerfile
        /// </summary>
        /// <param name="options">Command options</param>
        /// <exception cref="DockerException"></exception>
        /// <returns>Build log</returns>
        Task<string> Build(DockerBuildOptions options);

        /// <summary>
        /// Removes an image by the tag
        /// </summary>
        /// <param name="imageName">Image tag</param>
        /// <exception cref="DockerException"></exception>
        Task Rmi(string imageName);
        
        /// <summary>
        /// Creates a new docker container and starts it
        /// </summary>
        /// <param name="options">Options for the docker run command</param>
        /// <exception cref="DockerException"></exception>
        /// <returns>Output of the command</returns>
        Task<string> Run(DockerRunOptions options);

        /// <summary>
        /// Stops a docker container
        /// Does nothing if already stopped
        /// </summary>
        /// <param name="containerId">Container name or id</param>
        /// <param name="time">Seconds to wait before killing</param>
        /// <exception cref="DockerException"></exception>
        Task Stop(string containerId, int time = 10);

        /// <summary>
        /// Removes a docker container
        /// </summary>
        /// <param name="containerId">Container name or id</param>
        /// <exception cref="DockerException"></exception>
        Task Rm(string containerId);

        /// <summary>
        /// Returns entire log history of a docker container
        /// </summary>
        /// <param name="containerId">Container name or ID</param>
        /// <returns>Entire log history as a multiline string</returns>
        /// <exception cref="DockerException"></exception>
        Task<string> Logs(string containerId);

        /// <summary>
        /// Obtains status of a docker container
        /// The container has to exist, otherwise an exception is thrown
        /// </summary>
        /// <param name="containerId"></param>
        /// <exception cref="DockerException"></exception>
        Task<ContainerStatus> GetStatus(string containerId);
    }
}