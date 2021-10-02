using System;
using System.Drawing;
using FluentAssertions;
using SpaceY.Core.CacheProvider;
using SpaceY.Core.Exceptions;
using SpaceY.Core.Models;
using Xunit;

namespace SpaceY.Core.Tests.Models
{
    public class LandingAreaTests
    {
        [Fact]
        public void DefaultSizeOfLandingArea_MustBeEqualToExpected_WhenInstantiationHappened()
        {
            var expectedSize = new Size(100, 100);

            var landingAre = new LandingArea(new SimpleCacheProvider());

            landingAre.Size.Should().Be(expectedSize);
        }
        
        [Fact]
        public void DefaultSizeOfLandingPlatform_MustBeEqualToExpected_WhenInstantiationHappened()
        {
            var expectedSize = new Size(10, 10);

            var landingAre = new LandingArea(new SimpleCacheProvider());

            landingAre.LandingPlatformSize.Should().Be(expectedSize);
        }
        
        [Fact]
        public void DefaultPositionOfLandingPlatform_MustBeEqualToExpected_WhenInstantiationHappened()
        {
            var expectedPoint = new Point(5, 5);

            var landingAre = new LandingArea(new SimpleCacheProvider());

            landingAre.LandingPlatformPosition.Should().Be(expectedPoint);
        }
        
        
        [Fact]
        public void NameOfLandingArea_MustBeChangeAsExpected_WhenInstantiationHappenedInFluentWay()
        {
            var expectedName = "Ireland Station";
            
            var landingArea = new LandingArea(new SimpleCacheProvider())
                .WithName(expectedName);

            landingArea.Name.Should().Be(expectedName);
        }

        [Fact]
        public void SizeOfLandingArea_MustBeChangeAsExpected_WhenInstantiationHappenedInFluentWay()
        {
            var expectedSize = new Size(100,500);
            
            var landingArea = new LandingArea(new SimpleCacheProvider())
                .WithSize(expectedSize);

            landingArea.Size.Should().Be(expectedSize);
        }

        [Fact]
        public void SizeOfLandingPlatform_MustBeChangeAsExpected_WhenInstantiationHappenedInFluentWay()
        {
            var expectedSize = new Size(25,25);
            
            var landingArea = new LandingArea(new SimpleCacheProvider())
                .WithLandingPlatformSize(expectedSize);

            landingArea.LandingPlatformSize.Should().BeEquivalentTo(expectedSize);
        }
        
        
        [Fact]
        public void PositionOfLandingPlatform_MustBeChangeAsExpected_WhenInstantiationHappenedInFluentWay()
        {
            var expectedPoint = new Point(4,4);
            
            var landingArea = new LandingArea(new SimpleCacheProvider())
                .AtPosition(expectedPoint);

            landingArea.LandingPlatformPosition.Should().Be(expectedPoint);
        }


        
        public static TheoryData<Size, Size, Point> OutOfLandingAreaTestData = new()
        {
            //landingAreaSize               landingPlatformSize        landingPlatformPosition
            {new Size(100, 100), new Size(99, 99), new Point(1, 2)},
            {new Size(100, 100), new Size(99, 99), new Point(2, 1)},
            {new Size(100, 100), new Size(2, 1), new Point(99, 99)},
            {new Size(100, 100), new Size(1, 2), new Point(99, 99)},
            {new Size(100, 100), new Size(2, 1), new Point(-1, 5)},
            {new Size(100, 100), new Size(2, 1), new Point(2, -8)},
        };
        
        [Theory, MemberData(nameof(OutOfLandingAreaTestData))]
        public void LandingArea_ShouldThrowException_WhenLandingPlatformIsOutOfLandingArea
            (Size landingAreaSize, Size landingPlatformSize, Point landingPlatformPosition)
        {
            Action act = ()=> new LandingArea(new SimpleCacheProvider())
                .WithSize(landingAreaSize)
                .WithLandingPlatformSize(landingPlatformSize)
                .AtPosition(landingPlatformPosition);

            act.Should().ThrowExactly<LandingPlatformIsOutOfLandingAreaException>();
        }
    }
}