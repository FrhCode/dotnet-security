using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Security.Api;
using Security.Api.Enums;
using Security.Api.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// register services
builder.Services.Configure<SmtpOption>(builder.Configuration.GetSection("Smtp"));
builder.Services.Configure<JwtOption>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<FacebookOauthOption>(builder.Configuration.GetSection("FacebookOauth"));

builder.Services.AddSingleton<IEmailSender, SmtpMailSender>();

// register applicationDBContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// register Identity
builder.Services
	.AddIdentity<IdentityUser, IdentityRole>(options =>
	{
		options.SignIn.RequireConfirmedEmail = true;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

// register authentication middleware for JWT
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
		JwtOption jwtOption = builder.Configuration.GetSection("Jwt").Get<JwtOption>() ?? throw new InvalidOperationException("JwtOption is missing in appsettings.json");
		// get jwt service from jwt option class
		options.TokenValidationParameters = new TokenValidationParameters
		{
			// ValidateIssuer = true,
			// ValidateAudience = true,
			// ValidateLifetime = true,
			// ValidateIssuerSigningKey = true,
			ValidIssuer = jwtOption.Issuer,
			ValidAudience = jwtOption.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Key))
		};
	}).AddFacebook(options =>
	{
		FacebookOauthOption facebookOauthOption = builder.Configuration.GetSection("FacebookOauth").Get<FacebookOauthOption>() ?? throw new InvalidOperationException("FacebookOauthOption is missing in appsettings.json");
		options.AppId = facebookOauthOption.AppId;
		options.AppSecret = facebookOauthOption.AppSecret;
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

