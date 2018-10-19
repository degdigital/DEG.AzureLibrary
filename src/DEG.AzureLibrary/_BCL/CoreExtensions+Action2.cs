using System.Threading;
namespace System
{
    public static partial class CoreExtensions
    {
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        public static void TimeoutInvokePromise(this Action<Action> source, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(() => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                if (!hasAborted) { action.EndInvoke(result); onResolve(); }
                else onTimeout();
                return;
            }
            threadToKill.Abort(); onTimeout();
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        public static void TimeoutInvokePromise<T1>(this Action<T1, Action> source, T1 arg1, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                if (!hasAborted) { action.EndInvoke(result); onResolve(); }
                else onTimeout();
                return;
            }
            threadToKill.Abort(); onTimeout();
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        public static void TimeoutInvokePromise<T1, T2>(this Action<T1, T2, Action> source, T1 arg1, T2 arg2, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1, arg2, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                if (!hasAborted) { action.EndInvoke(result); onResolve(); }
                else onTimeout();
                return;
            }
            threadToKill.Abort(); onTimeout();
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="T3">The type of the t3.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="arg3">The arg3.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        public static void TimeoutInvokePromise<T1, T2, T3>(this Action<T1, T2, T3, Action> source, T1 arg1, T2 arg2, T3 arg3, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1, arg2, arg3, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                if (!hasAborted) { action.EndInvoke(result); onResolve(); }
                else onTimeout();
                return;
            }
            threadToKill.Abort(); onTimeout();
            return;
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="T3">The type of the t3.</typeparam>
        /// <typeparam name="T4">The type of the t4.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="arg3">The arg3.</param>
        /// <param name="arg4">The arg4.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        public static void TimeoutInvokePromise<T1, T2, T3, T4>(this Action<T1, T2, T3, T4, Action> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1, arg2, arg3, arg4, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                if (!hasAborted) { action.EndInvoke(result); onResolve(); }
                else onTimeout();
                return;
            }
            threadToKill.Abort(); onTimeout();
        }
        /// <summary>
        /// Timeouts the invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="T3">The type of the t3.</typeparam>
        /// <typeparam name="T4">The type of the t4.</typeparam>
        /// <typeparam name="T5">The type of the t5.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="arg3">The arg3.</param>
        /// <param name="arg4">The arg4.</param>
        /// <param name="arg5">The arg5.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        public static void TimeoutInvokePromise<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5, Action> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            bool hasAborted = false; Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1, arg2, arg3, arg4, arg5, () => { hasAborted = true; threadToKill.Abort(); }); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                if (!hasAborted) { action.EndInvoke(result); onResolve(); }
                else onTimeout();
                return;
            }
            threadToKill.Abort(); onTimeout();
        }
    }
}
