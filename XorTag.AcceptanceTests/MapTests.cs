using ImageMagick;

namespace XorTag.AcceptanceTests;

public class MapTests
{
    public class When_getting_map
    {
        private IRestResponse mapResponse;
        private MagickImage parsedImage;

        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = new AcceptanceTestSettings();
            var client = new RestClient(settings.BaseUrl);

            mapResponse = client.Execute(new RestRequest("map"));
            var data = client.DownloadData(new RestRequest("map"));
            parsedImage = new MagickImage(data);
        }

        [Test]
        public void It_should_succeed() => Assert.That(mapResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        [Test]
        public void It_should_create_a_valid_image_of_the_correct_dimensions()
        {
            Assert.That(parsedImage.Width, Is.EqualTo(500));
            Assert.That(parsedImage.Height, Is.EqualTo(300));
        }
    }
}