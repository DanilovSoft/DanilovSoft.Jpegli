namespace DanilovSoft.Jpegli.Native.ExtensionMethods;

internal static class PointerExtensions
{
    public static unsafe T* ToPointer<T>(IntPtr ptr)
    {
        return (T*)ptr.ToPointer();
    }
}
