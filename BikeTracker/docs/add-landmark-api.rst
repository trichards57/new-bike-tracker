Add Landmark API
================

Description
-----------

Registers a landmark with the tracking service.  A landmark will remain plotted
on the map for a week.

Endpoint
--------

::

  GET: /Map/AddLandmark

Arguments
---------

name
  The name of the landmark
lat
  The latitude of the landmark
lon
  The longitude of the landmark

Response
--------

HTTP Status 400 - Bad Request
  Reported if the name argument is not provided

HTTP Status 201 - Created
  Reported if the landmark has been added to the system