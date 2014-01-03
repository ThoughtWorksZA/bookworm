#BookWorm

BookWorm is an open source CMS specifically created to manage information about books. It powers [puku.co.za](http://puku.co.za).

It helps you:

* Create and manage a catalogue of books
* Offer a book search and the ability to browse books by language, intended audience and genre
* Post reviews and other information related to each book
* Post articles

##Getting Started

###Setting up your database

BookWorm uses [RavenDB](http://ravendb.net/) as its database. To set RavenDB up locally, [download it](http://ravendb.net/download), put it somewhere sensible and run the `Start.cmd` batch file that comes bundled. That's it. Be sure to check RavenDB's license terms.

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

##Deployment

BookWorm has been successfully deployed to [AppHarbor](https://appharbor.com/).

###Setting up
To deploy BookWorm to AppHarbor:

* Create and new application on AppHarbor
* Add the application's git repo as a remote to your fork of BookWorm
* Push the code to AppHarbor: `git push my-app master`
* AppHarbor will compile the code, run the tests and deploy the application

###Configuration variables
For BookWorm to be able to send emails, it requires certain configuration variables to be set. These are used to replace the development values in the `appsettings` section of the `web.config`:
<code>

    <add key="emailSenderAddress" value="dev-value"/>
    <add key="emailSenderPassword" value="dev-value"/>
    <add key="emailServerAddress" value="localhost"/>
    <add key="emailEnableSsl" value="false"/>
</code>

To set these on AppHarbor, navigate to your application on appharbor.com, click on `Configuration variables` and then on the `New configuration variable` link. You can also use the [AppHarbor command line tool](http://blog.appharbor.com/2012/4/25/introducing-the-appharbor-command-line-utility).

Here are some example values:

* emailSenderAddress = "sender@example.com"
* emailSenderPassword = "secretpassword"
* emailServerAddress = "smtp.gmail.com"
* emailEnableSsl = "true"

###RavenDB

You'll need to set up a database for your application to run against on AppHarbor. This can be done using an add on. BookWorm has been tested using the [CloudBird add-on](https://appharbor.com/applications/puku-staging/addons/cloudbird). CloudBird will automatically replace the `RavenDB` connection string in the `web.config`.
 