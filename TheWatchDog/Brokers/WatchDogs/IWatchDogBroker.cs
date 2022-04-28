// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using TheWatchDog.Models;

namespace TheWatchDog.Brokers.WatchDogs
{
	public interface IWatchDogBroker
	{
		void RunAndListen(WatchDog backgroundDog);
		void Cancel();
	}
}
