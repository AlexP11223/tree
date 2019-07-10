using System;
using System.IO;
using System.Linq;

namespace FsTree
{
    public class Tree
    {
        public Tree(string startDir)
        {
            StartDir = startDir;
        }

        public string StartDir { get; }

        /// <summary>
        /// Show unix "hidden" files/dirs like .git, .vs, .gitignore
        /// </summary>
        public bool ShowAll { get; set; } = false;

        public Action<string> Write { get; set; } = Console.Write;
        public Action<ConsoleColor> SetColor { get; set; } = color => Console.ForegroundColor = color;

        public ConsoleColor DefaultColor { get; set; } = Console.ForegroundColor;
        public ConsoleColor DirColor { get; set; } = ConsoleColor.Blue;
        public ConsoleColor FileColor { get; set; } = ConsoleColor.Green;

        public void Print()
        {
            WriteColored(StartDir, DirColor);
            WriteLine();

            PrintTree(StartDir);
        }

        private void WriteLine(string text = "")
        {
            Write(text + Environment.NewLine);
        }

        private void WriteColored(string text, ConsoleColor color)
        {
            SetColor(color);
            Write(text);
            SetColor(DefaultColor);
        }

        private void WriteName(FileSystemInfo fsItem)
        {
            WriteColored(fsItem.Name, fsItem.IsDirectory() ? DirColor : FileColor);
        }

        private void PrintTree(string startDir, string prefix = "")
        {
            var di = new DirectoryInfo(startDir);
            var fsItems = di.GetFileSystemInfos()
                .Where(f => ShowAll || !f.Name.StartsWith(".")) // 
                .OrderBy(f => f.Name)
                .ToList();

            foreach (var fsItem in fsItems.Take(fsItems.Count - 1))
            {
                Write(prefix + "├── ");
                WriteName(fsItem);
                WriteLine();
                if (fsItem.IsDirectory())
                {
                    PrintTree(fsItem.FullName, prefix + "│   ");
                }
            }

            var lastFsItem = fsItems.LastOrDefault();
            if (lastFsItem != null)
            {
                Write(prefix + "└── ");
                WriteName(lastFsItem);
                WriteLine();
                if (lastFsItem.IsDirectory())
                {
                    PrintTree(lastFsItem.FullName, prefix + "    ");
                }

            }
        }
    }

    public static class FileSystemInfoExtensions
    {
        public static bool IsDirectory(this FileSystemInfo fsItem)
        {
            return (fsItem.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}