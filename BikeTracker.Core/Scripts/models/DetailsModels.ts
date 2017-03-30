// Model Generator v0.3.26
interface ICallsignRecordDetails {
    callsign: string;
    createdBy: any;
    createdById: string;
    createdDate: string;
    id: number;
    locationRecords: ILocationRecordDetails[];
    updatedBy: any;
    updatedById: string;
    updatedDate: string;
    vehicleType: VehicleType;
}

// Model Generator v0.3.26
interface ILocationRecordDetails {
    callsign: ICallsignRecordDetails;
    callsignId: number;
    id: number;
    latitude: number;
    longitude: number;
    readingTime: string;
}
