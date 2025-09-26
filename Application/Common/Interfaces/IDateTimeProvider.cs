namespace Application.Common.Interfaces
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset UtcNow { get; }
    }
}
