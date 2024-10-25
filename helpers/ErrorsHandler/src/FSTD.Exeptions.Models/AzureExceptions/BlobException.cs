namespace FSTD.Exeptions.Models.AzureExceptions
{
    [Serializable]
    public class BlobException : Exception
    {
        public BlobException()
        {
        }

        public BlobException(string message)
            : base(message)
        {
        }

        public BlobException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BlobException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
