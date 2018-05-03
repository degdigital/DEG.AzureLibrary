using System.Collections.Generic;
using System.Threading;
namespace System
{
    public static partial class CoreExtensions
    {
        /// <summary>
        /// Timeouts the yield invoke.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        public static IEnumerable<TResult> TimeoutYieldInvoke<TResult>(this Func<IEnumerable<TResult>> source, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            var ready = new ManualResetEventSlim(false);
            Action action = () => { ready.Wait(timeoutMilliseconds); };
            var result = action.BeginInvoke(null, null);
            var values = source();
            var isCompleted = false;
            if (values != null)
                foreach (var x in values)
                {
                    if (isCompleted = result.IsCompleted) break;
                    yield return x;
                }
            ready.Set();
            if (isCompleted) { action.EndInvoke(result); onTimeout(); }
            else onResolve();
        }
        /// <summary>
        /// Timeouts the yield invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        public static IEnumerable<TResult> TimeoutYieldInvoke<T1, TResult>(this Func<T1, IEnumerable<TResult>> source, T1 arg1, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            var ready = new ManualResetEventSlim(false);
            Action action = () => { ready.Wait(timeoutMilliseconds); };
            var result = action.BeginInvoke(null, null);
            var values = source(arg1);
            var isCompleted = false;
            if (values != null)
                foreach (var x in values)
                {
                    if (isCompleted = result.IsCompleted) break;
                    yield return x;
                }
            ready.Set();
            if (isCompleted) { action.EndInvoke(result); onTimeout(); }
            else onResolve();
        }
        /// <summary>
        /// Timeouts the yield invoke.
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
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        public static IEnumerable<TResult> TimeoutYieldInvoke<T1, T2, TResult>(this Func<T1, T2, IEnumerable<TResult>> source, T1 arg1, T2 arg2, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            var ready = new ManualResetEventSlim(false);
            Action action = () => { ready.Wait(timeoutMilliseconds); };
            var result = action.BeginInvoke(null, null);
            var values = source(arg1, arg2);
            var isCompleted = false;
            if (values != null)
                foreach (var x in values)
                {
                    if (isCompleted = result.IsCompleted) break;
                    yield return x;
                }
            ready.Set();
            if (isCompleted) { action.EndInvoke(result); onTimeout(); }
            else onResolve();
        }
        /// <summary>
        /// Timeouts the yield invoke.
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
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        public static IEnumerable<TResult> TimeoutYieldInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, IEnumerable<TResult>> source, T1 arg1, T2 arg2, T3 arg3, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            var ready = new ManualResetEventSlim(false);
            Action action = () => { ready.Wait(timeoutMilliseconds); };
            var result = action.BeginInvoke(null, null);
            var values = source(arg1, arg2, arg3);
            var isCompleted = false;
            if (values != null)
                foreach (var x in values)
                {
                    if (isCompleted = result.IsCompleted) break;
                    yield return x;
                }
            ready.Set();
            if (isCompleted) { action.EndInvoke(result); onTimeout(); }
            else onResolve();
        }
        /// <summary>
        /// Timeouts the yield invoke.
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
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        public static IEnumerable<TResult> TimeoutYieldInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, IEnumerable<TResult>> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            var ready = new ManualResetEventSlim(false);
            Action action = () => { ready.Wait(timeoutMilliseconds); };
            var result = action.BeginInvoke(null, null);
            var values = source(arg1, arg2, arg3, arg4);
            var isCompleted = false;
            if (values != null)
                foreach (var x in values)
                {
                    if (isCompleted = result.IsCompleted) break;
                    yield return x;
                }
            ready.Set();
            if (isCompleted) { action.EndInvoke(result); onTimeout(); }
            else onResolve();
        }
        #region Extend
        /// <summary>
        /// Timeouts the yield invoke.
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
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        public static IEnumerable<TResult> TimeoutYieldInvoke<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, IEnumerable<TResult>> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            var ready = new ManualResetEventSlim(false);
            Action action = () => { ready.Wait(timeoutMilliseconds); };
            var result = action.BeginInvoke(null, null);
            var values = source(arg1, arg2, arg3, arg4, arg5);
            var isCompleted = false;
            if (values != null)
                foreach (var x in values)
                {
                    if (isCompleted = result.IsCompleted) break;
                    yield return x;
                }
            ready.Set();
            if (isCompleted) { action.EndInvoke(result); onTimeout(); }
            else onResolve();
        }
        /// <summary>
        /// Timeouts the yield invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="T3">The type of the t3.</typeparam>
        /// <typeparam name="T4">The type of the t4.</typeparam>
        /// <typeparam name="T5">The type of the t5.</typeparam>
        /// <typeparam name="T6">The type of the t6.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="arg3">The arg3.</param>
        /// <param name="arg4">The arg4.</param>
        /// <param name="arg5">The arg5.</param>
        /// <param name="arg6">The arg6.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        public static IEnumerable<TResult> TimeoutYieldInvoke<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, IEnumerable<TResult>> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            var ready = new ManualResetEventSlim(false);
            Action action = () => { ready.Wait(timeoutMilliseconds); };
            var result = action.BeginInvoke(null, null);
            var values = source(arg1, arg2, arg3, arg4, arg5, arg6);
            var isCompleted = false;
            if (values != null)
                foreach (var x in values)
                {
                    if (isCompleted = result.IsCompleted) break;
                    yield return x;
                }
            ready.Set();
            if (isCompleted) { action.EndInvoke(result); onTimeout(); }
            else onResolve();
        }
        /// <summary>
        /// Timeouts the yield invoke.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <typeparam name="T3">The type of the t3.</typeparam>
        /// <typeparam name="T4">The type of the t4.</typeparam>
        /// <typeparam name="T5">The type of the t5.</typeparam>
        /// <typeparam name="T6">The type of the t6.</typeparam>
        /// <typeparam name="T7">The type of the t7.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        /// <param name="arg3">The arg3.</param>
        /// <param name="arg4">The arg4.</param>
        /// <param name="arg5">The arg5.</param>
        /// <param name="arg6">The arg6.</param>
        /// <param name="arg7">The arg7.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="onResolve">The on resolve.</param>
        /// <param name="onTimeout">The on timeout.</param>
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        public static IEnumerable<TResult> TimeoutYieldInvoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<TResult>> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, int timeoutMilliseconds, Action onResolve = null, Action onTimeout = null)
        {
            var ready = new ManualResetEventSlim(false);
            Action action = () => { ready.Wait(timeoutMilliseconds); };
            var result = action.BeginInvoke(null, null);
            var values = source(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            var isCompleted = false;
            if (values != null)
                foreach (var x in values)
                {
                    if (isCompleted = result.IsCompleted) break;
                    yield return x;
                }
            ready.Set();
            if (isCompleted) { action.EndInvoke(result); onTimeout(); }
            else onResolve();
        }
        #endregion
    }
}
