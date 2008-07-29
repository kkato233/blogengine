Running BlogEngine.NET 1.4.5 using SQLite:

If you wish to use SQLite to store all your blog data, this is the guide for you.
Included in this folder is a default SQLite database, that you can use to get you 
started with your blog.  In addition, you will find a sample web.config file with
the needed changes to use SQLite and an upgrade script for current SQLite users 
who wish to upgrade from 1.4 to 1.4.5

Instructions for new setup:

1. Download the SQLite ADO Providers Binaries from the ADO.NET 2.0 Provider for 
SQLite project. http://sourceforge.net/projects/sqlite-dotnet2
2. Find the System.Data.SQLite.DLL from the download and copy it to your blog's bin folder.
3. Copy BlogEngine.s3db from the SQLite folder to your App_Data folder.
4. Rename SQLiteWeb.Config to Web.config and copy it to your blog folder.  (This will
overwrite your existing web.config file.  If this is not a new installation, make sure 
you have a backup.)
5. Surf out to your Blog and see the welcome post.
6. Login with the username admin and password admin.  Change the password.

Upgrading from 1.4.0

1. If you don't already have SQLite Admin tool installed, you'll need to get one. SQLite
Admin has worked great for me.  (http://sqliteadmin.orbmu2k.de/)
2. Open your BlogEngine.s3db database and execute the upgrade script against it.  (You will 
likely need to copy your BlogEngine.s3db file from your web server, perform the update, and 
copy it back out depending on your setup.
3. The web.config file has changed from 1.4.0 to 1.4.5.  It will likely be easiest to start
with the sample web.config file as described above, but if you have other changes in it, 
you'll need to merge them.

Additional information can be found at http://dotnetblogengine.net