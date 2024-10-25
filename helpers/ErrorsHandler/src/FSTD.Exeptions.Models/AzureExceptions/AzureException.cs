namespace FSTD.Exeptions.Models.AzureExceptions
{
    [Serializable]
    public class AzureException : Exception
    {
        public AzureException()
        {
        }

        public AzureException(string message)
            : base(message)
        {
        }

        public AzureException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected AzureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
