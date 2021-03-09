using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sekmen.Core.Extensions
{
    public static class FileExtensions
    {
        public static void AppendTextToFile(this string path, string content)
        {
            File.AppendAllLines(path, new[] { content });
        }

        public static string CalculateHash(this string path, long size, DateTime infoLastWriteTime)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(path + size + infoLastWriteTime.FormatUniversalWithTime()))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Create a directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DirectoryInfo CreateDirectory(this string path)
        {
            try
            {
                return Directory.CreateDirectory(path);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Create a directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileStream CreateFile(this string path)
        {
            try
            {
                return File.Create(path);
            }
            catch
            {
                return null;
            }
        }

        public static bool CreateFile(this string path, Stream content)
        {
            try
            {
                FileStream fileStream = File.Create(path);
                content.Seek(0, SeekOrigin.Begin);
                content.CopyTo(fileStream);
                fileStream.Close();
                fileStream.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete directory
        /// </summary>
        public static bool DeleteDirectory(this string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// delete file
        /// </summary>
        public static bool DeleteFile(this string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// check if directory exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DirectoryExists(this string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// get Directory Info
        /// </summary>
        /// <param name="path"></param>
        /// <returns>DirectoryInfo</returns>
        public static DirectoryInfo DirectoryInfo(this string path)
        {
            try
            {
                return new DirectoryInfo(path);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Checks file exists
        /// </summary>
        public static bool FileExists(this string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// get file info
        /// </summary>
        /// <param name="path"></param>
        /// <returns>FileInfo</returns>
        public static FileInfo FileInfo(this string path)
        {
            try
            {
                return new FileInfo(path);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get the file size of a given filename.
        /// </summary>
        public static long FileSize(this string filePath)
        {
            long bytes = 0;

            try
            {
                FileInfo oFileInfo = new FileInfo(filePath);
                bytes = oFileInfo.Length;
            }
            catch
            {
                //ignored
            }
            return bytes;
        }

        /// <summary>
        /// Nicely formatted file size. This method will return file size with bytes, KB, MB and GB in it. You can use this alongside the Extension method named FileSize.
        /// </summary>
        public static string FormatFileSize(this int fileSize)
        {
            return fileSize.ToLong().FormatFileSize();
        }

        public static string FormatFileSize(this long fileSize)
        {
            //declarations
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int j = 0;

            //loop and divide
            while (fileSize > 1024 && j < (suffix.Length - 1))
            {
                fileSize = fileSize / 1024;
                j++;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return $"{fileSize:0} {suffix[j]}";
        }

        /// <summary>
        /// Get all sub directories recursively
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetAllSubDirectories(this string path)
        {
            //create final list
            List<string> returnList = new List<string> { path };
            //get sub directories
            List<string> root = path.GetSubDirectories();
            //loop through each folder & get sub directories
            foreach (string item in from item in root let subs = item.GetSubDirectories() where subs != null select item)
            {
                returnList.AddRange(item.GetAllSubDirectories());
            }

            return returnList.ToList();
        }

        public static DateTime GetDirectoryLastWriteTime(this string serverFilePath)
        {
            return Directory.GetLastWriteTime(serverFilePath);
        }

        /// <summary>
        /// GET File extension
        /// </summary>
        public static string GetFileExtension(this string serverFilePath, bool withoutDot = true)
        {
            string result = Path.GetExtension(serverFilePath);
            if (withoutDot)
            {
                result = result?.Replace(".", "");
            }

            return result;
        }

        /// <summary>
        /// GET File GetLastWriteTime
        /// </summary>
        /// <param name="serverFilePath">file path</param>
        /// <returns>date time as DateTime</returns>
        public static DateTime GetFileLastWriteTime(this string serverFilePath)
        {
            return File.GetLastWriteTime(serverFilePath);
        }

        /// <summary>
        /// get files
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetFilesInDirectory(this string path)
        {
            try
            {
                return Directory.GetFiles(path).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// GET File GetLastWriteTime
        /// </summary>
        /// <param name="serverFilePath">file path</param>
        /// <param name="format">optional param as return format</param>
        /// <returns>date time as selected format</returns>
        public static string GetLastWriteTime(this string serverFilePath, string format)
        {
            return serverFilePath.GetFileLastWriteTime().ToString(format);
        }

        /// <summary>
        /// get sub directories
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetSubDirectories(this string path)
        {
            try
            {
                return Directory.GetDirectories(path).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// read all text from a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// read all text from a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadAllText(this string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Rename directory
        /// </summary>
        public static bool RenameDirectory(this string path, string destination)
        {
            try
            {
                if (Directory.Exists(path) && !Directory.Exists(destination))
                {
                    Directory.Move(path, destination);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RenameFile(this string path, string destination)
        {
            try
            {
                if (File.Exists(path) && !File.Exists(destination))
                {
                    File.Move(path, destination);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static IEnumerable<byte> GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}