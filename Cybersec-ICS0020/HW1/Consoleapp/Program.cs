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

        public static string CEncipher(string input, int key)
        {
            string output = string.Empty;
            foreach (char ch in input)
                output += cipher(ch, key);
            return output;
        }

        public static string Decipher(string input, int key)
        {
            return CEncipher(input, 26 - key);
        }

        private static char[] _alphabet = new char[]
        {
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            ' '
        };

        private static string VigenereEncrypt(string plainText, string keyText)
        {
            var result = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                var letterKey = keyText[i % keyText.Length];
                var indexInAlphabetKey = Array.IndexOf(_alphabet, letterKey);
                var letterText = plainText[i % plainText.Length];
                var indexInAlphabetText = Array.IndexOf(_alphabet, letterText);
                var indexNewLetter = (indexInAlphabetKey + indexInAlphabetText) % _alphabet.Length;
                result += _alphabet[indexNewLetter];
            }

            return result;
        }

        private static string VigenereDecrypt(string plainText, string keyText)
        {
            var result = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                var letterKey = keyText[i % keyText.Length];
                var indexInAlphabetKey = Array.IndexOf(_alphabet, letterKey);
                var letterText = plainText[i % plainText.Length];
                var indexInAlphabetText = Array.IndexOf(_alphabet, letterText);
                var indexNewLetter = 0;
                if (indexInAlphabetText - indexInAlphabetKey < 0)
                {
                    indexNewLetter = _alphabet.Length - Math.Abs(indexInAlphabetText - indexInAlphabetKey);
                }
                else
                {
                    indexNewLetter = (indexInAlphabetText - indexInAlphabetKey) % _alphabet.Length;
                }

                result += _alphabet[indexNewLetter];
            }

            return result;
        }

        public static string RunDiffieHellmanEncrypt()
        {
            long primeNumber = 60000049;
            Console.WriteLine("Prime number : " + primeNumber);
            Console.WriteLine("Base number.");
            Console.Write(">");
            var baseNumberText = Console.ReadLine()?.Trim() ?? "";

            long baseNumber = 0;
            if (!long.TryParse(baseNumberText, out baseNumber))
            {
                Console.WriteLine("Error" + baseNumberText + " isn't a number!!");
            }

            Console.WriteLine("Secret Number A.");
            Console.Write(">");
            var secretAText = Console.ReadLine()?.Trim() ?? "";

            long secretA = 0;

            if (!long.TryParse(secretAText, out secretA))
            {
                Console.WriteLine("Error" + secretAText + " isn't a number!!");

            }

            Console.WriteLine("Secret Number B.");
            Console.Write(">");
            var secretBText = Console.ReadLine()?.Trim() ?? "";

            long secretB = 0;
            if (!long.TryParse(secretBText, out secretB))
            {
                Console.WriteLine("Error" + secretBText + " isn't a number!!");
            }

            long entityADiffie = DiffieHellmanEncrypt(baseNumber, secretA, primeNumber);

            long temp = entityADiffie;

            long entityBDiffie = DiffieHellmanEncrypt(baseNumber, secretB, primeNumber);

            entityADiffie = entityBDiffie;
            entityBDiffie = temp;
            entityADiffie = DiffieHellmanEncrypt(entityADiffie, secretA, primeNumber);
            entityBDiffie = DiffieHellmanEncrypt(entityBDiffie, secretB, primeNumber);

            if (entityADiffie == entityBDiffie)
            {
                Console.WriteLine("Shared Secret:  {0} ", entityADiffie);
            }
            else
            {
                Console.WriteLine("Something's Wrong: Please check if prime number really is a prime number!'");
            }

            return "";
        }

        public static long DiffieHellmanEncrypt(long generator, long secret, long prime)
        {
            if (secret == 0)
            {
                return 1;
            }
            else if (secret % 2 == 0) // if even
            {
                long recurs = DiffieHellmanEncrypt(generator, secret / 2, prime);
                return (recurs * recurs) % prime;
            }
            else
            {
                return ((generator % prime) * DiffieHellmanEncrypt(generator, secret - 1, prime)) % prime;
            }
        }
        

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome !! ");
            var pro = "";
            do
            {
                Console.WriteLine("--------------");
                Console.WriteLine("Enter C to use Caesar algorithm");
                Console.WriteLine("--------------");
                Console.WriteLine("Enter V to use vigenere algorithm");
                Console.WriteLine("--------------");
                Console.WriteLine("Enter D to use Diffie Helman key exchange ");
                Console.WriteLine("--------------");
                Console.WriteLine("enter X to exit");
                pro = Console.ReadLine()?.ToUpper();
                if (pro == "C")
                {
                    var num = "";
                    do
                    {
                        Console.WriteLine("--------------");
                        Console.WriteLine("Enter E to encrypt");
                        Console.WriteLine("--------------");
                        Console.WriteLine("Enter D to Decrypt");
                        Console.WriteLine("--------------");
                        Console.WriteLine("enter M to go back");
                        num = Console.ReadLine()?.ToUpper();
                        switch (num)
                        {
                            case "E":
                                Encrypt();
                                break;
                            case "D":
                                Decrypt();
                                break;
                        }
                    } while (num != "M");

                    static void Encrypt()
                    {
                        Console.WriteLine("Type your plaintext");
                        string userString = Console.ReadLine()?.Trim().ToUpper() ?? "";

                        Console.WriteLine("\n");
                        Console.WriteLine("Enter your key");
                        Console.Write(">");
                        string keyText = Console.ReadLine();

                        int key = 0;
                        if (int.TryParse(keyText, out key))
                        {
                            Console.WriteLine("shift:" + key);
                            Console.WriteLine("Cipher text");
                            string cipherText = CEncipher(userString, key);
                            Console.WriteLine(cipherText);
                            Console.WriteLine("Original text");
                            string t = Decipher(cipherText, key);
                            Console.WriteLine(t);
                            Console.Write("\n");
                        }
                        else
                        {
                            Console.WriteLine("Error" + keyText + " isn't a number!!");
                        }
                    }


                    static void Decrypt()
                    {
                        Console.WriteLine("Type your cipher text");
                        string userString2 = Console.ReadLine()?.Trim().ToUpper() ?? "";
                        Console.WriteLine("\n");
                        Console.WriteLine("Enter your key");
                        Console.Write(">");
                        string key2Text = Console.ReadLine();

                        int key2 = 0;
                        if (int.TryParse(key2Text, out key2))
                        {
                            Console.WriteLine("Plain text");
                            string plainText = Decipher(userString2, key2);
                            Console.WriteLine(plainText);
                            Console.WriteLine("Original text");
                            string m = CEncipher(plainText, key2);
                            Console.WriteLine(m);
                            Console.Write("\n");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Error" + key2Text + " isn't a number!!");
                        }
                    }
                }
                else if (pro == "V")
                {
                    var num = "";
                    do
                    {
                        Console.WriteLine(" Vigenere ===>");
                        Console.WriteLine("--------------");
                        Console.WriteLine("Enter E to encrypt");
                        Console.WriteLine("--------------");
                        Console.WriteLine("Enter D to Decrypt");
                        Console.WriteLine("--------------");
                        Console.WriteLine("enter M to go back");
                        num = Console.ReadLine()?.ToUpper();
                        switch (num)
                        {
                            case "E":
                                Encrypt();
                                break;
                            case "D":
                                Decrypt();
                                break;
                        }
                    } while (num != "M");

                    static void Encrypt()
                    {
                        Console.WriteLine("Type your plaintext");
                        string userString = Console.ReadLine()?.Trim().ToUpper() ?? "";
                        Console.WriteLine("\n");
                        Console.WriteLine("Enter your key");
                        Console.Write(">");
                        string keyText = Console.ReadLine()?.Trim().ToUpper() ?? "";
                        Console.WriteLine("plain text" + keyText);
                        Console.WriteLine("Cipher text");
                        string cipherText = VigenereEncrypt(userString, keyText);
                        Console.WriteLine(cipherText);
                        Console.WriteLine("Original text");
                        string t = VigenereDecrypt(cipherText, keyText);
                        Console.WriteLine(t);
                        Console.Write("\n");
                    }


                    static void Decrypt()
                    {
                        Console.WriteLine("Type your cipher text");
                        string userString2 = Console.ReadLine()?.Trim().ToUpper() ?? "";
                        Console.WriteLine("\n");
                        Console.WriteLine("Enter your key");
                        Console.Write(">");
                        string key2Text = Console.ReadLine()?.Trim().ToUpper() ?? "";
                        Console.WriteLine("Key text: ");
                        string plainText = VigenereDecrypt(userString2, key2Text);
                        Console.WriteLine(plainText);
                        Console.WriteLine("Original text");
                        string m = VigenereEncrypt(plainText, key2Text);
                        Console.WriteLine(m);
                        Console.Write("\n");
                    }
                }
                else if (pro == "D")
                {
                    var num = "";
                    do
                    {
                        Console.WriteLine("Diff helme exchange ===>");
                        Console.WriteLine("--------------");
                        Console.WriteLine("Enter D to get the shared seceret");
                        Console.WriteLine("--------------");
                        Console.WriteLine("Enter M to go back");
                        num = Console.ReadLine()?.ToUpper();
                        switch (num)
                        {
                            case "D":
                                RunDiffieHellmanEncrypt();
                                break;
                        }
                    } while (num != "M");
                }
            } while (pro != "X");
        }
    }
}