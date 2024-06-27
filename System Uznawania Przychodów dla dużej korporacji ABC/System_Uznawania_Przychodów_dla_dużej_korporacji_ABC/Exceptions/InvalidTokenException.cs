namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Exceptions;

public class InvalidTokenException : Exception 
{
    public InvalidTokenException(string message) : base(message) { }
    public InvalidTokenException() : base("") { }
}