﻿using System;
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
                .Where(f => !f.Name.StartsWith(".")) // hide unix "hidden" files/dirs like .git, .vs. ToDo: add -a flag for all files
                .OrderBy(f => f.Name)
                .ToList();

            for (int i = 0; i < fsItems.Count; i++)
            {
                var fsItem = fsItems[i];
                bool isLast = i == fsItems.Count - 1;

                if (isLast)
                {
                    Write(prefix + "└── ");
                    WriteName(fsItem);
                    WriteLine();
                    if (fsItem.IsDirectory())
                    {
                        PrintTree(fsItem.FullName, prefix + "    ");
                    }
                }
                else
                {
                    Write(prefix + "├── ");
                    WriteName(fsItem);
                    WriteLine();
                    if (fsItem.IsDirectory())
                    {
                        PrintTree(fsItem.FullName, prefix + "│   ");
                    }
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