using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FileVersion
{
    public class Program
    {
        private static bool _trace = false;
        private static int _longestFileLength = -1;

        static void Main(string[] args)
        {
            _trace = (args.Length > 2 && args[2] == "/t");

            for (var i = 0; i < args.Length; ++i)
            {
                TraceIt(string.Format("args[{0}] = {1}", i, args[i]));
            }

            var targetDirectory = LocateDirectory(args[0], args[1]);

            if (string.IsNullOrEmpty(targetDirectory))
            {
                Console.WriteLine("Unable to locate directory");
                return;
            }

            IEnumerable<string> fileNames = ExecutableFileList(targetDirectory);

            SetLongestFileLength(fileNames);

            foreach (var fileName in fileNames)
            {
                Console.WriteLine(FileVersion(fileName));
            }
        }

        public static string LocateDirectory(string startPath, string directoryName)
        {
            var startPoint = Path.Combine(startPath, directoryName);

            TraceIt("Searching for directory: " + startPoint);

            if (Directory.Exists(startPoint))
            {
                TraceIt("Target directory found");
                return startPoint;
            }

            DirectoryInfo parentDirectory;
            if ((parentDirectory = Directory.GetParent(startPath)) == null)
            {
                return string.Empty;
            }

            return LocateDirectory(parentDirectory.FullName, directoryName);
        }

        [DebuggerStepThrough]
        private static void TraceIt(string traceLine)
        {
            if (_trace)
            {
                Console.WriteLine(traceLine);
            }
        }

        private static void SetLongestFileLength(IEnumerable<string> fileNames)
        {
            _longestFileLength = fileNames.Max(fn => fn.Length);
        }

        public static IEnumerable<string> ExecutableFileList(string path)
        {
            var dllFiles = Directory.GetFiles(path, "*.dll").ToList();
            TraceIt(string.Format("Found {0} dll files", dllFiles.Count));

            var exeFiles = Directory.GetFiles(path, "*.exe").ToList();
            TraceIt(string.Format("Found {0} exe files", exeFiles.Count));

            return dllFiles.Concat(exeFiles).OrderBy(s => s);
        }

        public static string FileVersion(string fileName)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(fileName);
            return string.Format("{0}{1}v{2}"
                , Path.GetFileName(versionInfo.FileName)
                , new string(' ', _longestFileLength + 1 - fileName.Length)
                , versionInfo.FileVersion);
        }
    }
}
