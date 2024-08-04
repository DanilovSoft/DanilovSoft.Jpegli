using System.Buffers;
using System.Diagnostics;
using System.Text;
using static DanilovSoft.Jpegli.Native.PInvoke.Jpeg62;

namespace DanilovSoft.Jpegli.Native;

internal sealed unsafe class LibJpegCompressor : IDisposable
{
#if DEBUG
    [ModuleInitializer]
    public static void InitializeModule()
    {
        Debug.Assert(Marshal.SizeOf<jpeg_common_struct>() == 40); // 40 for x64
        Debug.Assert(Marshal.SizeOf<jpeg_error_mgr>() == 168); // 164 absolute; 168 for x64 (Pack 8)
        Debug.Assert(Marshal.SizeOf<jpeg_compress_struct>() == 504); // 504 for x64
        //Debug.Assert(Marshal.SizeOf<jpeg_destination_mgr>() == 40); 
        Debug.Assert(Marshal.SizeOf<jpeg_progress_mgr>() == 24);
        Debug.Assert(Marshal.SizeOf<jpeg_memory_mgr>() == 96);
    }
#endif

    private readonly jpeg_common_ptr_delegate _errorExit;
    private readonly jpeg_emit_message_delegate _emitMessage;
    private readonly jpeg_output_message_delegate _outputMessage;
    private readonly jpeg_compress_ptr_delegate _initDestination;
    private readonly jpeg_compress_ptr_delegate _emptyOutputBuffer;
    private readonly jpeg_compress_ptr_delegate _termSestination;
    private readonly jpeg_common_ptr_delegate _progressMonitor;
    
    private readonly JpegErrorMgr _errMgr = new();
    private readonly JpegCompressStruct _cinfo = new();
    private readonly ManagedPtr<jpeg_progress_mgr> _progress = new();
    private ManagedPtr<jpeg_destination_mgr>? _dest;
    private ManagedPtr<jpeg_memory_mgr>? _memoryManager;

    public LibJpegCompressor()
    {
        // (!) нельзя терять ссылку на делегат пока делегат используется в неуправляемом коде!
        _errorExit = new(ErrorExit);
        _emitMessage = new(EmitMessage);
        _outputMessage = new(OutputMessage);
        _initDestination = new(InitDestination);
        _emptyOutputBuffer = new(EmptyOutputBuffer);
        _termSestination = new(TermDestination);
        _progressMonitor = new(ProgressMonitor);

        _cinfo.Structure.err = _errMgr;
        //_cinfo.Structure.dest = _dest;

        _progress.Save();
        _cinfo.Save();
    }

    public void Dispose()
    {
        _errMgr.Dispose();
        _cinfo.Dispose();
    }

    public unsafe void Compress(ReadOnlySpan<byte> inputImage, int width, int height, int stride, int channels, int quality, IBufferWriter<byte> output)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(quality, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(quality, 100);

        JpegStdError(_errMgr);
        SetErrCallbacks(_errMgr);
        CompressCore(inputImage, quality, width, height, stride, channels, output);
    }

    private static void JpegStdError(JpegErrorMgr errMgr)
    {
        var err = errMgr.Structure;

        errMgr.Save();
        IntPtr ptrRet = jpeg_std_error(errMgr); // it's actually cals jpegli_std_error
        Debug.Assert(ptrRet == errMgr.Ptr);

        errMgr.Load(); // копируем значения взад.
           
        Debug.Assert(err.jpeg_message_table != default); // array of string messages
        Debug.Assert(err.last_jpeg_message == 129); // array size

#if DEBUG
        for (int i = 0; i < err.last_jpeg_message; i++)
        {
            IntPtr stringPtr = Marshal.ReadIntPtr(err.jpeg_message_table, i * IntPtr.Size); // Получаем указатель на текущую строку
            var message = Marshal.PtrToStringAnsi(stringPtr);
            Debug.WriteLine($"message_table[{i}]: {message}");
        }
#endif
    }

