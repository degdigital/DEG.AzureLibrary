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
        public static TResult TimeoutInvoke<TResult>(this Func<TResult> source, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                onResolve();
                return action.EndInvoke(result);
            }
            threadToKill.Abort();
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
        public static TResult TimeoutInvoke<T1, TResult>(this Func<T1, TResult> source, T1 arg1, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                onResolve();
                return action.EndInvoke(result);
            }
            threadToKill.Abort();
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
        public static TResult TimeoutInvoke<T1, T2, TResult>(this Func<T1, T2, TResult> source, T1 arg1, T2 arg2, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1, arg2); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                onResolve();
                return action.EndInvoke(result);
            }
            threadToKill.Abort();
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
        public static TResult TimeoutInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> source, T1 arg1, T2 arg2, T3 arg3, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1, arg2, arg3); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                onResolve();
                return action.EndInvoke(result);
            }
            threadToKill.Abort();
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
        public static TResult TimeoutInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            Thread threadToKill = null;
            Func<TResult> action = () => { threadToKill = Thread.CurrentThread; return source(arg1, arg2, arg3, arg4); };
            var result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                onResolve();
                return action.EndInvoke(result);
            }
            threadToKill.Abort();
            onTimeout();
            return default(TResult);
        }
    }
}