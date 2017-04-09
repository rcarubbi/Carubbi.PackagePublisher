using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carubbi.Communication.NamedPipe
{
    public  class Unsubscriber<DTOOutput> : IDisposable
        where DTOOutput : class
    {
        private List<IObserver<DTOOutput>> _observers;
        private IObserver<DTOOutput> _observer;

        internal Unsubscriber(List<IObserver<DTOOutput>> observers, IObserver<DTOOutput> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
