using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rover.Models;

namespace Rover.Tests
{
    [TestClass]
    public class RoverMoveTests
    {
        RoverViewModel _roverModel;
        RoverMovement _roverMovement;

        public RoverMoveTests()
        {
            InitializeRover();
        }

        public void InitializeRover()
        {
            _roverModel = new RoverViewModel();

            _roverModel.PlateauSizeLatitude = 5;
            _roverModel.PlateauSizeLongitude = 5;

            _roverModel.Rover1 = new Models.Rover() {
                Position = new Position(),
            };

            _roverModel.Rover2 = new Models.Rover() {
                Position = new Position(),
            };
        }

        [DataTestMethod]
        [DataRow(2, 2)]
        [DataRow(1, 2)]
        [DataRow(5000, 3402)]
        public void CheckPlateauHasDimensions_positive(int lonSize, int latSize)
        {
            _roverModel.PlateauSizeLongitude = lonSize;
            _roverModel.PlateauSizeLatitude = latSize;

            _roverMovement = new RoverMovement(_roverModel);
            
            var valid = _roverMovement.MatrixHasDimensions();

            Assert.IsTrue(valid);
        }

        [DataTestMethod]
        [DataRow(0, 2)]
        [DataRow(-11, 2)]
        [DataRow(5000, -3402)]
        public void CheckPlateauHasDimensions_negative(int lonSize, int latSize)
        {
            _roverModel.PlateauSizeLongitude = lonSize;
            _roverModel.PlateauSizeLatitude = latSize;

            _roverMovement = new RoverMovement(_roverModel);

            bool valid = false;
            try
            {
                valid = _roverMovement.MatrixHasDimensions();

            }
            catch (System.Exception)
            {
            }

            Assert.IsFalse(valid);
        }

        [DataTestMethod]
        [DataRow(2, 3, 3, 4)]
        [DataRow(0, 3, 2, 4)]
        [DataRow(2, 1, 3, 3)]
        [DataRow(0, 3, 1, 1)]
        [DataRow(0, 0, 0, 0)]
        public void CheckRoverPositionTest_AllowedPositions(int lon1, int lat1, int lon2, int lat2)
        {
            _roverModel.PlateauSizeLongitude = 5;
            _roverModel.PlateauSizeLatitude = 5;

            _roverModel.Rover1.Position.Longitude = lon1;
            _roverModel.Rover1.Position.Latitude = lat1;

            _roverModel.Rover2.Position.Longitude = lon2;
            _roverModel.Rover2.Position.Latitude = lat2;

            _roverMovement = new RoverMovement(_roverModel);

            var valid = _roverMovement.CheckRoverPositionsAllowed();

            Assert.IsTrue(valid);
        }

        [DataTestMethod]
        [DataRow(5, 3, 3, 4)]
        //[DataRow(0, 3, 2, 5)]
        //[DataRow(2, 6, 3, 3)]
        //[DataRow(7, 3, 1, 1)]
        //[DataRow(0, 0, 5, 0)]
        public void CheckRoverPositionTest_NotAllowedPositions(int lon1, int lat1, int lon2, int lat2)
        {
            _roverModel.PlateauSizeLongitude = 5;
            _roverModel.PlateauSizeLatitude = 5;

            _roverModel.Rover1.Position.Longitude = lon1;
            _roverModel.Rover1.Position.Latitude = lat1;

            _roverModel.Rover2.Position.Longitude = lon2;
            _roverModel.Rover2.Position.Latitude = lat2;

            _roverMovement = new RoverMovement(_roverModel);

            bool valid = false;
            try
            {
                valid = _roverMovement.CheckRoverPositionsAllowed();
            }
            catch (System.Exception)
            {
            }

            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ProcessInstruction_ChangesHeading_N()
        {
            _roverModel.PlateauSizeLongitude = 5;
            _roverModel.PlateauSizeLatitude = 5;

            _roverModel.Rover1.Position.Longitude = 1;
            _roverModel.Rover1.Position.Latitude = 1;
            _roverModel.Rover1.Position.Heading = Heading.E;

            _roverModel.Rover1.Instructions = "N";

            _roverMovement = new RoverMovement(_roverModel);
            
            _roverMovement.Navigate(_roverModel.Rover1);

            Assert.IsTrue(_roverModel.Rover1.Position.Heading == Heading.N);
        }

        [TestMethod]
        public void ProcessInstruction_ChangesPosition_NM()
        {
            _roverModel.PlateauSizeLongitude = 5;
            _roverModel.PlateauSizeLatitude = 5;

            _roverModel.Rover1.Position.Longitude = 1;
            _roverModel.Rover1.Position.Latitude = 1;
            _roverModel.Rover1.Position.Heading = Heading.E;

            _roverModel.Rover1.Instructions = "NM";

            _roverMovement = new RoverMovement(_roverModel);

            _roverMovement.Navigate(_roverModel.Rover1);

            Assert.IsTrue(_roverModel.Rover1.Position.Heading == Heading.N);
            Assert.AreEqual(1, _roverModel.Rover1.Position.Longitude);
            Assert.AreEqual(2, _roverModel.Rover1.Position.Latitude);
        }

        [TestMethod]
        public void ProcessInstruction_ChangesPosition_NMEMMSMWMMM()
        {
            _roverModel.PlateauSizeLongitude = 5;
            _roverModel.PlateauSizeLatitude = 5;

            _roverModel.Rover1.Position.Longitude = 1;
            _roverModel.Rover1.Position.Latitude = 1;
            _roverModel.Rover1.Position.Heading = Heading.E;

            _roverModel.Rover1.Instructions = "NMEMMSMWMM";

            _roverMovement = new RoverMovement(_roverModel);

            _roverMovement.Navigate(_roverModel.Rover1);

            Assert.IsTrue(_roverModel.Rover1.Position.Heading == Heading.W);
            Assert.AreEqual(1, _roverModel.Rover1.Position.Longitude);
            Assert.AreEqual(1, _roverModel.Rover1.Position.Latitude);
        }

        [TestMethod]
        public void ProcessInstruction_ChangesPosition_11E()
        {
            _roverModel.PlateauSizeLongitude = 5;
            _roverModel.PlateauSizeLatitude = 5;

            _roverModel.Rover1.Position.Longitude = 1;
            _roverModel.Rover1.Position.Latitude = 1;
            _roverModel.Rover1.Position.Heading = Heading.E;

            _roverModel.Rover1.Instructions = "M";

            _roverMovement = new RoverMovement(_roverModel);

            _roverMovement.Navigate(_roverModel.Rover1);

            Assert.IsTrue(_roverModel.Rover1.Position.Heading == Heading.E);
            Assert.AreEqual(2, _roverModel.Rover1.Position.Longitude);
            Assert.AreEqual(1, _roverModel.Rover1.Position.Latitude);
        }
    }
}
