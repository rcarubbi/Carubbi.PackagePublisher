using System.ComponentModel;
using System.Threading;

namespace Carubbi.Utils.UIControls
{
    /// <summary>
    /// BackgroundWorker com capacidade de abortar a thread no caso de uma ThreadAbortException
    /// </summary>
    public class AbortableBackgroundWorker : BackgroundWorker
    {
        private Thread workerThread;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            workerThread = Thread.CurrentThread;
            try
            {
                base.OnDoWork(e);
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true; //We must set Cancel property to true!
                Thread.ResetAbort(); //Prevents ThreadAbortException propagation
            }
        }

        /// <summary>
        /// Aborta a thread interna
        /// </summary>
        public void Abort()
        {
            if (workerThread != null)
            {
                workerThread.Abort();
                workerThread = null;
            }
        }
    }
}
