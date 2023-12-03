using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQLXCommon.ObjectExtensions
{
	public static class ExtendTask
	{
		///// <summary>
		///// Times out if the task execution takes longer that the specified number of milliseconds. 
		///// Up to 596.5 hours of timeout can be specified.
		///// </summary>
		///// <param name="task">The task to execute</param>
		///// <param name="millisecondsTimeout">How long to wait for the task to return (&lt;= 0 causes 
		///// this value to be = int.MaxValue.</param>
		///// <returns></returns>
		//public static async Task TimeoutAfter(this Task task, int millisecondsTimeout)
		//{
		//	// make sure we have a sane timout value
		//	millisecondsTimeout = (millisecondsTimeout <= 0) ? int.MaxValue : millisecondsTimeout;
		//	if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
		//	{
		//		await task;
		//	}
		//	else
		//	{
		//		throw new TimeoutException();
		//	}
		//}
		//public static Task TimeoutAfter(this Task task, int millisecondsTimeout)
		//{
		//	// Short-circuit #1: infinite timeout or task already completed
		//	if (task.IsCompleted || (millisecondsTimeout == Timeout.Infinite))
		//	{
		//		// Either the task has already completed or timeout will never occur.
		//		// No proxy necessary.
		//		return task;
		//	}

		//	// tcs.Task will be returned as a proxy to the caller
		//	TaskCompletionSource<VoidTypeStruct> tcs = new TaskCompletionSource<VoidTypeStruct>();

		//	// Short-circuit #2: zero timeout
		//	if (millisecondsTimeout == 0)
		//	{
		//		// We've already timed out.
		//		tcs.SetException(new TimeoutException());
		//		return tcs.Task;
		//	}

		//	// Set up a timer to complete after the specified timeout period
		//	Timer timer = new Timer(state =>
		//	{
		//		// Recover your state information
		//		var myTcs = (TaskCompletionSource<VoidTypeStruct>)state;
		//		// Fault our proxy with a TimeoutException
		//		myTcs.TrySetException(new TimeoutException());
		//	}, tcs, millisecondsTimeout, Timeout.Infinite);

		//	// Wire up the logic for what happens when source task completes
		//	task.ContinueWith(antecedent =>
		//						{
		//							timer.Dispose(); // Cancel the timer
		//							MarshalTaskResults(antecedent, tcs); // Marshal results to proxy
		//						},
		//						CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

		//	return tcs.Task;
		//}
	}

}
