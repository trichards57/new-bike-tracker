using BikeTracker.Core.Models.LocationModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebsiteHelpers.Interfaces;

#pragma warning disable 1591

namespace BikeTracker.Core.Models.LocationViewModels
{
	[GeneratedCode("Model Generator", "v0.3.26"), ExcludeFromCodeCoverage]
	public partial class CallsignRecordDetails
	{
		public CallsignRecordDetails(CallsignRecord item)
		{
			Callsign = item.Callsign;
			CreatedBy = item.CreatedBy;
			CreatedById = item.CreatedById;
			CreatedDate = item.CreatedDate;
			Id = item.Id;
			LocationRecords = item.LocationRecords.Select(i=> new LocationRecordDetails(i));
			UpdatedBy = item.UpdatedBy;
			UpdatedById = item.UpdatedById;
			UpdatedDate = item.UpdatedDate;
			VehicleType = item.VehicleType;
		}
		public string Callsign { get; set; }
		public ApplicationUser CreatedBy { get; set; }
		public string CreatedById { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public int Id { get; set; }
		public IEnumerable<LocationRecordDetails> LocationRecords { get; set; }
		public ApplicationUser UpdatedBy { get; set; }
		public string UpdatedById { get; set; }
		public DateTimeOffset UpdatedDate { get; set; }
		public VehicleType VehicleType { get; set; }

		public override bool Equals(object other)
		{
			return Equals(other as CallsignRecordDetails);
		}

		public bool Equals(CallsignRecordDetails other)
		{
			if (other == null)
				return false;

			var res = true;

			res &= Callsign.Equals(other.Callsign);
			res &= CreatedBy.Equals(other.CreatedBy);
			res &= CreatedById.Equals(other.CreatedById);
			res &= (CreatedDate - other.CreatedDate).TotalSeconds < 30;
			res &= Id.Equals(other.Id);
			res &= UpdatedBy.Equals(other.UpdatedBy);
			res &= UpdatedById.Equals(other.UpdatedById);
			res &= (UpdatedDate - other.UpdatedDate).TotalSeconds < 30;
			res &= VehicleType.Equals(other.VehicleType);

			return res;
		}

		public override int GetHashCode()
		{
			int hash = 17;

			hash = hash * 31 + Callsign.GetHashCode();
			hash = hash * 31 + CreatedBy.GetHashCode();
			hash = hash * 31 + CreatedById.GetHashCode();
			hash = hash * 31 + CreatedDate.GetHashCode();
			hash = hash * 31 + Id.GetHashCode();
			hash = hash * 31 + LocationRecords.GetHashCode();
			hash = hash * 31 + UpdatedBy.GetHashCode();
			hash = hash * 31 + UpdatedById.GetHashCode();
			hash = hash * 31 + UpdatedDate.GetHashCode();
			hash = hash * 31 + VehicleType.GetHashCode();

			return hash;
		}
	}

	[GeneratedCode("Model Generator", "v0.3.26"), ExcludeFromCodeCoverage]
	public partial class LocationRecordDetails
	{
		public LocationRecordDetails(LocationRecord item)
		{
			Callsign = item.Callsign;
			CallsignId = item.CallsignId;
			Id = item.Id;
			Latitude = item.Latitude;
			Longitude = item.Longitude;
			ReadingTime = item.ReadingTime;
		}
		public CallsignRecord Callsign { get; set; }
		public int CallsignId { get; set; }
		public int Id { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public DateTimeOffset ReadingTime { get; set; }

		public override bool Equals(object other)
		{
			return Equals(other as LocationRecordDetails);
		}

		public bool Equals(LocationRecordDetails other)
		{
			if (other == null)
				return false;

			var res = true;

			res &= Callsign.Equals(other.Callsign);
			res &= CallsignId.Equals(other.CallsignId);
			res &= Id.Equals(other.Id);
			res &= Latitude.Equals(other.Latitude);
			res &= Longitude.Equals(other.Longitude);
			res &= (ReadingTime - other.ReadingTime).TotalSeconds < 30;

			return res;
		}

		public override int GetHashCode()
		{
			int hash = 17;

			hash = hash * 31 + Callsign.GetHashCode();
			hash = hash * 31 + CallsignId.GetHashCode();
			hash = hash * 31 + Id.GetHashCode();
			hash = hash * 31 + Latitude.GetHashCode();
			hash = hash * 31 + Longitude.GetHashCode();
			hash = hash * 31 + ReadingTime.GetHashCode();

			return hash;
		}
	}

}

#pragma warning restore 1591
