﻿using Application.Core;
using Infrastructure.CommonFrame.EntityFramework;
using Infrastructure.Modules;
using System.Data.Entity;
using System.Reflection;

namespace Application.EntityFramework
{
    [DependsOn(typeof(CommonFrameEntityFrameworkModule), typeof(ApplicationCoreModule))]
    public class ApplicationDataModule : InfrastructureModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
