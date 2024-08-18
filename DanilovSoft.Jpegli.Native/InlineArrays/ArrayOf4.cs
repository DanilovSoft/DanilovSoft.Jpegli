using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DanilovSoft.Jpegli.Native.InlineArrays;

[InlineArray(4)]
[DebuggerTypeProxy(typeof(ArrayOf4<>.DebugView))]
[DebuggerDisplay("\\{{typeof(T).Name,nq}[4]\\}")]
internal struct ArrayOf4<T>
{
    internal T _field0;

    class DebugView(ArrayOf4<T> thisRef)
    {
        private ArrayOf4<T> _thisRef = thisRef;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Span<T> AsSpan => _thisRef;
    }
}
