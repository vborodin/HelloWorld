namespace AuthenticationService.Repository.Filter
{
    public interface IFilter<T>
    {
        public IEnumerable<T> Filter(IEnumerable<T> source);
    }
}
