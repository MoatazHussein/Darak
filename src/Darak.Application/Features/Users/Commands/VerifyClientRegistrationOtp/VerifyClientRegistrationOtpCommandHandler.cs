using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Constants;
using Darak.Domain.Entities;
using Darak.Domain.Enums;
using Darak.Domain.Exceptions;
using MediatR;

namespace Darak.Application.Features.Users.Commands.VerifyClientRegistrationOtp;

public class VerifyClientRegistrationOtpCommandHandler(
    IOtpService otpService,
    IUserService userService
    ) : IRequestHandler<VerifyClientRegistrationOtpCommand, bool>
{
    public async Task<bool> Handle(VerifyClientRegistrationOtpCommand request, CancellationToken ct)
    {

        // 1) Verify OTP
        var ok = await otpService.VerifyAsync(request.PhoneNumber, OtpPurpose.Register, request.Code, ct);
        if (!ok) throw new BusinessRuleException("Invalid or expired OTP.");

        // 2) Normalize the phone (E.164 ideally) – implement inside IUserService
        var phone = await userService.NormalizePhoneAsync(request.PhoneNumber, ct);

        // 3) Lookup by phone
        var existing = await userService.FindByPhoneAsync(phone, ct);
        if (existing is not null)
        {
            // ensure it's confirmed (idempotent)
            if (!existing.PhoneNumberConfirmed)
                await userService.MarkPhoneConfirmedAsync(existing.Id, ct);

            throw new BusinessRuleException("This User with associated phone already exists");
        }

        // 4) Create phone-only user (no email, no password)
        var newUser = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = phone,               
            PhoneNumber = phone,
            PhoneNumberConfirmed = true,    
            Email = null,                   
            EmailConfirmed = false,
            FirstName = request.FirstName ?? phone,            
            LastName = request.LastName ?? null,
            Location = request.Location,
            UserType = UserType.Client,
            CreatedAt = DateTime.UtcNow,
        };

        // Delegate creation to your service (wraps UserManager/AppDbContext)
        var created = await userService.CreateUserAsync(newUser, ct);

        // Assign default role (e.g., Client)
        await userService.AddToRoleAsync(created.Id, UserRoles.Client, ct);


        return ok;

    }
}
