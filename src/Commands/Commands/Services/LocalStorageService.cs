using Commands.Contracts.Services;
using Commands.Core.Contracts.Services;
using Commands.Core.Helpers;
using Newtonsoft.Json;
using Windows.Storage;

namespace Commands.Services;

public class LocalStorageService : ILocalStorageService
{
    // private readonly IFileService fileService;
    private readonly StorageFolder localFolder = ApplicationData.Current.LocalFolder;
    private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

    public LocalStorageService(IFileService fileService)
    {
        // this.fileService = fileService;
    }

    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        if (localSettings.Values.TryGetValue(key, out var obj))
        {
            return await Json.ToObjectAsync<T>((string)obj);
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        localSettings.Values[key] = await Json.StringifyAsync(value);
    }

    public async Task<T?> ReadDataAsync<T>(string name, string? folderName = null)
    {
        // return await fileService.ReadAsync<T>(localFolder.Path, name);

        var folder = folderName == null ? localFolder : await localFolder.GetFolderAsync(folderName);
        var dataFile = await folder.GetFileAsync(name);
        if (dataFile != null)
        {
            var fileContent = await FileIO.ReadTextAsync(dataFile);
            return JsonConvert.DeserializeObject<T>(fileContent);
        }

        return default;
    }

    public async Task SaveDataAsync<T>(string name, T data, string? folderName = null)
    {
        var folder = folderName == null ? localFolder : await localFolder.GetFolderAsync(folderName);
        var dataFile = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
        var fileContent = JsonConvert.SerializeObject(data);
        await FileIO.WriteTextAsync(dataFile, fileContent);

        // await fileService.SaveAsync(localFolder.Path, name, data);
    }

    public async IAsyncEnumerable<string> GetSavedDataNames(string? folderName = null)
    {
        var folder = folderName == null ? localFolder : await localFolder.GetFolderAsync(folderName);
        var items = await folder.GetFilesAsync();
        foreach (var item in items)
        {
            yield return item.Name;
        }
    }
}
