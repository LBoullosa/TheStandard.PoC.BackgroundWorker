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
		public void ShouldListenToWatchDogEvent()
		{
			// given
			var watchDogEventHandlerMock =
				new Mock<Func<WatchDog, Task>>();

			// when
			watchDogService.RunAndListen(
				eventHandler: watchDogEventHandlerMock.Object);

			// then
			this.watchDogBrokerMock.Verify(broker =>
				broker.RunAndListen(
					It.Is<WatchDog>(x => x.CompletedEventHandler == watchDogEventHandlerMock.Object))
				, Times.Once());

			this.watchDogBrokerMock.VerifyNoOtherCalls();
		}
	}
}
