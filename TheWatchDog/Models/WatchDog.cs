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
		public Guid Id { get; set; }

		public WatchDogState State { get; set; }

		public bool IsCompleted { get; set; }
		
		public bool IsCancelled { get; set; }

		public object Result { get; set; }

		public Exception Exception { get; set; }

		public int ProgressPercentage { get; set; }

		public object UserState { get; set; }

		public Action<int, object> NotifyProgress { get; set; }

	}
}
