using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Suspended.GameIconExtractor
{
    [ComImport]
    [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IShellItemImageFactory
    {
        [PreserveSig]
        int GetImage(
            [In, MarshalAs(UnmanagedType.Struct)] SIZE size,
            [In] SIIGBF flags,
            out IntPtr phbm);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;
    }

    [Flags]
    enum SIIGBF
    {
        SIIGBF_RESIZETOFIT = 0x00,
        SIIGBF_BIGGERSIZEOK = 0x01,
        SIIGBF_MEMORYONLY = 0x02,
        SIIGBF_ICONONLY = 0x04,
        SIIGBF_THUMBNAILONLY = 0x08,
        SIIGBF_INCACHEONLY = 0x10,
    }

    [ComImport]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IShellItem
    {
    }

    public static class IconHelper
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        static extern void SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            IntPtr pbc,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out IShellItem ppv);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);

        public static Bitmap GetExeIcon(string exePath, int size)
        {
            Guid shellItemGuid = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe");

            SHCreateItemFromParsingName(exePath, IntPtr.Zero, shellItemGuid, out IShellItem shellItem);

            var factory = (IShellItemImageFactory)shellItem;

            SIZE sz = new SIZE { cx = size, cy = size };

            factory.GetImage(sz, SIIGBF.SIIGBF_BIGGERSIZEOK, out IntPtr hBitmap);

            Bitmap bmp = Bitmap.FromHbitmap(hBitmap);

            DeleteObject(hBitmap);

            return bmp;
        }
    }
}
