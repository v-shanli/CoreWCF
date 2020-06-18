using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ClientContract
{
	public class Book
	{
		// Token: 0x04000238 RID: 568
		[DataMember]
		public string Name;

		// Token: 0x04000239 RID: 569
		[DataMember]
		public Guid ISBN;

		// Token: 0x0400023A RID: 570
		[DataMember]
		public string Publisher;
	}
	[ServiceContract(Namespace = "http://microsoft.samples", Name = "ISampleServiceTaskServerside")]
	public interface ISampleServiceTaskServerside
	{
		// Token: 0x060007EA RID: 2026
		[OperationContract]
		Task<List<Book>> SampleMethodAsync(string name, string publisher);

		// Token: 0x060007EB RID: 2027
		[OperationContract]
		Task SampleMethodAsync2(string name);
	}

}
