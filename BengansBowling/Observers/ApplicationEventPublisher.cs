using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
