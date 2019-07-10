using System;

namespace FsTree
{
    static class Program
    {
        static void ShowUsage()
        {
            Console.WriteLine("Usage: FsTree [dir] [-a]");
            Console.WriteLine("  -a - show unix 'hidden' files/dirs like .git, .vs, .gitignore");
        }

        static void Main(string[] args)
        {
            string startDir = null;
            bool showAll = false;
            // should use a library like https://github.com/commandlineparser/commandline but there are only 2 parameters for now, so whatever
            foreach (var arg in args)
            {
                if (arg == "-a")
                {
                    showAll = true;
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
                ShowAll = showAll
            }.Print();
        }
    }
}
