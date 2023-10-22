namespace bobbylite.realmikefacts.web;

/// <summary>
/// Exception thrown when <see cref="double"/> tryParse method returns unsuccessful bool.
/// </summary>
public class UnsuccessfulDoubleParseException : Exception
{
    /// <summary>
    /// Creates an instance of <see cref="UnsuccessfulDoubleParseException"/>
    /// </summary>
    public UnsuccessfulDoubleParseException() { }

    /// <summary>
    /// Creates an instance of <see cref="UnsuccessfulDoubleParseException"/>
    /// </summary>
    /// <param name="message">The exception message.</param>
    public UnsuccessfulDoubleParseException(string message)
        : base(message) { }

    /// <summary>
    /// Creates an instance of <see cref="UnsuccessfulDoubleParseException"/>.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The exception that triggered this exception.</param>
    public UnsuccessfulDoubleParseException(string message, Exception inner)
        : base(message, inner) { }
}