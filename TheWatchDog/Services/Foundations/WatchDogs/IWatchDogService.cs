// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using TheWatchDog.Models;

namespace TheWatchDog.Services.Foundations.WatchDogs
	{
	public interface IWatchDogService
		{
		void RunAndListen(Action<WatchDog> actionToBeExecuted
						, Action<WatchDog> actionOnChange
						, Action<WatchDog> actionOnSuccessful = null
						, Action<WatchDog> actionOnException = null
						, Action<WatchDog> actionOnCancel = null);

		void Cancel();
		}
	}
