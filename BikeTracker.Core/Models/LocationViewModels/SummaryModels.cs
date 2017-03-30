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
	public partial class CallsignRecordSummary
	{
		public CallsignRecordSummary(CallsignRecord item)
		{
			Callsign = item.Callsign;
			Id = item.Id;
			VehicleType = item.VehicleType;
		}
		public string Callsign { get; set; }
		public int Id { get; set; }
		public VehicleType VehicleType { get; set; }

		public override bool Equals(object other)
		{
			return Equals(other as CallsignRecordSummary);
		}

		public bool Equals(CallsignRecordSummary other)
		{
			if (other == null)
				return false;

			var res = true;

			res &= Callsign.Equals(other.Callsign);
			res &= Id.Equals(other.Id);
			res &= VehicleType.Equals(other.VehicleType);

			return res;
		}

		public override int GetHashCode()
		{
			int hash = 17;

			hash = hash * 31 + Callsign.GetHashCode();
			hash = hash * 31 + Id.GetHashCode();
			hash = hash * 31 + VehicleType.GetHashCode();

			return hash;
		}
	}

}

#pragma warning restore 1591
