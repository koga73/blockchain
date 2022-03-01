namespace Q.Data.Common
{
    public class Paths
    {
        const string APP_DIR = "Q";
        const string KEYS_DIR = "keys";
        const string LOGS_DIR = "logs";

        public static string ApplicationPath {
            get {
                string envPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string dirPath = Path.Join(envPath, APP_DIR);
                TryCreateDir(dirPath);
                return dirPath;
            }
        }

        public static string KeysPath
        {
            get
            {
                string appPath = ApplicationPath;
                string dirPath = Path.Join(appPath, KEYS_DIR);
                TryCreateDir(dirPath);
                return dirPath;
            }
        }

        public static string LogsPath
        {
            get
            {
                string appPath = ApplicationPath;
                string dirPath = Path.Join(appPath, LOGS_DIR);
                TryCreateDir(dirPath);
                return dirPath;
            }
        }

        private static void TryCreateDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                try
                {
                    Directory.CreateDirectory(dirPath);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
    }
}
