using System.Drawing;
using SpaceY.Core.Enums;

namespace SpaceY.Core.Models
{
    public class Rocket
    {
        #region Fields

        private readonly ILandingArea _landingArea;

        #endregion


        #region Properties

        public string Name { get; private set; }

        public Point Position { get; private set; }

        #endregion


        #region Construction

        public Rocket(ILandingArea landingArea)
        {
            _landingArea = landingArea;
        }



        #region Fluent Interface Design Pattern

        public Rocket WithName(string name)
        {
            Name = name;

            return this;
        }

        public Rocket AtPosition(Point point)
        {
            Position = point;
            return this;
        }

        #endregion


        #endregion


        #region Methods

        public ResponseType CheckPosition()
        {
            return _landingArea.CheckRocketIsOnGoodWay(Name, Position);
        }
        
        #endregion
    }
}