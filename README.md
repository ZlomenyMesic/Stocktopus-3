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
After the position is an optional list of moves from that position after the ```moves``` keyword:
```position startpos moves e2e4 e7e5```. The moves must be in a form of LAN (```square_from square_to optional_promotion```, e.g. ```b7b8q```).

```go```   

```stop```   

```setoption```

## Usage
