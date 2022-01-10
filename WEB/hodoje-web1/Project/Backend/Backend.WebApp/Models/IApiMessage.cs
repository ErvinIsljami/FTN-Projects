namespace Backend.Models
{
    public interface IApiMessage<V, T> where T : class
    {
        V Key { get; set; }
        T Data { get; set; }
    }
}