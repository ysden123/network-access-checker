using System.Net;

namespace network_access_checker
{
    internal class Program
    {
        static void Main()
        {
            var logDir = "log";
            var di = new DirectoryInfo(logDir);
            if (!di.Exists)
            {
                Directory.CreateDirectory(logDir);
            }

            using TextWriter log = new StreamWriter(Path.Combine(logDir, "log.txt"), true);

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = "";
            if (fvi != null && fvi.FileVersion != null)
            {
                version = fvi.FileVersion;
            }

            Log($"network-access-checker {version}", log);

            var interval = 60; // In minutes
            var client = new HttpClient();
            while (true)
            {
                var timeMessage = $"{DateTime.Now}: ";

                try
                {
                    using (var response = client.Send(new HttpRequestMessage(HttpMethod.Get, "http://google.com")))
                    {
                        if (response?.StatusCode == HttpStatusCode.OK)
                        {
                            Log(timeMessage + "Network is available!", log);
                        }
                        else
                        {
                            Log(timeMessage + "ERROR!", log);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(timeMessage + "Internet connection is unavailable. " + ex.Message, log);
                }

                Thread.Sleep(interval * 60 * 1000);
            }
        }

        private static void Log(string message, TextWriter writer)
        {
            Console.WriteLine(message);
            writer.WriteLine(message);
            writer.Flush();
        }
    }
}
