using System;
using System.IO;
using NUnit.Framework;

namespace FileVersion.Test
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void LocateWithFolderAtThisLevelExpectPathReturned()
        {
            string startPath = Environment.CurrentDirectory;
            string directoryName = "testdirectory";
            Directory.CreateDirectory(directoryName);
            Assert.AreEqual(Path.Combine(startPath, directoryName), Program.LocateDirectory(startPath, directoryName));
            Directory.Delete(directoryName);
        }

        [Test]
        public void LocateWithFolderOneLevelHigherExpectPathReturned()
        {
            string startPath = Environment.CurrentDirectory;
            string directoryName = "testdirectory";
            Environment.CurrentDirectory += "\\..";
            Directory.CreateDirectory(directoryName);
            var expectedPath = Path.Combine(startPath, "..", directoryName);
            Assert.AreEqual(expectedPath, Program.LocateDirectory(startPath, directoryName));
            Directory.Delete(expectedPath);
        }

    }
}
