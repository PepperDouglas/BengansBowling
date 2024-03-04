using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Observer pattern används för att etablera en prenumerationsmodell där objekt (observatörer) kan lyssna på och
//reagera på händelser eller förändringar i ett annat objekt. I samband med ett loggningssystem tillåter det
//olika delar av applikationen att meddela en loggningsobservatör om förändringar som kan loggas.

namespace BengansBowling.Observers
{
    public class ApplicationEventPublisher : INotifiable
    {
        private readonly List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer) {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer) {
            _observers.Remove(observer);
        }

        public void Notify(string message) {
            foreach (var observer in _observers) {
                observer.Update(message);
            }
        }
    }

}
