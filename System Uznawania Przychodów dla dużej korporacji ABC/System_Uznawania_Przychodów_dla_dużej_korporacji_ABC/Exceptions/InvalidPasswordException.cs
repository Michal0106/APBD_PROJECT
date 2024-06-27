namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Invalid password") { }
}