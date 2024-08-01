namespace DanilovSoft.Jpegli.Native.Exceptions;

public class LibJpegException : Exception
{
    public LibJpegException()
    {
    }

    public LibJpegException(int msgCode)
    {
        MsgCode = msgCode;
    }

    public LibJpegException(string? message) : base(message)
    {
    }

    public LibJpegException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public int MsgCode { get; init; }
}
