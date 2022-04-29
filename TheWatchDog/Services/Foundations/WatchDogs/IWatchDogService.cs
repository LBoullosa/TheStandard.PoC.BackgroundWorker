// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;

namespace TheWatchDog.Services.Foundations.WatchDogs
{
	public interface IWatchDogService
	{
		void RunAndListen(Action actionOnRun = null
				, Action actionDuringRun = null
				, Action actionOnSuccessfulRun = null
				, Action actionOnException = null
				, Action actionOnCancel = null);

		void Cancel();
	}
}
