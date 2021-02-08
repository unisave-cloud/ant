using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Antling.Docker
{
    /// <summary>
    /// Set of static methods for working with docker
    /// </summary>
    public class DockerViaBash : IDocker
    {
        /// <inheritdoc/>
        public async Task<string> Build(DockerBuildOptions options)
        {
            // TODO: also pass CPU usage limitations
            
            string flags =
                $"--file {options.DockerfilePath} " +
                $"--tag {options.Tag}";
            
            DockerOutput output = await DockerCommandAsync(
                $"build {flags} {options.ContextPath}"
            );

            return output.completeOutput;
        }
        
        /// <inheritdoc/>
        public async Task Rmi(string imageName)
        {
            DockerOutput output = await DockerCommandAsync(
                $"rmi {imageName}"
            );
            
            // TODO: guard no such image
            // TODO: guard image in use
            GuardOtherErrors(output);
        }

        /// <inheritdoc/>
        public async Task<string> Run(DockerRunOptions options)
        {
            string arguments = string.Join(" ", options.GetArguments());

            DockerOutput output = await DockerCommandAsync(
                $"run {arguments}"
            );
            
            // TODO: extract to method
            if (output.exitCode == 125
                && output.stderr.Contains("Conflict. The container name"))
            {
                throw new DockerException(
                    DockerError.ContainerNameTaken,
                    arguments
                );
            }
            
            GuardOtherErrors(output);

            return output.completeOutput;
        }
        
        /// <inheritdoc/>
        public async Task Stop(string containerId, int time = 10)
        {
            DockerOutput output = await DockerCommandAsync(
                $"stop -t {time} {containerId}"
            );
            
            // NOTE: stopping a stopped container causes no error
            
            GuardNoSuchContainer(output, containerId);
            GuardOtherErrors(output);
        }

        /// <inheritdoc/>
        public async Task Rm(string containerId)
        {
            DockerOutput output = await DockerCommandAsync(
                $"rm {containerId}"
            );

            if (output.exitCode == 1
                && output.stderr.Contains("You cannot remove a running container"))
            {
                throw new DockerException(
                    DockerError.RemovingRunningContainer,
                    containerId
                );
            }

            GuardNoSuchContainer(output, containerId);
            GuardOtherErrors(output);
        }
        
        /// <inheritdoc/>
        public async Task<string> Logs(string containerId)
        {
            DockerOutput output = await DockerCommandAsync(
                $"logs {containerId}"
            );

            GuardNoSuchContainer(output, containerId);
            GuardOtherErrors(output);

            // IMPORTANT NOTE: docker logs stdout and stderr
            // into their respective streams
            return output.completeOutput;
        }

        /// <inheritdoc/>
        public async Task<ContainerStatus> GetStatus(
            string containerId
        )
        {
            DockerOutput output = await DockerCommandAsync(
                "inspect --format='{{.State.Status}}' " + containerId
            );
            
            GuardNoSuchContainer(output, containerId);
            GuardOtherErrors(output);

            if (output.stdout.Contains("created"))
                return ContainerStatus.Created;
            
            if (output.stdout.Contains("restarting"))
                return ContainerStatus.Restarting;
            
            if (output.stdout.Contains("running"))
                return ContainerStatus.Running;
            
            if (output.stdout.Contains("paused"))
                return ContainerStatus.Paused;
            
            if (output.stdout.Contains("exited"))
                return ContainerStatus.Exited;
            
            throw new DockerException(
                DockerError.Other,
                "Unknown container status: " + output.stdout
            );
        }

        /// <summary>
        /// Throws docker exception if the output means, no such container exists
        /// </summary>
        /// <param name="output"></param>
        /// <param name="containerId"></param>
        /// <exception cref="DockerException"></exception>
        private static void GuardNoSuchContainer(
            DockerOutput output,
            string containerId
        )
        {
            if (output.exitCode != 1)
                return;

            if (
                !output.stderr.Contains("No such container")
                && !output.stderr.Contains("No such object") // 'inspect' cmd
            )
                return;
            
            throw new DockerException(
                DockerError.NoSuchContainer,
                containerId
            );
        }

        
        /// <summary>
        /// Throws docker exception on any other nonzero exit code
        /// </summary>
        /// <exception cref="DockerException"></exception>
        private static void GuardOtherErrors(DockerOutput output)
        {
            if (output.exitCode != 0)
            {
                throw new DockerException(
                    DockerError.Other,
                    "Exit code: " + output.exitCode +
                    " Stderr: " + output.stderr
                );
            }
        }
        
        ///////////////////////////
        // Raw process interface //
        ///////////////////////////
        
        /// <summary>
        /// What a docker command returns
        /// </summary>
        private class DockerOutput
        {
            public string stdout;
            public string stderr;
            public string completeOutput;
            public int exitCode;
        }
        
        /// <summary>
        /// Executes the docker command and returns the docker output
        /// </summary>
        /// <param name="arguments">Argument string, e.g. "logs --tail 50"</param>
        /// <returns>Output from the docker process</returns>
        private static Task<DockerOutput> DockerCommandAsync(string arguments)
        {
            var process = new Process {
                EnableRaisingEvents = true,
                StartInfo = {
                    UseShellExecute = false,
                    FileName = "docker",
                    Arguments = arguments,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            
            var stdout = new List<string>();
            var stderr = new List<string>();
            var combinedOut = new List<string>();

            process.OutputDataReceived += (s, e) => {
                stdout.Add(e.Data + "\n");
                combinedOut.Add(e.Data + "\n");
            };
            process.ErrorDataReceived += (s, e) => {
                stderr.Add(e.Data + "\n");
                combinedOut.Add(e.Data + "\n");
            };

            var tcs = new TaskCompletionSource<DockerOutput>();
            
            process.Exited += (sender, args) => {
                var output = new DockerOutput {
                    // BUG: Collection was modified; enumeration operation may not execute.
                    stdout = string.Concat(stdout), // <-- here
                    stderr = string.Concat(stderr), // <-- or here
                    completeOutput = string.Concat(combinedOut),
                    exitCode = process.ExitCode
                };

                process.Dispose();
                
                tcs.SetResult(output);
            };
            
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }
    }
}