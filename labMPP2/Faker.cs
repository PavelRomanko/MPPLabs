using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace labMPP2
{
    public class Faker
    {
        private Dictionary<Type, Func<object>> generators = new Dictionary<Type, Func<object>>();

        public T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        private object Create(Type type)
        {
            if (type.IsValueType || type == typeof(string))
            {
                return CreateValue(type);
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var array = Array.CreateInstance(elementType, 1);
                var element = Create(elementType);
                array.SetValue(element, 0);
                return array;
            }

            if (typeof(IEnumerable<object>).IsAssignableFrom(type))
            {
                var elementType = type.GetGenericArguments().FirstOrDefault() ?? typeof(object);
                var collectionType = typeof(List<>).MakeGenericType(elementType);
                var collection = (ICollection<object>)Activator.CreateInstance(collectionType);
                var element = Create(elementType);
                collection.Add(element);
                return collection;
            }

            if (generators.ContainsKey(type))
            {
                return generators[type].Invoke();
            }

            var instance = Activator.CreateInstance(type);

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var fieldValue = Create(field.FieldType);
                field.SetValue(instance, fieldValue);
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && p.GetSetMethod() != null);

            foreach (var property in properties)
            {
                var propertyValue = Create(property.PropertyType);
                property.SetValue(instance, propertyValue);
            }

            return instance;
        }

        public object CreateValue(Type type)
        {
            if (type == typeof(int))
            {
                return GenerateIntValue();
            }
            else if (type == typeof(long))
            {
                return GenerateLongValue();
            }
            else if (type == typeof(double))
            {
                return GenerateDoubleValue();
            }
            else if (type == typeof(float))
            {
                return GenerateFloatValue();
            }
            else if (type == typeof(string))
            {
                return GenerateStringValue();
            }
            else if (type == typeof(DateTime))
            {
                return GenerateDateTimeValue();
            }
            else if (type == typeof(Uri))
            {
                return GenerateUriValue();
            }

            throw new NotSupportedException($"Type {type} is not supported.");
        }

        private int GenerateIntValue()
        {
            return new Random().Next();
        }

        private long GenerateLongValue()
        {
            return new Random().Next();
        }

        private double GenerateDoubleValue()
        {
            return new Random().NextDouble();
        }

        private float GenerateFloatValue()
        {
            return (float)new Random().NextDouble();
        }

        private string GenerateStringValue()
        {
            return Guid.NewGuid().ToString();
        }

        private DateTime GenerateDateTimeValue()
        {
            var startDateTime = new DateTime(1970, 1, 1);
            var endDateTime = DateTime.Now;

            var timeSpan = endDateTime - startDateTime;
            var randomTicks = (long)(new Random().NextDouble() * timeSpan.Ticks);

            return startDateTime.AddTicks(randomTicks);
        }

        private Uri GenerateUriValue()
        {
            var uriScheme = GenerateUriScheme();
            var uriHost = GenerateUriHost();
            var uriPath = GenerateUriPath();
            var uriQuery = GenerateUriQuery();
            var uriFragment = GenerateUriFragment();

            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = uriScheme;
            uriBuilder.Host = uriHost;
            uriBuilder.Path = uriPath;
            uriBuilder.Query = uriQuery;
            uriBuilder.Fragment = uriFragment;

            return uriBuilder.Uri;
        }

        private string GenerateUriScheme()
        {
            var schemes = new[] { "http", "https", "ftp", "file" };
            return PickRandom(schemes);
        }

        private string GenerateUriHost()
        {
            var hostNames = new[] { "example.com", "google.com", "stackoverflow.com", "github.com" };
            return PickRandom(hostNames);
        }

        private string GenerateUriPath()
        {
            var pathSegments = new[] { "path", "to", "resource" };
            var randomPathSegments = Enumerable.Range(1, 3)
                .Select(_ => PickRandom(pathSegments))
                .ToArray();
            return string.Join("/", randomPathSegments);
        }

        private string GenerateUriQuery()
        {
            var queryParameters = new Dictionary<string, string>()
            {
                { "param1", GenerateStringValue() },
                { "param2", GenerateStringValue() },
                { "param3", GenerateStringValue() }
             };

            var queryBuilder = new StringBuilder();
            foreach (var parameter in queryParameters)
            {
                queryBuilder.Append($"{Uri.EscapeDataString(parameter.Key)}={Uri.EscapeDataString(parameter.Value)}&");
            }

            return queryBuilder.ToString().TrimEnd('&');
        }

        private string GenerateUriFragment()
        {
            var fragmentValues = new[] { "section1", "section2", "section3" };
            return PickRandom(fragmentValues);
        }

        public void RegisterGenerator<T>(Func<object> generator)
        {
            generators[typeof(T)] = generator;
        }

        public T PickRandom<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("The array is null or empty.", nameof(array));
            }

            int index = new Random().Next(array.Length - 1);
            return array[index];
        }

        public List<T> GenerateList<T>(int limit)
        {
            var list = new List<T>();
            var elementType = typeof(T);
            var count = new Random().Next(limit + 1);

            for (int i = 0; i < count; i++)
            {
                var element = (T)Create(elementType);
                list.Add(element);
            }

            return list;
        }
    }
}
