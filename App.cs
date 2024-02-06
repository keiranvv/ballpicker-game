class App {
  private const int MULTIPLE_ROUNDS_COUNT = 10;
  private readonly string WELCOME_MESSAGE = $@"
Welcome to Ball Picker!
Commands:
=====================
q: Quit the game
r: Play another round
t: Play {MULTIPLE_ROUNDS_COUNT} rounds
=====================
";

  private readonly Game game;

  private void GameEnded(object? sender, GameEndedEventArgs e) {
    Console.WriteLine("Game over, thanks for playing!");
    Console.WriteLine("======================");
    Console.WriteLine($"Winnings: {e.Winnings}\nSpent: {e.SpentCredits}\nRounds played: {e.RoundsPlayed}");
    Console.WriteLine("======================");

    Environment.Exit(0);
  }

  private void BallPicked(object? sender, BallPickedEventArgs e) {
    Console.WriteLine($"Picked ball {e.PickedBall}! Balance {e.Balance}");
  }

  private void RoundEnded(object? sender, RoundEndedEventArgs e) {
    var RTP = Math.Round(e.Winnings / e.SpentCredits * 100, 2);
    
    Console.WriteLine($"Round played. Winnings: {e.Winnings} Spent: {e.SpentCredits} RTP: {RTP}");
  }

  internal App() : this(null, 100, false) { }

  // seed: Optional seed to enforce same results for potential testing
  // startingBalance: Self explanatory
  // demoMode: Balance is unaffected/unlimited
  internal App(int? seed, double startingBalance, bool demoMode) {
    game = new Game(seed, startingBalance, demoMode);

    game.GameEnded += GameEnded;
    game.BallPicked += BallPicked;
    game.RoundEnded += RoundEnded;
  }

  internal void WaitForInput() {
    var command = Console.ReadKey(true).Key;

    if (command == ConsoleKey.R) {
      Console.WriteLine("Playing another round...");
      game.PlayRound();
    } else if (command == ConsoleKey.T) {
      Console.WriteLine($"Playing {MULTIPLE_ROUNDS_COUNT} rounds...");
      game.PlayRounds(MULTIPLE_ROUNDS_COUNT);
    } if (command == ConsoleKey.Q) {
      return;
    }
    
    WaitForInput();
  }

  internal void Start() {
    Console.Write(WELCOME_MESSAGE);

    game.Start();

    WaitForInput();

    game.End();
  }
}