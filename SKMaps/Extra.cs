using System;

namespace SKMaps
{
	public partial class SKMapsService
	{
		public static void InitializeSKMapsWithAPIKey(string apiKey)
		{
			SKMaps.SKMapsService.SharedInstance.InitializeSKMapsWithAPIKey(apiKey, SKMaps.SKMapsInitSettings.MapsInitSettings);
		}
	}
}
