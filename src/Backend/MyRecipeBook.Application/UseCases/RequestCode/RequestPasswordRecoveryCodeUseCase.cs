using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Repositories.VerificationCode;
using System.Security.Cryptography;

namespace MyRecipeBook.Application.UseCases.RequestCode;

public class RequestPasswordRecoveryCodeUseCase : IRequestPasswordRecoveryCodeUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IVerificationCodeWriteOnlyRepository _verificationCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestPasswordRecoveryCodeUseCase(IUserReadOnlyRepository userReadOnlyRepository, IVerificationCodeWriteOnlyRepository verificationCodeRepository, IUnitOfWork unitOfWork)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _verificationCodeRepository = verificationCodeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestPasswordRecoveryJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);

        if (user is null) return;

        var code = RandomNumberGenerator.GetInt32(1, 1_000_000);

        var verificationCode = new VerificationCode
        {
            Code = code.ToString("D6"),
            Type = VerificationCodeType.PasswordRecovery,
            UserId = user.Id,
        };

        await _verificationCodeRepository.Add(verificationCode);
        await _unitOfWork.Commit();
    }
}
