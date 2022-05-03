// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------


using Moq;
using Xunit;

namespace TheWatchDog.Tests.Unit.Services.Foundations.WatchDogs
	{
	public partial class WatchDogServiceTests
		{
		[Fact]
		public void ShouldCancelWatchDog()
			{
			// when
			watchDogService.Cancel();

			// then
			this.watchDogBrokerMock.Verify(broker =>
				broker.Cancel()
				, Times.Once());

			this.watchDogBrokerMock.VerifyNoOtherCalls();
			}
		}
	}
