﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Infrastructure.Persistences.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(POSContext).Assembly.FullName;

            services.AddDbContext<POSContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("POSConnectionString"), b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

            services.AddTransient<IUnitOfWork, UnitOfWork>()
;
            return services;
        }
    }
}
