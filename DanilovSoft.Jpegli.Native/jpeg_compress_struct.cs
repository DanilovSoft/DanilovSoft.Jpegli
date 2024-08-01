#nullable disable

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct jpeg_compress_struct
{
    public IntPtr err; // jpeg_error_mgr*
    public IntPtr mem; // jpeg_memory_mgr*
    public IntPtr progress; // jpeg_progress_mgr*
    public IntPtr client_data; // void *client_data; Available for use by application.
    [MarshalAs(UnmanagedType.I1)]
    public bool is_decompressor; // So common code can tell which is which
    public GlobalState global_state; // For checking call sequence validity
    public IntPtr dest; // jpeg_destination_mgr*

    public uint image_width; // JDIMENSION
    public uint image_height; // JDIMENSION
    public int input_components;
    public J_COLOR_SPACE in_color_space; // J_COLOR_SPACE

    public double input_gamma;

    public int data_precision;
    public int num_components;
    public int jpeg_color_space; // J_COLOR_SPACE
    public IntPtr comp_info; // jpeg_component_info*

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public IntPtr[] quant_tbl_ptrs; // JQUANT_TBL* [NUM_QUANT_TBLS]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public IntPtr[] dc_huff_tbl_ptrs; // JHUFF_TBL* [NUM_HUFF_TBLS]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public IntPtr[] ac_huff_tbl_ptrs; // JHUFF_TBL* [NUM_HUFF_TBLS]

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] arith_dc_L; // UINT8 [NUM_ARITH_TBLS]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] arith_dc_U; // UINT8 [NUM_ARITH_TBLS]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] arith_ac_K; // UINT8 [NUM_ARITH_TBLS]

    public int num_scans;
    public IntPtr scan_info; // jpeg_scan_info*

    [MarshalAs(UnmanagedType.I1)]
    public bool raw_data_in;
    [MarshalAs(UnmanagedType.I1)]
    public bool arith_code;
    [MarshalAs(UnmanagedType.I1)]
    public bool optimize_coding;
    [MarshalAs(UnmanagedType.I1)]
    public bool CCIR601_sampling;
    [MarshalAs(UnmanagedType.I1)]
    public bool do_fancy_downsampling;
    public int smoothing_factor;
    public int dct_method; // J_DCT_METHOD

    public uint restart_interval; // unsigned int
    public int restart_in_rows;

    [MarshalAs(UnmanagedType.I1)]
    public bool write_JFIF_header;
    public byte JFIF_major_version;
    public byte JFIF_minor_version;
    public byte density_unit;
    public ushort X_density;
    public ushort Y_density;

    [MarshalAs(UnmanagedType.I1)]
    public bool write_Adobe_marker;

    public uint next_scanline; // JDIMENSION

    [MarshalAs(UnmanagedType.I1)]
    public bool progressive_mode;
    public int max_h_samp_factor;
    public int max_v_samp_factor;

    public uint total_iMCU_rows; // JDIMENSION

    public int comps_in_scan;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public IntPtr[] cur_comp_info; // jpeg_component_info* [MAX_COMPS_IN_SCAN]

    public uint MCUs_per_row; // JDIMENSION
    public uint MCU_rows_in_scan; // JDIMENSION
    public int blocks_in_MCU;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public int[] MCU_membership; // int [D_MAX_BLOCKS_IN_MCU]

    public int Ss, Se, Ah, Al;

    public int block_size;
    public IntPtr natural_order; // const int*
    public int lim_Se;

    public jpeg_comp_master master; // jpeg_comp_master*
    public IntPtr main; // jpeg_c_main_controller*
    public IntPtr prep; // jpeg_c_prep_controller*
    public IntPtr coef; // jpeg_c_coef_controller*
    public IntPtr marker; // jpeg_marker_writer*
    public IntPtr entropy; // jpeg_entropy_encoder*
    public IntPtr script_space; // jpeg_scan_info*
    public int script_space_size;
}
