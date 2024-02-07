namespace ToLifeCloud.Worker.ConnectorMVDefault
{
    public class UtilLogs
    {
        public static void PrintLog(ILogger logger, AppSettings appSettings, string? message, params object?[] args)
        {
            if (appSettings.log)
            {
                logger.LogInformation(message, args);
            }
        }
    }
}
