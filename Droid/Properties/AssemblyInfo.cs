﻿using System.Reflection;
using System.Runtime.CompilerServices;
using Android.App;

// Information about this assembly is defined by the following attributes. 
// Change them to the values specific to your project.

[assembly: AssemblyTitle("Matna.Droid")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("${AuthorCopyright}")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

[assembly: AssemblyVersion("1.0.0")]

// The following attributes are used to specify the signing key for the assembly, 
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]

// NOTE: Facebook SDK rquires that the 'Value' point to a string resource
//       in your values/ folder (eg: strings.xml file).
//       It will not allow you to use the app_id value directly here!

// Facebook SDK
[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
// GoogleMaps API
[assembly: MetaData("com.google.android.geo.API_KEY", Value = Matna.Helpers.Keys.GoogleMapsApiKeyAndroid)]
// HockeyApp
[assembly: MetaData("net.hockeyapp.android.appIdentifier", Value = "02eaba22011f4839a0e7e34d84db1647")]

[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]