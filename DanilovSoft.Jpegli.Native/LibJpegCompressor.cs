using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using DanilovSoft.Jpegli.Native.InlineArrays;
using DanilovSoft.Jpegli.Native.Wrappers;
using static DanilovSoft.Jpegli.Native.PInvoke.Jpeg62;

namespace DanilovSoft.Jpegli.Native;

internal sealed unsafe class LibJpegCompressor : IDisposable
{
    private readonly jpeg_common_ptr_delegate _errorExit;
    private readonly jpeg_emit_message_delegate _emitMessage;
    private readonly jpeg_output_message_delegate _outputMessage;
    private readonly jpeg_compress_ptr_delegate _initDestination;
    private readonly jpeg_compress_ptr_delegate _emptyOutputBuffer;
    private readonly jpeg_compress_ptr_delegate _termSestination;
    
    private readonly JpegErrorMgr _errMgr = new();
    private readonly JpegCompressStruct _cinfo = new();
    //private PtrOwner<jpeg_destination_mgr>? _dest;
    //private PtrOwner<jpeg_memory_mgr>? _memoryManager;

    public LibJpegCompressor()
    {
        // (!) нельзя терять ссылку на делегат пока делегат используется в неуправляемом коде!
        _errorExit = new(ErrorExit);
        _emitMessage = new(EmitMessage);
        _outputMessage = new(OutputMessage);
        _initDestination = new(InitDestination);
        _emptyOutputBuffer = new(EmptyOutputBuffer);
        _termSestination = new(TermDestination);
        //_progressMonitor = new(ProgressMonitor);

        _cinfo.Structure.err = _errMgr;
        //_cinfo.Structure.dest = _dest;

        _cinfo.Save();
    }

    public void Dispose()
    {
        _errMgr.Dispose();
        _cinfo.Dispose();
    }

    public void Compress(IntPtr scan0, int stride, int width, int height, int channels, J_COLOR_SPACE inColorSpace, int quality, IBufferWriter<byte> output)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentOutOfRangeException.ThrowIfNegative(quality);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(quality, 100);

        JpegStdError(_errMgr);
        SetErrCallbacks(_errMgr);
        CompressCore(scan0, stride, width, height, channels, inColorSpace, quality, output);
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

    private void SetDestCallbacks(PtrOwner<jpeg_destination_mgr> dest)
    {
        dest.Load();

        dest.Structure.init_destination = Marshal.GetFunctionPointerForDelegate(_initDestination);
        //dest.Structure.empty_output_buffer = Marshal.GetFunctionPointerForDelegate(_emptyOutputBuffer);
        //dest.Structure.term_destination = Marshal.GetFunctionPointerForDelegate(_termSestination);

        dest.Save();
    }

