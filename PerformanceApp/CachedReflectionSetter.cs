using System.Reflection;

namespace PerformanceApp
{
    public sealed class CachedReflectionSetter : Setter
    {
        private readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _properties = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        public void Initialize(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            this._properties[type] = new Dictionary<string, PropertyInfo>();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                this._properties[type][prop.Name] = prop;
            }
        }

        public override void Set(object obj, string propertyName, object value)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(propertyName);

            var property = this.GetPropertyFor(obj.GetType(), propertyName);

            if (property != null)
            {
                property.SetValue(obj, value, null);
            }
            else
            {
                throw new InvalidOperationException("Property not found.");
            }
        }

        private PropertyInfo? GetPropertyFor(Type type, string propertyName)
        {
            if (this._properties.TryGetValue(type, out var properties))
            {
                if (properties.TryGetValue(propertyName, out var prop))
                {
                    return prop;
                }
            }

            return null;
        }
    }
}
