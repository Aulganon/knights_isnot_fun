using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;

namespace Chess_meme
{
    internal class Program
    {
        static string path_file = "";

        public static int[,] array_generator(int number, int identer)
        {
            int[,] chess_array = new int[number, number];

            for (int i = 0; i < number; ++i)
            {
                for (int j = 0; j < number; ++j)
                {
                    chess_array[i, j] = identer;
                }
            }
            return chess_array;
        }

        static int[] rearr(int[] arr, int[] array)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                int num = 9 - array[i];
                for (int j = 0; j < array.Length; ++j)
                {
                    if (array[j] == num)
                    {
                        var temp = arr[j];
                        arr[j] = arr[i];
                        arr[i] = temp;
                    }
                    break;
                }
            }
            return arr;
        }

        static void execute(int number, int startX, int startY)
        {
            /*
             * function --execute()-- 
             * return type --none--
             * 
             * <int> number -- volume of chessboard optional not less than 3
             * <int> startX -- start position by X
             * <int> startY -- start position by Y
             * 
             */


            int identer = -1; // -1 is identer of not used fields
            int[,] chess_array = array_generator(number,identer);
            Random rnd = new Random();

            int[] allowed_moveX = { 2, 1, - 1, -2, -2, -1, 1, 2 }; // co-allowed moves for knight by X
            int[] allowed_moveY = { 1, 2,  2, 1, -1, -2, -2, -1 }; // and by Y

            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8 }; // key for randomizer

            for (int i = array.Length - 1; i >= 1; i--)
            {
                int j = rnd.Next(i + 1);
                // обменять значения data[j] и data[i]
                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }

            allowed_moveX = rearr(allowed_moveX, array); // here we randomize first pair of moves to check
            allowed_moveY = rearr(allowed_moveY, array); // if not found then next pair of x,y goes

            chess_array[startX, startY] = 0;

            if (!decide(startX, startY, 1, chess_array, allowed_moveX, allowed_moveY, number, identer))
            {
                Console.WriteLine("No solution possible");
            }
            else {
                result_out(chess_array, number);
            }


        }//end of function


        static void result_out(int[,] chess_array, int volume)
        {
            for (int x = 0; x < volume; x++) 
            {
                for (int y = 0; y < volume; y++)
                {
                    Console.Write(chess_array[x, y] + " ");
                }
                Console.WriteLine();
            }
        }


        public static bool decide(int x, int y, int roll_num, int[,] chess_array,
            int[] allowed_moveX, int[] allowed_moveY, int volume, int identer) {
            /*
             * function --decide()-- 
             * return type --bool--
             * 
             * decides if end or not
             * if roll_num is equal squared volume then the bypass is over
             * and function breaks
             * In other cases the recursive roll is in process
             * 
             * <int> number -- volume of chessboard optional not less than 3
             * <int> startX -- start position by X
             * <int> startY -- start position by Y
             * 
             */



            int move_x;
            int move_y;

            if (roll_num == volume * volume) 
            {
                return true; // ends of bypass
            }

            for (int i = 0; i < 8; ++i) {  // i < 8 because
                                           // there is only 8 maxiamum avaliable
                                           // moves for knight posssible
                move_x = x + allowed_moveX[i];
                move_y = y + allowed_moveY[i];
                /*
                 * if ok then we make next move and save it by true returning
                 * 
                 * if not then we return identer which is -1 by default and leave 
                 * the field for a while
                 */
                if (isAllowed(move_x, move_y, chess_array,volume, identer)) {

                    chess_array[move_x, move_y] = roll_num;

                    if (decide(move_x, move_y, roll_num + 1,
                                chess_array, allowed_moveX, allowed_moveY,
                                volume, identer)) 
                    { 
                        return true; 
                    } 
                    else
                    {
                        chess_array[move_x, move_y] = identer;
                    }

                }
                
            }
            return false;

        }//end of function

        static bool isAllowed(int x, int y, int[,] chess_array, int volume, int identer)
        {
            return (x >= 0 && x < volume &&
                y >= 0 && y < volume && 
                chess_array[x, y] == identer); 
        }

        static void Main(string[] args)
        {
            execute(8,3,3); // 7 3 3
                            // 8 0 0
                            // 5 4 4
            Console.ReadKey();
        }
    }
}
