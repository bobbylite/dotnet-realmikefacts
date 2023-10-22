namespace bobbylite.realmikefacts.web;

/// <summary>
/// Exception thrown when an authorization token is used and null or empty.
/// </summary>
public class NullOrEmptyAuthorizationTokenException : Exception
{
    /// <summary>
    /// Creates an instance of NullOrEmptyAuthorizationTokenException.
    /// </summary>
    public NullOrEmptyAuthorizationTokenException() { }

    /// <summary>
    /// Creates an instance of NullOrEmptyAuthorizationTokenException.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public NullOrEmptyAuthorizationTokenException(string message)
        : base(message) { }

    /// <summary>
    /// Creates an instance of NullOrEmptyAuthorizationTokenException.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The exception that triggered this exception.</param>
    public NullOrEmptyAuthorizationTokenException(string message, Exception inner)
        : base(message, inner) { }
}