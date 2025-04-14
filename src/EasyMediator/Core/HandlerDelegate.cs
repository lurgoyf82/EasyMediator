namespace EasyMediator.Core
{
    public delegate Task<object> HandlerDelegate(object request, CancellationToken cancellationToken);
}
