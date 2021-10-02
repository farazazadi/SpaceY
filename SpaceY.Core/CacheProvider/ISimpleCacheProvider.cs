using System.Collections.Generic;
using System.Drawing;
using SpaceY.Core.Enums;

namespace SpaceY.Core.CacheProvider
{
    public interface ISimpleCacheProvider
    {
        IReadOnlyList<(string RocketName, Point Point, ResponseType ResponseType)> All { get; }
        void AddOrUpdate(string rocketName, Point point, ResponseType responseType);
        (string RocketName, ResponseType ResponseType) Get(Point point);
        bool Any(Point point);
        void Remove(Point point);
        void Clear();
    }
}