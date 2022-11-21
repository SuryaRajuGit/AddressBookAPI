//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.AspNetCore.Hosting;
//using AutoMapper;
//using AddressBookAPI.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using AddressBookAPI.Repository;
//using AddressBookAPI.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace AddressBookUnitTest
//{
//    public class TestStartup
//    {
//        public TestStartup(IHostingEnvironment env)
//        {
//            var builder = new ConfigurationBuilder()
//                .SetBasePath(env.ContentRootPath)
//                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
//                .AddEnvironmentVariables();

//            Configuration = builder.Build();
//        }

//        public IConfigurationRoot Configuration { get; }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddSwaggerGen();
//            services.AddAuthentication(option =>
//            {
//                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//                .AddJwtBearer(option =>
//                {
//                    option.SaveToken = true;
//                    option.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuerSigningKey = true,
//                        ValidAudience = Configuration["Jwt:ValidAudience"],
//                        ValidIssuer = Configuration["Jwt:ValidIssuer"],
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])),
//                        ValidateIssuer = false,
//                        ValidateAudience = false,
//                    };
//                });
//            services.AddAutoMapper(typeof(TestStartup));
//            services.AddDbContext<AddressBookContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AddressBookDB")));
//            services.AddControllers();
//            services.AddTransient<IAddressBookRepository, AddressBookRepository>();
//            services.AddTransient<IAddressBookServices, AddressBookServices>();
//            services.Configure<ApiBehaviorOptions>(options =>
//            {
//                options.SuppressModelStateInvalidFilter = true;
//            });

//            services.AddControllersWithViews()
//            .AddNewtonsoftJson(options =>
//            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//            );
//        }

//        public void Configure(IApplicationBuilder app)
//        {
//            //if (env.IsDevelopment())
//            //{
//            //    app.UseDeveloperExceptionPage();
//            //}
//            app.UseSwagger();
//            app.UseSwaggerUI();
//            app.UseRouting();
//            app.UseAuthorization();
//            app.UseAuthentication();
//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });

//        }
//    }
//}
