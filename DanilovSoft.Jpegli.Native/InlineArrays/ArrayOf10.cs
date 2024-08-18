using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DanilovSoft.Jpegli.Native.InlineArrays;

[InlineArray(10)]
[DebuggerTypeProxy(typeof(ArrayOf10<>.DebugView))]
[DebuggerDisplay("\\{{typeof(T).Name,nq}[10]\\}")]
internal struct ArrayOf10<T>
{
    internal T _field0;

    class DebugView(ArrayOf10<T> thisRef)
    {
        private ArrayOf10<T> _thisRef = thisRef;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Span<T> AsSpan => _thisRef;
    }
}
