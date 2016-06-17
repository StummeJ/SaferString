using System.Reflection;

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.1.0.0")]
// This should be the same version as below
[assembly: AssemblyFileVersion("1.1.0.0")]

#if DEBUG
[assembly: AssemblyInformationalVersion("1.1.0-PreRelease")]
#else
[assembly: AssemblyInformationalVersion("1.1.0")]
#endif
