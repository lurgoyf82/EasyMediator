using EasyMediator.Core;

namespace EasyMediator.Registration
{
    public class HandlerRegistry
    {
        private readonly Dictionary<(Type requestType, Type responseType), HandlerDelegate> _handlers =
            new Dictionary<(Type, Type), HandlerDelegate>();

        public void Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
            where TRequest : IRequest<TResponse>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var key = (typeof(TRequest), typeof(TResponse));
            if (_handlers.ContainsKey(key))
            {
                throw new InvalidOperationException(
                    $"A handler is already registered for '{typeof(TRequest).FullName}' -> '{typeof(TResponse).FullName}'.");
            }

            HandlerDelegate compiledDelegate = async (request, cancellationToken) =>
            {
                return await handler.Handle((TRequest)request, cancellationToken).ConfigureAwait(false);
            };

            _handlers[key] = compiledDelegate;
        }

        public bool TryGetHandler(Type requestType, Type responseType, out HandlerDelegate handlerDelegate)
        {
            return _handlers.TryGetValue((requestType, responseType), out handlerDelegate);
        }
    }
}
