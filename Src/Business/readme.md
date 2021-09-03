# Blazor 商業用專案範本

對於一般的商業應用 Web 網站專案，存在著許多共同都需要進行設計的需求與功能，因此，為了要能夠加速商業應用類型的 Web 網站專案開發速度，便著手進行採用 Blazor Server UI Framework 開發框架來進行設計出一個適用於商業專案需求的專案範本。

因此，便誕生了這個專案範本，這個專案範本具有底下的特色與功能

- [提供 Cookie 的身分認證與授權](#提供-Cookie-的身分認證與授權)
- [提供 Jwt 的身分認證與授權](#提供-Jwt-的身分認證與授權)

Asynchronous programming has been around for several years on the .NET platform but has historically been very difficult to do well. Since the introduction of async/await
in C# 5 asynchronous programming has become mainstream. Modern frameworks (like ASP.NET Core) are fully asynchronous and it's very hard to avoid the async keyword when writing
web services. As a result, there's been lots of confusion on the best practices for async and how to use it properly. This section will try to lay out some guidance with examples of bad and good patterns of how to write asynchronous code.


Asynchronous programming has been around for several years on the .NET platform but has historically been very difficult to do well. Since the introduction of async/await
in C# 5 asynchronous programming has become mainstream. Modern frameworks (like ASP.NET Core) are fully asynchronous and it's very hard to avoid the async keyword when writing
web services. As a result, there's been lots of confusion on the best practices for async and how to use it properly. This section will try to lay out some guidance with examples of bad and good patterns of how to write asynchronous code.


 
# 提供 Jwt 的身分認證與授權

Asynchronous programming has been around for several years on the .NET platform but has historically been very difficult to do well. Since the introduction of async/await

# 提供 Jwt 的身分認證與授權

Asynchronous programming has been around for several years on the .NET platform but has historically been very difficult to do well. Since the introduction of async/await
in C# 5 asynchronous programming has become mainstream. Modern frameworks (like ASP.NET Core) are fully asynchronous and it's very hard to avoid the async keyword when writing
web services. As a result, there's been lots of confusion on the best practices for async and how to use it properly. This section will try to lay out some guidance with examples of bad and good patterns of how to write asynchronous code.

## Asynchrony is viral 

Once you go async, all of your callers **SHOULD** be async, since efforts to be async amount to nothing unless the entire callstack is async. In many cases, being partially async can be worse than being entirely synchronous. Therefore it is best to go all in, and make everything async at once.
