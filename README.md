Streamus™
=========

A Google Chrome extension which allows users to add YouTube videos to playlists, share playlists and discover new YouTube videos.

Overview
========

Streamus consists of a front-end client, the Google Chrome extension, a back-end server and a website. This repository contains the files for the server. Please see the other repositories, StreamusChromeExtension and StreamusWebsite, to gain a full understanding of the product.

The server's modules are managed by NuGet Package Manager.

The server is used to record information about a given user's listening experience. All videos, playlist items, playlists and folders are written to the database.
The server is used to enable sharing of playlists between users by copying a playlist row and providing a link to the new row to other users.


Server
------

* [C# ASP.NET MVC (v4.0)](http://www.asp.net/mvc/mvc4)
* [NUnit (v2.0+)](http://www.nunit.org/)
* [NHibernate (v3.3.3+)](http://nhforge.org/)
* [AutoFac (v3.1.1+)](https://code.google.com/p/autofac/)
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [log4net](http://logging.apache.org/log4net/)


Installation
========

1. Build 'Streamus Server' in Visual Studio 2012.
2. Build 'Streamus Server Tests' in Visual Studio 2012.
3. Create a new, local database called 'Streamus'
4. Run the test case 'ResetDatabase' to populate the database with tables and schema information.
5. Ensure all other test cases pass.
6. Run Streamus Server.

Deployment
========

The server is hosted on http://www.appharbor.com. AppHarbor has a hook into the solution which will detect commits to the master branch. AppHarbor will build the solution, run test cases and, if successful, hot swap the server out with a new version. All connection string information is automatically handled by AppHarbor.

License
=======
This work is licensed under the GNU General Public License v2 (GPL-2.0)

Authors
=======

* MeoMix - Original developer, main contributor.