namespace AuthenticationService.Services.Hashing.HashCalculator;

public interface IHashCalculator<TInput, TOutput>
{
    TOutput Calculate(TInput data);
}
