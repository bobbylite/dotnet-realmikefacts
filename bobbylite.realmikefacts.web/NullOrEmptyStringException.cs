namespace bobbylite.realmikefacts.web;

/// <summary>
/// Exception thrown when <see cref="string"/> is null or empty.
/// </summary>
public class NullOrEmptyStringException : Exception
{
    /// <summary>
    /// Creates an instance of <see cref="NullOrEmptyStringException"/>
    /// </summary>
    public NullOrEmptyStringException() { }

    /// <summary>
    /// Creates an instance of <see cref="NullOrEmptyStringException"/>
    /// </summary>
    /// <param name="message">The exception message.</param>
    public NullOrEmptyStringException(string message)
        : base(message) { }

    /// <summary>
    /// Creates an instance of <see cref="NullOrEmptyStringException"/>.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The exception that triggered this exception.</param>
    public NullOrEmptyStringException(string message, Exception inner)
        : base(message, inner) { }
}