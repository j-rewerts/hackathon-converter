﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Converter.Services.Data;
using Google.Cloud.PubSub.V1;

namespace Converter.Services.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        private const string ANALYSIS_TOPIC_NAME = "ConverterAnalysis";
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddAnalysisRepository(options =>
            {
                // TODO: configure database options (like connection string)
            });

            // configure publisher
            string projectId = Configuration["Google:ProjectId"];
            if (string.IsNullOrWhiteSpace(projectId))
                throw new InvalidOperationException("Unable to get the projectId from configuration");
            var topicNameString = ANALYSIS_TOPIC_NAME;
            var publisherClient = PublisherClient.Create();
            var topicName = new TopicName(projectId, topicNameString);
            // ensure the topic exists (is there a better way?)
            try { publisherClient.CreateTopic(topicName); }
            catch (Grpc.Core.RpcException e) {  /* topic already exists */ }

            var publisher = SimplePublisher.Create(
                topicName, new[] { publisherClient });
            services.AddSingleton(publisher);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
