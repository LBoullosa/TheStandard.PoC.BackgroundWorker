// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Tynamix.ObjectFiller;
using TheWatchDog.Brokers.WatchDogs;
using TheWatchDog.Services.Foundations.WatchDogs;
using System;
using System.Linq.Expressions;
using TheWatchDog.Models;
using System.Threading;

namespace TheWatchDog.Tests.Unit.Services.Foundations.WatchDogs
{
	public partial class WatchDogServiceTests
	{
		private readonly Mock<IWatchDogBroker> watchDogBrokerMock;
		private readonly IWatchDogService watchDogService = null;

		public WatchDogServiceTests()
		{
			this.watchDogBrokerMock = new Mock<IWatchDogBroker>();
			watchDogService = new WatchDogService(watchDogBrokerMock.Object);
		}

		private static int GetRandomMiliseconds() =>
			new IntRange(min: 1000, max: 3500).GetValue();

		private Expression<Func<WatchDog, bool>> SameWatchDogAs(WatchDog expectedWatchDog)
		{
			return actualWatchDog => actualWatchDog.Id.Equals(expectedWatchDog.Id);
		}

		/*private static WatchDog CreateRandomWatchDog() =>
			new WatchDog()
			{
				Id = Guid.NewGuid()
				, ActionOnRun = CreateRandomAction()
			};

		private static Action CreateRandomAction() => () =>
			{
			Thread.Sleep(GetRandomMiliseconds());
			};*/

	}
}
