namespace FSTD.Exeptions.Models.HttpResponseExceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NotFoundException(string name, object key)
           : base($"Entity \"{name}\" ({key}) was not found.")
        {

        }
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
