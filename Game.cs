using System.Security.Cryptography;

internal class BallPickedEventArgs(double winnings, double spentCredits, double balance, string pickedBall) : EventArgs {
  internal readonly string PickedBall = pickedBall;
  internal readonly double SpentCredits = spentCredits;
  internal readonly double Winnings = winnings;
  internal readonly double Balance = balance;
}

internal class RoundEndedEventArgs(double winnings, double spentCredits, double balance) : EventArgs {
  internal readonly double SpentCredits = spentCredits;
  internal readonly double Winnings = winnings;
  internal readonly double Balance = balance;
}

internal class GameEndedEventArgs(double winnings, double spentCredits, double balance, int roundsPlayed) : EventArgs {
  internal readonly double SpentCredits = spentCredits;
  internal readonly double Winnings = winnings;
  internal readonly double Balance = balance;
  internal readonly int RoundsPlayed = roundsPlayed;
}

internal class Game(int? seed, double startingBalance = 0, bool demoMode = false) { 
  // Config
  private const int PICK_PRICE = 10;
  private const int WIN_AMOUNT = 20;
  private Dictionary<string, int> _balls = [];

  // Game Data
  private int roundsPlayed = 0;
  private double spentCredits = 0;
  private double winnings = 0;
  private double startingBalance = startingBalance;
  private double balance = startingBalance;

  // Events
  internal event EventHandler<GameEndedEventArgs>? GameEnded;
  internal virtual void OnGameEnded()
  {
    GameEnded?.Invoke(this, new GameEndedEventArgs(winnings, spentCredits, balance, roundsPlayed));
    Reset();
  }

  internal event EventHandler<BallPickedEventArgs>? BallPicked;
  internal virtual void OnBallPicked(string pickedBall)
  {
    spentCredits += PICK_PRICE;

    if (pickedBall == "win") {
      winnings += WIN_AMOUNT;

      if (!demoMode) {
        balance += WIN_AMOUNT;
      }
    }

    if (!demoMode) {
      balance -= PICK_PRICE;
    }

    BallPicked?.Invoke(this, new BallPickedEventArgs(winnings, spentCredits, balance, pickedBall));
  }

  internal event EventHandler<RoundEndedEventArgs>? RoundEnded;
  internal virtual void OnRoundEnded()
  {
    RoundEnded?.Invoke(this, new RoundEndedEventArgs(winnings, spentCredits, balance));
  }

  private readonly Random r = seed.HasValue ? new Random(seed.Value) : new Random();

  private void Reset() {
    _balls = new Dictionary<string, int>() {
      {"extra_pick", 1},
      {"win", 5},
      {"no_win", 14}
    };

    balance = startingBalance;
    roundsPlayed = 0;
    spentCredits = 0;
    winnings = 0;
  }

  // Select one of the balls based on their weight.
  // No need to really keep track of the ball counts now, as the ball is put back immedidately
  // after picking.
  private string Pick() {
    // Sort these so that we can start at the lowest weight and check if our roll was under that, going up until we find a value <= rolled value.
    // This can of course be done once when the ball are set, but it might then be unclear that it's required. The tradeoff would be a small amount of memory overhead.
    _balls.ToDictionary().OrderBy(x => x.Value);

    var numberOfBalls = _balls.Sum(b => b.Value);

    var roll = r.Next(numberOfBalls);

    var pickedBall = "";

    for (int i = 0; i < _balls.Count; i++) {
      if (_balls.ElementAt(i).Value <= roll) {
        pickedBall = _balls.ElementAt(i).Key;
      }
    }

    return pickedBall;
  }

  internal void PlayRound() {
    string? pickedBall;

    do {
      // Give the feeling of a game being played rather than just having everything happen instantly
      Thread.Sleep(500);

      roundsPlayed++;
      pickedBall = Pick();
      OnBallPicked(pickedBall);

      Thread.Sleep(500);

      if (balance - PICK_PRICE < 0) {
        End();
        break;
      }

    } while (pickedBall == "extra_pick");

    OnRoundEnded();
  }

  internal void PlayRounds(int rounds) {
    for (var i = 0; i < rounds; i++) {

      if (balance - PICK_PRICE < 0) {
        End();
        break;
      }

      PlayRound();
    }
  }

  internal void Start() {
    Reset();
  }

  internal void End() {
    // Maybe do something like save user history or perform final caluclations, etc.

    OnGameEnded();
  }
}