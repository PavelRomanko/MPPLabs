using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace labMPP2
{
    class ObjectInfoPrinter
    {
        public static void PrintObjectInfo(object obj)
        {
            Type objectType = obj.GetType();
            Console.WriteLine($"Type: {objectType.Name}");

            PropertyInfo[] properties = objectType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object propertyValue = property.GetValue(obj);
                string propertyTypeName = property.PropertyType.Name;
                string propertyName = property.Name;

                if (IsSimpleType(property.PropertyType))
                {
                    Console.WriteLine($"{propertyName}: {propertyValue}");
                }
                else if (propertyValue is IList list)
                {
                    Console.WriteLine($"{propertyName}:");
                    for (int i = 0; i < list.Count; i++)
                    {
                        object listItem = list[i];
                        Console.WriteLine($"  [{i}]: {listItem}");
                    }
                }
                else if (propertyValue is Array array)
                {
                    Console.WriteLine($"{propertyName}:");
                    for (int i = 0; i < array.Length; i++)
                    {
                        object arrayItem = array.GetValue(i);
                        Console.WriteLine($"  [{i}]: {arrayItem}");
                    }
                }
                else
                {
                    Console.WriteLine($"{propertyName}:");
                    PrintObjectInfo(propertyValue);
                }
            }
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime);
        }
    }
}