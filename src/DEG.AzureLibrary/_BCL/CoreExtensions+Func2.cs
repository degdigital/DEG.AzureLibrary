using System.Threading;
namespace System
{
    public static partial class CoreExtensions
    {
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>TResult.</returns>
        public static TResult TimeoutInvokePromise<TResult>(this Func<Action, TResult> source, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(() => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds) && !hasAborted)
            {
                onResolve();
                return action.EndInvoke(result);
            }
            if (!hasAborted) threadToKill.Abort();
            onTimeout();
            return default(TResult);
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>TResult.</returns>
        public static TResult TimeoutInvokePromise<T1, TResult>(this Func<T1, Action, TResult> source, T1 arg1, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds) && !hasAborted)
            {
                onResolve();
                return action.EndInvoke(result);
            }
            if (!hasAborted) threadToKill.Abort();
            onTimeout();
            return default(TResult);
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>TResult.</returns>
        public static TResult TimeoutInvokePromise<T1, T2, TResult>(this Func<T1, T2, Action, TResult> source, T1 arg1, T2 arg2, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1, arg2, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds) && !hasAborted)
            {
                onResolve();
                return action.EndInvoke(result);
            }
            if (!hasAborted) threadToKill.Abort();
            onTimeout();
            return default(TResult);
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="T3">The type of the t3.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="arg3">The arg3.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>TResult.</returns>
        public static TResult TimeoutInvokePromise<T1, T2, T3, TResult>(this Func<T1, T2, T3, Action, TResult> source, T1 arg1, T2 arg2, T3 arg3, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1, arg2, arg3, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds) && !hasAborted)
            {
                onResolve();
                return action.EndInvoke(result);
            }
            if (!hasAborted) threadToKill.Abort();
            onTimeout();
            return default(TResult);
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="T3">The type of the t3.</typeparam>
        /// <typeparam name="T4">The type of the t4.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="arg3">The arg3.</param>
        /// <param name="arg4">The arg4.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>TResult.</returns>
        public static TResult TimeoutInvokePromise<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, Action, TResult> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1, arg2, arg3, arg4, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds) && !hasAborted)
            {
                onResolve();
                return action.EndInvoke(result);
            }
            if (!hasAborted) threadToKill.Abort();
            onTimeout();
            return default(TResult);
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="T3">The type of the t3.</typeparam>
        /// <typeparam name="T4">The type of the t4.</typeparam>
        /// <typeparam name="T5">The type of the t5.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="arg3">The arg3.</param>
        /// <param name="arg4">The arg4.</param>
        /// <param name="arg5">The arg5.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>TResult.</returns>
        public static TResult TimeoutInvokePromise<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, Action, TResult> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1, arg2, arg3, arg4, arg5, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds) && !hasAborted)
            {
                onResolve();
                return action.EndInvoke(result);
            }
            if (!hasAborted) threadToKill.Abort();
            onTimeout();
            return default(TResult);
        }
    }
}
