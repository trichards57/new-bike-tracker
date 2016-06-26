Check-In API
============

Description
-----------

Registers a current location with the tracking service.  Two versions
are supported:

Version 1
    Intended for the trackable Nokia Phones (uses a specific date and time format)

Version 2
	Intended for more modern tracking tools that use a standard date and time format
	that the .Net Framework can recognise without pre-processing.

Endpoint
--------
GET: /Map/CheckIn

Arguments
---------

v *(optional)*
	The version of the endpoint to use.  1 or 2, defaulting to 1
	if not provided.

imei
	The unique identifier of the device checking in.  Normally
	it is the IMEI number of the mobile phone, but any string can be
	registered with the system.

lat
	The current latitude of the device, expressed as a decimal
	fraction.

lon
	The current longitude of the device, expressed as a decimal
	fraction.

time
	The time that that location reading was taken.

	If using version 1, it must be in the format::

	  HHmmss.fff

	If using version 2 it can be in any format recognizable by the
	.Net Framework and must contain both date and time.

date *(version 1 only)*
	The date that the location reading was taken.
	
	If using version 1, it must be in the format::

	  ddMMyy

	This parameter is not used in version 2.

Response
--------

No Location Given
	Reported if either the lat or lon fields are missing or not
	decimal numbers.

No Date or Time Given
    Reported if time is null or if date is null when using version 1

Bad Date or Time Given
	Reported if the date and time cannot be parsed.

Bad Version Given
	Reported if the given version is not 1 or 2

No IMEI Given
	Reported if the imei parameter is missing or empty

Location Received from [Callsign]
	Reported if the location has been registered