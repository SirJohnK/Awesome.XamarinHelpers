using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Awesome.XamarinHelpers
{
    public static class ResourcesUtility
    {
        private static T GetResource<T>(object? resource)
        {
            //Verify Parameter
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            //Check resource type
            switch (resource)
            {
                case OnPlatform<T> value:
                    //Get platform specific resource
                    return GetResource<T>(value.Platforms?.FirstOrDefault(p => p.Platform.Contains(Device.RuntimePlatform))?.Value);

                case DynamicResource value:
                    //Get dynamic resource
                    return FindResource<T>(value.Key);

                case string value:
                    {
                        //Attempt to cast to resource type or return default
                        try
                        {
                            return (T)Convert.ChangeType(value, typeof(T));
                        }
                        catch
                        {
                            return default;
                        }
                    }

                case T value:
                    //Return found resource value
                    return value;

                default:
                    //Return default resource value
                    return default;
            }
        }

        public static T FindResource<T>(string resourceKey)
        {
            //Verify parameter
            if (string.IsNullOrWhiteSpace(resourceKey))
                return default;

            //Attempt to find resource
            if (Application.Current.Resources.TryGetValue(resourceKey, out var resource))
                //Get resource
                return GetResource<T>(resource);
            else
                //If nothing found, return default
                return default;
        }
    }
}