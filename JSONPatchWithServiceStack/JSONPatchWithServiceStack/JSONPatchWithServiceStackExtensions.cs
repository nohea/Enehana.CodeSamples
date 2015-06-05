using JSONPatchWithServiceStack.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace JSONPatchWithServiceStack.Extensions
{
    public static class JSONPatchWithServiceStackExtensions
    {
        public static void populateFromJsonPatch(this object updateObject, JsonPatchRequest jsonPatchRequest)
        {
            // read from request dto properties
            PropertyInfo[] properties = updateObject.GetType().GetProperties();

            foreach (var op in jsonPatchRequest)
            {
                string fieldName = op.path.Replace("/", "").ToLower(); // assume leading /slash only for example
                string valueString = op.value;

                // patch field is in type
                if (properties.ToList().Where(x => x.Name.ToLower() == fieldName).Count() > 0)
                {
                    // case-insensitive match by propertyname
                    var persistentProperty = properties.ToList().Where(x => x.Name.ToLower() == fieldName).First();

                    // update property on persistent object
                    // i'm sure this can be improved, but you get the idea...
                    if (persistentProperty.PropertyType == typeof(string))
                    {
                        persistentProperty.SetValue(updateObject, valueString, null);
                    }
                    else if (persistentProperty.PropertyType == typeof(int) || persistentProperty.PropertyType == typeof(int?))
                    {
                        int valInt = 0;
                        if (valueString == null)
                        {
                            // set to default value
                            persistentProperty.SetValue(updateObject, null, null);
                        }
                        else if (Int32.TryParse(valueString, out valInt))
                        {
                            persistentProperty.SetValue(updateObject, valInt, null);
                        }
                    }
                    else if (persistentProperty.PropertyType == typeof(long) || persistentProperty.PropertyType == typeof(long?))
                    {
                        long val = 0;
                        if (valueString == null)
                        {
                            // set to default value
                            persistentProperty.SetValue(updateObject, null, null);
                        }
                        else if (long.TryParse(valueString, out val))
                        {
                            persistentProperty.SetValue(updateObject, val, null);
                        }
                    }
                    else if (persistentProperty.PropertyType == typeof(DateTime) || persistentProperty.PropertyType == typeof(DateTime?))
                    {
                        DateTime valDt = default(DateTime);
                        if (string.IsNullOrEmpty(valueString))
                        {
                            // set to default value
                            persistentProperty.SetValue(updateObject, null, null);
                        }
                        else if (DateTime.TryParse(valueString, out valDt))
                        {
                            persistentProperty.SetValue(updateObject, valDt, null);
                        }
                    }
                    else if (persistentProperty.PropertyType == typeof(float) || persistentProperty.PropertyType == typeof(float?))
                    {
                        float val = default(float);
                        if (string.IsNullOrEmpty(valueString))
                        {
                            // set to default value
                            persistentProperty.SetValue(updateObject, null, null);
                        }
                        else if (float.TryParse(valueString, out val))
                        {
                            persistentProperty.SetValue(updateObject, val, null);
                        }
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("type {0} conversion not supported yet", persistentProperty.PropertyType.ToString()));
                    }

                }
            }
        }

    }
}