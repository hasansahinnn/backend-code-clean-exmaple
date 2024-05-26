using System.Reflection;
using Core.Mailing;
using Core.Rules;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;

namespace Service;

/// <summary>
/// Extension methods for registering service services in the DI container.
/// </summary>
/// <remarks>
/// This class is used to register services in the DI container.
/// </remarks>
/// <returns>
/// The service collection.
/// </returns>
/// <param name="services">The service collection.</param>
public static class ServiceServicesRegistration
{
    public static IServiceCollection AddServiceServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IUserService, UserService>();

        services.AddSingleton<IMailService, MailKitMailService>();

        return services;
    }

    /// <summary>
    /// Add all sub classes of a type from an assembly.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="type"></param>
    /// <param name="addWithLifeCycle"></param>
    public static IServiceCollection AddSubClassesOfType(
        this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
        {
            if (addWithLifeCycle == null)
            {
                services.AddScoped(item);
            }
            else
            {
                addWithLifeCycle(services, type);
            }
        }
        return services;
    }
}