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
        public static void TimeoutInvoke(this Action source, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                action.EndInvoke(result);
                onResolve();
                return;
            }
            threadToKill.Abort();
            onTimeout();
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
        public static void TimeoutInvoke<T1>(this Action<T1> source, T1 arg1, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                action.EndInvoke(result);
                onResolve();
                return;
            }
            threadToKill.Abort();
            onTimeout();
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
        public static void TimeoutInvoke<T1, T2>(this Action<T1, T2> source, T1 arg1, T2 arg2, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1, arg2); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                action.EndInvoke(result);
                onResolve();
                return;
            }
            threadToKill.Abort();
            onTimeout();
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
        public static void TimeoutInvoke<T1, T2, T3>(this Action<T1, T2, T3> source, T1 arg1, T2 arg2, T3 arg3, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1, arg2, arg3); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                action.EndInvoke(result);
                onResolve();
                return;
            }
            threadToKill.Abort();
            onTimeout();
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
        public static void TimeoutInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Action action = () => { threadToKill = Thread.CurrentThread; source(arg1, arg2, arg3, arg4); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                action.EndInvoke(result);
                onResolve();
                return;
            }
            threadToKill.Abort();
            onTimeout();
        }
    }
}
