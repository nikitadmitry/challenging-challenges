﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Business.Challenges.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Business.Challenges.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///
        ///public class Challenge
        ///{
        ///	public static void Main()
        ///	{
        ///		// your code goes here
        ///	}
        ///}.
        /// </summary>
        internal static string Answer_Template_CSharp {
            get {
                return ResourceManager.GetString("Answer_Template_CSharp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /* package whatever; // don&apos;t place package name! */
        ///
        ///import java.util.*;
        ///import java.lang.*;
        ///import java.io.*;
        ///
        ////* Name of the class has to be &quot;Main&quot; only if the class is public. */
        ///class Challenge
        ///{
        ///	public static void main (String[] args) throws java.lang.Exception
        ///	{
        ///		// your code goes here
        ///	}
        ///}.
        /// </summary>
        internal static string Answer_Template_Java {
            get {
                return ResourceManager.GetString("Answer_Template_Java", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string Answer_Template_Other {
            get {
                return ResourceManager.GetString("Answer_Template_Other", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # your code goes here.
        /// </summary>
        internal static string Answer_Template_Python {
            get {
                return ResourceManager.GetString("Answer_Template_Python", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # your code goes here.
        /// </summary>
        internal static string Answer_Template_Ruby {
            get {
                return ResourceManager.GetString("Answer_Template_Ruby", resourceCulture);
            }
        }
    }
}
