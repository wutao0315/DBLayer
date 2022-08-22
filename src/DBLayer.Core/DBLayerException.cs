using System.Runtime.Serialization;

namespace DBLayer;

/// <summary>
/// Defines the base class for the namespace exceptions.
/// </summary>
/// <remarks>
/// This class is the base class for exceptions that may occur during
/// execution of the namespace members.
/// </remarks>
[Serializable]
public class DBLayerException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DBLayerException"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the <see cref="Exception.Message"/>
	/// property of the new instance such as "A Build Type exception has occurred.".
	/// </remarks>
	public DBLayerException()
		: base("A Build Type exception has occurred.")
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DBLayerException"/> class
	/// with the specified error message.
	/// </summary>
	/// <param name="message">The message to display to the client when the
	/// exception is thrown.</param>
	/// <seealso cref="Exception.Message"/>
	public DBLayerException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DBLayerException"/> class
	/// with the specified error message and InnerException property.
	/// </summary>
	/// <param name="message">The message to display to the client when the
	/// exception is thrown.</param>
	/// <param name="innerException">The InnerException, if any, that threw
	/// the current exception.</param>
	/// <seealso cref="Exception.Message"/>
	/// <seealso cref="Exception.InnerException"/>
	public DBLayerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DBLayerException"/> class
	/// with the specified InnerException property.
	/// </summary>
	/// <param name="innerException">The InnerException, if any, that threw
	/// the current exception.</param>
	/// <seealso cref="Exception.InnerException"/>
	public DBLayerException(Exception innerException)
		: base(innerException.Message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DBLayerException"/> class
	/// with serialized data.
	/// </summary>
	/// <param name="info">The object that holds the serialized object data.</param>
	/// <param name="context">The contextual information about the source or
	/// destination.</param>
	/// <remarks>This constructor is called during deserialization to
	/// reconstitute the exception object transmitted over a stream.</remarks>
	protected DBLayerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
