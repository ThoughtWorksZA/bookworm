#BookWorm

BookWorm is an open source CMS specifically created to manage information about books. It powers [puku.co.za](http://puku.co.za).

##Getting Started

###Setting up your database

BookWorm uses [RavenDB](http://ravendb.net/) as its database. To set RavenDB up locally, [download it](http://ravendb.net/download), put it somewhere sensible and run the `Start.cmd` batch file that comes bundled. That's it.

###Supporting HTTP DELETE and PUT keywords in IIS Express

Open `%userprofile%\documents\IISExpress\config\applicationhost.config` in a text editor, and make the changes suggested on this [StackOverflow question](http://stackoverflow.com/a/10907343). In summary:

**Change this**
<code>

	<add name="ExtensionlessUrl-Integrated-4.0" path="*." verb="GET,HEAD,POST,DEBUG" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />
</code>

**to this:**
<code>

	<add name="ExtensionlessUrl-Integrated-4.0" path="*." verb="GET,HEAD,POST,DEBUG,PUT,DELETE" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />
</code>

**and make sure these three lines are commented out:**
<code>

	<add name="WebDAVModule" image="%IIS_BIN%\webdav.dll" />
	<add name="WebDAVModule" /> 
	<add name="WebDAV" path="*" verb="PROPFIND,PROPPATCH,MKCOL,PUT,COPY,DELETE,MOVE,LOCK,UNLOCK" modules="WebDAVModule" resourceType="Unspecified" requireAccess="None" />
</code>

We checked app harbor and they're cool with HTTP DELETE, so no worries we hope.

###Getting emails locally
BookWorm sends some emails, notably on user creation. It pulls its configuration to send emails from its web.config file.

To receive emails locally while developing / testing, download and run [Papercut](http://papercut.codeplex.com/) and set it to receive emails on port 587.
