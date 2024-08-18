#nullable disable
using System.Runtime.InteropServices;
using DanilovSoft.Jpegli.Native.InlineArrays;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
public struct RowBuffer
{
    public size_t xsize_;
    public size_t ysize_;
    public size_t stride_;
    public size_t offset_;
    public IntPtr data;
}

public enum JpegliDataType
{
    JPEGLI_TYPE_FLOAT = 0,
    JPEGLI_TYPE_UINT8 = 2,
    JPEGLI_TYPE_UINT16 = 3
}

public enum JpegliEndianness
{
    JPEGLI_NATIVE_ENDIAN = 0,
    JPEGLI_LITTLE_ENDIAN = 1,
    JPEGLI_BIG_ENDIAN = 2
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal sealed class jpeg_comp_master
{
    public ArrayOf4<RowBuffer> input_buffer;
    public ArrayOf4<IntPtr> smooth_input;
    public ArrayOf4<IntPtr> raw_data;

    [MarshalAs(UnmanagedType.I1)]
    public bool force_baseline;

    [MarshalAs(UnmanagedType.I1)]
    public bool xyb_mode;

    public byte cicp_transfer_function;

    [MarshalAs(UnmanagedType.I1)]
    public bool use_std_tables;

    [MarshalAs(UnmanagedType.I1)]
    public bool use_adaptive_quantization;

    public int progressive_level;
    public ulong xsize_blocks;
    public ulong ysize_blocks;
    public ulong blocks_per_iMCU_row;
    public IntPtr scan_token_info; // jpegli::ScanTokenInfo*
    public JpegliDataType data_type;
    public JpegliEndianness endianness;
    public IntPtr input_method; // указатель на функцию
    public IntPtr color_transform; // указатель на функцию

    public ArrayOf4<IntPtr> downsample_method; // массив указателей на функции
    public ArrayOf4<IntPtr> quant_mul;
    public ArrayOf4<IntPtr> zero_bias_offset;
    public ArrayOf4<IntPtr> zero_bias_mul;

    public ArrayOf4<int> h_factor;
    public ArrayOf4<int> v_factor;

    public IntPtr huffman_tables;
    public ulong num_huffman_tables;
    public IntPtr slot_id_map;
    public IntPtr context_map;
    public ulong num_contexts;
    public IntPtr ac_ctx_offset;
    public IntPtr coding_tables;
    public IntPtr diff_buffer;
    public RowBuffer fuzzy_erosion_tmp;
    public RowBuffer pre_erosion;
    public RowBuffer quant_field;
    public IntPtr coeff_buffers; // jvirt_barray_ptr*
    public ulong next_input_row;
    public ulong next_iMCU_row;
    public ulong next_dht_index;
    public ulong last_restart_interval;
    public ArrayOf4<int> last_dc_coeff;

    //public JpegBitWriter bw;
    //public IntPtr dct_buffer;
    //public IntPtr block_tmp;
    //public IntPtr token_arrays;
    //public ulong cur_token_array;
    //public IntPtr next_token;
    //public ulong num_tokens;
    //public ulong total_num_tokens;
    //public IntPtr next_refinement_token;
    //public IntPtr next_refinement_bit;
    //public float psnr_target;
    //public float psnr_tolerance;
    //public float min_distance;
    //public float max_distance;
}