namespace SeaMist.Model
{
    public interface IDataStore
    {
        string DataStoreName { get; }

        void AddMetadata(string key, string value);

        void AddHeaders(string key, string value);
    }
}