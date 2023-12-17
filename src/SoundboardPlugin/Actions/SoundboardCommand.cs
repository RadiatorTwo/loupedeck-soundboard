namespace Loupedeck.SoundboardPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
    using NAudio.Wave;

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
        }

        protected override bool RunCommand(ActionEditorActionParameters actionParameters)
        {
            var filename = actionParameters.Parameters["filename"];

            var audioFile1 = new AudioFileReader(filename);
            var audioFile2 = new AudioFileReader(filename);

            var device1 = new WaveOutEvent();
            var device2 = new WaveOutEvent();

            device1.DeviceNumber = SoundDevice.DeviceIndex1;
            device2.DeviceNumber = SoundDevice.DeviceIndex2;

            device1.Volume = SoundDevice.Volume1;
            device2.Volume = SoundDevice.Volume2;

            device1.Init(audioFile1);
            device2.Init(audioFile2);

            device1.Play();
            device2.Play();

            while (device1.PlaybackState == PlaybackState.Playing || device2.PlaybackState == PlaybackState.Playing)
            {
                System.Threading.Thread.Sleep(100);
            }

            device1.Stop();
            device1.Dispose();
            audioFile1.Dispose();

            device2.Stop();
            device2.Dispose();
            audioFile2.Dispose();

            return true;
        }

        protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, int imageWidth, int imageHeight)
        {
            //var resourcePath = EmbeddedResources.FindFile("AudioIcon.png");
            return SoundDevice.IconImageData.ToImage();
        }
    }
}
