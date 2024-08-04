using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DanilovSoft.Jpegli.Native;

[DebuggerTypeProxy(typeof(ManagedPtr<>.DebugView))]
internal class ManagedPtr<T> : IDisposable 
    where T : class, new()
{
    private bool _ownPtr;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private T? _structure = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IntPtr _nativePtr;

    public ManagedPtr()
    {
        _nativePtr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        _ownPtr = true;
    }

    public ManagedPtr(IntPtr nativePtr)
    {
        if (nativePtr == IntPtr.Zero)
        {
            throw new ArgumentException("Null pointer", nameof(nativePtr));
        }

        _nativePtr = nativePtr;
        _ownPtr = false;
        Load();
    }

    public int Size => Marshal.SizeOf<T>();

    public T Structure
    {
        get
        {
            ThrowIfDisposed();
            return _structure;
        }
    }

    public IntPtr Ptr
    {
        get
        {
            ThrowIfDisposed();
            return _nativePtr;
        }
    }

    public bool IsDisposed => _nativePtr == IntPtr.Zero;

    public unsafe T* Pointer => (T*)Ptr;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Save()
    {
        ThrowIfDisposed();
        Marshal.StructureToPtr(_structure, _nativePtr, false);
    }

    public void Load()
    {
        ThrowIfDisposed();
        Marshal.PtrToStructure(_nativePtr, _structure);
    }

    [MemberNotNull(nameof(_structure))]
    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_structure == null, this);
    }

    private void Dispose(bool disposing)
    {
        if (_nativePtr != IntPtr.Zero && _ownPtr)
        {
            Marshal.FreeHGlobal(_nativePtr);
        }

        _structure = default;
        _nativePtr = IntPtr.Zero;
    }

    ~ManagedPtr()
    {
        Dispose(false);
    }

    public static implicit operator T(ManagedPtr<T> thisRef) => thisRef.Structure;
    public static implicit operator IntPtr(ManagedPtr<T> thisRef) => thisRef.Ptr;
    public static unsafe implicit operator T*(ManagedPtr<T> thisRef) => thisRef.Pointer;

    class DebugView(ManagedPtr<T> thisRef)
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T? Structure = thisRef._structure;
    }
}
