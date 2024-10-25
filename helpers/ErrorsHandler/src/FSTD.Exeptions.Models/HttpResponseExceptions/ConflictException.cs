namespace FSTD.Exeptions.Models.HttpResponseExceptions
{
    [Serializable]
    public class ConflictException : Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string message)
            : base(message)
        {
        }

        public ConflictException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
