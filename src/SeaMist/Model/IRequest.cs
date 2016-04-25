namespace SeaMist.Model
{
    public interface IRequest
    {
        Authentication Authentication { get; set; }
        bool Dev { get; set; }
    }
}