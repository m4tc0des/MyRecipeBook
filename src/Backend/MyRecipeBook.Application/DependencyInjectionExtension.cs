using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Login.WithEmailAndPassword;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddUseCases();
    }

    public static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();

        services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();

        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();

        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();

        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
    }
}
