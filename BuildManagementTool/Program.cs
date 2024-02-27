using System.Collections.Generic;

static List<String> GetBinPaths(String repoPath)
{
    List<String> output = new List<String>();

    DirectoryInfo dir = new DirectoryInfo(repoPath);
  
    IEnumerable<DirectoryInfo> folderList = dir.GetDirectories("*.*", SearchOption.AllDirectories);
    
    //Create the query  
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
        String newDir = bp + "ExtraBinBuilds";

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
    string[] files = Directory.GetFiles(backupPath);
    foreach (string file in files)
    {
        output.Add(file);
    }

    return output;
}

static void MainMenu()
{
    Console.WriteLine();
}

static void Main(string[] args)
{
    List<String> names = new List<string>();
    List<String> binPaths = new List<string>();
    List<String> binBackupPaths = new List<string>();
    String repoPath = @"C:\git\wtg";

    Console.WriteLine("Enter repository path from your wtg folder");
    repoPath = Console.ReadLine();

    binPaths = GetBinPaths(repoPath);
    binBackupPaths = CreateBinFolders(binPaths);
    names = ExistingBinBuilds(binBackupPaths[1]);

    String input;
    while (true)
    {
        Console.WriteLine("List of existing backups:");
        foreach (String backup in names)
        {
            Console.WriteLine(backup);
        }
        Console.WriteLine("Type: /n n to create new backup /n d to delete backup /n s to switch to backup /n x to exit application");

        input = Console.ReadLine();

        switch (input)
        {
            case "n":
                Console.WriteLine("Input backup name");
                String newName = Console.ReadLine();
                foreach (String bp in binBackupPaths)
                {
                    String np = bp + "bin_" + newName;
                    Directory.CreateDirectory(binBackupPaths + newName);
                    throw new NotImplementedException();
                }
                break;
            case "d":
                Console.WriteLine("Input backup name to delete");
                String name = Console.ReadLine();
                foreach (String bp in binBackupPaths)
                {
                    String np = bp + "bin_" + name;
                    Directory.Delete(binBackupPaths + name);
                }
                break;
            case "s":
                throw new NotImplementedException();
                break;
            case "x":
                Environment.Exit(0);
                break;
        }
    }
}