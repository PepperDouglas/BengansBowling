using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Singletons
{
    using System;
    using System.IO;


    //Use with: Logger.Instance.Log("This is a log message.");
    public class Logger
    {
        private static Logger _instance;
        private static readonly object _lock = new object();
        private readonly string _logFilePath;

        // Constructor is private to prevent instantiation outside of the class.
        private Logger() {
            _logFilePath = Path.GetFullPath(@"../../../LogOutput/LogOutput.txt"); // Specify your log file path here
                                            // Ensure the log file exists
            if (!File.Exists(_logFilePath)) {
                File.Create(_logFilePath).Close();
            }
        }

        // Property to access the singleton instance
        public static Logger Instance {
            get
            {
                if (_instance == null) {
                    lock (_lock) {
                        if (_instance == null) {
                            _instance = new Logger();
                        }
                    }
                }
                return _instance;
            }
        }

        // Method to log messages to the log file
        public void Log(string message) {
            // Prepend the log entry with the current timestamp
            string logEntry = $"{DateTime.Now}: {message}\n";
            File.AppendAllText(_logFilePath, logEntry);
        }
    }

}
