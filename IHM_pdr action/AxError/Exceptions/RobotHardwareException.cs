using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
namespace AxError.Exceptions
{
    public class RobotHardwareException : RobotException
    {
        public RobotHardwareException(byte nodeId, FrameHeaders adresse, ErrorEmcyCodes errorCode, string message)
            : base (nodeId,adresse,errorCode,message)
        {

        }
    }
}
