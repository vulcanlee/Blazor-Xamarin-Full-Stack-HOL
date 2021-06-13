using System;

namespace AC03.Services
{
    public interface IMyService
    {
        Guid Guid { get; set; }
        int HashCode { get; }
    }
    public interface IMyServiceTransient : IMyService { }
    public interface IMyServiceScoped : IMyService { }
    public interface IMyServiceSingleton : IMyService { }
}