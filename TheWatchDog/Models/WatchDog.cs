// ---------------------------------------------------------------
// Copyright (c) Raúl Lorenzo Boullosa
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;

namespace TheWatchDog.Models
	{
	public class WatchDog
		{
		public Guid Id { get; init; }

		public int ThreadId { get; set; }

		public WatchDogStatus Status { get; set; }

		public object Result { get; set; }

		public Exception Exception { get; set; }

		public int ProgressPercentage { get; set; }

		public object UserState { get; set; }

		public Func<bool> IsBusy { get; init; }

		public Func<bool> IsRequestedForCancellation { get; init; }

		public Action<int, object> NotifyProgress { get; init; }

		}
	}
