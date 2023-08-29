namespace Post.Service.Base.HelperClasses.IHelperClasses
{
    public interface ILoggerManager
    {
        void LogDebugObject(object obj);
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
