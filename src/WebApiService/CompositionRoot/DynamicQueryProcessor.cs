using BusinessLayer;

namespace WebApiService.CompositionRoot
{
    using System.Diagnostics;
    
    using Contract;

    using SimpleInjector;

    public sealed class DynamicQueryProcessor : IQueryProcessor
    {
        private readonly Container container;
	    private readonly ILogger _logger;

	    public DynamicQueryProcessor(Container container, ILogger logger)
        {
	        this.container = container;
	        _logger = logger;
        }

	    [DebuggerStepThrough]
        public TResult Execute<TResult>(IQuery<TResult> query)
        {
			_logger.Log("DynamicQueryProcessor Execute");
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

			_logger.Log("DynamicQueryProcessor handler Type: " + handlerType);

            dynamic handler = this.container.GetInstance(handlerType);

            return handler.Handle((dynamic)query);
        }
    }
}