using System;
using System.IO;
using System.Threading.Tasks;
using Antling.Docker;
using Antling.Models;

namespace Antling.Common
{
    /// <summary>
    /// Service for building docker images
    /// </summary>
    public class ImageBuilder : IImageBuilder
    {
        private const string BuilderDirectory = "image-builder";
        
        private readonly IDocker docker;
        
        public ImageBuilder(IDocker docker)
        {
            this.docker = docker;
        }

        public void Initialize()
        {
            if (Directory.Exists(BuilderDirectory))
                Directory.Delete(BuilderDirectory, true);
        }
        
        public async Task<string> Build(
            DockerImageSpecification spec,
            string tag
        )
        {
            if (spec.Dockerfile == null)
                throw new ArgumentException("Dockerfile has to be provided");
            
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentNullException(nameof(tag));
            
            // === prepare ===

            // build directory
            string buildDirectoryPath = $"{BuilderDirectory}/{tag}";
            Directory.CreateDirectory(buildDirectoryPath);

            // dockerfile
            string dockerfilePath = Path.Join(buildDirectoryPath, "Dockerfile");
            await WriteFile(dockerfilePath, spec.Dockerfile);
            
            // context
            string contextDirectoryPath = $"{buildDirectoryPath}/context";
            Directory.CreateDirectory(contextDirectoryPath);
            
            foreach (var file in spec.Context)
            {
                string content = file.Content;
                
                if (content == null)
                    throw new NotImplementedException("Download file from URL");
                
                await WriteFile(
                    Path.Join(contextDirectoryPath, file.Path),
                    content
                );
            }
            
            // === build ===
            
            string buildLog = await docker.Build(new DockerBuildOptions {
                ContextPath = contextDirectoryPath,
                DockerfilePath = dockerfilePath,
                Tag = tag
            });
            
            // === cleanup ===
            
            Directory.Delete(buildDirectoryPath, true);

            return buildLog;
        }
        
        private async Task WriteFile(string path, string content)
        {
            await using var s = File.CreateText(path);
            await s.WriteAsync(content);
        }
    }
}