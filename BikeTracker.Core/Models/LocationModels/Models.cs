using BikeTracker.Core.Models.LocationViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WebsiteHelpers.Interfaces;

#pragma warning disable 1591

namespace BikeTracker.Core.Models.LocationModels
{
	[GeneratedCode("Model Generator", "v0.3.26"), ExcludeFromCodeCoverage]
	public partial class CallsignRecord: IIdentifiable, ICloneable, IEquatable<CallsignRecord>, IDetailable<CallsignRecordDetails>, ISummarisable<CallsignRecordSummary>
	{
		[Required]
		[RegularExpression(@"^[A-Z]{2}((0[1-9])|[1-9][0-9]{1,2})$")]
		public string Callsign { get; set; } = string.Empty;
		[Required]
		public ApplicationUser CreatedBy { get; set; }
		[Required]
		public string CreatedById { get; set; } = string.Empty;
		[Required]
		public DateTimeOffset CreatedDate { get; set; }
		public int Id { get; set; }
		public ICollection<LocationRecord> LocationRecords { get; set; } = new HashSet<LocationRecord>();
		[Required]
		public ApplicationUser UpdatedBy { get; set; }
		[Required]
		public string UpdatedById { get; set; } = string.Empty;
		[Required]
		public DateTimeOffset UpdatedDate { get; set; }
		[Required]
		public VehicleType VehicleType { get; set; }
		public object Clone()
		{
			var item = new CallsignRecord
			{
				Callsign = Callsign,
				CreatedBy = CreatedBy,
				CreatedById = CreatedById,
				CreatedDate = CreatedDate,
				Id = Id,
				LocationRecords = LocationRecords,
				UpdatedBy = UpdatedBy,
				UpdatedById = UpdatedById,
				UpdatedDate = UpdatedDate,
				VehicleType = VehicleType
			};

			return item;
		}

		public override bool Equals(object other)
		{
			return Equals(other as CallsignRecord);
		}

		public bool Equals(CallsignRecord other)
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
		public CallsignRecordDetails ToDetail()
		{
			return new CallsignRecordDetails(this);
		}
		public CallsignRecordSummary ToSummary()
		{
			return new CallsignRecordSummary(this);
		}
	}

	[GeneratedCode("Model Generator", "v0.3.26"), ExcludeFromCodeCoverage]
	public partial class LocationRecord: IIdentifiable, ICloneable, IEquatable<LocationRecord>, IDetailable<LocationRecordDetails>
	{
		[Required]
		public CallsignRecord Callsign { get; set; }
		[Required]
		public int CallsignId { get; set; }
		[Required]
		public int Id { get; set; }
		[Required]
		public decimal Latitude { get; set; }
		[Required]
		public decimal Longitude { get; set; }
		[Required]
		public DateTimeOffset ReadingTime { get; set; }
		public object Clone()
		{
			var item = new LocationRecord
			{
				Callsign = Callsign,
				CallsignId = CallsignId,
				Id = Id,
				Latitude = Latitude,
				Longitude = Longitude,
				ReadingTime = ReadingTime
			};

			return item;
		}

		public override bool Equals(object other)
		{
			return Equals(other as LocationRecord);
		}

		public bool Equals(LocationRecord other)
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
		public LocationRecordDetails ToDetail()
		{
			return new LocationRecordDetails(this);
		}
	}

}

#pragma warning restore 1591
