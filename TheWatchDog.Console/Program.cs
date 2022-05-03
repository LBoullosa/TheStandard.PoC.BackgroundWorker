// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading;
using TheWatchDog.Brokers.WatchDogs;
using TheWatchDog.Models;
using TheWatchDog.Services.Foundations.WatchDogs;

IWatchDogBroker watchDogBroker = new WatchDogBroker();
IWatchDogService watchDogService = new WatchDogService(watchDogBroker);

watchDogService.RunAndListen(
	actionToBeExecuted: (WatchDog watchDog) =>
		{
			watchDog.NotifyProgress(10, "Doing stuff ...");
			Thread.Sleep(10000);
			watchDog.NotifyProgress(90, "Stuff done.");
		}

	, actionOnChange: (WatchDog watchDog) =>
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder
				.AppendLine()
				.AppendLine($"[{DateTime.Now.ToString("o")}] Event received from watchdog dispatcher on Thread {Thread.CurrentThread.ManagedThreadId}")
				.AppendLine($"\tBackground Thread Id -> {watchDog.ThreadId}")
				.AppendLine($"\tId -> {watchDog.Id}")
				.AppendLine($"\tStatus -> {watchDog.Status}")
				.AppendLine($"\tIs Busy -> {watchDog.IsBusy()}")
				.AppendLine($"\tIs Requested for cancellation -> {watchDog.IsRequestedForCancellation()}")
				.AppendLine($"\tProgress -> {watchDog.ProgressPercentage}%")
				.AppendLine($"\tUserState -> {watchDog.UserState}");
			Console.Write(stringBuilder.ToString());
		}

	, actionOnSuccessful: (WatchDog watchDog) =>
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder
				.AppendLine()
				.AppendLine($"[{DateTime.Now.ToString("o")}] Event completed received from watchdog dispatcher on Thread {Thread.CurrentThread.ManagedThreadId}")
				.AppendLine($"\tThread {watchDog.ThreadId} - Completed");

			Console.Write(stringBuilder.ToString());
		}
	);

Console.Write($"[{DateTime.Now.ToString("o")}] Listening WatchDogService from Thread {Thread.CurrentThread.ManagedThreadId} ...{Environment.NewLine}");
Console.ReadKey(true);
