using System;
using Contract;
using Contract.Commands.Orders;

namespace BusinessLayer.CommandHandlers
{
	public class ValidateOrderCommandHandler : ICommandHandler<ValidateOrderCommand>
	{
		private readonly ILogger logger;

		public ValidateOrderCommandHandler(ILogger logger)
		{
			this.logger = logger;
		}

		public void Handle(CreateOrderCommand command)
		{
			// Do something useful here.
			command.CreatedOrderId = Guid.NewGuid();

			this.logger.Log(this.GetType().Name + " has been executed successfully.");
		}

		public void Handle(ValidateOrderCommand command)
		{
			// Do something useful here.
			command.Validated = true;

			logger.Log(GetType().Name + " has been executed successfully.");
		}
	}
}