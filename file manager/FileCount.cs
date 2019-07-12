using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager
{
    class FileCount
    {
        static int countFolder;
        public static long CalculateFolderSize(string folder)
        {
            float folderSize = 0.0f;
            try
            {
                if (!Directory.Exists(folder))
                    return (long)folderSize;
                else
                {
                    try
                    {
                        foreach (string file in Directory.GetFiles(folder))
                        {
                            if (File.Exists(file))
                            {
                                FileInfo finfo = new FileInfo(file);
                                folderSize += finfo.Length;
                            }
                        }

                        foreach (string dir in Directory.GetDirectories(folder))
                            folderSize += CalculateFolderSize(dir);
                    }
                    catch (NotSupportedException e)
                    {
                        Console.WriteLine("Unable to calculate folder size: {0}", e.Message);
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Unable to calculate folder size: {0}", e.Message);
            }
            return (long)folderSize;
        }

        public static string GetSubDirectories(string patch)
        {
            //countFolder = 0;
            string root = patch;
            string[] subdirectoryEntries = Directory.GetDirectories(root);

            foreach (string subdirectory in subdirectoryEntries)
                LoadSubDirs(subdirectory);

            return countFolder.ToString();
        }

        private static void LoadSubDirs(string dir)
        {
            countFolder++;
            string[] subdirectoryEntries = Directory.GetDirectories(dir);

            foreach (string subdirectory in subdirectoryEntries)
                LoadSubDirs(subdirectory);
        }

    }
}
