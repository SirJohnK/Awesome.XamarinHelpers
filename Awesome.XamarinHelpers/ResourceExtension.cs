using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Awesome.XamarinHelpers
{
    /// <summary>
    /// Extension methods of the ResourceDictionary class.
    /// </summary>
    public static class ResourceExtension
    {
        /// <summary>
        /// Extension method to resolve supplied resource from a resource dictionary.
        /// </summary>
        /// <typeparam name="T">Expected type of requested resource.</typeparam>
        /// <param name="resources">ResourceDictionary from where to resolve resource.</param>
        /// <param name="resource">Resource to resolve.</param>
        /// <param name="defaultValue">Default value of expected type.</param>
        /// <returns>Resolved resource in expected type or supplied default value of expected type.</returns>
        /// <remarks>Recursion is used to resolve resources from within other resources.</remarks>
        static T GetResource<T>(this ResourceDictionary resources, object? resource, T defaultValue)
        {
            //Init
            var comparer = EqualityComparer<T>.Default;

            //Check resource type
            switch (resource)
            {
                case OnPlatform<T> value:
                    //Get platform specific resource
                    var platformValue = value.Platforms?.FirstOrDefault(p => p.Platform.Contains(Device.RuntimePlatform))?.Value;
                    return resources.GetResource<T>(platformValue, comparer.Equals(value.Default, default(T) ?? defaultValue) ? defaultValue : value.Default ?? defaultValue);

                case OnIdiom<T> value:
                    {
                        //Get idiom specific resource
                        return Device.Idiom switch
                        {
                            TargetIdiom.Phone => value.Phone,
                            TargetIdiom.Tablet => value.Tablet,
                            TargetIdiom.Desktop => value.Desktop,
                            TargetIdiom.TV => value.TV,
                            TargetIdiom.Watch => value.Watch,
                            _ => comparer.Equals(value.Default, default(T) ?? defaultValue) ? defaultValue : value.Default ?? defaultValue,
                        };
                    }

                case DynamicResource value:
                    //Get dynamic resource
                    return resources.FindResource<T>(value.Key, defaultValue);

                case string value:
                    {
                        //Attempt to cast to resource type or return default
                        try
                        {
                            return (T)Convert.ChangeType(value, typeof(T));
                        }
                        catch
                        {
                            return defaultValue;
                        }
                    }

                case T value:
                    //Return found resource value
                    return value;

                default:
                    //Return default resource value
                    return defaultValue;
            }
        }

        /// <summary>
        /// Extension method to find and resolve named resource from a resource dictionary.
        /// </summary>
        /// <typeparam name="T">Expected type of requested resource.</typeparam>
        /// <param name="resources">ResourceDictionary where to find resource.</param>
        /// <param name="resourceKey">Named resource to find.</param>
        /// <param name="defaultValue">Default value of expected type.</param>
        /// <returns>Found resource in expected type or supplied default value of expected type.</returns>
        public static T FindResource<T>(this ResourceDictionary resources, string resourceKey, T defaultValue)
        {
            //Verify parameter
            if (string.IsNullOrWhiteSpace(resourceKey))
                return defaultValue;

            //Attempt to find resource
            if (resources.TryGetValue(resourceKey, out var resource))
            {
                //Get resource
                return resources.GetResource<T>(resource, defaultValue);
            }
            else
                //If nothing found, return default
                return defaultValue;
        }

        /// <summary>
        /// Extension method to find and resolve named resource from a resource dictionary.
        /// </summary>
        /// <typeparam name="T">Expected type of requested resource.</typeparam>
        /// <param name="resources">ResourceDictionary where to find resource.</param>
        /// <param name="resourceKey">Named resource to find.</param>
        /// <returns>Found resource in expected type or system default value of expected type.</returns>
        public static T FindResource<T>(this ResourceDictionary resources, string resourceKey) => resources.FindResource<T>(resourceKey, default!);
    }
}