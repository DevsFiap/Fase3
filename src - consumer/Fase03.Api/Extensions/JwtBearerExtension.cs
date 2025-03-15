using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Fase03.Api.Extensions;

public static class JwtBearerExtension
{
    public static IServiceCollection AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        //var _secret = configuration.GetSection("JwtSettings").GetSection("secret").Value;
        //var _key = Encoding.ASCII.GetBytes(_secret);
        //var _audience = configuration.GetSection("JwtSettings").GetSection("audience").Value;
        //var _issuer = configuration.GetSection("JwtSettings").GetSection("issuer").Value;

        //services.AddAuthentication(opt =>
        //{
        //    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //})
        //.AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = _issuer,
        //        ValidAudience = _audience,
        //        IssuerSigningKey = new SymmetricSecurityKey(_key)
        //    };
        //});

        return services;
    }
}