using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SchoolManangement.Business.Behaviors;
using SchoolManangement.Business.CQRS.Absenteeisms.Commands.CreateAbsenteeism;
using SchoolManangement.Business.CQRS.Courses.Commands.CreateCourse;
using SchoolManangement.Business.MappingProfile;
using SchoolManangement.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.ServiceRegistrations
{
    public static class BuilderExtension
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateAbsenteeismCommand).Assembly);
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssemblyContaining<CreateCourseCommand>();
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingConfig>());
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
