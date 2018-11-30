using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace BackupBigFilesData
{
    class SplittingFiles
    {
        public static void SplitFile(string inputFolder, int chunkSize, string OutPutPath)
        {
            //chunksize in megabytes
            var fileStatusCopyAt = string.Empty;
            var inputPositionLengh = string.Empty; ;
            var backupLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "backupLog.log");
            //get all files in folder
            try
            {
                List<string> filesInFolder = GetallFilesinFolder(inputFolder);
                string DateFormatSubPath = DateTime.Now.ToString("MM-dd-yyyy_T_hhmmss");
                int fileNum = 0;
                foreach (var inputFile in filesInFolder)
                {

                    fileStatusCopyAt = inputFile;


                    const int BUFFER_SIZE = 20 * 1024;
                    byte[] buffer = new byte[BUFFER_SIZE];

                    var backupDir = Path.Combine(OutPutPath, DateFormatSubPath, "file" + fileNum);
                    //var backupDir = OutPutPath + "/" + DateTime.Now.ToString("MM-dd-yyyy");
                    if (!Directory.Exists(backupDir))
                    {
                        Directory.CreateDirectory(backupDir);
                    }

                    using (Stream input = File.OpenRead(inputFile))
                    {
                        int index = 0;
                        while (input.Position < input.Length)
                        {
                            inputPositionLengh = input.Position.ToString();


                            var fileCreatedForBackup = Path.Combine(backupDir, "file" + fileNum + "-part_" + index + "-OrgExt_" + Path.GetExtension(inputFile) + "_" + Guid.NewGuid() + ".vmbkp");

                            // using (Stream output = File.Create(fileCreatedForBackup))
                            using (FileStream output = File.Create(fileCreatedForBackup))
                            {
                                float remaining = chunkSize * 1024f * 1024f;
                                float totalSize = remaining;
                                int bytesRead;
                                Console.WriteLine("\rWriting file: " + fileCreatedForBackup + " ");

                                while (remaining > 0 && (bytesRead = input.Read(buffer, 0,
                                       BUFFER_SIZE)) > 0)
                                {
                                    output.Write(buffer, 0, bytesRead);
                                    remaining -= bytesRead;
                                    var percentage = Math.Round((1.00 - ((double)remaining / totalSize)) * 100, 2);
                                    //Console.SetCursorPosition(0, 2);

                                    Console.Write("\r{0}%  ", percentage);
                                    //  System.Threading.Thread.Sleep(1);

                                }


                            }

                            index++;

                            Thread.Sleep(500); // experimental; perhaps try it
                        }
                    }
                    fileNum++;

                }
                Console.Clear();
                Console.WriteLine("");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(DateTime.Now.ToString());
                sb.AppendLine("There are no errors");
                sb.AppendLine("__________________________________________________");
                File.AppendAllText(backupLog, sb.ToString());
            }

            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(DateTime.Now.ToString());
                sb.AppendLine("error at file: " + fileStatusCopyAt);
                sb.AppendLine("Position: " + inputPositionLengh);
                sb.AppendLine("error: " + ex.Message);
                sb.AppendLine("__________________________________________________");
                File.AppendAllText(backupLog, sb.ToString());
            }


        }

        private static List<string> GetallFilesinFolder(string folderPath)
        {
            List<string> myfiles = new List<string>();
            try
            {

                string[] files = Directory.GetFiles(folderPath,
            "*.*",
            SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    myfiles.Add(file);
                }

            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return myfiles;
        }
    }
}
