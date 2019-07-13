using System;
using System.IO;

namespace PhoneBook.Library
{
    /// <summary>
    /// Class Constants where I stored some constants variables that I was going to use in my project.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// the name of the file that I was going to use.
        /// </summary>
        public static string FileName => "PhoneBook.bin";
        /// <summary>
        /// the current directory where the file it will be stored.
        /// </summary>
        public static string FileLocation => Environment.CurrentDirectory;
        /// <summary>
        /// the full path where my file it will be stored.
        /// </summary>
        public static string FilePath => Path.Combine(FileLocation, FileName);
        /// <summary>
        ///  the maximum length of the file that is 10MB.
        /// </summary>
        public static int FileMaxSize => 10000000; 
    }
}
