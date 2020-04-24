using System;
using System.Diagnostics;
using System.Reflection;
namespace br.corp.bonus630.QrCodeDocker
{
    public static class VGCoreReferenceChecker
    {
        public static void Init(System.Type type)
        {
            // This variable holds the amount of indenting that 
            // should be used when displaying each line of information.
            Int32 indent = 0;
            // Display information about the EXE assembly.
            Assembly a = type.Assembly;
            Display(indent, "Assembly identity={0}", a.FullName);
            Display(indent + 1, "Codebase={0}", a.CodeBase);

            // Display the set of assemblies our assemblies reference.

            Display(indent, "Referenced assemblies:");
            foreach (AssemblyName an in a.GetReferencedAssemblies())
            {
                Display(indent + 1, "Name={0}, Version={1}, Culture={2}, PublicKey token={3}", an.Name, an.Version, an.CultureInfo.Name, (BitConverter.ToString(an.GetPublicKeyToken())));
                Display(indent + 2, "FullName={0}", an.FullName);
                
            }
            Display(indent, "");
            return;
            // Display information about each assembly loading into this AppDomain.
            foreach (Assembly b in AppDomain.CurrentDomain.GetAssemblies())
            {
                Display(indent, "Assembly: {0}", b);

                // Display information about each module of this assembly.
                foreach (Module m in b.GetModules(true))
                {
                    Display(indent + 1, "Module: {0}", m.Name);
                }

                // Display information about each type exported from this assembly.

                indent += 1;
                foreach (Type t in b.GetExportedTypes())
                {
                    Display(0, "");
                    Display(indent, "Type: {0}", t);

                    // For each type, show its members & their custom attributes.

                    indent += 1;
                    foreach (MemberInfo mi in t.GetMembers())
                    {
                        Display(indent, "Member: {0}", mi.Name);
                        DisplayAttributes(indent, mi);

                        // If the member is a method, display information about its parameters.

                        if (mi.MemberType == MemberTypes.Method)
                        {
                            foreach (ParameterInfo pi in ((MethodInfo)mi).GetParameters())
                            {
                                Display(indent + 1, "Parameter: Type={0}, Name={1}", pi.ParameterType, pi.Name);
                            }
                        }

                        // If the member is a property, display information about the property's accessor methods.
                        if (mi.MemberType == MemberTypes.Property)
                        {
                            foreach (MethodInfo am in ((PropertyInfo)mi).GetAccessors())
                            {
                                Display(indent + 1, "Accessor method: {0}", am);
                            }
                        }
                    }
                    indent -= 1;
                }
                indent -= 1;
            }
        }

        // Displays the custom attributes applied to the specified member.
        public static void DisplayAttributes(Int32 indent, MemberInfo mi)
        {
            // Get the set of custom attributes; if none exist, just return.
            object[] attrs = mi.GetCustomAttributes(false);
            if (attrs.Length == 0) { return; }

            // Display the custom attributes applied to this member.
            Display(indent + 1, "Attributes:");
            foreach (object o in attrs)
            {
                Display(indent + 2, "{0}", o.ToString());
            }
        }

        // Display a formatted string indented by the specified amount.
        public static void Display(Int32 indent, string format, params object[] param)

        {
           Debug.Write(new string(' ', indent * 2));
           Debug.WriteLine(format, param);
        }
    }
}

