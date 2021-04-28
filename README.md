# Donutz:studio

https://www.somkiat.cc/deploy-container-on-heroku/

---

```csharp
// Program.cs
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            var port = Environment.GetEnvironmentVariable("PORT");

            webBuilder.UseStartup<Startup>().UseUrls("http://*:" + port); ;
        });
```

---

Use **only** endpoint as url. Hostname not needed

```javascript
fetch('/ENDPOINT')
```
