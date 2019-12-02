using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Awesome.XamarinHelpers
{
    public static class ResourceHelpers
    {
        private static T GetResource<T>(object? resource, T defaultValue)
        {
            //Verify Parameter
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            //Check resource type
            switch (resource)
            {
                case OnPlatform<T> value:
                    //Get platform specific resource
                    return GetResource<T>(value.Platforms?.FirstOrDefault(p => p.Platform.Contains(Device.RuntimePlatform))?.Value, defaultValue);

                case OnIdiom<T> value:
                    {
                        //Get idiom specific resource
                        switch (Device.Idiom)
                        {
                            case TargetIdiom.Phone:
                                return value.Phone;

                            case TargetIdiom.Tablet:
                                return value.Tablet;

                            case TargetIdiom.Desktop:
                                return value.Desktop;

                            case TargetIdiom.TV:
                                return value.TV;

                            case TargetIdiom.Watch:
                                return value.Watch;

                            default:
                                return defaultValue;
                        }
                    }

                case DynamicResource value:
                    //Get dynamic resource
                    return FindResource<T>(value.Key, defaultValue);

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

        public static T FindResource<T>(string resourceKey, T defaultValue)
        {
            //Verify parameter
            if (string.IsNullOrWhiteSpace(resourceKey))
                return defaultValue;

            //Attempt to find resource
            if (Application.Current.Resources.TryGetValue(resourceKey, out var resource))
                //Get resource
                return GetResource<T>(resource, defaultValue);
            else
                //If nothing found, return default
                return defaultValue;
        }
    }
}