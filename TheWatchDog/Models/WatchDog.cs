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
		public Action ActionOnRun { get; set; }
		public Action ActionDuringRun { get; set; }
		public Action ActionOnSuccessfulRun { get; set; }
		public Action ActionOnException { get; set; }
		public Action ActionOnCancel { get; set; }
	}
}
