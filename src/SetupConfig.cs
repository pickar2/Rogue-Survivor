using System;
using System.IO;

namespace djack.RogueSurvivor
{
    public static class SetupConfig
    {
        public const string GAME_VERSION = "alpha 10.1";

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
