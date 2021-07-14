using System;
using NLog;

namespace Divuss.Service
{
	internal static class Logger
	{
		private static NLog.Logger logger;

		public static void LogTrace(string message)
		{
			logger.Trace(message);
		}

		public static void LogError(string message)
		{
			logger.Error("ОШИБКА: " + message);
		}

		public static void Initialize()
		{
			logger = LogManager.GetCurrentClassLogger();
			logger.Trace("Запуск приложения...");
		}

		public static void CompleteLaunch()
		{
			logger.Trace("Приложение успешно запущено!");
		}

		public static void Close()
		{
			logger.Trace("Приложение завершило работу...\n");
		}

		public static void ForcedClose()
		{
			logger.Trace("Приложение принудительно остановлено...\n");
		}
	}
}
