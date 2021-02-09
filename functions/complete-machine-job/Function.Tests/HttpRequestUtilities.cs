using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;

namespace Function.Tests
{
    internal static class HttpRequestUtilities
    {
        public static HttpRequest ToHttpRequest(this object o)
        {
            var json = JsonConvert.SerializeObject(o);
            var byteArray = Encoding.ASCII.GetBytes(json);
 
            var memoryStream = new MemoryStream(byteArray);
            memoryStream.Flush();
            memoryStream.Position = 0;
 
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(memoryStream);
 
            return mockRequest.Object;
        }
    }
}