using Microsoft.AspNetCore.Mvc;
using Milestone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/**
 * @author Alex Vergara and Kacey Morris
 * February 17, 2021
 * CST 247 Milestone 2
 * Minesweeper Web App Basic Game Board
 * CellController.cs
 * 
 * This class controls all background for the game play. It sets up the game and returns views. 
 * 
 * This is our own work, as influenced by class time and video tutorials for previous projects.
 */

namespace Milestone.Controllers
{
    public class CellController : Controller
    {
        // reference to the class Board. Contains the values of the board.
        static Board myBoard;

        // booleans to allow for the game to continue and for a win
        Boolean continueGame = true;
        Boolean win = false;

        public IActionResult Index()
        {
            // pass the size and difficulty depending on level to create the board
            myBoard = new Board(10, 10);

            // set up the bombs
            myBoard.setupLiveNeighbors();
            // calculate all live neighbors
            myBoard.calculateLiveNeighbors();
            // set the number of bombs to what was passed through
            myBoard.SetNumberOfBombs(10);

            // pass list of cells to view
            return View("Index", myBoard.GetCellList());
        }
        public IActionResult HandleButtonClick(string cellID)
        {
            // convert from string to int
            int cellNum = int.Parse(cellID);

            myBoard.GetCellList();

            Cell currentCell = myBoard.GetCellList().ElementAt(cellNum);

            // visit cell
            currentCell.SetVisited(true);

            // call flood fill on that cell spot
            myBoard.floodFill(currentCell.GetRowNumber(), currentCell.GetColumnNumber());

            // if they selected a bomb, end the game and let them know
            if (currentCell.GetLive())
            {
                // set the game to false
                continueGame = false;
            }
            // if they cleared all non bomb spots, end the game and display win
            else if (myBoard.checkForWin())
            {
                // set win to true
                win = true;
                // stop the game
                continueGame = false;
            }

            // redisplay the button grid
            return View("Index", myBoard.GetCellList());
        }
    }
}
