using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Gomoku
{
    class chessBoard
    {
        public Grid view;
        public int[,] Board = new int[12, 12];

        
        public void play(int column, int row, int Activer)
        {
            

            Board[column, row] = Activer;
            double width = view.Width / 12;
            double height = view.Height / 12;
            Image co = new Image();
            if (Activer == 1)
            {

                co.Source = new BitmapImage(new Uri("pack://application:,,,/icon/Letter-O-black-icon.png"));
            }
            else
            {
                co.Source = new BitmapImage(new Uri("pack://application:,,,/icon/Letter-X-red-icon.png"));
            }
            co.Width = width;
            co.Height = height;
            Grid.SetColumn(co, column);
            Grid.SetRow(co, row);
            view.Children.Add(co);
        }
        public chessBoard()
        {
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 12; j++)
                    Board[i, j] = 0;
        }

        public chessBoard(Grid temp)
        {
            view = temp;
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 12; j++)
                    Board[i, j] = 0;
        }
        public int checkWin(int column, int row) // tra ve 0 neu chua ai thang. 
        {
            
                if (checkInLine(column, row) == true)
                    return 1;
                if (checkDiagonalLine(column, row) == true)
                    return 1;
          
            return 0;

        }
        public bool checkInLine(int column, int row)  //check da co 5 o lien tiep o hang hoac cot hay chua
        {
            int count = 0;
            if (column > 3)
            {
                count = 0;
                for (int i = column; i >= column - 4; i--)
                {
                    if (Board[i, row] == Board[column, row])
                        count++;
                }
                if (count == 5)
                    return true;
            }
            if (column < 8)
            {
                count = 0;
                for (int i = column; i <= column + 4; i++)
                    if (Board[i, row] == Board[column, row])
                        count++;
                if (count == 5)
                    return true;
            }
            if (row > 3)
            {
                count = 0;
                for (int i = row; i >= row - 4; i--)
                {
                    if (Board[column, i] == Board[column, row])
                        count++;
                }
                if (count == 5)
                    return true;
            }
            if (row < 8)
            {
                count = 0;
                for (int i = row; i <= row + 4; i++)
                    if (Board[column, i] == Board[column, row])
                        count++;
                if (count == 5)
                    return true;
            }
            return false;
        }
        public bool checkDiagonalLine(int column, int row)  //check da co 5 o lien tiep o duong cheo hay chua
        {
            int count = 0;
            if (column > 3 && row > 3)
            {
                count = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (Board[column - i, row - i] == Board[column, row])
                        count++;
                }
                if (count == 5)
                    return true;
            }
            if (column < 8 && row < 8)
            {
                count = 0;
                for (int i = 0; i < 5; i++)
                    if (Board[column + i, row + i] == Board[column, row])
                        count++;
                if (count == 5)
                    return true;
            }
            if (column > 3 && row < 8)
            {
                count = 0;
                for (int i = 0; i < 5; i++)
                    if (Board[column - i, row + i] == Board[column, row])
                        count++;
                if (count == 5)
                    return true;
            }
            if (column < 8 && row > 3)
            {
                for (int i = 0; i < 5; i++)
                    if (Board[column + i, row - i] == Board[column, row])
                        count++;
                if (count == 5)
                    return true;
            }
            return false;
        }
    }
}
