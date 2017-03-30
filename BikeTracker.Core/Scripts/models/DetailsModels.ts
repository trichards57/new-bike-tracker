// Model Generator v0.3.26
interface ICallsignRecordDetails {
    callsign: string;
    createdBy: ApplicationUser;
    createdById: string;
    createdDate: string;
    id: number;
    locationRecords: ILocationRecordDetails[];
    updatedBy: ApplicationUser;
    updatedById: string;
    updatedDate: string;
    vehicleType: VehicleType;
}

// Model Generator v0.3.26
interface ILocationRecordDetails {
    callsign: CallsignRecord;
    callsignId: number;
    id: number;
    latitude: decimal;
    longitude: decimal;
    readingTime: string;
}

