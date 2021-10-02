using System.Drawing;
using FluentAssertions;
using SpaceY.Core.CacheProvider;
using SpaceY.Core.Enums;
using SpaceY.Core.Models;
using Xunit;

namespace SpaceY.Core.Tests.Models
{
    public class LandingFunctionalityTests
    {
                
        public static TheoryData<Size, Size, Point, Point> OkForLandingTestData = new()
        {
            //landingAreaSize               landingPlatformSize        landingPlatformPosition      rocketPosition
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(5, 5)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(8, 10)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(14, 14)},
            
            {new Size(25, 25), new Size(15, 15), new Point(0, 1), new Point(0, 1)},
            {new Size(25, 25), new Size(15, 15), new Point(0, 1), new Point(5, 9)},
            {new Size(25, 25), new Size(15, 15), new Point(0, 1), new Point(14, 15)},
        };
        
        [Theory, MemberData(nameof(OkForLandingTestData))]
        public void Rocket_ShouldGetOkForLandingResponse_whenItsPointIsInsideTheLandingPlatformArea
            (Size landingAreaSize, Size landingPlatformSize, Point landingPlatformPosition, Point rocketPosition)
        {
            var landingArea = new LandingArea(new SimpleCacheProvider())
                .WithName("Ireland Station")
                .WithSize(landingAreaSize)
                .WithLandingPlatformSize(landingPlatformSize)
                .AtPosition(landingPlatformPosition);

            var rocket = new Rocket(landingArea)
                .WithName("Falcon 9")
                .AtPosition(rocketPosition);

            
            var response = rocket.CheckPosition();


            response.Should().Be(ResponseType.OkForLanding);
        }

        
        
        public static TheoryData<Size, Size, Point, Point> OutOfLandingPlatformTestData = new()
        {
            //landingAreaSize               landingPlatformSize        landingPlatformPosition      rocketPosition
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(16, 5)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(5, 16)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(4, 5)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(16, 15)},
            
            {new Size(20, 20), new Size(5, 5), new Point(1, 1), new Point(5, 6)},
            {new Size(20, 20), new Size(5, 5), new Point(1, 1), new Point(6, 5)},
            {new Size(20, 20), new Size(5, 5), new Point(1, 1), new Point(0, 5)},
            {new Size(20, 20), new Size(5, 5), new Point(1, 1), new Point(0, 0)},
        };
        
        [Theory, MemberData(nameof(OutOfLandingPlatformTestData))]
        public void Rocket_ShouldGetOutOfPlatformResponse_whenItsPointIsOutOfLandingPlatformArea
            (Size landingAreaSize, Size landingPlatformSize, Point landingPlatformPosition, Point rocketPosition)
        {
            var landingArea = new LandingArea(new SimpleCacheProvider())
                .WithName("Ireland Station")
                .WithSize(landingAreaSize)
                .WithLandingPlatformSize(landingPlatformSize)
                .AtPosition(landingPlatformPosition);

            var rocket = new Rocket(landingArea)
                .WithName("Falcon 9")
                .AtPosition(rocketPosition);

            
            var response = rocket.CheckPosition();


            response.Should().Be(ResponseType.OutOfPlatform);
        }
        
        
        
        public static TheoryData<Size, Size, Point, Point> ClashTestData = new()
        {
            //landingAreaSize               landingPlatformSize        landingPlatformPosition      rocketPosition
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(5, 5)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(8, 10)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(14, 14)},
            
            {new Size(25, 25), new Size(15, 15), new Point(0, 1), new Point(0, 1)},
            {new Size(25, 25), new Size(15, 15), new Point(0, 1), new Point(5, 9)},
            {new Size(25, 25), new Size(15, 15), new Point(0, 1), new Point(14, 15)},
        };
        
        [Theory, MemberData(nameof(ClashTestData))]
        public void Rocket_ShouldGetClashResponse_whenItsPositionIsCheckedByAnotherRocketBefore
            (Size landingAreaSize, Size landingPlatformSize, Point landingPlatformPosition, Point rocketPosition)
        {
            var landingArea = new LandingArea(new SimpleCacheProvider())
                .WithName("Ireland Station")
                .WithSize(landingAreaSize)
                .WithLandingPlatformSize(landingPlatformSize)
                .AtPosition(landingPlatformPosition);

            var firstRocket = new Rocket(landingArea)
                .WithName("Falcon 9")
                .AtPosition(rocketPosition);

            var secondRocket = new Rocket(landingArea)
                .WithName("Falcon 5")
                .AtPosition(rocketPosition);

            
            
            var firstRocketResponse = secondRocket.CheckPosition();
            var secondRocketResponse = secondRocket.CheckPosition();


            secondRocketResponse.Should().Be(ResponseType.Clash);
        }

        
        
        public static TheoryData<Size, Size, Point, Point, Point> LandingOnSafeAreaTestData = new()
        {
            //landingAreaSize              landingPlatformSize       landingPlatformPosition  firstRocketPosition   secondRocketPosition
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(7, 7), new Point(6, 6)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(7, 7), new Point(6, 8)},
            {new Size(100, 100), new Size(10, 10), new Point(5, 5), new Point(7, 7), new Point(7, 8)},
            
            
            {new Size(100, 100), new Size(20, 20), new Point(1, 2), new Point(11, 5), new Point(10, 4)},
            {new Size(100, 100), new Size(20, 20), new Point(1, 2), new Point(11, 5), new Point(10, 5)},
            {new Size(100, 100), new Size(20, 20), new Point(1, 2), new Point(11, 5), new Point(10, 6)},
            
            {new Size(100, 100), new Size(20, 20), new Point(1, 2), new Point(11, 5), new Point(11, 4)},
            {new Size(100, 100), new Size(20, 20), new Point(1, 2), new Point(11, 5), new Point(11, 6)},
            
            {new Size(100, 100), new Size(20, 20), new Point(1, 2), new Point(11, 5), new Point(12, 4)},
            {new Size(100, 100), new Size(20, 20), new Point(1, 2), new Point(11, 5), new Point(12, 5)},
            {new Size(100, 100), new Size(20, 20), new Point(1, 2), new Point(11, 5), new Point(12, 6)},

        };
        
        /// <summary>
        /// Rocket should get 'Clash' response when its position is not at least one unit away from the position of the other rockets
        /// </summary>
        [Theory, MemberData(nameof(LandingOnSafeAreaTestData))]
        public void Rocket_ShouldGetClashResponse_whenItsPositionIsNotInSafeArea
            (Size landingAreaSize, Size landingPlatformSize, Point landingPlatformPosition, Point firstRocketPosition, Point secondRocketPosition)
        {
            var landingArea = new LandingArea(new SimpleCacheProvider())
                .WithName("Ireland Station")
                .WithSize(landingAreaSize)
                .WithLandingPlatformSize(landingPlatformSize)
                .AtPosition(landingPlatformPosition);

            var firstRocket = new Rocket(landingArea)
                .WithName("Falcon 9")
                .AtPosition(firstRocketPosition);

            var secondRocket = new Rocket(landingArea)
                .WithName("Falcon 5")
                .AtPosition(secondRocketPosition);

            
            
            var firstRocketResponse = firstRocket.CheckPosition();
            var secondRocketResponse = secondRocket.CheckPosition();


            secondRocketResponse.Should().Be(ResponseType.Clash);
        }
        
    }
}