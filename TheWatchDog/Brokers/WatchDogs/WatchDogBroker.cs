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
		WatchDog backgroundDog;

		public WatchDogBroker() =>
			backgroundWorker = new BackgroundWorker();

		public void RunAndListen(WatchDog backgroundDog)
		{
			this.backgroundDog = backgroundDog;
			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundDog.State = WatchDogState.Initialized;
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.RunWorkerAsync();
		}

		public void Cancel()
		{
			backgroundWorker.CancelAsync();
		}

		private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			backgroundDog.State = WatchDogState.Running;
			backgroundDog.ActionOnRun?.Invoke();
			backgroundDog.State = WatchDogState.Runned;
		}

		private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			backgroundDog.ActionDuringRun?.Invoke();
		}

		private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				backgroundDog.State = WatchDogState.Canceling;
				backgroundDog.ActionOnCancel?.Invoke();
				backgroundDog.State = WatchDogState.Cancelled;
			}

			if (e.Error is not null)
			{
				backgroundDog.State = WatchDogState.Error;
				backgroundDog.ActionOnException?.Invoke();
			}
			else
			{
				backgroundDog.State = WatchDogState.Finalizing;
				backgroundDog.ActionOnSuccessfulRun?.Invoke();
				backgroundDog.State = WatchDogState.Finalized;
			}
		}
	}
}
