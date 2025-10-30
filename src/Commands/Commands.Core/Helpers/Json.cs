using Newtonsoft.Json;

namespace Commands.Core.Helpers;

public static class Json
{
    public static Task<T> ToObjectAsync<T>(string value)
    {
        // Synchronous JSON deserialization is fast enough; no need to offload to thread pool
        return Task.FromResult(JsonConvert.DeserializeObject<T>(value));
    }

    public static Task<string> StringifyAsync(object value)
    {
        // Synchronous JSON serialization is fast enough; no need to offload to thread pool
        return Task.FromResult(JsonConvert.SerializeObject(value));
    }
}
