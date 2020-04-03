using System;
using System.Threading.Tasks;

namespace Simix.Extensions.Logging.Extensions {
    /// <summary>
    /// Task extensions
    /// </summary>
    internal static class TaskExtensions {
        /// <summary>
        /// Método baseado na documentação <see href="https://github.com/brminnick/AsyncAwaitBestPractices/blob/master/Src/AsyncAwaitBestPractices/TaskExtensions.cs">Async Await Best Practices</see>
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="continueOnCapturedContext">Quando <c>true</c> continua quando captura o contexto; Isso vai garantir que o "Synchronization Context" vai retornar para a thread original.
        /// Se for <c>false</c> continua no contexto diferente, oque possibilida a execução em uma thread diferente</param>
        /// <param name="onException">Se uma exceção for lançada na task, <c>onException</c> vai executar. Se onException for null, a exception irá dar re-thrown</param>
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
