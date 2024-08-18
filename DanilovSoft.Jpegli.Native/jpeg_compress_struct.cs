#nullable disable
using System.Runtime.InteropServices;
using DanilovSoft.Jpegli.Native.InlineArrays;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal unsafe class jpeg_compress_struct : jpeg_common_struct
{
    public jpeg_destination_mgr* dest;

    public uint image_width; // JDIMENSION
    public uint image_height; // JDIMENSION
    /// <summary>Number of color components per pixel.</summary>
    public int input_components;
    /// <summary>Colorspace of input image.</summary>
    public J_COLOR_SPACE in_color_space; // J_COLOR_SPACE

    public double input_gamma;

    /// <summary>Bits of precision in image data.</summary>
    public int data_precision;

    /// <summary># of color components in JPEG image.</summary>
    public int num_components;

    /// <summary>colorspace of JPEG image.</summary>
    public J_COLOR_SPACE jpeg_color_space; // J_COLOR_SPACE
    public jpeg_component_info* comp_info; // jpeg_component_info*

    public ArrayOf4<IntPtr> quant_tbl_ptrs; // JQUANT_TBL* [NUM_QUANT_TBLS]
    public ArrayOf4<IntPtr> dc_huff_tbl_ptrs; // JHUFF_TBL* [NUM_HUFF_TBLS]
    public ArrayOf4<IntPtr> ac_huff_tbl_ptrs; // JHUFF_TBL* [NUM_HUFF_TBLS]

    public ArrayOf16<byte> arith_dc_L; // UINT8 [NUM_ARITH_TBLS]
    public ArrayOf16<byte> arith_dc_U; // UINT8 [NUM_ARITH_TBLS]
    public ArrayOf16<byte> arith_ac_K; // UINT8 [NUM_ARITH_TBLS]

    /// <summary># of entries in scan_info array</summary>
    public int num_scans;
    public jpeg_scan_info* scan_info; // jpeg_scan_info*

    /// <summary>TRUE=caller supplies downsampled data</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool raw_data_in;

    /// <summary>TRUE=arithmetic coding, FALSE=Huffman</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool arith_code;

    /// <summary>TRUE=optimize entropy encoding parms</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool optimize_coding;

    /// <summary>TRUE=first samples are cosited</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool CCIR601_sampling;

    /// <summary>1..100, or 0 for no input smoothing</summary>
    public int smoothing_factor;
    /// <summary>DCT algorithm selector</summary>
    public int dct_method; // J_DCT_METHOD

    /// <summary>MCUs per restart, or 0 for no restart</summary>
    public uint restart_interval; // unsigned int
    /// <summary>if > 0, MCU rows per restart interval</summary>
    public int restart_in_rows;

    /// <summary>should a JFIF marker be written?</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool write_JFIF_header;
    /// <summary>What to write for the JFIF version number</summary>
    public byte JFIF_major_version;
    public byte JFIF_minor_version;
    /// <summary>JFIF code for pixel size units</summary>
    public byte density_unit;
    /// <summary>Horizontal pixel density</summary>
    public ushort X_density;
    /// <summary>Vertical pixel density</summary>
    public ushort Y_density;

    [MarshalAs(UnmanagedType.I1)]
    public bool write_Adobe_marker;

    /// <summary>0 .. image_height-1</summary>
    public uint next_scanline; // JDIMENSION

    /// <summary>TRUE if scan script uses progressive mode</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool progressive_mode;
    public int max_h_samp_factor;
    public int max_v_samp_factor;

    /// <summary># of iMCU rows to be input to coef ctlr</summary>
    public uint total_iMCU_rows; // JDIMENSION

    ///<summary># of JPEG components in this scan</summary>
    public int comps_in_scan;
    public ArrayOf4<IntPtr> cur_comp_info; // jpeg_component_info* [MAX_COMPS_IN_SCAN]

    ///<summary># of MCUs across the image</summary>
    public uint MCUs_per_row; // JDIMENSION
    ///<summary># of MCU rows in the image</summary>
    public uint MCU_rows_in_scan; // JDIMENSION
    /// <summary># of DCT blocks per MCU</summary>
    public int blocks_in_MCU;
    public ArrayOf10<int> MCU_membership; // int [D_MAX_BLOCKS_IN_MCU]

    // progressive JPEG parameters for scan
    public int Ss, Se, Ah, Al;

    public jpeg_comp_master* master; // jpeg_comp_master*
    public IntPtr main; // jpeg_c_main_controller*
    public IntPtr prep; // jpeg_c_prep_controller*
    public IntPtr coef; // jpeg_c_coef_controller*
    public IntPtr marker; // jpeg_marker_writer*
    public IntPtr cconvert; // jpeg_color_converter*
    public IntPtr downsample; // jpeg_downsampler*
    public IntPtr fdct; // jpeg_forward_dct*
    public IntPtr entropy; // jpeg_entropy_encoder*
    public IntPtr script_space; // jpeg_scan_info*
    public int script_space_size;
}
