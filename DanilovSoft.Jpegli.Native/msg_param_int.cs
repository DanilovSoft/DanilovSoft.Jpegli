using System.Diagnostics;

namespace DanilovSoft.Jpegli.Native;

[InlineArray(8)]
[DebuggerTypeProxy(typeof(DebugView))]
internal struct msg_param_int
{
    public int Value0;

    class DebugView(msg_param_int thisRef)
    {
        private msg_param_int _thisRef = thisRef;
        public Span<int> AsSpan => _thisRef;
    }
}
