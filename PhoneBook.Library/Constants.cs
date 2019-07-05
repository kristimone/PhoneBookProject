using System;
using System.IO;

namespace PhoneBook.Library
{
    public class Constants
    {
        public static string FileName => "PhoneBook.bin";

        public static string FileLocation => Environment.CurrentDirectory;

        public static string FilePath => Path.Combine(FileLocation, FileName);

        public static int FileMaxSize => 10000000; // 10 MB
    }
}
