using System;
using System.Collections.Generic;
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

        public int MaxDepth { get; set; } = int.MaxValue;

        public Action<string> Write { get; set; } = Console.Write;
        public Action<ConsoleColor> SetColor { get; set; } = color => Console.ForegroundColor = color;

        public ConsoleColor DefaultColor { get; set; } = Console.ForegroundColor;
        public ConsoleColor DirColor { get; set; } = ConsoleColor.Blue;
        public ConsoleColor FileColor { get; set; } = Console.ForegroundColor;
        public ConsoleColor ExeColor { get; set; } = ConsoleColor.Green;

        public HashSet<string> ExeExtensions { get; set; } = new HashSet<string> { ".exe", ".bat", ".cmd", ".com", ".ps1", ".vbs", ".vbe", ".wsf", ".wsh", ".msi", ".scr", ".reg" };

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

        private ConsoleColor GetColor(FileSystemInfo fsItem)
        {
            if (fsItem.IsDirectory())
            {
                return DirColor;
            }
            string ext = Path.GetExtension(fsItem.FullName).ToLower();
            if (ExeExtensions.Contains(ext))
            {
                return ExeColor;
            }
            return FileColor;
        }

        private void WriteName(FileSystemInfo fsItem)
        {
            WriteColored(fsItem.Name, GetColor(fsItem));
        }

        private void PrintTree(string startDir, string prefix = "", int depth = 0)
        {
            if (depth >= MaxDepth)
            {
                return;
            }

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
                    PrintTree(fsItem.FullName, prefix + "│   ", depth + 1);
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
                    PrintTree(lastFsItem.FullName, prefix + "    ", depth + 1);
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