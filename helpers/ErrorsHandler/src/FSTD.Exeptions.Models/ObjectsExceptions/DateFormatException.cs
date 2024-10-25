namespace FSTD.Exeptions.Models.ObjectsExceptions
{
    public class DateFormatException : Exception
    {
        public DateFormatException()
        {
        }

        public DateFormatException(string message)
            : base(message)
        {
        }

        public DateFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DateFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
