namespace bobbylite.realmikefacts.web;

/// <summary>
/// Exception thrown when http responses contain not successful status codes.
/// </summary>
public class NotSuccessfulHttpRequestException : Exception
{
    /// <summary>
    /// Creates an instance of <see cref="NotSuccessfulHttpRequestException"/>
    /// </summary>
    public NotSuccessfulHttpRequestException() { }

    /// <summary>
    /// Creates an instance of <see cref="NotSuccessfulHttpRequestException"/>
    /// </summary>
    /// <param name="message">The exception message.</param>
    public NotSuccessfulHttpRequestException(string message)
        : base(message) { }

    /// <summary>
    /// Creates an instance of <see cref="NotSuccessfulHttpRequestException"/>.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The exception that triggered this exception.</param>
    public NotSuccessfulHttpRequestException(string message, Exception inner)
        : base(message, inner) { }
}