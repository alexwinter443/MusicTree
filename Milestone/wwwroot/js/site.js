// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/*
 * Kacey Morris
 * Alex Vergara
 * March 15 2021
 * CST 247
 * Milestone 3 - Flags and Game Over
 * 
 * This file does all ajax functionality, like partial page updates and right click functions. 
 * 
 * This is my own work as influenced by class time and examples.
 */

$(function () {
    console.log("Page is ready for ajax");

    // to help with right click 
    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
        console.log("Right click. Prevent context menu from showing.");
    });

    // when the game is over, do not let the user continue playing 
    $(document).on("mousedown", ".game-button-no-function", function (event) {
        event.preventDefault();
        alert("Game over. Please start a new game.");
    });

    // to distinguish between left and right clicks 
    $(document).on("mousedown", ".game-button", function (event) {
        switch (event.which) {
            // left click 
            case 1:
                event.preventDefault();
                // value of the button is the ID as well
                var cellNum = $(this).val();
                console.log("Button number " + cellNum + " was left clicked.");
                // call the ajax method passing the desired controller method
                doGridUpdate(cellNum);
                break;
            // middle 
            case 2:
                e.preventDefault();
                alert('Middle mouse button is pressed.');
                break;
            // right click 
            case 3:
                event.preventDefault();
                // value of the button is the ID as well
                var cellNum = $(this).val();
                console.log("Button number " + cellNum + " was right clicked.");
                // call the ajax method passing the desired controller method
                doButtonUpdate(cellNum, '/Cell/onRightButtonClick');
                // log what is happening
                console.log("After doButtonUpdate for Right click");
                break;
            default:
                alert('nothing');
                break;
        }
    });
});

// do button now takes a url to distinguish between desired controller methods 
function doButtonUpdate(cellNum, urlString) {
    $.ajax({
        async: true,
        dataType: "json",
        type: "POST",
        url: urlString,
        data: {
            "cellNum": cellNum
        },
        success: function (data) {
            console.log(data);
            // replace specific location of button
            // each button is contained in div connected to ID
            $("#" + cellNum).html(data.part1);
            // ternary operators to check for game win and loss conditions
            (data.part2 == "true") ? alert("WINNER! Thank you for playing.") : (data.part2 == "false") ? alert("GAME OVER. You have selected a bomb! Better luck next time.") : "";
        }
    });
}

// this updates the entire grid to account for flood fill
function doGridUpdate(cellNum) {
    $.ajax({
        async: true,
        dataType: "json",
        type: "POST",
        url: '/Cell/HandleButtonClick',
        data: {
            "cellNum": cellNum
        },
        success: function (data) {
            console.log(data);
            // replace entire grid 
            $("#button-zone").html(data.part1);
            // ternary operator to distinguish between win and loss conditions
            (data.part2 == "true") ? alert("WINNER! Thank you for playing.") : (data.part2 == "false") ? alert("GAME OVER. You have selected a bomb! Better luck next time.") : "";
        }
    });
}