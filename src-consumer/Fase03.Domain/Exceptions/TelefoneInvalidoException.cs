namespace Fase03.Domain.Exceptions;

public class TelefoneInvalidoException : Exception
{
    public TelefoneInvalidoException(string message) : base(message)
    {
    }
}