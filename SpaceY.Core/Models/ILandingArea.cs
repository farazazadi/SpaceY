using System.Drawing;
using SpaceY.Core.Enums;

namespace SpaceY.Core.Models
{
    public interface ILandingArea
    {
        string Name { get; }
        Size Size { get; }
        Size LandingPlatformSize { get; }
        Point LandingPlatformPosition { get; }
        ResponseType CheckRocketIsOnGoodWay(string rocketName, Point area);
    }
}