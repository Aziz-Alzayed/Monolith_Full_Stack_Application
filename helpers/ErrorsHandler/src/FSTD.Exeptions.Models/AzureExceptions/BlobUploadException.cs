namespace FSTD.Exeptions.Models.AzureExceptions
{
    [Serializable]
    public class BlobUploadException : Exception
    {
        public BlobUploadException()
        {
        }

        public BlobUploadException(string message)
            : base(message)
        {
        }

        public BlobUploadException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BlobUploadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
