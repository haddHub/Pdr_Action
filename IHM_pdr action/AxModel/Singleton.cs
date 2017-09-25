using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class Singleton
    {
        private static Singleton instance = null;

        private Singleton() {  }

        public ListePatientDataGrid Patient { get; set; }
        public Patient PatientSingleton { get; set; }
        private static bool robotError = false;
        public static ConfigProgramme ConfigProg { get; set; }
        public Admin Admin { get; set; }
        public static bool IsCalibre { get; set; }
        public static bool CriticalError { get; set; }
        public static PdrActionType TypeAction { get; set; }
        public static PdrOptions Options { get; set; }
        public static int Viscosite { get; set; }
        public static PdrZoom Zoom { get; set; }
        public static int RepetitionGauche { get; set; }
        public static int RepetitionDroit { get; set; }
        public static List<int> CiblesGauche { get; set; }
        public static List<int> CiblesDroite { get; set; }
        public static int InitTime { get; set; }
        public static int GeneralTime { get; set; }
        public static int OutsideTime { get; set; }

        public static void identification()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
        }

        public static void identificationAdmin()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
        }

        public static void logOffPatient()
        {
            instance.Patient = null;
            instance.PatientSingleton = null;

        }

        public static void logOff()
        {
            instance = null;

        }

        public static bool GetRobotError()
        {
            return robotError;
        }

        public static void ChangeErrorStatu(bool newStatu)
        {
            robotError = newStatu;
        }

        public static Singleton getInstance()
        {
            return instance;
        }
    }
}
