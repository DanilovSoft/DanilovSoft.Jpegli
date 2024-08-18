using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DanilovSoft.Jpegli.Native.InlineArrays;

[InlineArray(8)]
[DebuggerTypeProxy(typeof(ArrayOf8<>.DebugView))]
[DebuggerDisplay("\\{{typeof(T).Name,nq}[8]\\}")]
internal struct ArrayOf8<T>
{
    internal T _field0;

    class DebugView(ArrayOf8<T> thisRef)
    {
        private ArrayOf8<T> _thisRef = thisRef;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Span<T> AsSpan => _thisRef;
    }
}
