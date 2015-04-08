//Josh Hickman
//Program Assignment #1 - Playfair Cipher - Encryption
//CS 484

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playfair
{
    class Encrypt
    {

        static void Main()
        {
            bool end = false;
            string input;
            string upper_input;
            string[] arry_input;
            string[] command_line = new string[4];

            Console.WriteLine("~~~~~~~~~~~Playfair Encryption Program~~~~~~~~~~~\n~~~~~Please Use The Command Below As Example~~~~~\n" +
            "~~~~~~~~~ pencrypt ATHENSSTATE plaintext ~~~~~~~~\n~~~~~~~ pencrypt -d ATHENSSTATE plaintext ~~~~~~~\n");

            while (end == false)
            {
                Console.WriteLine("\nPlease input your command and the plaintext. Type END to end program: ");
                input = Console.ReadLine();
                upper_input = input.ToUpper();
                input = upper_input;

                if (input == "END") { end = true; }

                else
                {
                    arry_input = input.Split(' ');
                    Playfair.process_String(arry_input, command_line);
                    if (command_line[0] == "PENCRYPT") { Playfair.pencrypt(command_line); }
                }

                Array.Clear(command_line, 0, command_line.Length);
            }

            Console.WriteLine("End of Program");
            Console.ReadLine();
        }
    }

    public class Playfair
    {
        public static string process_matrix(string key)     //Process the key into a matrix.
        {
            char[] key_char = new char[key.Length];
            key_char = key.ToCharArray(0, key.Length);
            string matrix = "";
            string reference = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            char[] alpha = new char[reference.Length];
            alpha = reference.ToCharArray(0, 25);

            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (key_char[i] == alpha[j])
                    {
                        if (key_char[i] == 'I')
                        {
                            matrix = matrix + "J";
                            j = 25;
                            alpha[j] = ' ';
                        }

                        else
                        {
                            matrix = matrix + alpha[j];
                            alpha[j] = ' ';
                            j = 25;
                        }
                    }
                }
            }

            for (int i = 0; i < alpha.Length; i++)
            {
                if (alpha[i] != ' ') { matrix = matrix + alpha[i]; }
            }
            return matrix;
        }

        public static void process_String(string[] input, string[] command) //Takes the command line and puts it into array of strings to be further processed.
        {
            int length;

            if (input[1] == "-D") { length = 4; }
            else { length = 3; }

            for (int i = 0; i < length; i++)
            {
                if (i == length - 1)
                {
                    for (int j = i; j < input.Length; j++)
                    {
                        command[i] = command[i] + input[j];
                    }
                }
                else { command[i] = input[i]; }
            }
        }

        public static string Prepare(string text)  //Prepares plaintext to be used in the cipher.
        {
            int length = text.Length;
            text = text.ToLower();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                char c = text[i];
                if (c >= 97 && c <= 122)
                {
                    if (sb.Length % 2 == 1 && sb[sb.Length - 1] == c)
                    {
                        sb.Append('x');
                    }
                    sb.Append(c);
                }
            }

            if (sb.Length % 2 == 1)
            {
                sb.Append('x');
            }
            text = sb.ToString();
            text = text.ToUpper();
            return text;
        }


        public static void pencrypt(string[] command)
        {

            int length;
            int count = 0;
            string plain_Text;
            string cipher_Text;
            string key;
            char x, y;
            int x_ind, y_ind, x_row, y_row, x_col, y_col;
            StringBuilder sb = new StringBuilder();

            if (command[1] == "-D")
            {
                command[3] = Playfair.Prepare(command[3]);
                plain_Text = command[3];
                key = Playfair.process_matrix(command[2]);
                length = plain_Text.Length;
                char[] key_char = new char[key.Length];
                key_char = key.ToCharArray(0, key.Length);

                Console.WriteLine("The Matrix is followed below:\n");
                for (int i = 0; i < key.Length; i++)
                {
                    count++;
                    Console.Write(key_char[i] + " ");
                    if (count >= 5) { Console.WriteLine(); count = 0; }
                }
            }
            else
            {
                command[2] = Playfair.Prepare(command[2]);
                plain_Text = command[2];
                key = Playfair.process_matrix(command[1]);
                length = plain_Text.Length;
            }

            for (int i = 0; i < length; i += 2)
            {
                x = plain_Text[i];
                y = plain_Text[i + 1];

                x_ind = key.IndexOf(x);
                y_ind = key.IndexOf(y);
                x_row = x_ind / 5;
                y_row = y_ind / 5;
                x_col = x_ind % 5;
                y_col = y_ind % 5;

                if (x_row == y_row)
                {
                    if (x_col == 4)
                    {
                        sb.Append(key[x_ind - 4]);
                        sb.Append(key[y_ind + 1]);
                    }
                    else if (y_col == 4)
                    {
                        sb.Append(key[x_ind + 1]);
                        sb.Append(key[y_ind - 4]);
                    }
                    else
                    {
                        sb.Append(key[x_ind + 1]);
                        sb.Append(key[y_ind + 1]);
                    }
                }
                else if (x_col == y_col)
                {
                    if (x_row == 4)
                    {
                        sb.Append(key[x_ind - 20]);
                        sb.Append(key[y_ind + 5]);
                    }
                    else if (y_row == 4)
                    {
                        sb.Append(key[x_ind + 5]);
                        sb.Append(key[y_ind - 20]);
                    }
                    else
                    {
                        sb.Append(key[x_ind + 5]);
                        sb.Append(key[y_ind + 5]);
                    }
                }
                else
                {
                    sb.Append(key[5 * x_row + y_col]);
                    sb.Append(key[5 * y_row + x_col]);
                }
            }
            cipher_Text = sb.ToString();
            Console.WriteLine("Cipher Text: " + cipher_Text);
        }
    }
}

