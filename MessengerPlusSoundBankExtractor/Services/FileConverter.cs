using MessengerPlusSoundBankExtractor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Threading.Tasks;

namespace MessengerPlusSoundBankExtractor.Services
{
    public class FileConverter
    {
        private static byte[] filePattern = { 0x53, 0x4E, 0x44, 0x55, 0x00 };
        public static List<int> FindPattern(ReadOnlySpan<byte> data)
        {
            var result = new List<int>();

            int[] skipTable = new int[256];
            for (int i = 0; i < skipTable.Length; i++)
            {
                skipTable[i] = filePattern.Length;
            }
            for (int i = 0; i < filePattern.Length - 1; i++)
            {
                skipTable[filePattern[i]] = filePattern.Length - i - 1;
            }

            int idx = 0;
            while (idx <= data.Length - filePattern.Length)
            {
                int j = filePattern.Length - 1;
                while (j >= 0 && filePattern[j] == data[idx + j])
                {
                    j--;
                }

                if (j < 0)
                {
                    result.Add(idx);
                    idx += filePattern.Length;
                }
                else
                {
                    idx += skipTable[data[idx + j]];
                }
            }
            result.Add(data.Length -1);
            return result;
        }

        public static List<AudioFile> GetFileList(List<int> indexes, ReadOnlyMemory<byte> file)
        {
            var result = new List<AudioFile>();
            /*SNDU [53 4e 44 55] (4 bytes)
            00 00 00 (3 bytes)
            17 bytes desconhecidos.
            260 bytes - Reservados para nome*/
            for(int i = 0; i < indexes.Count - 1;)
            {
                var nextFile = new AudioFile
                {
                    Name = Encoding.Latin1.GetString(file.Slice(start: indexes[i] + 24, length: 260).Span).Split('\0')[0].Normalize().Trim(),
                    File = file.Slice(start: indexes[i] + 284, length: indexes[i + 1] - indexes[i] - 284)
                };
                result.Add(nextFile);
                ++i;
            }



            return result;
        }

        public static async Task SaveFiles(List<AudioFile> files, string targetPath)
        {
            HashSet<string> names = new HashSet<string>();
            try
            {
                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return;
            }
            foreach (var file in files)
            {
                var finalFileName = TryAddName(names, string.Join("_", file.Name.Split(Path.GetInvalidFileNameChars())));
                try 
                {
                    await File.Create(Path.Combine(targetPath, $"{finalFileName}.mp3")).WriteAsync(file.File);
                }
                catch (Exception e) { Debug.WriteLine(e); }
            }
        }

        private static string TryAddName(HashSet<string> names, string newName)
        {
            if(!names.Add(newName))
                return TryAddName(names, newName + "_");
            return newName;
        }
    }
}
