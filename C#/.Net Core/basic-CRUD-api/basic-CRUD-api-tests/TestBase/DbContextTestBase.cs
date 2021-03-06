﻿using basic_CRUD_api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace basic_CRUD_api_tests.TestBase
{
    public class DbContextTestBase : TestBase
    {
        protected ApplicationDbContext CreateInMemoryContext(string databaseName = null)
        {
            databaseName ??= "basic_CRUD_api_test";
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName), ServiceLifetime.Scoped)
                .BuildServiceProvider();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInternalServiceProvider(serviceProvider)
                .UseInMemoryDatabase(databaseName)
                .Options;
            return new ApplicationDbContext(options);
        }

        protected void SaveAndDetachContext(DbContext context)
        {
            context.SaveChanges();
            foreach (var entity in context.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }
        }
    }
}
