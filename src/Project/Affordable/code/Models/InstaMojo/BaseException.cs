using System;
using System.Runtime.Serialization;

namespace Sitecore.Affordable.Website.Models.InstaMojo
{
    public class BaseException : Exception
    {
        public BaseException()
            : base() { }

        public BaseException(string message)
            : base(message) {  }

        public BaseException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public BaseException(string message, Exception innerException)
            : base(message, innerException) { }

        public BaseException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
