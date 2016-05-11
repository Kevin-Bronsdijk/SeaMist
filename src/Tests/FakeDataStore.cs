
namespace Tests
{
    public class FakeDataStore : SeaMist.Model.IDataStore
    {
        public string DataStoreName => "fakeDataStore";

        public void AddMetadata(string key, string value)
        {
        }

        public void AddHeaders(string key, string value)
        {
        }
    }
}
