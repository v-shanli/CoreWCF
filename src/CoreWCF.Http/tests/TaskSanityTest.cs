using ClientContract;
using CoreWCF.Configuration;
using Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class TaskSanityTest
    {
        private const string bookName = "Great Expectations";
        private const string bookPublisher = "Penguin Publishers";
        private ITestOutputHelper _output;


        public TaskSanityTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void BasicHttpRequestReplyEchoString()
        {
            
            var host = ServiceHelper.CreateWebHostBuilder<Startup>(_output).Build();
            using (host)
            {
                host.Start();

                var binding = ClientHelper.GetBufferedModeBinding();
                var channelFactory = new System.ServiceModel.ChannelFactory<ClientContract.ISampleServiceTaskServerside>(binding,
                    new System.ServiceModel.EndpointAddress(new Uri("http://localhost:8080/BasicWcfService/basichttp.svc")));
                var channel = channelFactory.CreateChannel();

                Task<List<ClientContract.Book>> task = channel.SampleMethodAsync(bookName, bookPublisher);
                //task.Wait(5000);

                StringBuilder sb = new StringBuilder();
                foreach (Book book in task.Result)
                {
                    sb.Append(book.Name);
                    sb.Append(",");
                    sb.Append(book.Publisher);
                    _output.WriteLine(book.Name);
                }
                string expectedResult = String.Format("{0},{1}", bookName, bookPublisher);
                if (sb.ToString() != expectedResult)
                {
                    throw new InvalidOperationException(string.Format("Incorrect response, expected response {0}, actual response {1}", expectedResult, sb.ToString()));
                }
                _output.WriteLine("Variation passed");
            }


        }

        internal class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddServiceModelServices();
            }

            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                app.UseServiceModel(builder =>
                {
                    builder.AddService<Services.SampleServiceTask>();
                    builder.AddServiceEndpoint<Services.SampleServiceTask, ServiceContract.ISampleServiceTaskServerside>(new CoreWCF.BasicHttpBinding(), "/BasicWcfService/basichttp.svc");
                });
            }
        }
    }
}