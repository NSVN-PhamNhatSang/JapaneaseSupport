using Google.Cloud.Speech.V1;
using JLearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JLearning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpeechToTextController : ControllerBase
    {
        private readonly string _credentialPath;

        public SpeechToTextController(IOptions<GoogleCloudSetting> googleCloudSettings)
        {
            _credentialPath = googleCloudSettings.Value.CredentialPath;
        }

        [HttpPost("transcribe")]
        public async Task<IActionResult> TranscribeAudio(IFormFile audioFile)
        {
            try
            {
                // Set Google Cloud credentials
                GCPCredentialManager.SetCredentials();

                // Create SpeechClient
                var speechClient = SpeechClient.Create();

                using (var stream = new MemoryStream())
                {
                    // Copy audio file data to stream
                    await audioFile.CopyToAsync(stream);
                    stream.Position = 0; // Reset stream position to beginning

                    // Log the audio file size and properties
                    Console.WriteLine($"Audio file size: {audioFile.Length} bytes");
                    Console.WriteLine($"Audio file content type: {audioFile.ContentType}");

                    // Recognize speech from audio stream
                    var response = await speechClient.RecognizeAsync(new RecognitionConfig
                    {
                        Encoding = RecognitionConfig.Types.AudioEncoding.OggOpus,
                        SampleRateHertz = 48000, // Adjust to match your recorded audio sample rate
                        LanguageCode = "ja-JP" // Use the provided language code
                    }, RecognitionAudio.FromStream(stream));

                    // Log the API response
                    Console.WriteLine("Google Speech API response received");

                    // Extract transcription from response
                    var transcription = string.Join("\n", response.Results.SelectMany(result => result.Alternatives).Select(alternative => alternative.Transcript));

                    // Check if transcription is empty
                    if (string.IsNullOrEmpty(transcription))
                    {
                        Console.WriteLine("Transcription is empty or null");
                        return BadRequest("No transcription result");
                    }

                    // Return transcription as JSON response
                    return Ok(new { Transcription = transcription });
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Exception occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                // Handle exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error processing audio file.", Error = ex.Message });
            }
        }
    }
}
