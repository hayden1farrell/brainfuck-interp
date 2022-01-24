using System;
using System.Collections.Generic;

namespace brainfuck_comp
{
    class Program
    {
        static byte[] DATA = new byte[20];
        static int POINTER = 0;
        static long commands = 0;
        static void SyntaxError(string errorMSG, int index, string Program){
            Console.Write("\n\n\n\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" [*] " + errorMSG + "  Index: " + index + "\n [*] ");
            Console.ResetColor();

            for (int i = 0; i < Program.Length; i++)
            {
                Console.ForegroundColor = i == index ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write(Program[i]);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nExiting program due to error");
            Console.ResetColor();
            System.Environment.Exit(1);
        }
        static void Main(string[] args)
        {
            bool display = false;
            DisplayData();
            Console.WriteLine("Enter Program:");
            string Program = Console.ReadLine();


            Console.WriteLine("Do you wish to preview instirctions Y/N");
            if(Console.ReadLine().ToUpper() == "Y") display = true;
            
            for (int i = 0; i < Program.Length; i++)
            {
                i = Translate(Program[i], i, Program, 1, display);   
                if(display == true) DisplayData();
            }

            DisplayData();
        }

        private static int Translate(char instruction, int index, string Program, int depth, bool display)
        {
            commands++;
            switch (instruction)
            {
                case '>':
                    Shift(1, index, Program);
                    break;
                case '<':
                    Shift(-1, index, Program);
                    break;
                case '+':
                    DATA[POINTER] = (byte)(DATA[POINTER] + 1);
                    break;
                case '-':
                    DATA[POINTER] = (byte)(DATA[POINTER] - 1);
                    break;
                case '[':   // loop start 
                    index = Loop(index, Program,  depth, display);
                    break;
                case ']': // loop end
                    break;
                default:
                    SyntaxError("invalid character in Program", index, Program);
                    break;
            }
            return index;
        }
        private static int GetLoopEnd(string program, int depth){// NEEDS WORK  
            string loop = "";
            List<char> order = new List<char>();
            int loopEndPoint = 0;
            while(true){
                loop += program[loopEndPoint];

                if(program[loopEndPoint] == '[')
                    order.Add(program[loopEndPoint]);
                if(program[loopEndPoint] == ']'){
                    if(order.Count == depth){
                        break;
                    }else{
                        order.RemoveAt(order.Count - 1);
                    }
                }
                loopEndPoint++;
            }  
            return loopEndPoint;          
        }

        private static int Loop(int index, string program, int depth, bool display)
        {
            var watch = new System.Diagnostics.Stopwatch();
            int loopEndPoint = GetLoopEnd(program, depth);
            int endPoint = loopEndPoint;
            int startPoint = POINTER;
            index++;
            int startIndex = index;
            watch.Start();
            while(DATA[startPoint] > 0){
                index = Translate(program[index], index, program, depth + 1, display);
                if(index + 1 == endPoint + 1){
                    index = startIndex;
                }
                else{
                    index++;
                }
                if(display == true){
                    watch.Stop();
                    DisplayData();
                    watch.Start();
                }

                if(watch.ElapsedMilliseconds >= 50000){
                    Console.WriteLine("Loop has exceed max time");
                    System.Environment.Exit(1);
                }
            }
            return endPoint;
        }

        private static void Shift(int shift, int index, string Program)
        {
            if(POINTER + shift >= 0 && POINTER + shift < 20){
                POINTER += shift;
            }else{
                SyntaxError("Shift took Pointer out of DATA range", index, Program);
            }
        }

        static void DisplayData()
        {
            // this routine will display the contents of the array
            Console.WriteLine("DATA - Pointer Position in red: Ran " + commands + " commands");
            for (int i = 0; i < 20; i++)
            {
                Console.ForegroundColor = i == POINTER ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("{0}|", DATA[i]);
            }
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
            Console.WriteLine(" ");
        }

    }
}