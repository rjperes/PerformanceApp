namespace PerformanceApp
{
    public sealed class FastMemberGetter : Getter
    {
        public override object? Get(object obj, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(propertyName);

            var getter = FastMember.ObjectAccessor.Create(obj);
            return getter[propertyName];
        }
    }
}
