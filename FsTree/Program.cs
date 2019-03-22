using System;
using System.IO;
using System.Linq;

namespace FsTree
{
    static class Program
    {
        static void Main(string[] args)
        {
            string startDir = args.Any() ? args[0] : ".";

            WriteColored(startDir, ConsoleColor.Blue);
            Console.WriteLine();

            PrintTree(startDir);
        }

        public static bool IsDirectory(this FileSystemInfo fsItem)
        {
            return (fsItem.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public static void WriteColored(string s, ConsoleColor color)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(s);
            Console.ForegroundColor = prevColor;
        }

        public static void WriteName(FileSystemInfo fsItem)
        {
            WriteColored(fsItem.Name, fsItem.IsDirectory() ? ConsoleColor.Blue : ConsoleColor.Green);
        }

        static void PrintTree(string startDir, string prefix = "")
        {
            var di = new DirectoryInfo(startDir);
            var fsItems = di.GetFileSystemInfos()
                .Where(f => !f.Name.StartsWith(".")) // hide unix "hidden" files/dirs like .git, .vs. ToDo: add -a flag for all files
                .OrderBy(f => f.Name)
                .ToList();

            for (int i = 0; i < fsItems.Count; i++)
            {
                var fsItem = fsItems[i];

                if (i == fsItems.Count - 1)
                {
                    Console.Write(prefix + "└── ");
                    WriteName(fsItem);
                    Console.WriteLine();
                    if (fsItem.IsDirectory())
                    {
                        PrintTree(fsItem.FullName, prefix + "    ");
                    }
                }
                else
                {
                    Console.Write(prefix + "├── ");
                    WriteName(fsItem);
                    Console.WriteLine();
                    if (fsItem.IsDirectory())
                    {
                        PrintTree(fsItem.FullName, prefix + "│   ");
                    }
                }
            }
        }
    }
}
