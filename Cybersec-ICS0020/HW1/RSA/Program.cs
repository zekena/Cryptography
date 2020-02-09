using System;
using System.Linq;
using System.Numerics;
using System.Text;


namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            var num = "";
            do
            {
                Console.WriteLine("--------------");
                Console.WriteLine("Enter E to encrypt");
                Console.WriteLine("--------------");
                Console.WriteLine("Enter D to decrypt");
                Console.WriteLine("--------------");
                Console.WriteLine("Enter B to Brute force your n ");
                Console.WriteLine("--------------");
                Console.WriteLine("enter X exit");
                num = Console.ReadLine()?.ToUpper();
                switch (num)
                {
                    case "E":
                        RunRSAEncrypt();
                        break;
                    case "D":
                    {
                        RunRSADecrypt();
                    }
                        break;
                    case "B":
                        BruteForce();
                        break;
                }
            } while (num != "X");
        }

        public static string BruteForce()
        {
            long n;
            string nText;
            Console.WriteLine("\nTrying to BruteForce the Private Key !");
            Console.WriteLine("Enter the Public Key:");
            do
            {
                Console.Write("n :");
                nText = Console.ReadLine();
                if (!long.TryParse(nText, out n))
                {
                    Console.WriteLine("Error : " + nText + " is not a number");
                }
            } while (!long.TryParse(nText, out n));

            long et;
            string eText;
            do
            {
                Console.Write("e :");
                eText = Console.ReadLine();
                if (!long.TryParse(eText, out et))
                {
                    Console.WriteLine("Error : " + eText + " is not a number");
                }
            } while (!long.TryParse(eText, out et));

            var primeFactors = FindPrimeFactors(n);
            var numbersOfPrimeFactors = 0;
            foreach (var t in primeFactors)
            {
                if (t != 0)
                {
                    numbersOfPrimeFactors++;
                }
            }

            if (numbersOfPrimeFactors == 2)
            {
                long p = primeFactors[0];
                long q = primeFactors[1];
                var P = PrivateKey(p, q);
                Console.WriteLine($"Private key is:d={P} \n Prime numbers are p*q=n : {p}*{q}={n} ");
            }
            else
            {
                Console.Write($"Error : More than 2 prime factors for {n}");
            }

            return "";
        }

        public static string RunRSAEncrypt()
        {
            Console.WriteLine("Hello RSA!");
            long p = CheckInputPrimeNumber("Prime Number1: ");
            long q = CheckInputPrimeNumber("PrimeNumber2: ");

            Console.WriteLine("---------------------");
            Console.WriteLine($"Prime number p : {p}");
            Console.WriteLine($"Prime number q : {q}");
            var encryptedNumber = RsaEncrypt(p, q);
            Console.WriteLine($"Encrypted message : {encryptedNumber}");
            Console.WriteLine("---------------------");


            return "";
        }

        public static bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int) Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        public static long CheckInputPrimeNumber(string message)
        {
            var primeNumberText = "";
            long primeNumber = 0;
            do
            {
                Console.WriteLine(message);
                Console.Write(">");
                primeNumberText = Console.ReadLine()?.Trim();
                if (!long.TryParse(primeNumberText, out primeNumber))
                {
                    Console.WriteLine($"Error : {primeNumberText} is not a number");
                }
                else
                {
                    if (!IsPrime(primeNumber))
                    {
                        Console.WriteLine($"Error : {primeNumber} is not a prime number");
                    }
                }
            } while (!long.TryParse(primeNumberText, out primeNumber) || !IsPrime(primeNumber));

            return primeNumber;
        }

        private static string RsaEncrypt(long p, long q)
        {
            (long n, long e,long d) = PublicKey(p, q);
            Console.WriteLine($"Public key => n : {n}, e : {e}\nPrivate key d: {d} ");
            Console.WriteLine("Text to encrypt.");
            Console.Write(">");
            var plainTextString = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine(plainTextString);
            var i = 0;
            var plainText = 0;
            BigInteger cipher = 0;
            var encryptText = "";
            do
            {
                    UTF8Encoding utf8 = new UTF8Encoding();
                    byte[] encodedBytes = utf8.GetBytes(plainTextString);
                    Console.WriteLine(encodedBytes[i]);
                    cipher = BigInteger.ModPow(encodedBytes[i], e, n);
                    encryptText += cipher + ".";
                    i++;
                    
            } while (i < plainTextString.Length);

            Console.WriteLine(encryptText);

            return encryptText;
        }

        private static string RunRSADecrypt()
        {
            var dText = "";
            var nText = "";
            long dd = 0;
            long nd = 0;
            Console.WriteLine("Enter the Private Key d:");
            do
            {
                Console.Write("d :");
                dText = Console.ReadLine();
                if (!long.TryParse(dText, out dd))
                {
                    Console.WriteLine("Error : " + dText + " is not a number");
                }
            } while (!long.TryParse(dText, out dd));
            Console.WriteLine("Enter the Private Key n:");
            do
            {
                Console.Write("n :");
                nText = Console.ReadLine();
                if (!long.TryParse(nText, out nd))
                {
                    Console.WriteLine("Error : " + nText + " is not a number");
                }
            } while (!long.TryParse(nText, out nd));
            Console.WriteLine("Cipher to decrypt.");
            Console.Write(">");
            var cipherText = Console.ReadLine();
            string trimmedAmount = cipherText.Replace(".", string.Empty);
            BigInteger cipher = 0;
            var encryptText = "";
            if (BigInteger.TryParse(trimmedAmount, out cipher))
            {
                string[] nums = cipherText.Split('.');
                long[] LongNum = nums.Select(long.Parse).ToArray();
                foreach (var t in LongNum)
                {
                    long decryptNum = BigNum( t, dd, nd);
                    //byte[] bytes = BitConverter.GetBytes(decryptNum);
                    //Console.WriteLine(bytes[i]);
                    var decryptText = Convert.ToChar(decryptNum);
                    encryptText += decryptText;
                }

                Console.WriteLine(encryptText);


            }
            else
            {
                Console.WriteLine("Error " + cipherText + " isn't a number!!");
            }
            
            return encryptText;
        }

        static long[] FindPrimeFactors(long n)
        {
            var index = (long) Math.Sqrt(n) + 1;
            bool[] isPrime = new bool[index];
            long[] primeFactors = new long[index];
            long j = 0;

            for (long i = 0; i < isPrime.Length; i++)
                isPrime[i] = true;
            for (long i = 0; i < primeFactors.Length; i++)
                primeFactors[i] = 0;


            for (long p = 2; p * p <= n; p++)
            {
                if (isPrime[p])
                {
                    for (long i = p * p; i < isPrime.Length; i += p)
                        isPrime[i] = false;
                }
            }

            for (long i = 2; i < isPrime.Length; i++)
            {
                if (isPrime[i])
                {
                    if (n % i == 0)
                    {
                        primeFactors[j] = i;
                        j++;
                        primeFactors[j] = n / i;
                        j++;
                    }
                }
            }

            return primeFactors;
        }


        public static long BigNum(long generator, long secret, long prime)
        {
            if (secret == 0)
            {
                return 1;
            }
            else if (secret % 2 == 0) // if odd
            {
                long recurs = BigNum(generator, secret / 2, prime);
                return (recurs * recurs) % prime;
            }
            else
            {
                return ((generator % prime) * BigNum(generator, secret - 1, prime)) % prime;
            }
        }

        public static (long n, long e,long d) PublicKey(long p, long q)
        {
            var n = p * q;
            var m = (p - 1) * (q - 1);
            long e = 7;
            long gcd = 0;
            do
            {
                e++;
                gcd = Gcd(m, e);
            } while (gcd != 1);
            var k = 0;
            do
            {
                if ((1 + k * m) % e == 0) break;
                k++;
            } while (true);

            var d = (1 + k * m) / e;

            return (n, e, d);
        }


        static (long n, long e,long d) PrivateKey(long p, long q)
        {
            var m = (p - 1) * (q - 1);
            (long n, long e,long d) = PublicKey(p, q);
            var k = 0;
            do
            {
                if ((1 + k * m) % e == 0) break;
                k++;
            } while (true);

            d = (1 + k * m) / e;

            return ( n, e, d);
        }


        // Function to return Greatest Common Divisor of a and b 
        static long Gcd(long a, long b)
        {
            if (a == 0)
                return b;
            return Gcd(b % a, a);
        }
    }
}