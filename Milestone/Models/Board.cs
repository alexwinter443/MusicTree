using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/**
 * @author Alex Vergara and Kacey Morris
 * February 17, 2021
 * CST 227 Milestone 2
 * Minesweeper Web App Basic Game Board
 * Board.cs
 * 
 * This class creates the board for the minesweeper game. The properties are defined with getters and setters and
 * methods to setup the bombs and calculate the live neigbors are created. Methods to check for a win
 * and flood fill with safe squares are also present. 
 * 
 * This is my own work, as influenced by class time and video tutorials for previous projects.
 */

namespace Milestone.Models
{
    public class Board
    {
        // the size of the board
        private int Size;

        // difficulty will be an integer between 0 and 100, which will determine the percentage of bombs
        private int Difficulty;

        // 2d array of type cell
        private Cell[,] theGrid;

        // need a list for display on the webpage
        private List<Cell> cellList;

        // properties to keep track
        private int numberOfBombs;
        private int bombsDiscovered = 0;
        private int nonBombsDiscovered = 0;

        // constructor which takes the size and difficulty of the board

        public Board(int size, int difficulty)
        {
            // initial size of the board is defined by s 
            Size = size;
            Difficulty = difficulty;

            // create a new 2D array of type cell
            theGrid = new Cell[Size, Size];

            // create new list
            cellList = new List<Cell>();

            // fill the 2D array with new Cells, each with unique x and y coordiates
            // will set the ID's for each cell from 0 to 99, should correspond to index in list
            int k = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    theGrid[i, j] = new Cell(i, j);
                    theGrid[i, j].SetID(k);
                    cellList.Add(theGrid[i, j]);
                    k++;
                }
            }
        }
        // getters and setters for all properties
        public int GetSize()
        {
            return Size;
        }
        public void SetSize(int value)
        {
            Size = value;
        }
        public int GetDifficulty()
        {
            return Difficulty;
        }
        public void SetDifficulty(int value)
        {
            Difficulty = value;
        }
        public Cell[,] GetGrid()
        {
            return theGrid;
        }
        public void SetGrid(Cell[,] value)
        {
            theGrid = value;
        }
        public int GetNumberOfBombs()
        {
            return numberOfBombs;
        }
        public void SetNumberOfBombs(int value)
        {
            numberOfBombs = value;
        }
        public int GetBombsDiscovered()
        {
            return bombsDiscovered;
        }
        public void SetBombsDiscovered(int value)
        {
            bombsDiscovered = value;
        }
        public int GetNonBombsDiscovered()
        {
            return nonBombsDiscovered;
        }
        public void SetNonBombsDiscovered(int value)
        {
            nonBombsDiscovered = value;
        }
        public List<Cell> GetCellList()
        {
            return cellList;
        }
        public void SetCellList(List<Cell> value)
        {
            cellList = value;
        }
        public void setupLiveNeighbors()
        {
            // random to generate location of bombs
            Random rand = new Random();
            // generate a random row and column, from 0 to the size of the board
            int randomRow = rand.Next(Size);
            int randomCol = rand.Next(Size);
            // the percentage will be a double value that will determine the number of bombs
            double percentage = Difficulty / 100.0;
            // Console.WriteLine("Difficulty Percentage: " + percentage);
            // the number of bombs will be an integer determined by the total number of cells on the board
            // multiplied by the percentage of cells that are bombs based on the difficulty, forced to be an int
            int numLiveCells = Convert.ToInt32((Size * Size) * percentage);
            // Console.WriteLine("Number of live cells: " + numLiveCells);
            // for however many bombs there are, choose random locations for them
            for (int i = 0; i < numLiveCells; i++)
            {
                // if the cell is already live, we want to keep generating more random locations
                while (theGrid[randomRow, randomCol].GetLive())
                {
                    randomRow = rand.Next(Size);
                    randomCol = rand.Next(Size);
                }
                // set that cell to live
                theGrid[randomRow, randomCol].SetLive(true);
                // Console.WriteLine("Cell at (" + randomRow + ", " + randomCol + ") is live.");
            }
        }
        public void calculateLiveNeighbors()
        {
            // loop through all cells on the board
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Cell currentCell = theGrid[i, j];
                    // if the current cell is live, it has 9 live neighbors as per the directions
                    if (currentCell.GetLive())
                    {
                        currentCell.SetNeighbors(9);
                    }
                    else
                    {
                        // set default values off the board so an accidental neighbor is not counted 
                        int manipulatedRow = -1;
                        int manipulatedCol = -1;
                        // set the neighbor counter to 0 for each cell to start
                        int neighborCount = 0;
                        // there are 8 possible neighbors for each cell, so loop through 8 times
                        for (int x = 0; x < 8; x++)
                        {
                            // on each loop through, check a different neighboring cell for a bomb
                            switch (x)
                            {
                                // each of these cases chooses a cell immediately surrounding the current cell
                                case 0:
                                    manipulatedRow = currentCell.GetRowNumber() + 1;
                                    manipulatedCol = currentCell.GetColumnNumber() + 1;
                                    break;
                                case 1:
                                    manipulatedRow = currentCell.GetRowNumber() + 1;
                                    manipulatedCol = currentCell.GetColumnNumber();
                                    break;
                                case 2:
                                    manipulatedRow = currentCell.GetRowNumber() + 1;
                                    manipulatedCol = currentCell.GetColumnNumber() - 1;
                                    break;
                                case 3:
                                    manipulatedRow = currentCell.GetRowNumber();
                                    manipulatedCol = currentCell.GetColumnNumber() - 1;
                                    break;
                                case 4:
                                    manipulatedRow = currentCell.GetRowNumber();
                                    manipulatedCol = currentCell.GetColumnNumber() + 1;
                                    break;
                                case 5:
                                    manipulatedRow = currentCell.GetRowNumber() - 1;
                                    manipulatedCol = currentCell.GetColumnNumber() + 1;
                                    break;
                                case 6:
                                    manipulatedRow = currentCell.GetRowNumber() - 1;
                                    manipulatedCol = currentCell.GetColumnNumber();
                                    break;
                                case 7:
                                    manipulatedRow = currentCell.GetRowNumber() - 1;
                                    manipulatedCol = currentCell.GetColumnNumber() - 1;
                                    break;
                                default:
                                    break;
                            }
                            // after the neighboring cell is selected, make sure it is on the board and check if it is live
                            if ((manipulatedRow < Size) && (manipulatedRow >= 0) && (manipulatedCol < Size) && (manipulatedCol >= 0)
                                && theGrid[manipulatedRow, manipulatedCol].GetLive())
                            {
                                // if live, up the bomb counter
                                neighborCount++;
                            }
                        }
                        // after all neigboring cells are looped through, set the neighbor count for that cell
                        currentCell.SetNeighbors(neighborCount);
                    }
                }
            }
        }
        // checks for a winning condition
        public Boolean checkForWin()
        {
            // Console.WriteLine("Checking for win");
            for (int i = 0; i < this.GetSize(); i++)
            {
                for (int j = 0; j < this.GetSize(); j++)
                {
                    // if there are spots which are not bombs and not visited, the user has not won
                    if (!this.GetGrid()[i, j].GetVisited() && !this.GetGrid()[i, j].GetLive())
                    {
                        // Console.WriteLine("No win.");
                        return false;
                    }
                }
            }
            // Console.WriteLine("Win found.");
            return true;
        }

        public void floodFill(int row, int col)
        {
            // different possible combination of surrounding cells
            int[] xMove = { 1, 1, 1, 0, -1, -1, -1, 0 };
            int[] yMove = { 1, 0, -1, -1, -1, 0, 1, 1 };

            int next_x, next_y;

            // make sure we have visited the cell
            // if the cell has neighbors, those will already show up on the print now
            this.GetGrid()[row, col].SetVisited(true);

            // if the spot had been flagged, set it to not flagged and decrement the non bombs discovered
            if (this.GetGrid()[row, col].GetFlagged())
            {
                this.GetGrid()[row, col].SetFlagged(false);
                this.SetNonBombsDiscovered(this.GetNonBombsDiscovered() - 1);
            }

            // if it is blank, we want to check its surrounding cells too
            if (this.GetGrid()[row, col].GetNeighbors() == 0)
            {
                // for all surrounding cells
                for (int k = 0; k < 8; k++)
                {
                    next_x = row + xMove[k];
                    next_y = col + yMove[k];
                    // check to make sure it is safe (in the grid and not yet visited)
                    // Console.WriteLine("Checking if (" + next_x  + ", " + next_y + ") is safe.");
                    if (safeSquare(next_x, next_y))
                    {
                        // Console.WriteLine("Safe.");
                        // recursion, call this method on the next new choices
                        floodFill(next_x, next_y);
                    }
                }
            }
        }

        bool safeSquare(int x, int y)
        {
            // check to see if x, y is on the board. no out of bounds index errors allowed
            // do we care if the cell has already been visited? We do. Stack overflow.
            return (x >= 0 && x < this.GetSize() && y >= 0 && y < this.GetSize() && !this.GetGrid()[x, y].GetVisited());
        }
    }
}
