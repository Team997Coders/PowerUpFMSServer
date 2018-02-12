using System;

namespace Server.Domain
{
  public class Alliance : ValueObject<Alliance>
  {
    public const int Red = 1;
    public const int Blue = 2;
    private int _alliance;

    public bool IsRed { 
      get
      {
        return _alliance == Red;
      }
    }

    public bool IsBlue { 
      get
      {
        return _alliance == Blue;
      }
    }

    public int Is {
      get
      {
        return _alliance;
      }
    }

    public Alliance(int alliance)
    {
      if (IsValid(alliance))
        _alliance = alliance;
      else
        throw new ArgumentException("Alliance identifier is not valid.");
    }

    public static Alliance Parse(string input) {
      if (input.ToUpper() == "RED") {
        return new Alliance(Red);
      } 
      if (input.ToUpper() == "BLUE") {
        return new Alliance(Blue);
      }
      throw new ArgumentException("Not a valid alliance.");
    }

    public static bool TryParse(string input) {
      if (input.ToUpper() != "RED" && input.ToUpper() != "BLUE")
        return false;
      else
        return true;
    }

    public override string ToString()
    {
      if (_alliance == Red) {
        return "Red";
      }
      if (_alliance == Blue) {
        return "Blue";
      }
      throw new ArgumentException("Not a valid alliance.");
    }

    public Alliance OtherAlliance()
    {
      if (_alliance == Red) {
        return new Alliance(Blue);
      }
      if (_alliance == Blue) {
        return new Alliance(Red);
      }
      throw new ArgumentException("Not a valid alliance.");
    }

    public static bool IsValid(int alliance)
    {
      if (alliance == Red || alliance == Blue)
        return true;
      else
        return false;
    }
  }
}