using DemoTests.BaseClasses;
using DemoUtilities;
using DemoWebApi.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;

namespace DemoTests.WebApiTests
{
    [TestClass()]
    public class PublicWebApiTests : TestBase
    {
        [TestMethod]
        public async Task GenerateSecretKey()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<PublicController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync("/public/secretkey");

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            response.EnsureSuccessStatusCode();
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Check result
            var actual = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}, {1}", actual, elapsedTime));
        }
    }
}