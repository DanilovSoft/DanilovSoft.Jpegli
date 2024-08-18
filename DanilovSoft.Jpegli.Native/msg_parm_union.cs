﻿using System.Runtime.InteropServices;
using DanilovSoft.Jpegli.Native.InlineArrays;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
internal unsafe struct msg_parm_union
{
    [FieldOffset(0)]
    public ArrayOf8<int> i;

    [FieldOffset(0)]
    public msg_param_str s; // char*
}
