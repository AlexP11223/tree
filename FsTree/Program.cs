using System.Linq;

namespace FsTree
{
    static class Program
    {
        static void Main(string[] args)
        {
            string startDir = args.Any() ? args[0] : ".";

            new Tree(startDir).Print();
        }
    }
}
