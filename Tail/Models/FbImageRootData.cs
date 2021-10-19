
using Newtonsoft.Json;

namespace Tail.Models
{
	public class FbImageRootData
	{
		[JsonProperty("data")]
		public FbImage Picture { get; set; }
	}

	public class FbImage
	{

		[JsonProperty("is_silhouette")]
		public bool is_silhouette { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}
