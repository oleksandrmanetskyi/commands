using System.Text;

using Commands.Core.Contracts.Services;

using Newtonsoft.Json;

namespace Commands.Core.Services;

public class FileService : IFileService
{
    public async Task<T> ReadAsync<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    public async Task SaveAsync<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content);
        await File.WriteAllTextAsync(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }
}
