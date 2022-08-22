using System.Runtime.Serialization;

namespace DBLayer.Data.RetryPolicy;

public class RetryLimitExceededException : DBLayerException
{
	const string RetryLimitExceededMessage = "Retry limit exceeded";

	public RetryLimitExceededException() : base(RetryLimitExceededMessage)
	{}

	public RetryLimitExceededException(Exception innerException) : base(RetryLimitExceededMessage, innerException)
	{}
	protected RetryLimitExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
	{}
}
