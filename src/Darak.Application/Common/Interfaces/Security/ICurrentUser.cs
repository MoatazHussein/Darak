using System;
using System.Collections.Generic;

namespace Darak.Application.Common.Interfaces.Security
{
    /// <summary>
    /// Framework-agnostic current user abstraction.
    /// Implement it in the API/Infrastructure by reading from HttpContext or the hosting environment.
    /// </summary>
    public interface ICurrentUser
    {
        Guid? Id { get; }
        string? Email { get; }
        IReadOnlyCollection<string> Roles { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}
