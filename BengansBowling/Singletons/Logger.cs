using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Singleton pattern säkerställer att en klass bara har en instans och ger en global åtkomstpunkt till den,
//vilket är idealiskt för loggningsmekanismer. Genom att använda en Singleton-logger säkerställer
//applikationen att alla komponenter använder samma instans av loggern.


namespace BengansBowling.Singletons
{
    using System;
    using System.IO;

    public class Logger
    {
        private static Logger _instance;
        private static readonly object _lock = new object();
        private readonly string _logFilePath;

        private Logger() {
            _logFilePath = Path.GetFullPath(@"../../../LogOutput/LogOutput.txt");
                                            
            if (!File.Exists(_logFilePath)) {
                File.Create(_logFilePath).Close();
            }
        }

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

        public void Log(string message) {
            string logEntry = $"{DateTime.Now}: {message}\n";
            File.AppendAllText(_logFilePath, logEntry);
        }
    }

}
