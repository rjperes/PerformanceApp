using System.Reflection;

namespace PerformanceApp
{
    public sealed class ReflectionGetter : Getter
    {
        public override object? Get(object obj, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(propertyName);

            var property = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);

            if (property != null)
            {
                return property.GetValue(obj, null);
            }
            else
            {
                throw new InvalidOperationException("Property not found.");
            }
        }
    }
}
