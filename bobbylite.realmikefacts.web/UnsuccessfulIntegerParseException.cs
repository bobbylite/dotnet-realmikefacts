namespace bobbylite.realmikefacts.web;

/// <summary>
/// Exception thrown when <see cref="int"/> tryParse method returns unsuccessful bool.
/// </summary>
public class UnsuccessfulIntegerParseException : Exception
{
    /// <summary>
    /// Creates an instance of <see cref="UnsuccessfulIntegerParseException"/>
    /// </summary>
    public UnsuccessfulIntegerParseException() { }

    /// <summary>
    /// Creates an instance of <see cref="UnsuccessfulIntegerParseException"/>
    /// </summary>
    /// <param name="message">The exception message.</param>
    public UnsuccessfulIntegerParseException(string message)
        : base(message) { }

    /// <summary>
    /// Creates an instance of <see cref="UnsuccessfulIntegerParseException"/>.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The exception that triggered this exception.</param>
    public UnsuccessfulIntegerParseException(string message, Exception inner)
        : base(message, inner) { }
}