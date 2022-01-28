using System.Linq.Expressions;
using System.Reflection;

namespace PerformanceApp
{
    public sealed class CompiledSetter : Setter
    {
        private readonly Dictionary<Type, Dictionary<string, Delegate>> _properties = new Dictionary<Type, Dictionary<string, Delegate>>();

        public void Initialize(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            this._properties[type] = new Dictionary<string, Delegate>();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                this.GenerateSetterFor(type, prop);
            }
        }

        public override void Set(object obj, string propertyName, object value)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(propertyName);

            var action = this.GetActionFor(obj.GetType(), propertyName);

            if (action is Action<object, object> act)
            {   
                act(obj, value);
            }
            else
            {
                throw new InvalidOperationException("Property not found.");
            }
        }

        private void GenerateSetterFor(Type type, PropertyInfo property)
        {
            var propertyName = property.Name;
            var propertyType = property.PropertyType;
            var parmExpression = Expression.Parameter(typeof(object), "it");
            var castExpression = Expression.Convert(parmExpression, type);
            var propertyExpression = Expression.Property(castExpression, propertyName);
            var valueExpression = Expression.Parameter(typeof(object), propertyName);
            var operationExpression = Expression.Assign(propertyExpression, Expression.Convert(valueExpression, propertyType));
            var lambdaExpression = Expression.Lambda(typeof(Action<,>).MakeGenericType(typeof(object), typeof(object)), operationExpression, parmExpression, valueExpression);
            var action = lambdaExpression.Compile();

            this._properties[type][propertyName] = action;
        }

        private Delegate? GetActionFor(Type type, string propertyName)
        {
            if (this._properties.TryGetValue(type, out var properties))
            {
                if (properties.TryGetValue(propertyName, out var action))
                {
                    return action;
                }
            }

            return null;
        }
    }
}