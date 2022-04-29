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
		public void ShouldPublishWatchDogEvent()
		{
			// given
			WatchDog randomWatchDog = CreateRandomWatchDog();
			WatchDog inputWatchDog = randomWatchDog;
			WatchDog expectedWatchDog = inputWatchDog;

			// when
			watchDogService.RunAndListen(
				eventHandler: inputWatchDog.CompletedEventHandler);

			// then

			this.watchDogBrokerMock.Verify(broker =>
				broker.RunAndListen(
					It.Is<WatchDog>(x=>x.CompletedEventHandler == inputWatchDog.CompletedEventHandler))
				, Times.Once());

			this.watchDogBrokerMock.VerifyNoOtherCalls();
		}
	}
}
