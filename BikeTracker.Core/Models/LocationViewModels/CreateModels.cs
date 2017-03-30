using BikeTracker.Core.Models.LocationModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WebsiteHelpers.Interfaces;

#pragma warning disable 1591

namespace BikeTracker.Core.Models.LocationViewModels
{
	[GeneratedCode("Model Generator", "v0.3.26"), ExcludeFromCodeCoverage]
	public partial class CallsignRecordCreate: ICreateViewModel<CallsignRecord>
	{
		[Required]
		[RegularExpression(@"^[A-Z]{2}((0[1-9])|[1-9][0-9]{1,2})$")]
		public string Callsign { get; set; }
		[Required]
		[EmailAddress]
		public VehicleType VehicleType { get; set; }
		public CallsignRecord ToItem()
		{
			var item = new CallsignRecord
			{
				Callsign = Callsign,
				VehicleType = VehicleType
			};

			return item;
		}
	}

}

#pragma warning restore 1591
