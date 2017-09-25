using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxReaLabToUnity;
using AxReaLabToUnity.Models;

namespace AxModel
{
    public interface IUnityGameController
    {
        event NewTrajectoryHandler onNewTrajectory;
        event NewLevelStartedHandler onLevelStarted;
        event NewCheckpointReachedHandler onCheckpointReached;
        event NewLevelStoppedHandler onLevelStopped;
        event NewValueHandler onNewValue;

        void StartGame(string gameName, int diff, int zoom, int lvl);
        void StopGame();
        void PauseGame();
        void NextLevel();
        void SetPositions(double x, double y);
        void SetForces(double x, double y);
        void ChangeConfig(int diff, int zoom, int lvl);
        void SetValue(string key, int value);
        void SetValues(string key, int[] value);
        object GetValue(string key);
    }
}
