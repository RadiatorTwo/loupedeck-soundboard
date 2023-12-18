namespace Loupedeck.SoundboardPlugin
{
    using System;
    using NAudio.Wave;
    using NAudio.Wave.SampleProviders;

    public class SoundboardCommand : ActionEditorCommand
    {
        public SoundboardCommand() : base(DeviceType.All)
        {
            DisplayName = "Play Sound";
            //Description = "Plays a soundfile on two different audio devices at the same time. Aka a Soundboard.";
            GroupName = "";
            
            SetDescription("Plays a soundfile on two different audio devices at the same time. Aka Soundboard.");

            var fileDialog = new ActionEditorFileSelector("Filename", "Filename");
            fileDialog.SetDialogTitle("Filename");
            fileDialog.SetInitialDirectory("C:\\Users\\Radi\\Documents\\Soundboard");
            fileDialog.SetRequired();
            fileDialog.Filters.Add("mp3");
            fileDialog.Filters.Add("wav");
            fileDialog.Filters.Add("aiff");

            ActionEditor.AddControlEx(fileDialog);

            var volumeSlider1 = new ActionEditorSlider("Volume1", "Volume 1");
            var volumeSlider2 = new ActionEditorSlider("Volume2", "Volume 2");

            volumeSlider1.SetValues(0, 100, 25, 1);
            volumeSlider2.SetValues(0, 100, 25, 1);

            ActionEditor.AddControlEx(volumeSlider1);
            ActionEditor.AddControlEx(volumeSlider2);
        }

        protected override bool RunCommand(ActionEditorActionParameters actionParameters)
        {
            var filename = actionParameters.Parameters["filename"];
            var volume1String = actionParameters.Parameters["volume1"];
            var volume2String = actionParameters.Parameters["volume2"];
         
            var volume1 = (float)Convert.ToInt32(volume1String) / 5;
            var volume2 = (float)Convert.ToInt32(volume2String) / 25;

            var device1 = new WaveOutEvent();
            var device2 = new WaveOutEvent();

            device1.DeviceNumber = SoundDevice.DeviceIndex1;
            device2.DeviceNumber = SoundDevice.DeviceIndex2;

            var audioFileReader1 = new AudioFileReader(filename);
            var audioFileReader2 = new AudioFileReader(filename);

            ISampleProvider sampleProvider1 = new VolumeSampleProvider(audioFileReader1.ToSampleProvider());
            ((VolumeSampleProvider)sampleProvider1).Volume = volume1; // Adjust the volume as needed

            ISampleProvider sampleProvider2 = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
            ((VolumeSampleProvider)sampleProvider2).Volume = volume2; // Adjust the volume as needed

            device1.Init(new SampleToWaveProvider(sampleProvider1));
            device2.Init(new SampleToWaveProvider(sampleProvider2));

            device1.Play();
            device2.Play();

            while (device1.PlaybackState == PlaybackState.Playing || device2.PlaybackState == PlaybackState.Playing)
            {
                System.Threading.Thread.Sleep(100);
            }

            device1.Stop();
            device1.Dispose();

            device2.Stop();
            device2.Dispose();

            audioFileReader1.Dispose();
            audioFileReader2.Dispose();

            return true;
        }

        protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, int imageWidth, int imageHeight)
        {
            //var resourcePath = EmbeddedResources.FindFile("AudioIcon.png");
            //return SoundDevice.IconImageData.ToImage();
            
            var settings = Plugin.ListPluginSettings();
            
            using (var bitmapBuilder = new BitmapBuilder(imageWidth, imageHeight))
            {
                bitmapBuilder.SetBackgroundImage(SoundDevice.IconImageData.ToImage());
                //bitmapBuilder.DrawText("My text");

                return bitmapBuilder.ToImage();
            }
        }
    }
}
