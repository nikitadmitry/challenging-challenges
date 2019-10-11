using System;

//pr 1
namespace Shared.Framework.Utilities
{
    public abstract class BaseDisposable : IDisposable
    {
        /// <summary>
        /// Determine if the object has already been disposed.
        /// </summary>
        protected bool isDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Finalizer if the object wasn't disposed properly.  
        /// </summary>
        /// <remarks>This is used to make sure unmanaged resources are cleaned up properly.</remarks>
        ~BaseDisposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// This is the main dispose method.  
        /// </summary>
        /// <remarks>Don't override this method.  Instead, override DisposeManaged and DisposeUnmanaged</remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary> 
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// are disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer: only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">
        /// Indicates, whether method has been
        /// called directly by user code from Dispose() or from Finalizer.
        /// </param>
        /// <remarks>Don't override this method.  Instead, override DisposeManaged and DisposeUnmanaged</remarks>
        protected void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                // no need to dispose twice.
                return;
            }

            // Can reference other managed objects
            if (disposing)
            {
                DisposeManaged();
            }

            // free unmanaged resources, but don't touch other managed objects here!
            DisposeUnmanaged();
            isDisposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This is called to dispose any IDisposable references to managed code that you have.  
        /// </summary>
        /// <remarks>This is only called when Dispose() is invoked directly.  In this case, all 
        /// references are valid and you can refer to any object you have a reference to.</remarks>
        protected virtual void DisposeManaged()
        {
        }

        /// <summary>
        /// This is called to dispose any unmanaged objects you have.
        /// </summary>
        /// <remarks>This is called when Dispose() is invoked directly and during finalization.  
        /// Therefore, you need to follow all the rules about finalization.
        /// Please see <a href="http://msdn.microsoft.com/en-us/library/system.object.finalize%28v=vs.110%29.aspx" /> for details.
        /// </remarks>
        protected virtual void DisposeUnmanaged()
        {
        }
    }
}