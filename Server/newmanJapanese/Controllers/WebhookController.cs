using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace JLearning.Controllers
{
    [Route("webhook")]
    public class WebhookController : Controller
    {
        private static readonly JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
        private readonly string connectionString;

        public WebhookController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public async Task<JsonResult> GetWebhookResponse()
        {
            WebhookRequest request;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                request = jsonParser.Parse<WebhookRequest>(body);
            }

            var intentName = request.QueryResult.Intent.DisplayName;
            var parameters = request.QueryResult.Parameters;

            StringBuilder sb = new StringBuilder();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                if (intentName == "iDich - kanji - dich")
                {
                    var wordKanji = parameters.Fields.ContainsKey("kanji") ? parameters.Fields["kanji"].StringValue : string.Empty;
                    if (!string.IsNullOrEmpty(wordKanji))
                    {
                        await GetWordAndCourseDetailsByWordKanji(wordKanji, sb, command);
                    }
                    else
                    {
                        sb.Append("Vui lòng cung cấp từ Kanji để dịch.");
                    }
                }

                if (sb.Length == 0)
                {
                    sb.Append("Vui lòng cung cấp đầu vào hợp lệ để dịch hoặc lấy thông tin khóa học.");
                }
            }

            var response = new WebhookResponse
            {
                FulfillmentText = sb.ToString()
            };

            return Json(response);
        }

        private async Task GetWordAndCourseDetailsByWordKanji(string wordKanji, StringBuilder sb, MySqlCommand command)
        {
            command.CommandText = "SELECT wordId, wordMean, wordHiragana, wordKanji, Example, wordImg FROM words WHERE wordKanji LIKE @wordKanji";
            command.Parameters.AddWithValue("@wordKanji", "%" + wordKanji + "%");

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    string wordId = reader.GetString(0);
                    string wordMean = reader.IsDBNull(1) ? null : reader.GetString(1);
                    string wordHiraganaResult = reader.IsDBNull(2) ? null : reader.GetString(2);
                    string wordKanjiResult = reader.IsDBNull(3) ? null : reader.GetString(3);
                    string example = reader.IsDBNull(4) ? null : reader.GetString(4);
                    string wordImg = reader.IsDBNull(5) ? null : reader.GetString(5);

                    sb.Append("Nghĩa của từ: " + wordMean + "\n");
                    sb.Append("Hiragana: " + wordHiraganaResult + "\n");
                    sb.Append("Kanji: " + wordKanjiResult + "\n");
                    sb.Append("Ví dụ: " + example + "\n");
                    sb.Append("Hình ảnh: " + wordImg + "\n");

                    await reader.CloseAsync();

                    await GetCourseDetailsByWordId(wordId, sb, command);
                }
                else
                {
                    sb.Append("Không tìm thấy từ với Kanji được cung cấp.");
                }
            }
        }

        private async Task GetCourseDetailsByWordId(string wordId, StringBuilder sb, MySqlCommand command)
        {
            command.CommandText = "SELECT c.courseId, c.courseName, c.creatorId, c.level, c.category, c.totalWord " +
                                  "FROM courses c INNER JOIN coursedetail cd ON c.courseId = cd.courseId WHERE cd.wordId = @wordId";
            command.Parameters.AddWithValue("@wordId", wordId);

            using (var courseReader = await command.ExecuteReaderAsync())
            {
                while (await courseReader.ReadAsync())
                {
                    string courseId = courseReader.GetString(0);
                    string courseName = courseReader.GetString(1);
                    string creatorId = courseReader.GetString(2);
                    int level = courseReader.GetInt32(3); // level is an int
                    string category = courseReader.GetString(4);
                    int totalWord = courseReader.GetInt32(5); // totalWord is an int

                    sb.Append("\nThông tin khóa học:\n");
                    sb.Append("Course ID: " + courseId + "\n");
                    sb.Append("Tên khóa học: " + courseName + "\n");
                    sb.Append("Giáo viên: " + creatorId + "\n");
                    sb.Append("Cấp độ: " + level + "\n");
                    sb.Append("Chủ đề: " + category + "\n");
                    sb.Append("Tổng số từ: " + totalWord + "\n");
                }
            }
        }
    }
}
