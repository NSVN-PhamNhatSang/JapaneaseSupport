using Google.Cloud.TextToSpeech.V1;
using System;
using System.IO;
namespace JLearning
{
    public class TextSpeechAPI
    {
        private static readonly string localBasePath = Path.Combine(Environment.CurrentDirectory, @"output");

        public static string GetSpeechSound(string text, string languageCode = "ja-JP", SsmlVoiceGender ssmlVoiceGender = SsmlVoiceGender.Female)
        {
            SynthesisInput input = new SynthesisInput { Text = text };
            VoiceSelectionParams voiceSelection = new VoiceSelectionParams
            {
        
                LanguageCode = languageCode,
                Name = "ja-JP-Wavenet-A",
                SsmlGender = ssmlVoiceGender,
            };
            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3,
                VolumeGainDb = 5,
                Pitch = 1.0,
                SpeakingRate = 1.0,
            };

            GCPCredentialManager.SetCredentials();
            TextToSpeechClient client = TextToSpeechClient.Create();
            SynthesizeSpeechResponse response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);
            return WriteLocalFile(Path.Combine(localBasePath, "output.mp3"), response);
        }

        private static string WriteLocalFile(string writePath, SynthesizeSpeechResponse response)
        {
            if (!Directory.Exists(localBasePath))
            {
                Directory.CreateDirectory(localBasePath);
            }

            using (FileStream stream = File.Create(writePath))
            {
                response.AudioContent.WriteTo(stream);
            }
            return writePath;
        }
    }
}
