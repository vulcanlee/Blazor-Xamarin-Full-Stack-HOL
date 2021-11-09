using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IPasswordPolicyService
    {
        ILogger<PasswordPolicyService> Logger { get; }
        IMapper Mapper { get; }

        Task CheckPasswordAge(CancellationToken cancellationToken);
    }
}