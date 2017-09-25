using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AxExerciceGenerator.Test
{
    public static class DeploymentItem
    {
        /// <summary>
        /// Deploys the files from the deployment directory to the target directory        
        /// using the specified file filter after emptying the target directory.
        /// </summary>
        /// <param name="deploymentDir">The deployment dir.</param>
        /// <param name="targetDir">The target dir.</param>
        /// <param name="fileFilter">The file filter.</param>
        public static void DeployFiles(string deploymentDir, string targetDir, string fileFilter)
        {
            if (Directory.Exists(targetDir))
                Directory.Delete(targetDir, true);

            Directory.CreateDirectory(targetDir);

            foreach (string file in Directory.GetFiles(deploymentDir, fileFilter))
            {
                string newFile = Path.Combine(targetDir, Path.GetFileName(file));

                File.Copy(file, newFile);
                File.SetAttributes(newFile, FileAttributes.Normal);
            }
        }

        /// <summary>
        /// Deploys the config deployment directory to the target directory        
        /// after emptying the target directory.
        /// </summary>
        /// <param name="configDir">The config dir.</param>
        /// <param name="targetDir">The target dir.</param>
        public static void DeployConfigs(string configDir, string targetDir)
        {
            DeployFiles(configDir, targetDir, "*.xml");
        }
    }
}
