using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

			var watchDogActionOnChangedMock =
				new Mock<Action<WatchDog>>();

			// when
			watchDogService.RunAndListen(
				actionToBeExecuted: watchDogActionToBeExecutedMock.Object
				, actionOnChange: watchDogActionOnChangedMock.Object);

			// then
			this.watchDogBrokerMock.Verify(broker =>
				broker.RunAndListen(
					It.IsAny<Action<WatchDog>>()
					, It.IsAny<Action<WatchDog>>())
				, Times.Once());

			this.watchDogBrokerMock.VerifyNoOtherCalls();
		}

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
