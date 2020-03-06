using System;
using System.IO;

namespace RogueSurvivor
{
    public static class SetupConfig
    {
        public const string GAME_VERSION = "10.2";

        public static string DirPath
        {
            get
            {
                return Environment.CurrentDirectory + @"\Config\";
            }
        }

        public static void CreateDir()
        {
            if (!Directory.Exists(DirPath))
                Directory.CreateDirectory(DirPath);
        }
    }
}
