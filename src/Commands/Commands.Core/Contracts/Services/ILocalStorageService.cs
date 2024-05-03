namespace Commands.Contracts.Services;

public interface ILocalStorageService
{
    Task<T> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);

    Task<T> ReadDataAsync<T>(string name, string folderName = null);

    Task SaveDataAsync<T>(string name, T value, string folderName = null);

    IAsyncEnumerable<string> GetSavedDataNames(string folderName = null);
}
