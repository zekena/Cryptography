using System;

namespace Consoleapp
{
    class Program
    {
        public static char cipher(char ch, int key)
        {
            if (!char.IsLetter(ch))
            {
                return ch;
            }

            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char) ((((ch + key) - d) % 26) + d);
        }

        public static string Encipher(string input, int key)
        {
            string output = string.Empty;
            foreach (char ch in input) 
                output += cipher(ch, key);
            return output;
        }

        public static string Decipher(string input, int key)
        {
            return Encipher(input, 26 - key);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to caesar program to ecrypt press 1 \n to decrypt press 2");
            int num = Convert.ToInt32(Console.ReadLine());
            
            if (num == 1)
            {
                Console.WriteLine("Type your plaintext");
                string UserString = Console.ReadLine();
                Console.WriteLine("\n");
                Console.WriteLine("Enter your key");
                int key = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Cipher text");
                string cipherText = Encipher(UserString, key);
                Console.WriteLine(cipherText);
                Console.WriteLine("Original text");
                string t = Decipher(cipherText, key);
                Console.WriteLine(t);
                Console.Write("\n");
                Console.ReadKey();
            }


            if (num == 2)
            {
                Console.WriteLine("Type your cipher text");
                string UserString2 = Console.ReadLine();
                Console.WriteLine("\n");
                Console.WriteLine("Enter your key");
                int key2 = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Plain text");
                string plainText = Decipher(UserString2, key2);
                Console.WriteLine(plainText);
                Console.WriteLine("Original text");
                string m = Encipher(plainText, key2);
                Console.WriteLine(m);
                Console.Write("\n");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Enter another number eithor 1 or 2!!");
                return;
            }



        }
        
        
    }
}