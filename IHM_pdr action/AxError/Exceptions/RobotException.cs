using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;

namespace AxError.Exceptions
{
    public class RobotException : Exception
    {
        public byte NodeId { get; set; }
        public FrameHeaders Adresse { get; set; }
        public ErrorEmcyCodes ErrorCode { get; set; }

        public RobotException(byte nodeId,FrameHeaders adresse, ErrorEmcyCodes errorCode, string message)
            : base(message)
        {
            NodeId = nodeId;
            Adresse = adresse;
            ErrorCode = errorCode;
        }
    }
}
