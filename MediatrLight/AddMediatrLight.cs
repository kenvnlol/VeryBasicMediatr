using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace MediatrLight;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMediatrLight(this IServiceCollection service, Assembly assembly)
    {
        var requestHandling = new RequestHandlerOperations();

        var mappings = requestHandling.CreateMappings(assembly);

        service.AddSingleton(mappings);
        service.AddScoped<IMediatrLight, MediatrLight>();


        return service;
    }
}
