using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ConfigProgramme
    {
        public double VersionReaplan { get; set; }
        public bool IsDebug { get; set; }
        public string DateInstallation { get; set; }
        public bool BonneFermeture { get; set; }
        public string Culture { get; set; }
        public ConfigProgramme(double version,bool debug, string dateInstallation, bool fermeture, string culture)
        {
            VersionReaplan = version;
            IsDebug = debug;
            DateInstallation = dateInstallation;
            BonneFermeture = fermeture;
            Culture = culture;
        }
    }
}
