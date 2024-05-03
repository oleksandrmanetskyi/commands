namespace Commands.Core.Contracts.Services;

public interface IFileService
{
    Task<T> ReadAsync<T>(string folderPath, string fileName);

    Task SaveAsync<T>(string folderPath, string fileName, T content);

    void Delete(string folderPath, string fileName);
}
