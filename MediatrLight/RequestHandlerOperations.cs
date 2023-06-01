using System.Reflection;


namespace MediatrLight;

public class RequestHandlerOperations
{
    public Dictionary<Type, Type> CreateMappings(Assembly assembly)
    {
        var mappings = assembly.GetTypes()
          .Where(x => x.GetInterfaces().Any(i =>
              i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
          .Select(x => new
          {
              QueryType = x.GetInterfaces()
                  .First(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                  .GetGenericArguments()[0],
              HandlerType = x
          }).ToDictionary(x => x.QueryType, x => x.HandlerType);


        return mappings;
    }
}
