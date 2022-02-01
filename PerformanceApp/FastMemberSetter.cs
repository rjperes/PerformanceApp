namespace PerformanceApp
{
    public sealed class FastMemberSetter : Setter
    {
        public override void Set(object obj, string propertyName, object value)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(propertyName);

            var setter = FastMember.ObjectAccessor.Create(obj);
            setter[propertyName] = value;
        }
    }
}
