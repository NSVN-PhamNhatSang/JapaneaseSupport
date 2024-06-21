// Controllers/TextToSpeechController.cs
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace JLearning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextToSpeechController : ControllerBase
    {
        [HttpGet("GetSpeech")]
        public IActionResult GetSpeech(string text)
        {
            string filePath = TextSpeechAPI.GetSpeechSound(text);
            byte[] audioBytes = System.IO.File.ReadAllBytes(filePath);
            return File(audioBytes, "audio/mpeg", "output.mp3");
        }
    }
}
