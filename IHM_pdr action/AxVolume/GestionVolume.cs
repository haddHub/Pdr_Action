using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AxModel;
using System.Windows.Threading;

namespace AxVolume
{
    /// <summary>
    /// Permet de gérer facilement l'ensemble de classe du module AxVolume.
    /// </summary>
    public class GestionVolume
    {
        private MMDeviceEnumerator devEnum;
        private MMDevice defaultDevice;
        private IMessageBoxService messageService;
        /// <summary>
        /// Le timer permet de ne pas avoir plusieur messagebox à l'écran
        /// et de pouvoir changer le volume sans être interompu par la messagebox.
        /// </summary>
        private DispatcherTimer timer;

        public GestionVolume()
        {
            devEnum = new MMDeviceEnumerator();
            defaultDevice = devEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);

            messageService = new MessageBoxService();

            //Enregistrement à l'event de changement de volume.
            defaultDevice.AudioEndpointVolume.OnVolumeNotification += new AudioEndpointVolumeNotificationDelegate(AudioEndpointVolume_OnVolumeNotification);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(_mainLoop_Tick);

            GererVolume();
        }

        /// <summary>
        /// Méthode utilisée quand le volume de Windows est modifié.
        /// </summary>
        /// <param name="data">Information sur le volume.</param>
        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            timer.Start();
        }

        private void _mainLoop_Tick(object sender, EventArgs e)
        {
            GererVolume();
            timer.Stop();
        }

        /// <summary>
        /// Aplique le traitement du Volume.
        /// </summary>
        private void GererVolume()
        {
            if (defaultDevice.AudioEndpointVolume.Mute || defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar < 0.5)
            {
                if (messageService.ShowYesNo(AxLanguage.Languages.REAplan_Volume_Faible, CustomDialogIcons.Warning) == CustomDialogResults.Yes)
                    AugementerVolume(1F);
            }
        }
       
        /// <summary>
        /// Permet d'augementer le volume de Windows.
        /// </summary>
        /// <param name="volume">Niveau de volume désiré.</param>
        private void AugementerVolume(float volume)
        {
            defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
            defaultDevice.AudioEndpointVolume.Mute = false;
        }
    }
}
