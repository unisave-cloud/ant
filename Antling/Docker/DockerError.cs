namespace Antling.Docker
{
    /// <summary>
    /// Specifies the problem behind a docker exception
    /// </summary>
    public enum DockerError
    {
        Other = 0,
        ContainerNameTaken,
        NoSuchContainer,
        RemovingRunningContainer,
    }
}