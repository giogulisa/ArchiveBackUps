using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace ArchiveBackUps
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program Starting ...");

            //პარამეტრების წაკითხვა 
            string folderPath = args[0];

            DirectoryInfo startDir = new DirectoryInfo(folderPath);

            RecurseFileStructure recurseFileStructure = new RecurseFileStructure();
            recurseFileStructure.TraverseDirectory(startDir);
        }

        public class RecurseFileStructure
        {
            public void TraverseDirectory(DirectoryInfo directoryInfo)
            {
                if (!directoryInfo.Exists)
                {
                    Console.WriteLine("Incorecte Path -- Canceling");

                    Thread.Sleep(5000);

                    return;
                };

                var files = directoryInfo.EnumerateFiles();

                foreach (var file in files)
                {
                    if(!file.Name.EndsWith(".bak")) continue;

                    Console.WriteLine("File Selected " + file.FullName);
                    Thread.Sleep(1000);

                    HandleFile(file, directoryInfo.ToString());
                }
            }

            void HandleFile(FileInfo file, string path)
            {
                string withoutExtentionZip = Path.GetFileNameWithoutExtension(file.Name) + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-"
                                             + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".zip";

                string outPath = path + withoutExtentionZip;
                AddFilesToZip(outPath, file);
            }

            public static void AddFilesToZip(string zipPath, FileInfo file)
            {
                using (var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                {
                    Console.WriteLine("Creating Archive " + zipPath);
                    zipArchive.CreateEntryFromFile(file.FullName, file.Name);

                    Console.WriteLine("Deleting File " + file.Name);
                    file.Delete();

                    Console.WriteLine("File Deleted " + file.Name);
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
