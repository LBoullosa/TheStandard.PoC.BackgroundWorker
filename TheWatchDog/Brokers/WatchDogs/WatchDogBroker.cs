// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.ComponentModel;
using TheWatchDog.Models;

namespace TheWatchDog.Brokers.WatchDogs
{
	public class WatchDogBroker : IWatchDogBroker
	{
		private readonly BackgroundWorker backgroundWorker;
		WatchDog watchDog;

		public WatchDogBroker() =>
			backgroundWorker = new BackgroundWorker();

		private void InitializeBackgroundWorker()
		{
			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
		}

		public void RunAndListen(WatchDog watchDog)
		{
			this.watchDog = watchDog;

			InitializeBackgroundWorker();

			watchDog.State = WatchDogState.Initialized;

			backgroundWorker.RunWorkerAsync();
		}

		private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			watchDog.State = WatchDogState.Running;
			
			watchDog.ActionOnRun?.Invoke();
			watchDog.State = WatchDogState.Runned;
		}

		private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			watchDog.ActionDuringRun?.Invoke();
		}

		private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				watchDog.State = WatchDogState.Canceling;
				watchDog.ActionOnCancel?.Invoke();
				watchDog.State = WatchDogState.Cancelled;
			}

			if (e.Error is not null)
			{
				watchDog.State = WatchDogState.Error;
				watchDog.ActionOnException?.Invoke();
			}
			else
			{
				watchDog.State = WatchDogState.Finalizing;
				watchDog.ActionOnSuccessfulRun?.Invoke();
				watchDog.State = WatchDogState.Finalized;
			}
		}

		public void Cancel()
		{
			backgroundWorker.CancelAsync();
		}

	}
}
