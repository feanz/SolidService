using System;
using System.Net;

namespace Contract.Commands.Orders
{
	[WebApiResponse(HttpStatusCode.Created)]
	public class ValidateOrderCommand
	{
		// Output property
		public Guid OrderId { get; set; }

		public bool Validated { get; set; }
	}
}