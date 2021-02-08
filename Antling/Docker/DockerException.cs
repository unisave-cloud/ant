using System;

namespace Antling.Docker
{
    /// <summary>
    /// Thrown, when a docker command fails
    /// </summary>
    public class DockerException : Exception
    {
        /// <summary>
        /// The reason for the exception
        /// </summary>
        public DockerError Kind { get; }

        public override string Message { get; }

        public DockerException(DockerError kind = DockerError.Other, string msg = "")
        {
            Kind = kind;

            switch (kind)
            {
                case DockerError.ContainerNameTaken:
                    Message = "Container name taken, when trying: docker run " + msg;
                    break;
                
                case DockerError.NoSuchContainer:
                    Message = "No such container exists: " + msg;
                    break;
                
                case DockerError.RemovingRunningContainer:
                    Message = "Cannot remove a running container: " + msg;
                    break;
                
                default:
                    Message = "Docker command execution failed: " + msg;
                    break;
            }
        }
    }
}