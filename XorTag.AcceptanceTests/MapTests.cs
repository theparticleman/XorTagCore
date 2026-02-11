using ImageMagick;

namespace XorTag.AcceptanceTests;

public class MapTests
{
    public class When_getting_map
    {
        private HttpResponseMessage mapResponse;
        private MagickImage parsedImage;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var factory = TestHelpers.CreateTestFactory();
            var client = factory.CreateClient();

            mapResponse = await client.GetAsync("/map");
            var data = await client.GetByteArrayAsync("/map");
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
