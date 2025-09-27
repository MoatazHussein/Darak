namespace Darak.Domain.Constants;

public static class OtpPurpose
{
    public const string Register = "Register";
    public const string Login = "Login";

    public static readonly HashSet<string> All = new(StringComparer.OrdinalIgnoreCase)
    { Register, Login  };
}

