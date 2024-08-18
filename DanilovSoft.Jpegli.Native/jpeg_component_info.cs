using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe class jpeg_component_info
{
    public int component_id;             /* identifier for this component (0..255) */
    public int component_index;          /* its index in SOF or cinfo->comp_info[] */
    public int h_samp_factor;            /* horizontal sampling factor (1..4) */
    public int v_samp_factor;            /* vertical sampling factor (1..4) */
    public int quant_tbl_no;             /* quantization table selector (0..3) */
    public int dc_tbl_no;                /* DC entropy table selector (0..3) */
    public int ac_tbl_no;                /* AC entropy table selector (0..3) */

    /* Remaining fields should be treated as private by applications. */
}
