using System;
using System.Collections.Generic;
using System.Drawing;
using SpaceY.Core.CacheProvider;
using SpaceY.Core.Enums;
using SpaceY.Core.Exceptions;

namespace SpaceY.Core.Models
{
    public class LandingArea : ILandingArea
    {

        #region Fields
        
        private readonly ISimpleCacheProvider _cache;

        #endregion


        #region Properties

        public string Name { get; private set; }
        public Size Size { get; private set; } = new Size(100, 100);
        public Size LandingPlatformSize { get; private set; } = new Size(10, 10);
        public Point LandingPlatformPosition { get; private set; } = new Point(5, 5);

        #endregion

        
        #region Construction

        public LandingArea(ISimpleCacheProvider cacheProvider)
        {
            _cache = cacheProvider;
        }

        
        #region Fluent Interface Design Pattern

        public LandingArea WithName(string name)
        {
            Name = name;
            
            return this;
        }
        
        public LandingArea WithSize(Size size)
        {
            Size = size;
            
            return this;
        }

        public LandingArea WithLandingPlatformSize(Size size)
        {
            ValidateLandingPlatformSize(size);
                    
            LandingPlatformSize = size;
            
            return this;
        }
        
        public LandingArea AtPosition(Point point)
        {
            ValidateLandingPlatformPosition(point);

            LandingPlatformPosition = point;
            
            return this;
        }

        #endregion
        
        #endregion


        #region Methods

        #region Public Methods

        public ResponseType CheckRocketIsOnGoodWay(string rocketName, Point point)
        {
            if (_cache.Any(point))
                return ResponseType.Clash;

            if (!IsInLandingPlatformArea(point))
            {
                _cache.AddOrUpdate(rocketName, point, ResponseType.OutOfPlatform);
                return ResponseType.OutOfPlatform;
            }

            if (!IsSafePointToLand(point))
            {
                _cache.AddOrUpdate(rocketName, point, ResponseType.Clash);
                return ResponseType.Clash;
            }

            
            _cache.AddOrUpdate(rocketName, point, ResponseType.OkForLanding);

            return ResponseType.OkForLanding;
            
        }

        #endregion

        #region Private Methods

        private bool IsInLandingPlatformArea(Point point)
        {
            var landingPlatformX = LandingPlatformPosition.X;
            var landingPlatformY = LandingPlatformPosition.Y;
            var landingPlatformWidth = LandingPlatformSize.Width;
            var landingPlatformHeight = LandingPlatformSize.Height;
            
           
            var result = IsInArea(point, landingPlatformX, landingPlatformY, landingPlatformWidth, landingPlatformHeight);
           
            return result;
        }

        private static bool IsInArea(Point point, int areaX, int areaY, int areaWidth, int areaHeight)
        {
            return areaX <= point.X
                   && point.X <= areaX + areaWidth - 1 
                   && areaY <= point.Y 
                   && point.Y <= areaY + areaHeight - 1;
        }

        private bool IsSafePointToLand(Point point)
        {
            var unsafePoints = new List<Point>();

            var allCachedPoints = _cache.All;
            
            foreach (var c in allCachedPoints)
                unsafePoints.AddRange(GetUnsafePoints(c.Point));

            return !unsafePoints.Contains(point);
        }
        
        private List<Point> GetUnsafePoints(Point point)
        {
            var unsafePoints = new List<Point>();

            var x = point.X > 0 ? point.X - 1 : point.X;
            var y = point.Y > 0 ? point.Y - 1 : point.Y;
            var tempY = y;

            for (; x <= point.X + 1; x++)
            {
                for (; y <= point.Y + 1; y++)
                    unsafePoints.Add(new Point(x,y));

                y = tempY;
            }


            return unsafePoints;
        }
        
        #endregion
        
        #endregion


        #region Validations

        private void ValidateLandingPlatformSize(Size size)
        {
            if (size.Width > Size.Width || size.Height > Size.Height)
                throw new ArgumentOutOfRangeException(nameof(size),
                    $"The size of the landing platform ({size.ToString()}) can not be larger than the size of the landing area ({Size.ToString()})!");
        }
        
        private void ValidateLandingPlatformPosition(Point position)
        {
            if (position.X < 0 || position.Y < 0
                || position.X + LandingPlatformSize.Width > Size.Width 
                || position.Y + LandingPlatformSize.Height > Size.Height)
                throw new LandingPlatformIsOutOfLandingAreaException($"Landing platform with with position ({position.ToString()}) and size ({LandingPlatformSize.ToString()}) is out of landing area!");
        }
        
        #endregion
    }
}