    private void CompressCore(IntPtr scan0, int stride, int width, int height, int channels, J_COLOR_SPACE inColorSpace, int quality, IBufferWriter<byte> output)
    {
        var cinfo = _cinfo.Structure;

        _cinfo.Save();

        jpeg_CreateCompress(_cinfo, 62, (nuint)_cinfo.Size);

        

        var outbuffer = IntPtr.Zero;
        uint initOutBufferSize = 8192;
        jpeg_mem_dest(_cinfo, ref outbuffer, ref initOutBufferSize); // установит cinfo.dest если он null

        _cinfo.Load();
        var dest = PtrHolder.Create(cinfo.dest);

        _cinfo.Load();
        //_memoryManager = new((IntPtr)cinfo.mem);

        cinfo.image_width = (uint)width;  /* image width and height, in pixels */
        cinfo.image_height = (uint)height;
        cinfo.input_components = channels;  // Number of color components per pixel. is 3 or 1 accordingly.
        cinfo.in_color_space = inColorSpace; // colorspace of input image. JCS_RGB or JCS_GRAYSCALE. You must set in_color_space correctly before calling jpeg_set_defaults()
        //cinfo.data_precision = 8;
        //cinfo.raw_data_in = false;
        //cinfo.optimize_coding = true;
        //cinfo.progressive_mode = true;
        _cinfo.Save();

        jpeg_set_defaults(_cinfo);
        _cinfo.Load();
        _errMgr.ThrowIfError(_cinfo);

        var comp_info = new PtrHolder<jpeg_component_info>(cinfo.comp_info);
        // Default is no chroma subsampling.
        comp_info.Structure.h_samp_factor = 1;
        comp_info.Structure.v_samp_factor = 1;
        comp_info.Save();

        

        //Span<byte> masterSpan = new Span<byte>(cinfo.master, 832);
        //File.WriteAllBytes("D:\\master.bin", masterSpan.ToArray());

        /* Make optional parameter settings here */
        //jpeg_set_quality(_cinfo, quality, force_baseline: true);

        //cinfo.optimize_coding = true;
        //_cinfo.Save();
        jpeg_simple_progression(_cinfo); // после set_defaults
        
        cinfo.optimize_coding = true;
        _cinfo.Save();

        _cinfo.Load();
        var master = new PtrHolder<jpeg_comp_master>(cinfo.master);

        fixed (bool* ptr = &master.Structure.force_baseline)
        {
            Span<byte> span = new Span<byte>(ptr, 100);
        }

        master.Structure.force_baseline = true;
        master.Structure.xyb_mode = false;
        master.Structure.cicp_transfer_function = 2; // unknown transfer function code
        master.Structure.use_std_tables = false;
        master.Structure.use_adaptive_quantization = true;
        master.Structure.progressive_level = 2;
        master.Structure.data_type = JpegliDataType.JPEGLI_TYPE_UINT8;
        master.Structure.endianness = JpegliEndianness.JPEGLI_NATIVE_ENDIAN;
        master.Structure.coeff_buffers = default;
        master.Save();

        jpeg_start_compress(_cinfo, write_all_tables: true);

        master.Load();
        //File.WriteAllBytes("D:\\master.bin", masterSpan.ToArray());

        comp_info.Load();
        _cinfo.Load();
        //masterCopy.Load();

        Span<IntPtr> scanlines = new IntPtr[height]; // TODO pool

        for (int i = 0; i < height; i++)
        {
            scanlines[i] = scan0 + (i * stride);
        }

        //while (cinfo.next_scanline < cinfo.image_height)
        //{
        //    var ret = jpeg_write_scanlines(_cinfo, scanlines.Slice((int)cinfo.next_scanline, 1), 1);
        //    _cinfo.Load();
        //}

        var scanlinesWritten = jpeg_write_scanlines(_cinfo, scanlines, scanlines.Length); // returns the number of scanlines actually written. This will normally be equal to the number passed in
        if (scanlinesWritten != scanlines.Length)
        {
            throw new JpegLibException($"Something went wrong. Number of written scanlines is not equal to passed in number. Expected result: {scanlines.Length}. Actual result: {scanlinesWritten}");
        }

        Debug.Assert(scanlinesWritten == scanlines.Length);

        //Debug.Assert((IntPtr)currentOutBuffer == initialOutBuffer);

        ///* similar to read file, clean up after we're done compressing */
        jpeg_finish_compress(_cinfo);

        _cinfo.Load();
        dest.Load();

        uint outsize = *dest.Structure.outsize;
        Debug.Assert(outsize == initOutBufferSize);

        byte* currentOutBuffer = dest.Structure.buffer;
        var bufSize = dest.Structure.bufsize;
        var totalWritten = bufSize - dest.Structure.free_in_buffer;

        var bufferSpan = new Span<byte>(currentOutBuffer, (int)totalWritten);
        output.Write(bufferSpan);

        jpeg_destroy_compress(_cinfo);
    }

    private void ErrorExit(IntPtr cinfo)
    {
        _cinfo.Load();
        _errMgr.ThrowIfError(cinfo); // Не уверен что это безопасно бросать исключения сквозь маршалинг.
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
