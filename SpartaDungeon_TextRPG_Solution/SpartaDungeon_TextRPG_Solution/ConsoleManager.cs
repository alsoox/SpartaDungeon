using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon_TextRPG_Solution
{
    static class ConsoleManager
    {
        public static int GetInput(int min, int max)
        {
            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                if (int.TryParse(Console.ReadLine(), out int input) && (min <= input) && (max >= input))
                    return input;

                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}
