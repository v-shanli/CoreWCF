using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CoreWCF;
using Xunit.Abstractions;
using ServiceContract;
using Helpers;
using Microsoft.AspNetCore.Builder;
using System.ServiceModel.Channels;
using CoreWCF.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace CoreWCF.NetTcp.Tests
{
    public class AsyncNetTcpBingTests
    {

        private const string bookName = "Great Expectations";
        private const string bookPublisher = "Penguin Publishers";
        private ITestOutputHelper _output;

        public AsyncNetTcpBingTests(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void HostServiceAndValidate()
        {

            // Create client
            var host = ServiceHelper.CreateWebHostBuilder<Startup>(_output).Build();
            using (host)
            {

                host.Start();

                var binding = ClientHelper.GetBufferedModeBinding();
                var channelFactory = new System.ServiceModel.ChannelFactory<ClientContract.ISampleServiceTaskServerside>(binding,
                    new System.ServiceModel.EndpointAddress(new Uri("net.tcp://localhost:8808//nettcp.svc/test")));
                var channel = channelFactory.CreateChannel();
                
                Task<List<ClientContract.Book>> task = channel.SampleMethodAsync(bookName, bookPublisher);
                task.Wait(5000);

                channelFactory.Close();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (ClientContract.Book book in task.Result)
                {
                    stringBuilder.Append(book.Name);
                    stringBuilder.Append(",");
                    stringBuilder.Append(book.Publisher);                 
                    _output.WriteLine(book.Name);

                }
                string text = string.Format("{0},{1}", bookName, bookPublisher);
                if (stringBuilder.ToString() != text)
                {
                    throw new InvalidOperationException(string.Format("Incorrect response, expected response {0}, actual response {1}", text, stringBuilder.ToString()));
                }
            }
        }
        public class Startup
        {
           

            public const string RelatveAddress = "/nettcp.svc/test";
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddServiceModelServices();

            }

            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {

                app.UseServiceModel(builder =>
                {
                    builder.AddService<Services.SampleServiceTask>();
                    builder.AddServiceEndpoint<Services.SampleServiceTask, ServiceContract.ISampleServiceTaskServerside>(new CoreWCF.NetTcpBinding(CoreWCF.SecurityMode.None), RelatveAddress);
                   
                });
            }
        }
    }

   
}




