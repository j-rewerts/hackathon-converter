using System;

namespace Converter.Services.OpenXml
{
    public class NotSpreadSheetException : Exception
    {
        public NotSpreadSheetException()
        {
        }

        public NotSpreadSheetException(string message) : base(message)
        {
        }

        public NotSpreadSheetException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}