    public static IDisposable TurboCompress(byte[] data, int width, int height, int stride, int quality)
    {
        var buf = IntPtr.Zero;

        throw new NotImplementedException();

        //ulong bufSize = 0;
        //try
        //{
        //    var result = NativeMethods.TjCompress2(
        //        this.compressorHandle,
        //        srcPtr,
        //        width,
        //        stride,
        //        height,
        //        (int)pixelFormat,
        //        ref buf,
        //        ref bufSize,
        //        (int)subSamp,
        //        quality,
        //        (int)flags);

        //    if (result == -1)
        //    {
        //        TJUtils.GetErrorAndThrow();
        //    }

        //    var jpegBuf = new byte[bufSize];
        //    Marshal.Copy(buf, jpegBuf, 0, (int)bufSize);
        //    return jpegBuf;
        //}
        //finally
        //{
        //    NativeMethods.TjFree(buf);
        //}
    }

    private void SetErrCallbacks(JpegErrorMgr errMgr)
    {
        // освобождать указатели не нужно.

        errMgr.Structure.error_exit = Marshal.GetFunctionPointerForDelegate(_errorExit);
        errMgr.Structure.emit_message = Marshal.GetFunctionPointerForDelegate(_emitMessage);
        errMgr.Structure.output_message = Marshal.GetFunctionPointerForDelegate(_outputMessage);
        //err.reset_error_mgr = Marshal.GetFunctionPointerForDelegate(_resetErrorMgr);

        errMgr.Save();
    }

    private void SetDestCallbacks(ManagedPtr<jpeg_destination_mgr> dest)
    {
        dest.Load();

        dest.Structure.init_destination = Marshal.GetFunctionPointerForDelegate(_initDestination);
        //dest.Structure.empty_output_buffer = Marshal.GetFunctionPointerForDelegate(_emptyOutputBuffer);
        //dest.Structure.term_destination = Marshal.GetFunctionPointerForDelegate(_termSestination);

        dest.Save();
    }

