using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ImageMagick;
using Moq;
using NUnit.Framework;
using XorTag.Domain;

namespace XorTag.UnitTests.Domain
{
    public class MapImageBuilderTests
    {
        public class When_building_a_map_image
        {
            private MagickImage image;
            private Mock<IPlayerRepository> playerRepository;
            private List<Player> players;
            private MapImageBuilder ClassUnderTest;

            [OneTimeSetUp]
            public void SetUp()
            {
                var settings = new Settings();
                playerRepository = new Mock<IPlayerRepository>();
                players = new List<Player>{
                    new Player{ Name = "Gimli", X = 0, Y = 0, IsIt = false},
                    new Player{ Name = "Darth Vader", X = 10, Y = 10, IsIt = true},
                    new Player{ Name = "Gandalf", X = 25, Y = 29, IsIt = false},
                    new Player{ Name = "Frodo", X = 25, Y = 0, IsIt = false}
                };
                playerRepository.Setup(x => x.GetAllPlayers()).Returns(players);
                ClassUnderTest = new MapImageBuilder(playerRepository.Object, settings);
                var imageBytes = ClassUnderTest.BuildImage();
                image = new MagickImage(imageBytes, MagickFormat.Png);
            }

            [OneTimeTearDown]
            public void CleanUp()
            {
                image?.Dispose();
                image = null;
            }

            [Test]
            public void It_should_make_an_image_of_the_correct_size()
            {
                Assert.That(image.Width, Is.EqualTo(500));
                Assert.That(image.Height, Is.EqualTo(300));
            }

            [Test]
            public void It_should_get_all_players() => playerRepository.Verify(x => x.GetAllPlayers());

            [Test]
            [Ignore("This test is only for manual debugging purposes")]
            public void Write_image_to_disk()
            {
                var imagePath = Path.Combine(Path.GetTempPath(), "image.png");
                image.Write(imagePath, MagickFormat.Png);
                Process.Start("cmd", $"/c {imagePath}");
                Thread.Sleep(2000);
                File.Delete(imagePath);
            }
        }
    }
}