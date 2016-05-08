using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BikeTracker.Controllers.API
{
    public class FileStringResult : IHttpActionResult
    {
        public string Content { get; set; }

        public string Filename { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage { Content = new StreamContent(GenerateStreamFromString(Content)) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = Filename };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

            // NOTE: Here I am just setting the result on the Task and not really doing any async stuff.
            // But let's say you do stuff like contacting a File hosting service to get the file, then you would do 'async' stuff here.

            return Task.FromResult(response);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}