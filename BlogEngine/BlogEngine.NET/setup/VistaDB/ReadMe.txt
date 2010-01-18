Running BlogEngine.NET 1.6 using VistaDB:

If you wish to use VistaDB (or VistaDB Express) to store all your blog data, this is 
where you want to be.  Included in this folder are all the scripts that 
you can use to get you started with your blog.  In addition, you will find a sample
web.config file with the needed changes to use VistaDB and an upgrade script for 
current VistaDB users who wish to upgrade from 1.4.5 or 1.5.

Since the last version of BlogEngine.NET, VistaDB Express is no longer freely available.
If you own a version of VistaDB or have VistaDB Express available, you are fine and will
have an easy setup. If you are already using VistaDB, but no longer have it installed 
and no longer have the installer, you can buy a VistaDB license or convert your data to 
a different free option using the Provider Migration tool. 
(http://www.nyveldt.com/blog/page/BlogEngineNET-Provider-Migration.aspx)

Instructions for new setup:

1. If you don't already have VistaDB or VistaDB Express installed locally, download 
VistaDB from vistadb.net and install it locally.  
2. Find VistaDB.NET20.dll on your PC and copy it to your blog's Bin folder. 
3. Create a database called BlogEngine, run the install script, and save.
4. Copy the newly created BlogEngine database file to your blog's App_Data folder.
5. Rename VistaDBWeb.Config to Web.config and copy it to your blog folder.  (This will
overwrite your existing web.config file.  If this is not a new installation, make sure 
you have a backup.)
6. Edit your web.config.  Update the connection string and assemblies as needed to 
match your file and version information.
7. Surf out to your Blog and see the welcome post.
8. Login with the username admin and password admin.  Change the password.

Upgrading from 1.4.5

1. If you don't already have VistaDB 3.4 or VistaDB Express 3.4 installed locally, download 
VistaDB Express from vistadb.net and install it locally.  (If you have a version other than
3.4.2.77, you will need to change the web.config to match the version you have.)
2. Open your BlogEngine.vdb3 database and execute the upgrade script against it.  (You will 
likely need to copy your BlogEngine.vdb3 file from your web server, perform the update, and 
copy it back out depending on your setup.
3. The web.config file has changed from 1.4.5 to 1.5.  It will likely be easiest to start
with the sample web.config file as described above, but if you have other changes in it, 
you'll need to merge them.

Additional information can be found at http://dotnetblogengine.net

Notice:

While BlogEngine.NET is open source and VistaDB Express is free to use, there are a few restrictions.  
VistaDB Express is only free to use for non commercial uses.  If you are commercial, you will need to 
purchase a license to use it.  In addition, the VistaDB Express license requires that you place a link 
back to them in your product.  A link back the vistadb.net in your page footer or side bar would show 
your appreciation.