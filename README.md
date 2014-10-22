SoftwarePassion.Common
----------------------

Core
====
Various low-level utilities. Some cool StringExtensions.

Worth checking out is the TimeProvider (inspired by Mark Seeman) and the 
PluginFinder - a poor-man's Plugin system.

Also very neat is the PropName in ObjectExtensions.

### ErrorHandling

A collection of Exception classes inspired by [my error handling blog](http://softwarepassion.eu/error-handling-the-easy-way/).

### Data

SqlDataAccessHandler. A handler for calling Sql Server with retry logic and mapping Sql Exceptions into
the standard ErrorHandling exceptions.

Core.Wcf
========
The SafeServiceHost is very useful in self-hosting scenarios.

