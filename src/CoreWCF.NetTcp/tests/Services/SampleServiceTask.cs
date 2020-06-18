using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreWCF;
using ServiceContract;

namespace Services
{

    
   // [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SampleServiceTask : ISampleServiceTaskServerside
    {
        // Token: 0x060007F4 RID: 2036 RVA: 0x0001F26C File Offset: 0x0001D46C
        public Task<List<Book>> SampleMethodAsync(string name, string publisher)
        {
            Func<List<Book>> function = () => new List<Book>
            {
                new Book
                {
                    Name = name,
                    Publisher = publisher
                }
            };
            Task<List<Book>> task = new Task<List<Book>>(function);
            task.Start();
            return task;
        }

        // Token: 0x060007F5 RID: 2037 RVA: 0x0001F2A8 File Offset: 0x0001D4A8
        public Task SampleMethodAsync2(string name)
        {
            Action action = delegate ()
            {
                name = (name ?? "Olga");
            };
            Task task = new Task(action);
            task.Start();
            return task;
        }
    }
   
}
