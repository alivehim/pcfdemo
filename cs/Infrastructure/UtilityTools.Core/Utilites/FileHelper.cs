using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UtilityTools.Core.Utilites
{
    public class FileHelper
    {
        public static string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }

        public static string GetValideName(string illegal, bool isShort = true)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }

            var newillegal = RemoveSpecialCharacters(illegal);

            if (isShort && illegal.Length > 25)
            {
                illegal = illegal.Substring(0, 25);
            }
            return illegal.Replace(".", "");
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public static string GetFolder(string filePath)
        {
            int pos = filePath.LastIndexOf('\\');

            return filePath.Substring(0, pos);
        }

        public static string GetNameWithExtension(string fileName)
        {
            int pos = fileName.LastIndexOf('.');

            return fileName.Substring(0, pos);
        }

        public static void DelectDir(string srcPath)
        {
            try
            {

                System.IO.DirectoryInfo di = new DirectoryInfo(srcPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                di.Delete(true);

            }
            catch (Exception )
            {
                throw ;
            }
        }

        public static string GetFileContent(string fullName, out string name)
        {
            var file = new FileInfo(fullName);
            name = file.Name;
            if (file.Extension == ".rar" || file.Extension == ".zip")
            {
                //读取txt文件
                return ZipHelper.GetContentFromZip(file.FullName);
            }
            else
            {
                return ReadTextContent(file.FullName);
            }
        }

        public static string ReadTextContent(string fullPath)
        {
            if (fullPath != null)//传入了文件或者存在默认文件
            {
                var encoding = Encoding.GetEncoding(0);

                {
                    byte[] bs = File.ReadAllBytes(fullPath);


                    int len = bs.Length;
                    if (len >= 3 && bs[0] == 0xEF && bs[1] == 0xBB && bs[2] == 0xBF)
                    {
                        return Encoding.UTF8.GetString(bs, 3, len - 3);
                    }
                    int[] cs = { 7, 5, 4, 3, 2, 1, 0, 6, 14, 30, 62, 126 };
                    for (int i = 0; i < len; i++)
                    {
                        int bits = -1;
                        for (int j = 0; j < 6; j++)
                        {
                            if (bs[i] >> cs[j] == cs[j + 6])
                            {
                                bits = j;
                                break;
                            }
                        }
                        if (bits == -1)
                        {
                            return System.Text.Encoding.GetEncoding("GB2312").GetString(bs);
                        }
                        while (bits-- > 0)
                        {
                            i++;
                            if (i == len || bs[i] >> 6 != 2)
                            {
                                return System.Text.Encoding.GetEncoding("GB2312").GetString(bs);
                                //return encoding.GetString(bs);
                            }
                        }
                    }
                    return Encoding.UTF8.GetString(bs);

                }
            }
            return string.Empty;
        }

        public static string ReadAllFormatText(string filename)
        {
            byte[] bs = File.ReadAllBytes(filename);
            int len = bs.Length;
            if (len >= 3 && bs[0] == 0xEF && bs[1] == 0xBB && bs[2] == 0xBF)
            {
                return Encoding.UTF8.GetString(bs, 3, len - 3);
            }
            int[] cs = { 7, 5, 4, 3, 2, 1, 0, 6, 14, 30, 62, 126 };
            for (int i = 0; i < len; i++)
            {
                int bits = -1;
                for (int j = 0; j < 6; j++)
                {
                    if (bs[i] >> cs[j] == cs[j + 6])
                    {
                        bits = j;
                        break;
                    }
                }
                if (bits == -1)
                {
                    return Encoding.Default.GetString(bs);
                }
                while (bits-- > 0)
                {
                    i++;
                    if (i == len || bs[i] >> 6 != 2)
                    {
                        return Encoding.Default.GetString(bs);
                    }
                }
            }
            return Encoding.UTF8.GetString(bs);
        }

        public static Encoding GetFileEncodeType(BinaryReader r, long length)
        {
            var bs = r.ReadBytes((int)length);
            int len = bs.Length;
            if (len >= 3 && bs[0] == 0xEF && bs[1] == 0xBB && bs[2] == 0xBF)
            {
                return Encoding.UTF8;
            }
            int[] cs = { 7, 5, 4, 3, 2, 1, 0, 6, 14, 30, 62, 126 };
            for (int i = 0; i < len; i++)
            {
                int bits = -1;
                for (int j = 0; j < 6; j++)
                {
                    if (bs[i] >> cs[j] == cs[j + 6])
                    {
                        bits = j;
                        break;
                    }
                }
                if (bits == -1)
                {
                    return Encoding.Default;
                }
                while (bits-- > 0)
                {
                    i++;
                    if (i == len || bs[i] >> 6 != 2)
                    {
                        return Encoding.Default;
                    }
                }
            }
            return Encoding.UTF8;


        }

        public static long GetFolderSize(DirectoryInfo folder)
        {
            long totalSizeOfDir = 0;

            // Get all files into the directory
            FileInfo[] allFiles = folder.GetFiles();

            // Loop through every file and get size of it
            foreach (FileInfo file in allFiles)
            {
                totalSizeOfDir += file.Length;
            }

            // Find all subdirectories
            DirectoryInfo[] subFolders = folder.GetDirectories();

            // Loop through every subdirectory and get size of each
            foreach (DirectoryInfo dir in subFolders)
            {
                totalSizeOfDir += GetFolderSize(dir);
            }

            // Return the total size of folder
            return totalSizeOfDir;
        }
    }
}
