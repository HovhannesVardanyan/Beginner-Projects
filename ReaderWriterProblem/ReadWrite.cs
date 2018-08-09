using System.Threading;

namespace Montior
{
    class ReadWrite
    {   
        private int readerCount = 0;
        private bool SomeoneIsWriting = false;
        private object Lock = new object();
        private bool WriterIsWaiting = false;

        public void BeginRead()
        {
            lock (Lock)
            {
                while (SomeoneIsWriting || WriterIsWaiting)
                    Monitor.Wait(Lock);
                readerCount++;
            }
        }
        public void EndRead()
        {
            lock (Lock)
            {
                readerCount--;
                if (readerCount == 0)
                    Monitor.Pulse(Lock);
            }
        }
        public void BeginWrite()
        {
            lock (Lock)
            {
                while (WriterIsWaiting)
                    Monitor.Wait(Lock);

                if (readerCount > 0)
                    WriterIsWaiting = true;
                

                while (readerCount != 0 || SomeoneIsWriting)
                    Monitor.Wait(Lock);
                
                WriterIsWaiting = false;
                SomeoneIsWriting = true;

            }
        }
        public void EndWrite()
        {
            lock (Lock)
            {
                SomeoneIsWriting = false;
                Monitor.PulseAll(Lock);
            }
        }
    }

}
