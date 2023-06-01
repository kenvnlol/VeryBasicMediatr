using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediatrLight;

public class MediatrLight : IMediatrLight
{
    public Dictionary<Type, Type> IRequestDict { get; set; }
    public MediatrLight(Dictionary<Type, Type> iRequestDict)
    {
        IRequestDict = iRequestDict;
    }
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        // Try to get the handler type for the request type
        if (IRequestDict.TryGetValue(requestType, out var handlerType))
        {
            // Create an instance of the handler
            var handlerInstance = Activator.CreateInstance(handlerType);

            // Invoke the handler's Handle method
            var handleMethod = handlerType.GetMethod("Handle");
            var result = (Task<TResponse>)handleMethod.Invoke(handlerInstance, new object[] { request, cancellationToken });

            return result;
        }
        else
        {
            throw new InvalidOperationException($"No handler found for request type: {requestType}");
        }
    }
}
