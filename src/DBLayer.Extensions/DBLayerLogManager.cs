using DBLayer.Logging;

namespace DBLayer.Extensions;

public static class DBLayerLogManager
{
    public static void UseConsoleLogging(LogLevel minimumLevel = LogLevel.Trace)
    {
        LogManager.UseConsoleLogging(minimumLevel);
    }
    public static void UseNLogLogging(LogLevel minimumLevel = LogLevel.Trace)
    {
        LogManager.SetLevel(minimumLevel);
        LogManager.LogFactory = name => (level, message, exception) =>
        {
            if (level < minimumLevel) return;

            var logger = NLog.LogManager.GetLogger(name);
            switch (level)
            {
                case LogLevel.Trace:
                    logger.Trace(exception, message);
                    break;
                case LogLevel.Debug:
                    logger.Debug(exception, message);
                    break;
                case LogLevel.Information:
                    logger.Info(exception, message);
                    break;
                case LogLevel.Warning:
                    logger.Warn(exception, message);
                    break;
                case LogLevel.Error:
                    logger.Error(exception, message);
                    break;
                case LogLevel.Critical:
                    logger.Fatal(exception, message);
                    break;
                default:
                    logger.Info(exception, message);
                    break;
            }
        };
    }
}
