PUKU Readme
===========

To do: write a proper Readme.

Notes on adding support for HTTP DELETE and PUT to your Visual Studio 2012 IIS Express environment:

Hey guys, you need to do the following to make HTTP delete's work on your machines.

Open %userprofile%\documents\IISExpress\config\applicationhost.config in a text editor, and make the changes suggested here:


http://stackoverflow.com/a/10907343

So basically change the line that looks like this:

# FIX ME
<add name="ExtensionlessUrl-Integrated-4.0" path="*." verb="GET,HEAD,POST,DEBUG" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />

to this:

# THIS LINE IS GOOD
<add name="ExtensionlessUrl-Integrated-4.0" path="*." verb="GET,HEAD,POST,DEBUG,PUT,DELETE" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />

and make sure if you have these three lines, that they should be commented out.

# COMMENT ME OUT
<add name="WebDAVModule" image="%IIS_BIN%\webdav.dll" />
<add name="WebDAVModule" /> 
<add name="WebDAV" path="*" verb="PROPFIND,PROPPATCH,MKCOL,PUT,COPY,DELETE,MOVE,LOCK,UNLOCK" modules="WebDAVModule" resourceType="Unspecified" requireAccess="None" />


We checked app harbor and they're cool with HTTP DELETE, so no worries we hope.

