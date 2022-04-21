using System;
using System.Collections.Generic;

namespace B_Tree
{
    class Program
    {
        //private static readonly int T = 50;
        private static readonly int N = 10000;
        internal static void Main(string[] args)
        {
            BTree<string> tree;
            Console.WriteLine("Enter 0 to enter test mode (t=3), or something else (t=50)");
            string? mode = Console.ReadLine();
            if (mode != null && mode == "0")
            {
                tree = new(Mode.Test);
            }
            else
            {
                tree = new(Mode.Normal);
            }
            Console.WriteLine("Welcome! Commands:");
            Console.WriteLine("read");
            Console.WriteLine("insert <Key> <Value>");
            Console.WriteLine("delete <Key>");
            Console.WriteLine("search <Key>");
            Console.WriteLine("randomcheck");
            Console.WriteLine("generate");
            Console.WriteLine("write");
            Console.WriteLine("exit");
            string? s = "";
            while (s != "exit")
            {
                Console.WriteLine("Enter command:");
                s = Console.ReadLine();
                if (s == null || s == "")
                {
                    Console.WriteLine("Just enter some commands, i believe in you!");
                }
                else if (s == "read")
                {
                    Processing.FillTree(tree);
                }
                else if (s == "generate")
                {
                    Processing.RandomizeFile(N);
                    Console.WriteLine("File generated!");
                }
                else if (s == "insert")
                {
                    Console.Write("Enter key: ");
                    int k = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter value: ");
                    string? val = Console.ReadLine();
                    if (val == null)
                    {
                        Console.WriteLine("Wrong value");
                    }
                    else
                    {
                        tree.Insert(val, k, false);
                    }
                }
                else if (s == "search")
                {
                    Console.WriteLine($"Enter key: ");
                    int toDelete = Convert.ToInt32(Console.ReadLine());
                }
                else if (s == "delete")
                {
                    Console.WriteLine($"Enter key: ");
                    int toDelete = Convert.ToInt32(Console.ReadLine());
                    tree.Delete(toDelete);
                }
                else if (s == "write")
                {
                    Processing.WriteTree(tree);
                    Console.WriteLine("Tree writed!");
                }
                else if (s == "randomcheck")
                {
                    Random random = new();

                    for (int i = 0; i < 10; i++)
                    {
                        Console.Write(i + ": ");
                        tree.SearchData(random.Next(1, 10000));
                    }
                }
                else if (s == "exit")
                {
                    Console.WriteLine("Goodbye! Поставте 5!");
                }
                else
                {
                    Console.WriteLine("Wrong command");
                }
            }
            //Console.WriteLine(tree);
        }
        public void Menu()
        {

        }
    }

}