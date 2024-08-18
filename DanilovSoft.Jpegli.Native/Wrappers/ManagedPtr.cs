using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native.Wrappers;

//[DebuggerTypeProxy(typeof(PtrOwner<>.DebugView))]
internal class PtrOwner<T> : PtrHolder<T>, IDisposable
    where T : class, new()
{
    private bool _disposed;

    public PtrOwner()
    {
    }

    public PtrOwner(IntPtr ptr) : base(ptr)
    {
    }

    public unsafe PtrOwner(T* nativePtr) : base((nint)nativePtr) { }

    public int Size => Marshal.SizeOf<T>();

    public override unsafe T* Pointer
    {
        get
        {
            ThrowIfDisposed();
            return base.Pointer;
        }
    }

    public override nint Ptr
    {
        get
        {
            ThrowIfDisposed();
            return base.Ptr;
        }
    }

    public override T Structure
    {
        get
        {
            ThrowIfDisposed();
            return base.Structure;
        }
    }

    public override void Load()
    {
        ThrowIfDisposed();
        base.Load();
    }

    public override void Save()
    {
        ThrowIfDisposed();
        base.Save();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && Ptr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(Ptr);
        }

        SetNull();
        _disposed = true;
    }

    ~PtrOwner()
    {
        Dispose(false);
    }

    public static implicit operator T(PtrOwner<T> thisRef) => thisRef.Structure;
    public static implicit operator nint(PtrOwner<T> thisRef) => thisRef.Ptr;
    public static unsafe implicit operator T*(PtrOwner<T> thisRef) => thisRef.Pointer;

    //class DebugView(PtrOwner<T> thisRef)
    //{
    //    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    //    public T? Structure = thisRef.Structure;
    //}
}
