// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using TheWatchDog.Models;

namespace TheWatchDog.Services.Foundations.WatchDogs
{
	public interface IWatchDogService
	{
		void RunAndListen(Func<WatchDog, Task> eventHandler
				, Action actionOnRun = null
				, Action actionDuringRun = null
				, Action actionOnSuccessfulRun = null
				, Action actionOnException = null
				, Action actionOnCancel = null);

		void Cancel();
	}
}
