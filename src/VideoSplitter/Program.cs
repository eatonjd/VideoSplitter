using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;



namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var files = Directory
                 .GetFiles(@"E:\HomeMovies", "*", SearchOption.TopDirectoryOnly)
                 .Select(f => Path.GetFileName(f));


            foreach (string f in files)
            {
                string ext = f.Substring(f.Length - 3);
                if (ext != "avi")
                {
                    ExecuteCommand(f);
                    for (int i = 1;i < 15;i++)
                    {
                        string root = f.Substring(0, f.Length - 4);
                        string pre = "";
                        if (i < 10) pre = "_00"; else pre = "_0";
                        MoveFiles(root + pre + i.ToString() + "." + ext, @"E:\HomeMovies-Split");
                        MoveFiles(f, @"E:\HomeMovies-Source");
                    }
                }
            }

           
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void MoveFiles(string file, string toDir)
        {
            try
            {
                var from = System.IO.Path.Combine(@"E:\HomeMovies", file);
                var to = System.IO.Path.Combine(toDir, file);

                File.Move(from, to); // Try to move
                Console.WriteLine(file + " Moved"); // Success
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex); // Write error
            }
        }

        static void ExecuteCommand(string file)
        {
            string command;
            
            command = @"-split 300 " + file;

            Console.WriteLine(command);

            var processInfo = new ProcessStartInfo("mp4box", command);
            processInfo.WorkingDirectory = @"E:\HomeMovies";
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("output>>" + e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("error>>" + e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            Console.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Dispose();
        }

    }


   
}
