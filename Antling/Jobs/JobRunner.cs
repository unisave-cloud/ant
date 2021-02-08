using System;
using System.IO;
using System.Threading.Tasks;
using Antling.Common;
using Antling.Docker;
using Antling.Models;

namespace Antling.Jobs
{
    /// <summary>
    /// Service capable of executing jobs
    /// </summary>
    public class JobRunner
    {
        private readonly IDocker docker;
        private readonly IImageBuilder imageBuilder;
        
        public JobRunner(IDocker docker, IImageBuilder imageBuilder)
        {
            this.docker = docker;
            this.imageBuilder = imageBuilder;
        }
        
        public async Task<ExecuteJobResponse> ExecuteJob(
            ExecuteJobRequestBody request
        )
        {
            int jobIndex = GenerateNextJobIndex();

            string tag = $"antling-job-{jobIndex}";

            string buildLog = await imageBuilder.Build(request.Image, tag);
            
            // TODO: run a container with the image
            string containerLog = await docker.Run(new DockerRunOptions {
                Image = tag,
                Rm = true,
                Name = $"antling_job_container_{jobIndex}"
            });
            
            // TODO: enforce time constraints
            
            // TODO: extract results from the container
            
            // TODO: destroy the container
            
            await docker.Rmi(tag);
            
            return new ExecuteJobResponse {
                ImageBuildLog = buildLog,
                ContainerLog = containerLog,
                Results = null
            };
        }
        
        #region "Job index generation"

        private int nextJobIndex = 1;

        private readonly object nextJobIndexLock = new object();
        
        private int GenerateNextJobIndex()
        {
            lock (nextJobIndexLock)
            {
                int i = nextJobIndex;
                nextJobIndex++;
                return i;
            }
        }
        
        #endregion
    }
}