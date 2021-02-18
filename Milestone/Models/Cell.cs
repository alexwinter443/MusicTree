using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/**
 * @author Alex Vergara and Kacey Morris
 * February 17, 2021
 * CST 247 Milestone 2
 * Minesweeper Web App Basic Game Board
 * Cell.cs
 * 
 * This class creates individual cells for the board. It defines the properties of a cell and the different constructors.
 * 
 * This is our own work, as influenced by class time and video tutorials for previous projects.
 */

namespace Milestone.Models
{
    public class Cell
    {
        // NEW
        private int ID = -1;
        // initialize properties with default values
        private int RowNumber = -1;
        private int ColumnNumber = -1;
        private bool Visited = false;
        private bool Live = false;
        private int Neighbors = 0;
        private bool Flagged = false;
        // for when the board is created, the cells only have locations
        public Cell(int row, int col)
        {
            // set the row and column number
            RowNumber = row;
            ColumnNumber = col;
            Visited = false;
        }
        // data constructor with all possible parameters
        public Cell(int row, int col, bool visited, bool live, int neighbors)
        {
            // assign passed values to properties
            SetRowNumber(row);
            SetColumnNumber(col);
            SetVisited(visited);
            SetLive(live);
            SetNeighbors(neighbors);
        }
        // getters and setters for all properties
        public int GetRowNumber()
        {
            return RowNumber;
        }

        public void SetRowNumber(int value)
        {
            RowNumber = value;
        }

        public int GetColumnNumber()
        {
            return ColumnNumber;
        }

        public void SetColumnNumber(int value)
        {
            ColumnNumber = value;
        }

        public bool GetVisited()
        {
            return Visited;
        }

        public void SetVisited(bool value)
        {
            Visited = value;
        }

        public bool GetLive()
        {
            return Live;
        }

        public void SetLive(bool value)
        {
            Live = value;
        }

        public int GetNeighbors()
        {
            return Neighbors;
        }

        public void SetNeighbors(int value)
        {
            Neighbors = value;
        }

        public bool GetFlagged()
        {
            return Flagged;
        }

        public void SetFlagged(bool value)
        {
            Flagged = value;
        }
        public int GetID()
        {
            return ID;
        }

        public void SetID(int value)
        {
            ID = value;
        }

    }
}
