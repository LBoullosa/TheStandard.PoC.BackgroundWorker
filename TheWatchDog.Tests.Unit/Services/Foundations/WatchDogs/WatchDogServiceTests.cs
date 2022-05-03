// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using TheWatchDog.Brokers.WatchDogs;
using TheWatchDog.Models;
using TheWatchDog.Services.Foundations.WatchDogs;
using Tynamix.ObjectFiller;

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

		private static WatchDog CreateRandomWatchDog() =>
			new WatchDog()
				{
				Id = Guid.NewGuid()
				};

		private static Action<WatchDog> CreateRandomAction() => (watchDog) =>
			{
			watchDog.NotifyProgress(10, "Doing stuff ...");
			Thread.Sleep(GetRandomMiliseconds());
			watchDog.NotifyProgress(90, "Stuff done.");
			};

		}
	}
