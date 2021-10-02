using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using SpaceY.Core.CacheProvider;
using SpaceY.Core.Enums;
using Xunit;

namespace SpaceY.Core.Tests.CacheProvider
{
    public class SimpleCacheProviderTests
    {
        [Fact]
        public void AddOrUpdateMethod_ShouldAddData_WhenInvoked()
        {
            var cacheProvider = new SimpleCacheProvider();
            var point = new Point(10, 25);
            var rocketName = "Falcon 9";
            var response = ResponseType.OkForLanding;

            cacheProvider.AddOrUpdate(rocketName, point, response);
            var item = cacheProvider.Get(point);

            item.RocketName.Should().Be(rocketName);
            item.ResponseType.Should().Be(response);
            cacheProvider.All.Count.Should().Be(1);
        }
        
        [Fact]
        public void AddOrUpdateMethod_ShouldUpdateData_WhenAnotherCacheItemForSpecificPointExist()
        {
            var cacheProvider = new SimpleCacheProvider();
            var point = new Point(10, 25);
            var rocketName = "Falcon 9";
            var response = ResponseType.OkForLanding;
            var lastRocketName = "Falcon 7";

            cacheProvider.AddOrUpdate(rocketName, point, response);
            cacheProvider.AddOrUpdate(rocketName, point, response);
            cacheProvider.AddOrUpdate(rocketName, point, ResponseType.OutOfPlatform);
            cacheProvider.AddOrUpdate(lastRocketName, point, response);
            
            var count = cacheProvider.All.Count(c => c.Point == point);
            var name = cacheProvider.Get(point).RocketName;

            name.Should().Be(lastRocketName);
            count.Should().Be(1);
        }

        
        [Fact]
        public void GetMethod_ShouldThrowInvalidOperationException_WhenPointDoesNotExist()
        {
            var cacheProvider = new SimpleCacheProvider();
            var rocketName = "Falcon 9";
            var response = ResponseType.OkForLanding;
            
            cacheProvider.AddOrUpdate(rocketName, new Point(10, 225), response);
            Action act = ()=> cacheProvider.Get(new Point(10, 5));

            act.Should().ThrowExactly<InvalidOperationException>();
        }

        
        [Fact]
        public void AnyMethod_ShouldReturnTrueForSpecificPoint_WhenAddedBefore()
        {
            var cacheProvider = new SimpleCacheProvider();
            var point = new Point(10, 25);
            var rocketName = "Falcon 9";
            var response = ResponseType.OkForLanding;

            cacheProvider.AddOrUpdate(rocketName, point, response);
            var any = cacheProvider.Any(point);
            
            any.Should().Be(true);
        }
        
        
        [Fact]
        public void RemoveMethod_ShouldRemoveSpecificPoint_WhenInvoked()
        {
            var cacheProvider = new SimpleCacheProvider();
            var rocketName = "Falcon 9";
            var response = ResponseType.OkForLanding;
            var point = new Point(10, 25);
            
            cacheProvider.AddOrUpdate(rocketName, point, response);
            cacheProvider.Remove(point);
            var any = cacheProvider.Any(point);
            
            any.Should().Be(false);
        }

        [Fact]
        public void ClearMethod_ShouldClearCache_WhenInvoked()
        {
            var cacheProvider = new SimpleCacheProvider();
            var rocketName = "Falcon 9";
            var response = ResponseType.OkForLanding;
            var point = new Point(10, 25);
            
            cacheProvider.AddOrUpdate(rocketName, point, response);
            cacheProvider.Clear();
            var any = cacheProvider.Any(point);
            
            any.Should().Be(false);
            cacheProvider.All.Count.Should().Be(0);
        }

    }
}