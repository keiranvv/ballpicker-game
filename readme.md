# Ball Picker for the Win

Ball Picker for the win is a game in which the player grabs balls from a basket and receives either double their money, another turn, or nothing.

The player can choose to play one or ten rounds at a time.

This is just a demo so that player has unlimited funds, however the total win/loss record is displayed as the game goes on. Demo mode and starting balance can be changed in the `Program.cs` file.

## Prerequisites
- DotNet 8.0

## Running the game
Clone this repository and use your IDE or Console to run the application. For example in the terminal you can run the following command:
```bash
dotnet run
```

## Game rules
- A round consists of one or more picks from the basket, ending when the player picks either a win or a no_win ball.
- Each **pick** costs 10 credits to play.
- After each pick the ball is immediately put back into the basket, before picking another ball.
- A **win** will increase the player's balance by 20 credits.
- A **loss** will not award any credits.

## Playing the game
The game will give instructions on how to play. i.e. Play one round, play ten rounds, or quit.

After each pick, the result of the pick should be shown to the player.

After each round, the player's running earnings and spendings should be displayed along with the current RTP value.