# Stocktopus-3
A simple UCI chess engine written in C#

## Features
* time control
* fast move generation using bitboards
* minimax with alpha-beta pruning
* transposition table
* evaluation based on the pieces' position
* modifiable openings book

## Commands
### ```uci```
This command is used by the GUI to confirm the UCI protocol.

### ```isready```   
This command is used to synchronize the GUI with the engine.

### ```position```   
This is used to set a chess position. The second parameter must be either ```startpos``` or ```fen```.
The startpos parameter sets the starting position, fen sets a custom position using a FEN string (e.g.
```position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1```).   
After the position is an optional list of moves from that position using the ```moves``` keyword:
```position startpos moves e2e4 e7e5```. The moves must be stored in a form of LAN (```square_from square_to optional_promotion```, e.g. ```b7b8q```).

### ```go```   
This starts the minimax search for the best move.
This is a list of all possible parameters:      
```infinite``` - this starts an infinite search which can be stopped with the stop command.   
```wtime <value>``` - this sets the white player's time in miliseconds. Default is 600000.   
```btime <value>``` - wtime but for the black player. Default is also 600000.   
```winc <value>``` - white's time incrementation after each move in miliseconds. Default is 0.   
```binc <value>``` - winc for the black player.   
```movestogo <value>``` - number of moves to the next time control. Default is 20.   

### ```stop```   
This force-stoppes the current search.

### ```setoption```
This allows the user to modify the engine's internal options.
UCI forces the second keyword to be ```name```, so the format is ```setoption name <option>```.
All options are toggleable, so they switch the values true and false.
The three available options are:   
```forceenpassant``` - this forces the engine to play en passant whenever it can.   
```randommoves``` - this makes the engine play random moves.   
```worstmoves``` - this makes the engine evaluate all moves and choose the worst move.   

## Usage
To use the engine, download Stocktopus-3.zip. The folder contains an .exe file, a .txt book file and two other important files (do not delete them!). The executable needs all three files to run.   
You can play either from the console, or using a GUI (I suggest [Cutechess](https://github.com/cutechess/cutechess/releases)).   

The book.txt file must be in the same folder as the executable. It is used to store openings and can be modified:   
* all lines starting with ```//``` are ignored.
* the format is ```<position> <move>```
* the position is stored using a custom system. After each ```position``` command, the engine displays the position in this format.
* the move is stored using a form of LAN as described in the ```position``` command section.
* if there are multiple possible moves from a position, the engine will choose a random one.
