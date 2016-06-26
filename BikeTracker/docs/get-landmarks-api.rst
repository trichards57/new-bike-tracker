Get Landmarks API
=================

Description
-----------

Gets a list of all the landmarks currently registered with the tracking system.

Endpoint
--------

::

  GET: /Map/GetLandmarks

Arguments
---------

None

Response
--------

Responds with a JSON object containing all of the current landmarks::

	[
		{
			"Expiry": "/Date(1467561500773)/",
			"Id": 18,
			"Latitude": 51.450264,
			"Longitude": -2.586331,
			"Name": "Test Landmark"
		},
		{
			"Expiry": "/Date(1467561588850)/",
			"Id": 19,
			"Latitude": 51.45738,
			"Longitude": -2.591067,
			"Name": "GAlleries"
		}
	]

