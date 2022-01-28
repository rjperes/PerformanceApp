using System.Reflection;

namespace PerformanceApp
{
    public sealed class ReflectionSetter : Setter
    {
        public override void Set(object obj, string propertyName, object value)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(propertyName);

            var property = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);

            if (property != null)
            {
                property.SetValue(obj, value, null);
            }
            else
            {
                throw new InvalidOperationException("Property not found.");
            }
        }
    }
}
