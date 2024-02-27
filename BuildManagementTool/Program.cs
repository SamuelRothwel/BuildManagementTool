using System.Collections.Generic;
using System.Diagnostics;

namespace program
{
    public class program{
        static List<String> GetBinPaths(String repoPath)
        {
            List<String> output = new List<String>();
            Console.WriteLine("Enter repository path from your wtg folder");
            DirectoryInfo dir = new DirectoryInfo(repoPath);

            IEnumerable<DirectoryInfo> folderList = dir.GetDirectories("*.*", SearchOption.AllDirectories);

            IEnumerable<DirectoryInfo> folderQuery =
                from folder in folderList
                orderby folder.Name
                select folder;

            foreach (DirectoryInfo f in folderQuery)
            {
                if (string.Equals(f.Name, "bin", StringComparison.OrdinalIgnoreCase))
                {
                    output.Add(f.FullName);
                }
            }

            return output;
        }

        static List<String> CreateBinFolders(List<String> binPaths)
        {
            List<String> output = new List<String>();
            foreach (String bp in binPaths)
            {
                bp.Remove(4);
                String newDir = bp + ".mm.Backups";

                if (!Directory.Exists(newDir))
                {
                    Directory.CreateDirectory(newDir);
                }
                output.Add(newDir);
            }

            return output;
        }

        static List<String> ExistingBinBuilds(String backupPath)
        {
            List<String> output = new List<string>();
            string[] directories = Directory.GetDirectories(backupPath);
            foreach (string dir in directories)
            {
                String[] split = dir.Split(@"\");
                output.Add(split[split.Count() - 1]);
            }

            return output;
        }

        static void DeleteDirectory(String dir)
        {
            DirectoryInfo del = new DirectoryInfo(dir);
            del.Delete(true);
        }

        static void CopyIntoDirectory(String inpDir, String outDir)
        {
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            string[] files = Directory.GetFiles(inpDir);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                string destinationFilePath = Path.Combine(outDir, fileName);

                File.Copy(file, destinationFilePath, true);
            }
        }

        static void SwapDirectories(String newDir, String curDir, String retireDir)
        {
            CopyIntoDirectory(curDir, retireDir);
            DeleteDirectory(curDir);
            CopyIntoDirectory(newDir, curDir);
        }

        static void Main(string[] args)
        {
            List<String> names = new List<string>();
            List<String> binPaths = new List<string>();
            List<String> binBackupPaths = new List<string>();
            String repoPath = @"C:\git\wtg\";

            Console.WriteLine("Enter repository path from your wtg folder");
            repoPath = repoPath + Console.ReadLine();

            binPaths = GetBinPaths(repoPath);
            Console.WriteLine("got paths");
            binBackupPaths = CreateBinFolders(binPaths);

            String input;
            while (true)
            {
                names = ExistingBinBuilds(binBackupPaths[0]);
                Console.WriteLine("List of existing backups:");
                foreach (String backup in names)
                {
                    Console.WriteLine(backup);
                }
                Console.WriteLine("Type: \n n to create new backup \n d to delete backup \n s to switch to backup \n x to exit application");

                input = Console.ReadLine();

                switch (input)
                {
                    case "n":
                        Console.WriteLine("Input backup name to create");
                        String newName = Console.ReadLine();
                        for (int i = 0; i <= binPaths.Count - 1; i++)
                        {
                            String np = binBackupPaths[i] + @"\" + "bin_" + newName;
                            Directory.CreateDirectory(binBackupPaths + newName);
                            CopyIntoDirectory(binPaths[i], np);
                        }
                        break;
                    case "d":
                        Console.WriteLine("Input backup name to delete");
                        String name = Console.ReadLine();
                        foreach (String bp in binBackupPaths)
                        {
                            String np = bp + @"\" + "bin_" + name;
                            DeleteDirectory(np);
                        }
                        break;
                    case "s":
                        Console.WriteLine("Input backup name to swap");
                        String swapName = Console.ReadLine();
                        Console.WriteLine("Input name to save current directory");
                        String retireName = Console.ReadLine();

                        for (int i = 0; i <= binPaths.Count - 1; i++)
                        {
                            String sp = binBackupPaths[i] + @"\" + "bin_" + swapName;
                            String np = binBackupPaths[i] + @"\" + "bin_" + retireName;
                            Directory.CreateDirectory(binBackupPaths + retireName);
                            SwapDirectories(sp, binPaths[i], np);
                        }
                        break;
                    case "x":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("invalid input");
                        break;
                }
            }
        }
    }
}