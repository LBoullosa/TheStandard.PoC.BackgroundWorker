// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Threading;
using TheWatchDog.Models;

namespace TheWatchDog.Brokers.WatchDogs
	{
	public class WatchDogBroker : IWatchDogBroker
		{
		private BackgroundWorker backgroundWorker;

		private Action<WatchDog> actionOnRunHandler;
		private Action<WatchDog, int, object> actionOnProgressHandler;
		private Action<WatchDog, bool, object, Exception> actionOnCompleteHandler;

		private WatchDog watchDog;

		public WatchDogBroker() { }

		private void InitializeBackgroundWorker()
			{
			backgroundWorker = new BackgroundWorker();

			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			}

		private void DisposeBackgroundWorker()
			{
			backgroundWorker.DoWork -= BackgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted -= BackgroundWorker_RunWorkerCompleted;
			backgroundWorker.ProgressChanged -= BackgroundWorker_ProgressChanged;
			backgroundWorker.Dispose();
			backgroundWorker = null;
			}

		private void BuildWatchDog()
			{
			this.watchDog = new WatchDog()
				{
				Id = Guid.NewGuid()
				, Status = WatchDogStatus.Initializating
				, IsBusy = () =>
					backgroundWorker.IsBusy
				, IsRequestedForCancellation = () =>
					backgroundWorker.CancellationPending
				, NotifyProgress = (int progressPorcentage, object userState) =>
					backgroundWorker.ReportProgress(progressPorcentage, userState)
				};
			}

		public void RunAndListen(Action<WatchDog> actionOnRunHandler
								, Action<WatchDog, int, object> actionOnProgressHandler
								, Action<WatchDog, bool, object, Exception> actionOnCompleteHandler)
			{
			this.actionOnRunHandler = actionOnRunHandler;
			this.actionOnProgressHandler = actionOnProgressHandler;
			this.actionOnCompleteHandler = actionOnCompleteHandler;

			InitializeBackgroundWorker();

			BuildWatchDog();

			backgroundWorker.RunWorkerAsync();
			}

		private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) =>
			actionOnRunHandler(watchDog);

		private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) =>
			actionOnProgressHandler(watchDog, e.ProgressPercentage, e.UserState);

		private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
			{
			actionOnCompleteHandler(watchDog, e.Cancelled, e.Result, e.Error);

			DisposeBackgroundWorker();
			}

		public void Cancel() =>
			backgroundWorker.CancelAsync();

		}
	}
