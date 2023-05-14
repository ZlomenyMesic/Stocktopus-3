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
```uci```   
This command is used by the GUI to confirm the UCI protocol.

```isready```   
This command is used to synchronize the GUI with the engine.

```position```   
This is used to set a chess position. The second parameter must be either ```startpos``` or ```fen```.
The startpos parameter sets the starting position, fen sets a custom position using a FEN string (e.g.
```position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1```).
After the position is an optional list of moves from that position using the ```moves``` keyword:
```position startpos moves e2e4 e7e5```. The moves must be stored in a form of LAN (```square_from square_to optional_promotion```, e.g. ```b7b8q```).

```go```   
This starts the minimax search for the best move.
This is a list of all possible parameters:   
```infinite``` - this starts an infinite search which can be stopped with the stop command.
```wtime <value>``` - this sets the white player's time in miliseconds. Default is 600000.
```btime <value>``` - wtime but for the black player. Default is also 600000.
```winc <value>``` - white's time incrementation after each move in miliseconds. Default is 0.
```binc <value>``` - winc for the black player.
```movestogo <value>``` - number of moves to the next time control. Default is 20.

```stop```   

```setoption```

## Usage
