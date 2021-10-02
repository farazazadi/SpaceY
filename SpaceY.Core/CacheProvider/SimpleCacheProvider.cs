using SpaceY.Core.Enums;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceY.Core.CacheProvider
{
    public class SimpleCacheProvider : ISimpleCacheProvider
    {
        #region Fields

        private readonly List<(string RocketName, Point Point, ResponseType ResponseType)> _cache;

        #endregion


        #region Properties

        public IReadOnlyList<(string RocketName, Point Point, ResponseType ResponseType)> All => _cache;

        #endregion


        #region Constructors

        public SimpleCacheProvider()
        {
            _cache = new List<(string, Point, ResponseType)>();
        }

        #endregion


        #region Methods

        public void AddOrUpdate(string rocketName, Point point, ResponseType responseType)
        {
            Remove(point);
            _cache.Add((rocketName, point, responseType));
        }

        public bool Any(Point point)
        {
            return _cache.Any(c => c.Point == point);
        }

        public void Clear() => _cache.Clear();

        public (string RocketName, ResponseType ResponseType) Get(Point point)
        {
            var cachedItem = _cache.Single(c => c.Point == point);

            return (cachedItem.RocketName, cachedItem.ResponseType);
        }

        public void Remove(Point point)
        {
            var index = _cache.FindIndex(c => c.Point == point);

            if (index >= 0)
                _cache.RemoveAt(index);
        }

        #endregion
        
    }
}
