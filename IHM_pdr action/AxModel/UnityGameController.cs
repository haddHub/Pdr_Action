using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxReaLabToUnity;
using AxReaLabToUnity.Models;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace AxModel
{
    public class UnityGameController : IUnityGameController
    {
        #region Field
        private string pathToGame = string.Empty;

        private Singleton singleton = Singleton.getInstance();

        /// <summary>
        /// Difficulté de l'exercice.
        /// </summary>
        private int difficulty;

        private int zoom;
        /// <summary>
        /// Level à charger
        /// </summary>
        private int level;

        /// <summary>
        /// Instance du serveur de communication
        /// </summary>
        private ReaLabServer server;

        /// <summary>
        /// Istance de l'exe du jeu
        /// </summary>
        private Process gameProcess;
        #endregion

        #region event
        public event NewTrajectoryHandler onNewTrajectory;
        public event NewLevelStartedHandler onLevelStarted;
        public event NewCheckpointReachedHandler onCheckpointReached;
        public event NewLevelStoppedHandler onLevelStopped;
        public event NewValueHandler onNewValue;
        #endregion

        #region Methodes
        /// <summary>
        /// Démarre le serveur de communication et lance le jeu unity
        /// </summary>
        public void StartGame(string gameName, int diff, int zoom, int lvl)
        {
            this.pathToGame = "Jeux\\" + gameName + ".exe";
            this.difficulty = diff;
            this.zoom = zoom;
            this.level = lvl;

            if (this.server != null || this.gameProcess != null)
            {
                StopGame();
            }

            // Démarrage du serveur et envoie de la config
            this.server = new ReaLabServer("127.0.0.1", 8888, 8889);
            this.server.Start();
            var config = new ReaLabConfiguration(singleton.Patient.Nom, singleton.Patient.Prenom, singleton.PatientSingleton.DateNaiss, 'm' ,this.difficulty, this.zoom, this.level, false);
            this.server.SetReaLabConfiguration(config);

            this.server.onNewTrajectory += new NewTrajectoryHandler(server_onNewTrajectory);
            this.server.onLevelStarted += new NewLevelStartedHandler(server_onLevelStarted);
            this.server.onCheckpointReached += new NewCheckpointReachedHandler(server_onCheckpointReached);
            this.server.onLevelStopped += new NewLevelStoppedHandler(server_onLevelStopped);
            this.server.onNewValue += new NewValueHandler(server_onNewValue);

            // Lancement du jeu
            // Trouve le handler de la fenetre mainP
            var handler = this.GetHandle("ReaPlan patient");
            // Creation du process du jeu
            this.gameProcess = new Process();
            this.gameProcess.StartInfo.FileName = this.pathToGame;
            this.gameProcess.StartInfo.Arguments = "-parentHWND " + handler.ToInt32() + " " + Environment.CommandLine;
            this.gameProcess.StartInfo.UseShellExecute = true;
            this.gameProcess.StartInfo.CreateNoWindow = true;
            this.gameProcess.Start();
            this.gameProcess.WaitForInputIdle();

            EnumChildWindows(handler, this.WindowEnum, IntPtr.Zero);
        }

        /// <summary>
        /// Arret le serveur de communication et coupe le jeu unity
        /// </summary>
        public void StopGame()
        {
            if (this.server != null)
            {
                this.server.StopGame();
                this.server.onNewTrajectory -= server_onNewTrajectory;
                this.server.onLevelStarted -= server_onLevelStarted;
                this.server.onCheckpointReached -= server_onCheckpointReached;
                this.server.onLevelStopped -= server_onLevelStopped;
                this.server.onNewValue -= server_onNewValue;

                Thread.Sleep(10);
                this.server.Stop();
                this.server = null;

                //if (this.gameProcess != null && this.gameProcess.HasExited == false)
                //{
                //    this.gameProcess.Kill();
                //}

                this.gameProcess = null;
            }
        }

        public void PauseGame()
        {
            if (this.server != null)
            {
                this.server.PauseGame();
            }
        }

        // Transmet les position au jeu
        public void SetPositions(double x, double y)
        {
            try
            {
                if (this.server != null)
                {
                    this.server.SetPositions(x, y);
                }
            }
            catch (Exception)
            {

            }
        }

        public void SetForces(double x, double y)
        {
            try
            {
                if (this.server != null)
                {
                    this.server.SetForces(x, y);
                }
            }
            catch (Exception)
            {

            }
        }

        public void NextLevel()
        {
            try
            {
                if (this.server != null)
                {
                    this.server.NextLevel();
                }
            }
            catch (Exception)
            {

            }
        }

        public void ChangeConfig(int diff, int zoom, int lvl)
        {
            try
            {
                if (this.server != null)
                {

                    this.difficulty = diff;
                    this.zoom = zoom;
                    this.level = lvl;

                    var config = new ReaLabConfiguration(singleton.Patient.Nom, singleton.Patient.Prenom, singleton.PatientSingleton.DateNaiss, 'm' ,this.difficulty, this.zoom, this.level, false);
                    this.server.SetReaLabConfiguration(config);
                }
            }
            catch (Exception)
            {

            }
        }

        public void SetValue(string key, int value)
        {
            try
            {
                if (this.server != null)
                {
                    this.server.SetValue(key, value);
                }
            }
            catch (Exception)
            {

            }
        }

        public void SetValues(string key, int[] value)
        {
            try
            {
                if (this.server != null)
                {
                    this.server.SetValues(key, value);
                }
            }
            catch (Exception)
            {

            }
        }

        public object GetValue(string key)
        {
            return this.server.GetValue(key);
        }

        private void server_onLevelStopped(object obj, EventArgs messageArgs)
        {
            if (this.onLevelStopped != null)
            {
                this.onLevelStopped(obj, messageArgs);
            }
        }

        private void server_onCheckpointReached(object obj, MessageEvent messageArgs)
        {
            if (this.onCheckpointReached != null)
            {
                this.onCheckpointReached(obj, messageArgs);
            }
        }

        private void server_onLevelStarted(object obj, EventArgs messageArgs)
        {
            if (this.onLevelStarted != null)
            {
                this.onLevelStarted(obj, messageArgs);
            }
        }

        private void server_onNewTrajectory(object obj, PathEvent trajectoryArgs)
        {
            if (this.onNewTrajectory != null)
            {
                this.onNewTrajectory(obj, trajectoryArgs);
            }
        }
        
        private void server_onNewValue(object obj, ValueEvent valueArgs)
        {
            if (this.onNewValue != null)
            {
                this.onNewValue(obj, valueArgs);
            }
        }

        private IntPtr GetHandle(string window)
        {
            var windows = Application.Current.Windows;
            var handle = IntPtr.Zero;

            foreach (Window win in windows)
            {
                if (win.Title == window)
                {
                    var interop = new WindowInteropHelper(win);
                    handle = interop.Handle;
                }
            }

            return handle;
        }
        #endregion

        #region Import
        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, uint lParam);

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            SendMessage(hwnd, 0x0006, 1, 0);
            return 0;
        }
        #endregion
    }
}
