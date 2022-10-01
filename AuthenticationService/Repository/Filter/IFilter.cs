namespace AuthenticationService.Repository.Filter
{
    public interface IFilter<T>
    {
        public IEnumerable<T> Apply(IEnumerable<T> source);
    }
}
