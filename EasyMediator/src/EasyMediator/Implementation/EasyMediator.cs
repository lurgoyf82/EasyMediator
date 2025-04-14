using EasyMediator.Core;
using EasyMediator.Registration;

namespace EasyMediator.Implementation
{
    public class EasyMediator : IMediator
    {
        private readonly HandlerRegistry _registry;

        public EasyMediator(HandlerRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            if (!_registry.TryGetHandler(requestType, responseType, out var handlerDelegate))
            {
                throw new InvalidOperationException(
                    $"Handler not found for request type '{requestType.FullName}' with response type '{responseType.FullName}'.");
            }

            var result = await handlerDelegate(request, cancellationToken).ConfigureAwait(false);
            return (TResponse)result;
        }
    }
}
