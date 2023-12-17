namespace Loupedeck.SoundboardPlugin
{
    using System;
    using System.IO;
    using NAudio.Wave;

    // This class contains the plugin-level logic of the Loupedeck plugin.

    public class SoundboardPlugin : Plugin
    {
        // Gets a value indicating whether this is an API-only plugin.
        public override Boolean UsesApplicationApiOnly => true;

        // Gets a value indicating whether this is a Universal plugin or an Application plugin.
        public override Boolean HasNoApplication => true;

        // Initializes a new instance of the plugin class.
        public SoundboardPlugin()
        {
            // Initialize the plugin log.
            PluginLog.Init(this.Log);

            // Initialize the plugin resources.
            PluginResources.Init(this.Assembly);
        }

        // This method is called when the plugin is loaded during the Loupedeck service start-up.
        public override void Load()
        {
            var resourcePath = EmbeddedResources.FindFile("AudioIcon.png");
            SoundDevice.IconImageData = EmbeddedResources.ReadBinaryFile(resourcePath);

            SoundDevice.DeviceIndex1 = -1;
            SoundDevice.DeviceIndex2 = -1;

            for (var i = 0; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities capabilities = WaveOut.GetCapabilities(i);
                if (capabilities.ProductName.Contains("SteelSeries Sonar - Microphone"))
                {
                    SoundDevice.DeviceIndex1 = i;
                }

                if (capabilities.ProductName.Contains("SteelSeries Sonar - Gaming"))
                {
                    SoundDevice.DeviceIndex2 = i;
                }

                if (SoundDevice.DeviceIndex1 != -1 && SoundDevice.DeviceIndex2 != -1)
                {
                    break;
                }
            }
        }

        // This method is called when the plugin is unloaded during the Loupedeck service shutdown.
        public override void Unload()
        {
        }
    }
}
