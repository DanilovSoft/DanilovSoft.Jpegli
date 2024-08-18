using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DanilovSoft.Jpegli.Native.InlineArrays;

[InlineArray(16)]
[DebuggerTypeProxy(typeof(ArrayOf16<>.DebugView))]
[DebuggerDisplay("\\{{typeof(T).Name,nq}[16]\\}")]
internal struct ArrayOf16<T>
{
    internal T _field0;

    class DebugView(ArrayOf16<T> thisRef)
    {
        private ArrayOf16<T> _thisRef = thisRef;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Span<T> AsSpan => _thisRef;
    }
}
