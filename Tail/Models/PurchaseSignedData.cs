using System;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class PurchaseSignedData
    {
		[JsonProperty("orderId")]
		public string OrderID
		{
			get;
			set;
		}

		[JsonProperty("packageName")]
		public string PackageName
		{
			get;
			set;
		}

		[JsonProperty("productId")]
		public string ProductID
		{
			get;
			set;
		}

		[JsonProperty("purchaseTime")]
		public long PurchaseTimeTicksUtc
		{
			get;
			set;
		}

		[JsonProperty("purchaseState")]
		public int PurchaseState
		{
			get;
			set;
		}

		[JsonProperty("developerPayload")]
		public string DeveloperPayload
		{
			get;
			set;
		}

		[JsonProperty("purchaseToken")]
		public string PurchaseToken
		{
			get;
			set;
		}

		[JsonProperty("autoRenewing")]
		public bool AutoRenewing
		{
			get;
			set;
		}
	}
}
