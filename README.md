![Nuget](https://img.shields.io/nuget/v/Simix.Extensions.Logging.svg)

> Veja essa documentação em: [Português](README.pt-BR.md)

# Símix Extensions Logging
Símix extension loggin package

## Getting Started

#### Startup.cs

```C#

public void ConfigureServices(IServiceCollection services) {
    services.Configure<SmtpConfig>(Configuration.GetSection("smtpConfig"));
    services.AddTransient<IEmailSender, AuthMessageSender>();
    services.AddTransient<IEmailLogger, EmailLogger>();

    services.AddTransient<IEmailLoggerProvider, EmailLoggerProvider>(p => new EmailLoggerProvider(p.GetService<IEmailLogger>()));
    ...
}

```


```C#

public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IEmailLoggerProvider emailLoggerProvider) {
    if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
    else app.UseHsts();

    loggerFactory.AddProvider(emailLoggerProvider);
}

```

#### appsettings.json


```json

    "smtpConfig": {
        "host": "smtp.gmail.com",
        "username": "example@gmail.com",
        "password": "yourpassword",
        "port": 587,
        "enablessl": true,
        "domain": "gmail.com",
        "senderEmail": "example@gmail.com",
        "senderName": "Your No Reply Service",
        "destination": [
          "user1@example.com",
          "user2@example.com"
        ]
    }

```

# Coding Style

This project uses [EditorConfig](http://editorconfig.org) settings.
For more info, check this repository [Guidelines](https://github.com/simixsistemas/Guidelines).
