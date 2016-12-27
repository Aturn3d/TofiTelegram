using System;

namespace Bot.DAL.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool disposed;

        ~Disposable()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if(!disposed && disposing)
            {
                DisposeCore();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeCore()
        {

        }

    }
}
