namespace Loupedeck.SoundboardPlugin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using NAudio.Wave;

    public static class SoundDevice
    {
        public static byte[] IconImageData { get; set; }

        public static Int32 DeviceIndex1 { get; set; }
        public static Int32 DeviceIndex2 { get; set; }

        public static Single Volume1 { get; set; } = 0.05f;
        public static Single Volume2 { get; set; } = 0.5f;
    }
}
