using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace AxModel.Message
{
    public class PatientMessage:MessageBase
    {
        public ListePatientDataGrid DataPatient { get; set; }
    }
}
