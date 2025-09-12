namespace Darak.Application.Common.Interfaces;

public interface ITimeZoneConverter
{
    T ConvertUtcToLocal<T>(T dto);
}
