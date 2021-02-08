namespace Antling.Docker
{
    /// <summary>
    /// Status of a Docker container
    /// </summary>
    public enum ContainerStatus
    {
        Created,
        Restarting,
        Running,
        Paused,
        Exited
    }
}