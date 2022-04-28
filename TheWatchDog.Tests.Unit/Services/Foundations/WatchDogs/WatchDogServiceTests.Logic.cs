// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using TheWatchDog.Models;
using Xunit;

namespace TheWatchDog.Tests.Unit.Services.Foundations.WatchDogs
{
	public partial class WatchDogServiceTests
	{
		[Fact]
		public void ShouldRunWorker()
		{
			// given
			bool inputResult = false;
			bool actualResult = inputResult;
			bool expectedResult = true;

			Action actionOnRun = async () =>
				{
				await Task.Delay(GetRandomMiliseconds());
				actualResult = true;
				};

			WatchDog inputJob = new WatchDog()
				{
				ActionOnRun = actionOnRun
				};

			watchDogBrokerMock
				.Setup(broker =>
					broker.RunAndListen(inputJob));

			// when
			watchDogService.Run(actionOnRun: actionOnRun);

			// then
			expectedResult.Should().Be(actualResult);

			this.watchDogBrokerMock.Verify(broker =>
				broker.RunAndListen(inputJob)
				, Times.Once());

			this.watchDogBrokerMock.VerifyNoOtherCalls();
		}
	}
}
