using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediatrLight;

public class MediatrLight : IMediatrLight
{
    private readonly IServiceProvider _serviceProvider;
    private Dictionary<Type, Type> IRequestDict { get; set; }
    public MediatrLight(Dictionary<Type, Type> iRequestDict, IServiceProvider serviceProvider)
    {
        IRequestDict = iRequestDict;
        _serviceProvider = serviceProvider;
    }
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        if (!IRequestDict.ContainsKey(requestType))
            throw new ArgumentException("The requested handled could not be found.");

        // Try to get the handler type for the request type
        IRequestDict.TryGetValue(requestType, out var handlerType);
            var handler = _serviceProvider.GetService(handlerType);

        return await ((IRequestHandler<IRequest<TResponse>, TResponse>)handler).Handle(request);
    }
}
