using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace B_Tree

{
    public class Processing
    {
        static char[] _chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();

        public static string RandomString()
        {
            Random random = new();
            int n = random.Next(5, 15);
            string s = "";
            for (int i = 0; i < n; i++)
            {
                s += _chars[random.Next(0, _chars.Length)];
            }
            return s;
        }

        public static void RandomizeFile(int recordsCount)
        {
            StreamWriter file = new("..\\..\\..\\records.txt", false);
            for (int i = 0; i < recordsCount; i++)
            {
                file.WriteLine(i + 1 + " " + RandomString());
            }
            file.Close();
        }

        public static void FillTree(BTree<string> tree)
        {
            using (StreamReader streamReader = new StreamReader("..\\..\\..\\records.txt"))
            {
                string str;
                while ((str = streamReader.ReadLine()) != null)
                {
                    string[] record = str.Split(" "); 
                    tree.Insert(record[1], Int32.Parse(record[0]), false);
                }
            }
            Console.Clear();
        }

        public static void WriteTree(BTree<string> tree)
        {
            using StreamWriter streamWriter = new("..\\..\\..\\records.txt", false);
            List<KeyValue<string>> records = tree.Obhid(); 
            foreach (var record in records)
            {
                streamWriter.WriteLine(record.Key + " " + record.Value);
            }
            streamWriter.Close();
        }

        private static string[] _strings =
        {
            "wa", "ra", "ya", "ma", "ha", "na", "ta", "sa", "ka", "a", "wi", "ri", "mi", "hi", "ni", "chi", "shi", "ki",
            "i", "ru", "yu", "mu", "fu", "nu", "tsu", "su", "ku", "u", "we", "re", "me", "he", "ne", "te", "se", "ke",
            "e", "wo", "ro", "yo", "mo", "ho", "no", "to", "so", "ko", "o"
        };

        private static string GetRecord(int lenght)
        {
            StringBuilder stringBuilder = new(); 
            Random random = new();
            for (int i = 0; i < lenght; i++)
            {
                stringBuilder.Append(_strings[random.Next(_strings.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}

