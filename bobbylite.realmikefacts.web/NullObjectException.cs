namespace bobbylite.realmikefacts.web;

/// <summary>
/// Exception thrown when <see cref="string"/> is null or empty.
/// </summary>
public class NullObjectException : Exception
{
    /// <summary>
    /// Creates an instance of <see cref="NullObjectException"/>
    /// </summary>
    public NullObjectException() { }

    /// <summary>
    /// Creates an instance of <see cref="NullObjectException"/>
    /// </summary>
    /// <param name="message">The exception message.</param>
    public NullObjectException(string message)
        : base(message) { }

    /// <summary>
    /// Creates an instance of <see cref="NullObjectException"/>.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The exception that triggered this exception.</param>
    public NullObjectException(string message, Exception inner)
        : base(message, inner) { }
}