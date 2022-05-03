// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading;
using TheWatchDog.Brokers.WatchDogs;
using TheWatchDog.Models;

namespace TheWatchDog.Services.Foundations.WatchDogs
	{
	public partial class WatchDogService : IWatchDogService
		{
		private readonly IWatchDogBroker watchDogBroker;

		private Action<WatchDog> actionOnChange;
		private Action<WatchDog> actionToBeExecuted;
		private Action<WatchDog> actionOnSuccessful;
		private Action<WatchDog> actionOnException;
		private Action<WatchDog> actionOnCancel;

		public WatchDogService(IWatchDogBroker watchDogBroker) => this.watchDogBroker = watchDogBroker;

		public void RunAndListen(
				Action<WatchDog> actionToBeExecuted
				, Action<WatchDog> actionOnChange
				, Action<WatchDog> actionOnSuccessful = null
				, Action<WatchDog> actionOnException = null
				, Action<WatchDog> actionOnCancel = null)
			{
			this.actionToBeExecuted = actionToBeExecuted;
			this.actionOnChange = actionOnChange;
			this.actionOnSuccessful = actionOnSuccessful;
			this.actionOnException = actionOnException;
			this.actionOnCancel = actionOnCancel;

			watchDogBroker.RunAndListen(OnRunHandler, OnProgressHandler, OnCompleteHandler);
			}

		private void SetWatchDogStatus(WatchDog watchDog, WatchDogStatus newStatus)
			{
			watchDog.Status = newStatus;
			OnChangeHandler(watchDog);
			}

		private void OnRunHandler(WatchDog watchDog)
			{
			watchDog.ThreadId = Thread.CurrentThread.ManagedThreadId;
			SetWatchDogStatus(watchDog, WatchDogStatus.Initialized);

			SetWatchDogStatus(watchDog, WatchDogStatus.Running);

			String message = $"[{DateTime.Now.ToString("o")}] Thread {Thread.CurrentThread.ManagedThreadId} - Running Long Time Execution ...";
			watchDog.NotifyProgress(0, message);

			actionToBeExecuted(watchDog);

			message = $"[{DateTime.Now.ToString("o")}] Thread {Thread.CurrentThread.ManagedThreadId} - End Long Time Execution.";
			watchDog.NotifyProgress(100, message);
			}

		private void OnProgressHandler(WatchDog watchDog
										, int progressPorcentage
										, object userState)
			{
			lock (watchDog)
				{
				watchDog.ProgressPercentage = progressPorcentage;
				watchDog.UserState = userState;

				OnChangeHandler(watchDog);
				}
			}

		private void OnCompleteHandler(WatchDog watchDog
										, bool isCancelled
										, object result
										, Exception exception)
			{
			SetWatchDogStatus(watchDog, WatchDogStatus.Runned);

			watchDog.Exception = exception;
			watchDog.Result = result;

			if (exception is not null)
				SetWatchDogStatus(watchDog, WatchDogStatus.Erroring);
			else if (isCancelled)
				SetWatchDogStatus(watchDog, WatchDogStatus.Canceling);
			else
				SetWatchDogStatus(watchDog, WatchDogStatus.Finalizing);
			}

		private void OnChangeHandler(WatchDog watchDog)
			{
			lock (watchDog)
				{
				actionOnChange(watchDog);

				switch (watchDog.Status)
					{
					case WatchDogStatus.Initialized:
						break;

					case WatchDogStatus.Running:
						break;

					case WatchDogStatus.Runned:
						break;

					case WatchDogStatus.Canceling:
						actionOnCancel?.Invoke(watchDog);
						SetWatchDogStatus(watchDog, WatchDogStatus.Cancelled);
						break;

					case WatchDogStatus.Erroring:
						actionOnException?.Invoke(watchDog);
						SetWatchDogStatus(watchDog, WatchDogStatus.Errored);
						break;

					case WatchDogStatus.Finalizing:
						actionOnSuccessful?.Invoke(watchDog);
						SetWatchDogStatus(watchDog, WatchDogStatus.Finalized);
						break;

					default:
						break;
					}
				}
			}

		public void Cancel() =>
			watchDogBroker.Cancel();

		}
	}
