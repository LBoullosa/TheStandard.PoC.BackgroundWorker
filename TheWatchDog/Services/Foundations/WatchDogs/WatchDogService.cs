// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using TheWatchDog.Brokers.WatchDogs;
using TheWatchDog.Models;

namespace TheWatchDog.Services.Foundations.WatchDogs
{
	public class WatchDogService : IWatchDogService
	{
		private readonly IWatchDogBroker watchDogBroker;

		public WatchDogService(IWatchDogBroker watchDogBroker) => this.watchDogBroker = watchDogBroker;

		public void Run(Action actionOnRun = null
						, Action actionDuringRun = null
						, Action actionOnSuccessfulRun = null
						, Action actionOnException = null
						, Action actionOnCancel = null)
		{
			WatchDog backgroundDog = new Models.WatchDog()
				{
				Id = Guid.NewGuid()
				, State = WatchDogState.Initializating
				, ActionOnRun = actionOnRun
				, ActionDuringRun = actionDuringRun
				, ActionOnSuccessfulRun = actionOnSuccessfulRun
				, ActionOnException = actionOnException
				, ActionOnCancel = actionOnCancel
				};

			watchDogBroker.RunAndListen(backgroundDog);
		}

		public void Cancel()
		{
			throw new NotImplementedException();
		}

	}
}
