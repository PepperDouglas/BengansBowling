using BengansBowling.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Observers
{
    public class LoggerObserver : IObserver
    {
        public void Update(string message) {
            Logger.Instance.Log(message);
        }
    }

}
