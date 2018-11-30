using System;
using System.IO;
using System.Linq;
using System.Text;

namespace BackupBigFilesData
{
    class Program
    {
        //get backup path + file for backup
        //get destination path + (subfolder)/nameof file.

        static void Main(string[] args)
        {

            var configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "backupConfig.txt");
            var logFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "backupLog.log");
            writeDefaultTextFile(configFile);
            //read from configure text path
            string BackUpConfig = configFile;
            string BackUpPath = File.ReadLines(configFile).Skip(1).First();
            int chunkSize = Convert.ToInt32(File.ReadLines(configFile).Skip(2).First());
            string OutputBackup = File.ReadLines(configFile).Skip(3).First();



            SplittingFiles.SplitFile(@"" + BackUpPath + "", chunkSize, OutputBackup);
            Console.WriteLine("Finish backing up files");

            // Console.WriteLine(Environment.SpecialFolder.ApplicationData);
            //  Console.Write(DateTime.Now.ToString("MM-dd-yyyy"));
            //CombineFiles.CombineMultipleFilesIntoSingleFile(@"C:\Temp\splitfiles\11-29-2018", "*.vmbkp", @"C:\Temp\joinfiles\testjoin.zip");
            Console.WriteLine("You may check your backup log in: " + logFile);
            Console.Read();
        }

        private static void writeDefaultTextFile(string configFile)
        {
          
            if (!File.Exists(configFile))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("#after this line, please put the following: #line2: BackupPath, #line3: chunksize in megabytes to let you know how large each file need to be split, #line4: outputbackupPath");
                sb.AppendLine("/home/yourprofile/backupfolder");
                sb.AppendLine("100");
                sb.AppendLine("/home/yourprofile/outputfolderForSplitFiles");
                File.WriteAllText(configFile, sb.ToString());

                Console.WriteLine("your config file is created in : " + configFile);
                Console.WriteLine("You may now exit this app and fix your config file before running this app again");
                Console.Read();
                Environment.Exit(0);

            }
            else
            {
                Console.WriteLine("Your config is now being use and located in: " + configFile);
                //Console.ReadLine();
            }

            
           

        }

    }
}
