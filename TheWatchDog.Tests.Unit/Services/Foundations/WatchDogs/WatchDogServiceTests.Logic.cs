// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TheWatchDog.Models;
using Xunit;

namespace TheWatchDog.Tests.Unit.Services.Foundations.WatchDogs
{
	public partial class WatchDogServiceTests
	{
		[Fact]
		public void ShouldListenToWorker()
		{
			// given
			var watchDogEventHandlerMock =
				new Mock<Func<WatchDog, Task>>();

			WatchDog randomWatchDog = CreateRandomWatchDog();
			WatchDog inputWatchDog = randomWatchDog;

			watchDogBrokerMock
				.Setup(broker =>
					broker.RunAndListen(
						It.IsAny<WatchDog>()))
				.Callback<WatchDog>(watchDog =>
					{
					watchDog.CompletedEventHandler.Invoke(inputWatchDog);
					});

			// when
			watchDogService.RunAndListen(
				eventHandler: watchDogEventHandlerMock.Object
				, actionOnRun: randomWatchDog.ActionOnRun);

			// then
			watchDogEventHandlerMock.Verify(handler =>
				handler.Invoke(It.Is(SameWatchDogAs(inputWatchDog)))
				, Times.Once);

			this.watchDogBrokerMock.Verify(broker =>
				broker.RunAndListen(It.IsAny<WatchDog>())
				, Times.Once());

			this.watchDogBrokerMock.VerifyNoOtherCalls();
		}
	}
}
