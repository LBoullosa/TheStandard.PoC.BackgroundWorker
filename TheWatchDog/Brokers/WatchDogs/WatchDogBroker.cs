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
		
		private Action<WatchDog> actionOnRunHandler;
		private Action<WatchDog> actionOnChangeHandler;

		private WatchDog watchDog;

		public WatchDogBroker()
		{
			backgroundWorker = new BackgroundWorker();
			InitializeBackgroundWorker();
		}

		private void InitializeBackgroundWorker()
		{
			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
		}

		public void RunAndListen(Action<WatchDog> actionOnRunHandler
								, Action<WatchDog> actionOnChangeHandler)
		{
			this.actionOnRunHandler = actionOnRunHandler;
			this.actionOnChangeHandler = actionOnChangeHandler;

			this.watchDog = new WatchDog()
				{
				Id = Guid.NewGuid()
				, State = WatchDogState.Initializating
				, NotifyProgress = (int progressPorcentage, object userState) =>
						backgroundWorker.ReportProgress(progressPorcentage, userState)
				};

			backgroundWorker.RunWorkerAsync();
		}

		private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			actionOnRunHandler(watchDog);
		}

		private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			watchDog.ProgressPercentage = e.ProgressPercentage;
			watchDog.UserState = e.UserState;

			actionOnChangeHandler(watchDog);
		}

		private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			watchDog.IsCompleted = true;
			watchDog.IsCancelled = e.Cancelled;
			watchDog.Result = e.Result;
			watchDog.Exception = e.Error;

			actionOnChangeHandler(watchDog);
		}

		public void Cancel()
		{
			backgroundWorker.CancelAsync();
		}

	}
}
