using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.Maui.Controls.Xaml
{
	[Flags]
	public enum XamlCompilationOptions
	{
		Skip = 1 << 0,
		Compile = 1 << 1
	}

	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class, Inherited = false)]
	public sealed class XamlCompilationAttribute : Attribute
	{
		public XamlCompilationAttribute(XamlCompilationOptions xamlCompilationOptions)
		{
			XamlCompilationOptions = xamlCompilationOptions;
		}

		public XamlCompilationOptions XamlCompilationOptions { get; set; }
	}

	static class XamlCExtensions
	{
		public static bool IsCompiled(this Type type)
		{
			var attr = type.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)

/* Unmerged change from project 'Controls.Xaml(net8.0)'
Before:
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			attr = type.Module.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)
After:
			{
*/

/* Unmerged change from project 'Controls.Xaml(net8.0-ios)'
Before:
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			attr = type.Module.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)
After:
			{
*/

/* Unmerged change from project 'Controls.Xaml(net8.0-maccatalyst)'
Before:
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			attr = type.Module.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)
After:
			{
*/

/* Unmerged change from project 'Controls.Xaml(net8.0-android)'
Before:
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			attr = type.Module.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)
After:
			{
*/

/* Unmerged change from project 'Controls.Xaml(net8.0-windows10.0.19041)'
Before:
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			attr = type.Module.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)
After:
			{
*/

/* Unmerged change from project 'Controls.Xaml(net8.0-windows10.0.20348)'
Before:
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			attr = type.Module.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)
After:
			{
*/
			{
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			}

			}

			attr = type.Module.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)
			{
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			}

			attr = type.Assembly.GetCustomAttribute<XamlCompilationAttribute>();
			if (attr != null)
			{
				return attr.XamlCompilationOptions == XamlCompilationOptions.Compile;
			}

			return false;
		}
	}
}