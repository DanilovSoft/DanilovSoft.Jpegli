using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native.Wrappers;

internal static class PtrHolder
{
    public static PtrHolder<T> Create<T>(IntPtr nativePtr) where T : class, new() => new PtrHolder<T>(nativePtr);
    public static unsafe PtrHolder<T> Create<T>(T* nativePtr) where T : class, new() => new PtrHolder<T>(nativePtr);
}

[DebuggerTypeProxy(typeof(PtrHolder<>.DebugView))]
internal class PtrHolder<T>
    where T : class, new()
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private T _structure = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private nint _nativePtr;

    public PtrHolder()
    {
        _nativePtr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
    }

    public unsafe PtrHolder(T* nativePtr) : this((nint)nativePtr) { }

    public PtrHolder(IntPtr nativePtr)
    {
        if (nativePtr == nint.Zero)
        {
            throw new ArgumentException("Null pointer", nameof(nativePtr));
        }

        _nativePtr = nativePtr;
        Marshal.PtrToStructure(nativePtr, _structure);
    }

    public virtual T Structure
    {
        get
        {
            return _structure;
        }
    }

    public virtual nint Ptr
    {
        get
        {
            return _nativePtr;
        }
    }

    public virtual unsafe T* Pointer
    {
        get
        {
            return (T*)Ptr;
        }
    }

    public virtual void Save()
    {
        Marshal.StructureToPtr(_structure, _nativePtr, false);
    }

    public virtual void Load()
    {
        Marshal.PtrToStructure(_nativePtr, _structure);
    }

    public static implicit operator T(PtrHolder<T> thisRef) => thisRef.Structure;
    public static implicit operator nint(PtrHolder<T> thisRef) => thisRef.Ptr;
    public static unsafe implicit operator T*(PtrHolder<T> thisRef) => thisRef.Pointer;

    protected void SetNull()
    {
        _nativePtr = IntPtr.Zero;
        _structure = null!;
    }

    class DebugView(PtrHolder<T> thisRef)
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T? Structure = thisRef._structure;
    }
}
