using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SaferString")]
[assembly: AssemblyDescription("Provides slightly safer String and SecureString operations")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("SaferString")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e278a905-2f45-4d94-b4d3-a91b4101a2c5")]

// Level 2 Security
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: AllowPartiallyTrustedCallers]

// ReSharper disable once CheckNamespace
namespace SaferString
{
    /// <summary>
    /// A library to provide string operations to reduce attack service on AnyCPU platforms.
    /// </summary>
    internal class NamespaceDoc
    {
    }
}