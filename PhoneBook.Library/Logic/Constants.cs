using System;
using System.IO;

namespace PhoneBook.Library
{
    /// Class Constants where I stored some constants variables that I was going to use in my project.
    public class Constants
    {
        /// the name of the file that I was going to use.
        public static string FileName => "PhoneBook.bin";
        /// the current directory where the file it will be stored.
        public static string FileLocation => Environment.CurrentDirectory;
        /// the full path where my file it will be stored.
        public static string FilePath => Path.Combine(FileLocation, FileName);
        /// the maximum length of the file that is 10MB.
        public static int FileMaxSize => 10000000; 
    }
}
