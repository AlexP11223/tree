using System;

namespace FsTree
{
    static class Program
    {
        static void ShowUsage()
        {
            Console.WriteLine("Usage: FsTree [dir] [-a] [-L level]");
            Console.WriteLine("  -a - show unix 'hidden' files/dirs like .git, .vs, .gitignore");
            Console.WriteLine("  -L level - do not descend more than <level> directories deep");
            Console.WriteLine("Examples:");
            Console.WriteLine("  FsTree");
            Console.WriteLine("  FsTree myDir");
            Console.WriteLine("  FsTree -a -L 3");
        }

        static void Main(string[] args)
        {
            string startDir = null;
            bool showAll = false;
            int maxDepth = int.MaxValue;
            // should use a library like https://github.com/commandlineparser/commandline but there are only 3 parameters for now, so whatever
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg == "-a")
                {
                    showAll = true;
                }
                else if (arg == "-L")
                {
                    maxDepth = int.Parse(args[i + 1]);
                    i++;
                }
                else
                {
                    if (startDir == null)
                    {
                        startDir = arg;
                    }
                    else
                    {
                        Console.WriteLine("Unknown parameter {0}", arg);
                        ShowUsage();
                        return;
                    }
                }
            }

            if (String.IsNullOrEmpty(startDir))
            {
                startDir = ".";
            }

            new Tree(startDir)
            {
                ShowAll = showAll,
                MaxDepth = maxDepth,
            }.Print();
        }
    }
}
