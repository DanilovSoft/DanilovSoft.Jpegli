using System.Text;
using DanilovSoft.Jpegli.Native.Wrappers;

namespace DanilovSoft.Jpegli.Native;

internal sealed class JpegErrorMgr : PtrOwner<jpeg_error_mgr>
{
    public unsafe void ThrowIfError(IntPtr cinfo)
    {
        Load();

        if (Structure.msg_code == 0)
        {
            return;
        }

        Span<byte> messageBuffer = stackalloc byte[200];

        fixed (byte* messageBufferPtr = messageBuffer)
        {
            Structure.format_message(cinfo, messageBufferPtr);
        }

        var nullTerminator = messageBuffer.IndexOf((byte)0);
        if (nullTerminator != -1)
        {
            messageBuffer = messageBuffer[0..nullTerminator];
        }

        var message = Encoding.ASCII.GetString(messageBuffer);

        throw new JpegLibException(message) { MsgCode = Structure.msg_code };
    }
}
