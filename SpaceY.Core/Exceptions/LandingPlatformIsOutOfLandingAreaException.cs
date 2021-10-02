using System;

namespace SpaceY.Core.Exceptions
{
    public class LandingPlatformIsOutOfLandingAreaException : Exception
    {
        public LandingPlatformIsOutOfLandingAreaException()
            :base() { }

        public LandingPlatformIsOutOfLandingAreaException(string message)
            : base(message) {}
        
        public LandingPlatformIsOutOfLandingAreaException(string message, Exception inner) 
            : base(message, inner) { }
    }
}