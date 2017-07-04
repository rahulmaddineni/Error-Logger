using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace CommonCode
{
    public class Logger
    {
        private static string BASE_PATH = AppDomain.CurrentDomain.BaseDirectory;
        private string DIRECTORY_PATH = BASE_PATH+"Logs//";
        private const string FILE_NAME = "SystemLogs.txt";
        BlockingCollection<String> bq;

        // Store Logs into a file
        public void LogSystemError(string log)
        {
            // Create the directory, if one does not exsist
            if (!System.IO.Directory.Exists(DIRECTORY_PATH))
            {
                System.IO.Directory.CreateDirectory(DIRECTORY_PATH);
            }
            
            string pathString = System.IO.Path.Combine(DIRECTORY_PATH, FILE_NAME);
            
            // Create the file, if one does not exsist
            if (!System.IO.File.Exists(pathString))
            {
                System.IO.File.Create(pathString);
            }
            
            System.IO.StreamWriter sr = null;
            try
            {
                sr = new System.IO.StreamWriter(pathString,true);
                sr.WriteLine(log);
            }
            catch
            {
                sr.Close();
            }
            sr.Close();
        }

        public void LogSystemError(string user, string ipAddress, Exception exc)
        {
            // Create the directory, if one does not exsist
            if (!System.IO.Directory.Exists(DIRECTORY_PATH))
            {
                System.IO.Directory.CreateDirectory(DIRECTORY_PATH);
            }

            string pathString = System.IO.Path.Combine(DIRECTORY_PATH, FILE_NAME);

            // Create the file, if one does not exsist
            if (!System.IO.File.Exists(pathString))
            {
                System.IO.File.Create(pathString);
            }

            System.IO.StreamWriter sr = null;
            try
            {
                sr = new System.IO.StreamWriter(pathString, true);
                sr.WriteLine("<Log>");
                sr.WriteLine("  <User>" + user + "</User>");
                //sr.WriteLine("  <IPAddress>" + ipAddress + "<IPAddress>");
                sr.WriteLine("  <Time>" + DateTime.Now + "</Time>");
                sr.WriteLine("  <Type>" + exc.GetType() + "</Type>");
                sr.WriteLine("  <Message>" + exc.Message + "</Message>");
                sr.WriteLine("  <Inner>" + exc.InnerException + "</Inner>");
                sr.WriteLine("  <Exception>" + exc.ToString() + "</Exception>");
                sr.WriteLine("</Log>");
                sr.WriteLine(" ");
            }
            catch
            {
                sr.Close();
            }
            sr.Close();
        }
    }
}
