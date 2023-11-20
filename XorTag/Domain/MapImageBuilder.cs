using ImageMagick;

namespace XorTag.Domain
{
    public class MapImageBuilder
    {
        private const int ImageWidth = 500;
        private const int ImageHeight = 300;
        private readonly IPlayerRepository playerRepository;
        private int mapWidth;
        private int mapHeight;
        private int cellWidth;
        private int cellHeight;

        public MapImageBuilder(IPlayerRepository playerRepository, IMapSettings mapSettings)
        {
            this.playerRepository = playerRepository;
            mapWidth = mapSettings.MapWidth;
            mapHeight = mapSettings.MapHeight;
            cellWidth = ImageWidth / mapWidth;
            cellHeight = ImageHeight / mapHeight;
        }

        public byte[] BuildImage()
        {
            var settings = new MagickReadSettings
            {
                Font = "AovelSansRounded-rdDL.ttf",
                BackgroundColor = MagickColors.Gray,
                Height = ImageHeight,
                Width = ImageWidth,
            };
            using (var image = new MagickImage("map.png", settings))
            {
                DrawGridLines(image);
                DrawPlayers(image);

                var ms = new MemoryStream();
                image.Write(ms, MagickFormat.Png);
                return ms.ToArray();
            }
        }

        private void DrawGridLines(MagickImage image)
        {
            var drawables = new Drawables();
            drawables.FillColor(MagickColors.DarkGray);
            for (int x = 0; x <= mapWidth; x++)
            {
                int lineX = Math.Min(x * cellWidth, ImageWidth - 1);
                drawables.Line(lineX, 0, lineX, ImageHeight);
            }
            for (int y = 0; y <= mapHeight; y++)
            {
                int lineY = Math.Min(y * cellHeight, ImageHeight - 1);
                drawables.Line(0, lineY, ImageWidth, lineY);
            }
            drawables.Draw(image);
        }

        private void DrawPlayers(MagickImage image)
        {
            var players = playerRepository.GetAllPlayers();
            var drawables = new Drawables();
            foreach (var player in players)
            {
                drawables.FillColor(player.IsIt ? MagickColors.Red : MagickColors.Black);
                drawables.Ellipse(player.X * cellWidth + (cellWidth / 2), player.Y * cellHeight + (cellHeight / 2), cellWidth / 2, cellHeight / 2, 0, 360);
                var yPosition = player.Y < mapHeight - 2
                    ? player.Y * cellHeight + (cellHeight * 2)
                    : player.Y * cellHeight;

                drawables.FillColor(MagickColors.Black);
                drawables.TextAlignment(TextAlignment.Center);
                drawables.Text(player.X * cellWidth + (cellWidth / 2), yPosition, player.Name);
            }
            drawables.Draw(image);
        }

    }
}