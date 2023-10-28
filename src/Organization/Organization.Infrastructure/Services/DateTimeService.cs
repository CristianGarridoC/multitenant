using Organization.Application.Common.Interfaces.Services;

namespace Organization.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}