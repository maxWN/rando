namespace Rando.Helpers;

public interface IApiCallerHelper<T>
{
    Task<IEnumerable<T>> GetExternalDataAsync(string uri, string dataType);
}