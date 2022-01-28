using System.Linq.Expressions;
using System.Reflection;

namespace PerformanceApp
{
    public sealed class CompiledGetter : Getter
    {
        private readonly Dictionary<Type, Dictionary<string, Delegate>> _properties = new Dictionary<Type, Dictionary<string, Delegate>>();

        public void Initialize(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            this._properties[type] = new Dictionary<string, Delegate>();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                this.GenerateGetterFor(type, prop);
            }
        }

        public override object? Get(object obj, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(propertyName);

            var action = this.GetActionFor(obj.GetType(), propertyName);

            if (action is Func<object, object> func)
            {   
                return func(obj);
            }
            else
            {
                throw new InvalidOperationException("Property not found.");
            }
        }

        private void GenerateGetterFor(Type type, PropertyInfo property)
        {
            var propertyName = property.Name;
            var parmExpression = Expression.Parameter(typeof(object), "it");
            var castExpression = Expression.Convert(parmExpression, type);
            var propertyExpression = Expression.Convert(Expression.Property(castExpression, propertyName), typeof(object));
            var lambdaExpression = Expression.Lambda(typeof(Func<,>).MakeGenericType(typeof(object), typeof(object)), propertyExpression, parmExpression);
            var func = lambdaExpression.Compile();

            this._properties[type][propertyName] = func;
        }

        private Delegate? GetActionFor(Type type, string propertyName)
        {
            if (this._properties.TryGetValue(type, out var properties))
            {
                if (properties.TryGetValue(propertyName, out var func))
                {
                    return func;
                }
            }

            return null;
        }
    }
}