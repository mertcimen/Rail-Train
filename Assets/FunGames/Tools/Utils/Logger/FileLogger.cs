using UnityEngine;
using System.IO;

public class FileLogger
{
    private static FileLogger instance = null;
    private static readonly object padlock = new object();

    FileLogger()
    {
        installLogFile();
    }

    public static FileLogger Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new FileLogger();
                }
                return instance;
            }
        }
    }

    public static void WriteLog(string log)
    {
        File.AppendAllText(getLogFile(), "\r\n" + log);
    }

    private void installLogFile()
    {
        string filePath = getLogFile();
        if (File.Exists(filePath)) return;
        File.WriteAllText(filePath, "Logger installed - " + Time.time);
    }

    private static string getLogFile()
    {
        return Application.persistentDataPath + "/logger.log";
    }
}
