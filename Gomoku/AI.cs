using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    class AI
    {
        public bool isWin = false;
        public bool isActi = false;
        public void play(chessBoard board)
        {
            if (isActi == false)
                return;
            else
            {
                //int count = 0;
                Cell check = null;
                for (int i = 0; i < 12; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if (board.Board[i, j] != 0)
                        {
                            check = checkInLine(i, j, 4, board);
                            if (check == null)
                                check = checkDiagonalLine(i, j, 4, board);
                            if (check != null)
                            {
                                board.play(check.c, check.r, 2);
                                if (board.checkWin(check.c, check.r) == 1)
                                    isWin = true;
                                return;
                            }
                        }
                    }
                }
                if(check == null)
                {
                    for (int i = 0; i < 12; i++)
                        for (int j = 0; j < 12; j++)
                        {
                            if (board.Board[i, j] != 0)
                            {
                                check = checkInLine(i, j, 3, board);
                                if (check == null)
                                    check = checkDiagonalLine(i, j, 3, board);
                                if (check != null)
                                {
                                    board.play(check.c, check.r, 2);
                                    return;
                                }
                            }
                        }
                }
                if (check == null)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            if (board.Board[i, j] != 0)
                            {
                                check = checkInLine(i, j, 2, board);
                                if (check == null)
                                    check = checkDiagonalLine(i, j, 2, board);
                                if (check != null)
                                {
                                    board.play(check.c, check.r, 2);
                                    return;
                                }
                            }
                        }
                    }
                }
                if (check == null)
                {
                    for (int i = 0; i < 12; i++)
                        for (int j = 0; j < 12; j++)
                        {
                            if (board.Board[i, j] != 0)
                            {
                                check = checkInLine(i, j, 1, board);
                                if (check == null)
                                    check = checkDiagonalLine(i, j, 1, board);
                                if (check != null)
                                {
                                    board.play(check.c, check.r, 2);
                                    return;
                                }
                            }
                        }
                }
                
                isActi = false;
                return;
            }
        }
        
        public Cell checkInLine(int column, int row, int n, chessBoard Board) // n la so o lien tiep danh 
        {
            int count = 0;
            if (column > n - 2) // dich trai
            {
                count = 0;
                for (int i = column; i >= column - n + 1; i--)
                {
                    if (Board.Board[i, row] == Board.Board[column, row])
                        count++;
                }
                if (count == n)
                {
                    if (Board.Board[column + 1, row] != 0 && column > n - 1)
                    {
                        Cell temp = new Cell() { c = column - n, r = row };
                        return temp;
                    }
                    else if (Board.Board[column + 1, row] == 0)
                    {
                        Cell temp = new Cell() { c = column + 1, r = row };
                        return temp;
                    }
                    else if (Board.Board[column + 1, row] != 0 && column == n - 1)
                    {
                        return null;
                    }
                }
            }
            if (column < 12 - n + 1)  // dich sang phai
            {
                count = 0;
                for (int i = column; i <= column + n - 1; i++)
                    if (Board.Board[i, row] == Board.Board[column, row])
                        count++;
                if (count == n)
                {
                    if ((Board.Board[column - 1, row] != 0 && column < 12 - n + 1 ))
                    {
                        Cell temp = new Cell() { c = column - 1, r = row };
                        return temp;
                    }
                    else if ( Board.Board[column - 1, row] == 0)
                    {
                        Cell temp = new Cell() { c = column - 1, r = row };
                        return temp;
                    }
                    
                }
            }
            if (row > n - 2) // dich len
            {
                count = 0;
                for (int i = row; i >= row - n + 1; i--)
                {
                    if (Board.Board[column, i] == Board.Board[column, row])
                        count++;
                }
                if (count == n)
                {
                    if (Board.Board[column, row + 1] != 0 && row > n - 1)
                    {
                        Cell temp = new Cell() { c = column, r = row - n };
                        return temp;
                    }
                    if (Board.Board[column, row + 1] == 0)
                    {
                        Cell temp = new Cell() { c = column, r = row + 1 };
                        return temp;
                    }
                }
            }
            if (row < 12 - n + 1)
            {
                count = 0;
                for (int i = row; i <= row + n - 1; i++)
                    if (Board.Board[column, i] == Board.Board[column, row])
                        count++;
                if (count == n)
                {
                    if (column > 0 && row > 0)
                    {
                        if (Board.Board[column, row - 1] != 0 && row < 12 - n + 1)
                        {
                            Cell temp = new Cell() { c = column, r = row + n };
                            return temp;
                        }
                        if (Board.Board[column, row - 1] == 0)
                        {
                            Cell temp = new Cell() { c = column, r = row - 1 };
                            return temp;
                        }
                    }
                }
            }
            return null;
        }
        public Cell checkDiagonalLine(int column, int row, int n, chessBoard Board)  //check da co 5 o lien tiep o duong cheo hay chua
        {
            int count = 0;
            if (column > n - 2 && row > n - 2)
            {
                count = 0;
                for (int i = 0; i < n; i++)
                {
                    if (Board.Board[column - i, row - i] == Board.Board[column, row])
                        count++;
                }
                if (count == n)
                {
                    if (Board.Board[column + 1, row + 1] != 0 && column > n - 1 && row > n - 1)
                    {
                        Cell temp = new Cell() { c = column - n, r = row - n };
                        return temp;
                    }
                    if (Board.Board[column + 1, row + 1] == 0)
                    {
                        Cell temp = new Cell() { c = column + 1, r = row + 1 };
                        return temp;
                    }
                }
            }
            if (column < 12 - n + 1 && row < 12 - n + 1)
            {
                count = 0;
                for (int i = 0; i < n; i++)
                    if (Board.Board[column + i, row + i] == Board.Board[column, row])
                        count++;
                if (count == n)
                {
                    if (Board.Board[column - 1, row - 1] != 0 && row < 12 - n - 2 && column < 12 - n - 2)
                    {
                        Cell temp = new Cell() { c = column + 5, r = row + 5 };
                        return temp;
                    }
                }
            }
                if (column > n - 2 && row < 12 - n + 1)
                {
                    count = 0;
                    for (int i = 0; i < n; i++)
                        if (Board.Board[column - i, row + i] == Board.Board[column, row])
                            count++;
                    if (count == n)
                    {
                        if (Board.Board[column + 1, row - 1] != 0 && column > n - 1 && row < 12 - n - 2)
                        {
                            Cell temp = new Cell() { c = column - 5, r = row + 5 };
                            return temp;
                        }
                        if (Board.Board[column + 1, row - 1] == 0)
                        {
                            Cell temp = new Cell() { c = column + 1, r = row - 1 };
                            return temp;
                        }
                    }
                }
                if (column < 12 - n + 1 && row > n - 2)
                {
                    for (int i = 0; i < n; i++)
                        if (Board.Board[column + i, row - i] == Board.Board[column, row])
                            count++;
                    if (count == n)
                    {
                        if (Board.Board[column - 1, row + 1] != 0 && row > n - 1 && column < 12 - n - 2)
                        {
                            Cell temp = new Cell() { c = column + 5, r = row - 5 };
                            return temp;
                        }
                        if (Board.Board[column - 1, row + 1] == 0)
                        {
                            Cell temp = new Cell() { c = column - 1, r = row + 1 };
                            return temp;
                        }
                    }
                }
                return null;
            }

        }
    
}
