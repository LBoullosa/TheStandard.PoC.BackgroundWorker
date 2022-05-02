// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using TheWatchDog.Brokers.WatchDogs;
using TheWatchDog.Models;

namespace TheWatchDog.Services.Foundations.WatchDogs
{
	public class WatchDogService : IWatchDogService
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

			watchDogBroker.RunAndListen(OnRunHandler, OnChangeHandler);
		}

		private void OnRunHandler(WatchDog watchDog)
		{
			watchDog.State = WatchDogState.Running;
			actionToBeExecuted(watchDog);
			watchDog.State = WatchDogState.Runned;
		}

		private void OnChangeHandler(WatchDog watchDog)
		{
			actionOnChange(watchDog);

			if (watchDog.IsCompleted)
			{
				if (watchDog.Exception is not null)
				{
					watchDog.State = WatchDogState.Erroring;
					actionOnException?.Invoke(watchDog);
					watchDog.State = WatchDogState.Errored;
				}
				else if (watchDog.IsCancelled)
				{
					watchDog.State = WatchDogState.Canceling;
					actionOnCancel?.Invoke(watchDog);
					watchDog.State = WatchDogState.Cancelled;
				}
				else
				{
					watchDog.State = WatchDogState.Finalizing;
					actionOnSuccessful?.Invoke(watchDog);
					watchDog.State = WatchDogState.Finalized;
				}
			}
		}

		public void Cancel()
		{
			watchDogBroker.Cancel();
		}

	}
}
