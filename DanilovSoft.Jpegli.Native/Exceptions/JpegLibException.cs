namespace DanilovSoft.Jpegli.Native.Exceptions;

public class JpegLibException : Exception
{
    public JpegLibException()
    {
    }

    public JpegLibException(int msgCode)
    {
        MsgCode = msgCode;
    }

    public JpegLibException(string? message) : base(message)
    {
    }

    public JpegLibException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public int MsgCode { get; init; }
}
