using System.Net;

namespace Backend.Helpers;

public static class HttpContextExtensions
{
    //https://gist.github.com/jjxtra/3b240b31a1ed3ad783a7dcdb6df12c36

    public static IPAddress GetRemoteIPAddress(this HttpContext context, bool allowForwarded = true)
    {
        if (allowForwarded)
        {
            string header = (context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault());
            if (IPAddress.TryParse(header, out IPAddress ip))
            {
                return ip;
            }
        }
        return context.Connection.RemoteIpAddress;
    }
}
