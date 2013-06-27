using System;

namespace Draughts.Library.Exceptions
{

    [Serializable]
    public class BoardException : Exception
    {

        public BoardException() { }

        public BoardException(string message) : base(message) { }

        public BoardException(string message, Exception inner) : base(message, inner) { }

        protected BoardException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }

}