    private void CompressCore(ReadOnlySpan<byte> inputImage, int quality, int width, int height, int stride, int channels, IBufferWriter<byte> output)
    {
        var cinfo = _cinfo.Structure;

        //cinfo.mem = DebugCreateUnmanaged<jpeg_memory_mgr>();
        //cinfo.global_state = GlobalState.CSTATE_START;
        //cinfo.comp_info = DebugCreateUnmanaged<jpeg_component_info>();
        //cinfo.master = DebugCreateUnmanaged<jpeg_comp_master>();
        //cinfo.scan_info = DebugCreateUnmanaged<jpeg_scan_info>();

        _cinfo.Save();

        try
        {
            jpeg_CreateCompress(_cinfo, 62, (nuint)_cinfo.Size);
            //uint outBufferSize = 4096;
            //Span<byte> outBuffer = new byte[outBufferSize];

            //IntPtr outBufferPtr = IntPtr.Zero;
            //uint initOutBufferSize = 256;
            //IntPtr outBufferPtr = Marshal.AllocHGlobal((int)initOutBufferSize);



            IntPtr unmanagedPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr))); // (!) в ходе выполнения, jpeglib может изменить этот указатель.
            var p = (IntPtr*)unmanagedPointer;
            *p = IntPtr.Zero;

            //byte* outBufferRef = (byte*)outBufferPtr; // (!) в ходе выполнения, jpeglib может изменить этот указатель.

            var outbuffer = IntPtr.Zero;
            uint initOutBufferSize = 8192;
            jpeg_mem_dest(_cinfo, ref outbuffer, ref initOutBufferSize); // установит cinfo.dest если он null

            //IntPtr initialOutBuffer = (IntPtr)outBufferRef;
            //var s = initialOutBuffer.ToString("X");

            //Span<byte> outBufferSpan = new((void*)outBufferPtr, (int)initOutBufferSize);

            _cinfo.Load();
            _dest = new((IntPtr)cinfo.dest);
            //SetDestCallbacks(_dest);

            //var outBufferSpan = new Span<byte>((void*)outBuffer, (int)outSize);

            _cinfo.Load();
            _memoryManager = new((IntPtr)cinfo.mem);

            cinfo.image_width = (uint)width;  /* image width and height, in pixels */
            cinfo.image_height = (uint)height;
            cinfo.input_components = channels;	// Number of color components per pixel. is 3 or 1 accordingly.
            cinfo.in_color_space = J_COLOR_SPACE.JCS_RGB; // colorspace of input image. JCS_RGB or JCS_GRAYSCALE. You must set in_color_space correctly before calling jpeg_set_defaults()
            _cinfo.Save();

            jpeg_set_defaults(_cinfo);
            
            /* Make optional parameter settings here */
            jpeg_set_quality(_cinfo, quality, force_baseline: true);
            
            

            jpeg_start_compress(_cinfo, write_all_tables: true);

            _cinfo.Load();

            


            //var scanlineLen = (int)(cinfo.image_width * cinfo.input_components);
            //var scanlinePtr = Marshal.AllocHGlobal(scanlineLen);
            //scanlines[0] = scanlinePtr;

            int row_stride;                 /* physical row width in buffer */
            row_stride = (int)(cinfo.image_width * cinfo.input_components);

            var scanlines = new byte[cinfo.image_height][];
            var spanCursor = inputImage;
            for (int i = 0; i < cinfo.image_height; i++)
            {
                var scanline = spanCursor[..row_stride];
                spanCursor = spanCursor[row_stride..];
                scanlines[i] = scanline.ToArray();
            }

            //for (int i = 0; i < cinfo.image_height; i++)
            //while (cinfo.next_scanline < cinfo.image_height)
            {
                //var scanline = inputImage[(int)(cinfo.next_scanline * row_stride)..row_stride];

                //Marshal.Copy(scanline.ToArray(), 0, scanlinePtr, scanline.Length);

                //scanlines[0] = scanline.ToArray();
                //scanlines[0] = scanlinePointer;

                //row_pointer[0] = &raw_image[cinfo.next_scanline * cinfo.image_width * cinfo.input_components];
                var scanlinesWritten = jpeg_write_scanlines(_cinfo, scanlines, scanlines.Length); // returns the number of scanlines actually written. This will normally be equal to the number passed in
                Debug.Assert(scanlinesWritten == scanlines.Length);

                _cinfo.Load();
                _dest.Load();

                uint outsize = *_dest.Structure.outsize;
                Debug.Assert(outsize == initOutBufferSize);

                byte* currentOutBuffer = _dest.Structure.buffer;
                var bufSize = _dest.Structure.bufsize;
                var totalWritten = bufSize - _dest.Structure.free_in_buffer;

                //Debug.Assert((IntPtr)currentOutBuffer == initialOutBuffer);

                var bufferSpan = new Span<byte>(currentOutBuffer, (int)totalWritten);
                output.Write(bufferSpan);
            }

            ///* similar to read file, clean up after we're done compressing */
            jpeg_finish_compress(_cinfo);
            jpeg_destroy_compress(_cinfo);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private static T* DebugCreateUnmanaged<T>() where T : new()
    {
        var structure = new T();

        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>()); // (!) do not forget to free
        Marshal.StructureToPtr(structure, ptr, true);

        return (T*)ptr;
    }

    private void ErrorExit(jpeg_common_struct* cinfo)
    {
        _cinfo.Load();
        _errMgr.Load();

        //Marshal.PtrToStructure((IntPtr)cinfo, _cinfo);
        //Marshal.PtrToStructure((IntPtr)_cinfo.err, _errMgr);

        Span<byte> messageBuffer = stackalloc byte[200];

        fixed (byte* messageBufferPtr = messageBuffer)
        {
            _errMgr.Structure.format_message(cinfo, messageBufferPtr);
        }

        var nullTerminator = messageBuffer.IndexOf((byte)0);
        if (nullTerminator != -1)
        {
            messageBuffer = messageBuffer[0..nullTerminator];
        }

        var message = Encoding.ASCII.GetString(messageBuffer);

        throw new LibJpegException(message) { MsgCode = _errMgr.Structure.msg_code };
    }

    private void EmitMessage(IntPtr cinfo, int msg_level)
    {
        // Custom emit message handler
        Console.WriteLine($"Emit message: {msg_level}");
    }

    private void OutputMessage(IntPtr cinfo)
    {
        // Custom output message handler
        Console.WriteLine("Output message");
    }

    private void ResetErrorMgr(IntPtr cinfo)
    {
        // Custom reset error manager handler
        Console.WriteLine("Reset error manager");
    }

    private void InitDestination(jpeg_compress_struct* cinfo)
    {

    }

    private void TermDestination(jpeg_compress_struct* cinfo)
    {

    }

    private void EmptyOutputBuffer(jpeg_compress_struct* cinfo)
    {

    }

    private void ProgressMonitor(jpeg_common_struct* cinfo)
    {
        
    }
}
