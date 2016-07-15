Get Locations API
=================

Description
-----------

Gets a list of all the locations registered with the tracking system.

Endpoint
--------

::

  GET: /Map/GetLocations

Arguments
---------
None

Response
--------

Responds with a JSON object containing the last reported location of every
callsign ever registered with the system::

	[
		{
			"Callsign": "WR934",
			"Id": 839,
			"Latitude": 51.016038,
			"Longitude": -3.118242,
			"ReadingTime": "/Date(1459671196995)/",
			"Type": 1
		}
		{
			"Callsign": "WR113",
			"Id": 255,
			"Latitude": 51.016962,
			"Longitude": -3.091042,
			"ReadingTime": "/Date(1445100821000)/",
			"Type": 3
		}
	]

Types are defined as follows:

0. Unknown
1. Bike
2. Foot Patrol
3. Ambulance
4. Other