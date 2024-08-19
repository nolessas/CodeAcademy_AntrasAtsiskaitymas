public class ErrorHandler
{
    private const string LogFilePath = "error_log.txt";

    public static void LogError(string message, Exception ex = null)
    {
        string errorMessage = $"[{DateTimeManager.GetCurrentDateTime()}] ERROR: {message}";
        if (ex != null)
        {
            errorMessage += $"Exception: {ex.Message}  Stack Trace: {ex.StackTrace}";
        }

        Console.WriteLine(errorMessage);
        WriteToLogFile(errorMessage);
    }

    private static void WriteToLogFile(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine(message);
                writer.WriteLine(new string('-', 50));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write to log file: {ex.Message}");
        }
    }
}
