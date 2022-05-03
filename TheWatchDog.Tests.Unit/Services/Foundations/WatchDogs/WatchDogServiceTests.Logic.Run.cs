// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------


using Moq;
using System;
using TheWatchDog.Models;
using Xunit;

namespace TheWatchDog.Tests.Unit.Services.Foundations.WatchDogs
	{
	public partial class WatchDogServiceTests
		{
		[Fact]
		public void ShouldRunWatchDog()
			{
			// given
			var watchDogActionToBeExecutedMock =
				new Mock<Action<WatchDog>>();

			var watchDogActionOnChangeMock =
				new Mock<Action<WatchDog>>();

			// when
			watchDogService.RunAndListen(
				actionToBeExecuted: watchDogActionToBeExecutedMock.Object
				, actionOnChange: watchDogActionOnChangeMock.Object);

			// then
			this.watchDogBrokerMock.Verify(broker =>
				broker.RunAndListen(
					It.IsAny<Action<WatchDog>>()
					, It.IsAny<Action<WatchDog, int, object>>()
					, It.IsAny<Action<WatchDog, bool, object, Exception>>())
				, Times.Once());

			this.watchDogBrokerMock.VerifyNoOtherCalls();
			}
		}
	}
