namespace bobbylite.realmikefacts.web;

/// <summary>
/// Exception thrown when deserializing json responses returns null.
/// </summary>
public class JsonDeserializationException : Exception
{
    /// <summary>
    /// Creates an instance of <see cref="JsonDeserializationException"/>
    /// </summary>
    public JsonDeserializationException() { }

    /// <summary>
    /// Creates an instance of <see cref="JsonDeserializationException"/>
    /// </summary>
    /// <param name="message">The exception message.</param>
    public JsonDeserializationException(string message)
        : base(message) { }

    /// <summary>
    /// Creates an instance of <see cref="JsonDeserializationException"/>.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The exception that triggered this exception.</param>
    public JsonDeserializationException(string message, Exception inner)
        : base(message, inner) { }
}