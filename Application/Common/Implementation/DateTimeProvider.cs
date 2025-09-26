using Application.Common.Interfaces;

namespace Application.Common.Implementation
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
