using System.Reflection;


namespace MediatrLight;

public class RequestHandlerOperations
{
    public Dictionary<Type, Type> CreateMappings(Assembly assembly)
    {
        var mappings = new Dictionary<Type, Type>();

        // add any class that implements irequest in the key
        // Add class that implements the corresponding irequesthandler in the value
 
            var requestTypes = assembly.GetTypes().Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)));

            foreach (var type in requestTypes)
            {
                var handlerType = assembly.GetTypes().FirstOrDefault(t =>
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)) &&
                t.GetInterfaces().Any(i => i.GetGenericArguments()[0] == type));

                if (handlerType != null)
                {
                    mappings.Add(type, handlerType);
                }
            }
        

        return mappings;
    }
}
