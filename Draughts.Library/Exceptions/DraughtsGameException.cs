using System;

namespace Draughts.Library.Exceptions
{

    [Serializable]
    public class DraughtsGameException : Exception
    {

        public DraughtsGameException() { }

        public DraughtsGameException(string message) : base(message) { }

        public DraughtsGameException(string message, Exception inner) : base(message, inner) { }

        protected DraughtsGameException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }

}
