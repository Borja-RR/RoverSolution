using Rover.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rover
{
    public class RoverMovement
    {
        RoverViewModel _roverModel;

        public RoverMovement(RoverViewModel roverModel)
        {
            _roverModel = roverModel;
        }

        public bool ValidatePlateau()
        {
            if (MatrixHasDimensions() && CheckRoverPositionsAllowed())
                return true;

            return false;
        }

        public bool MatrixHasDimensions()
        {
            if (_roverModel.PlateauSizeLongitude < 1)
            {
                throw new Exception("The plateau needs a longitude size bigger than 0");
            }

            if (_roverModel.PlateauSizeLatitude < 1)
            {
                throw new Exception("The plateau needs a latitude size bigger than 0");
            }

            return true;
        }

        private bool CheckRoverPositionAllowed(Models.Rover rover)
        {
            if (_roverModel.PlateauSizeLongitude <= rover.Position.Longitude)
            {
                throw new Exception(string.Format("Rover Longitude ({0}) is out of plateau (max {1})"
                    , rover.Position.Longitude.ToString()
                    , _roverModel.PlateauSizeLongitude - 1));
            }
            if(_roverModel.PlateauSizeLatitude <= rover.Position.Latitude)
            {
                throw new Exception(string.Format("Rover Latitude ({0}) is out of plateau (max {1})"
                    , rover.Position.Longitude.ToString()
                    , _roverModel.PlateauSizeLatitude - 1));
            }

            return true;
        }

        public bool CheckRoverPositionsAllowed()
        {
            try
            {
                CheckRoverPositionAllowed(_roverModel.Rover1);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Rover 1 position out of the plateau: ", ex.Message));
            }

            try
            {
                CheckRoverPositionAllowed(_roverModel.Rover2);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Rover 2 position out of the plateau: ", ex.Message));
            }

            return true;
        }

        public void Navigate(Models.Rover rover)
        {
            Position finalPosition = new Position();

            char[] instructions = rover.Instructions.ToCharArray();

            foreach (var instruction in instructions)
            {
                ProcessInstruction(rover, Char.ToUpper(instruction));
            }
        }

        private void ProcessInstruction(Models.Rover rover, char instruction)
        {
            Heading heading;

            heading = ConvertToHeading(instruction);

            switch (heading)
            {
                case Heading.M:
                    Move(rover);
                    if (!CheckRoverPositionAllowed(rover))
                    {
                        throw new Exception("The Rover has moved out of the plateau");
                    }
                    break;
                default:
                    rover.Position.Heading = heading;
                    break;
            }
        }

        private Heading ConvertToHeading(char instruction)
        {
            switch (instruction)
            {
                case 'N':
                    return Heading.N;
                case 'E':
                    return Heading.E;
                case 'S':
                    return Heading.S;
                case 'W':
                    return Heading.W;
                case 'M':
                    return Heading.M;
                default:
                    throw new Exception(string.Format("'{0}' is not a valid instruction, it should be N, E, S, W or M", instruction));
            }
        }

        private void Move(Models.Rover rover)
        {
            switch (rover.Position.Heading)
            {
                case Heading.N:
                    rover.Position.Latitude++;
                    break;
                case Heading.E:
                    rover.Position.Longitude++;
                    break;
                case Heading.S:
                    rover.Position.Latitude--;
                    break;
                case Heading.W:
                    rover.Position.Longitude--;
                    break;
                default:
                    throw new Exception("The current heading does not exist");
            }
        }
    }
}
