using System;
using System.Threading.Tasks;

namespace Simix.Extensions.Logging.Extensions {
    /// <summary>
    /// Task extensions
    /// </summary>
    internal static class TaskExtensions {
        /// <summary>
        /// Based on Brandon Minnick repository <see href="https://github.com/brminnick/AsyncAwaitBestPractices/blob/master/Src/AsyncAwaitBestPractices/TaskExtensions.cs">Async Await Best Practices</see>
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread.
        /// If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        /// <param name="onException">If an exception is thrown in the ValueTask, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
#pragma warning disable RECS0165
        public static async void SafeFireAndForget(this Task task,
            bool continueOnCapturedContext = true, Action<Exception> onException = null) {
#pragma warning restore RECS0165
            try {
                await task.ConfigureAwait(continueOnCapturedContext);
            } catch (Exception ex) when (onException != null) {
                onException(ex);
            }
        }
    }
}
