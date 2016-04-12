namespace SeaMist.Model
{
    public interface IOptimizeRequest
    {
        Authentication Authentication { get; set; }
        bool Dev { get; set; }
    }
}