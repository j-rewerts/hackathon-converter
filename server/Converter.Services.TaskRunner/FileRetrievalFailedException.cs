using System;

namespace Converter.Services.TaskRunner
{
    [Serializable]
    internal class FileRetrievalFailedException : Exception
    {
        public FileRetrievalFailedException()
        {
        }

        public FileRetrievalFailedException(string message) : base(message)
        {
        }

        public FileRetrievalFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}