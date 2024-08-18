using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DanilovSoft.Jpegli.Native;

[InlineArray(80)]
[DebuggerTypeProxy(typeof(DebugView))]
[DebuggerDisplay("Length = 80")]
[DebuggerDisplay("\\{byte[80]\\}")]
internal struct msg_param_str
{
    public byte Value0;

    class DebugView(msg_param_str thisRef)
    {
        public int Length => ((Span<byte>)thisRef).Length;

        public string AsText
        {
            get
            {
                Span<byte> source = thisRef;
                var nullTerm = source.IndexOf((byte)0);
                if (nullTerm != -1)
                {
                    source = source[0..nullTerm];
                }

                return Encoding.ASCII.GetString(source);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public unsafe Span<char> AsSpan => MemoryMarshal.Cast<byte, char>(thisRef).ToArray();
    }
}