using System;

namespace Server.Domain
{
  public class Plate
  {
    public Alliance Alliance { get; private set; }
    public bool IsOwned { get; private set; }
    public event EventHandler Owned;
    public event EventHandler Unowned;

    public Plate(Alliance alliance, bool isOwned)
    {
      Alliance = alliance;
      IsOwned = isOwned;
    }

    public virtual void OnOwned(EventArgs e)
    {
      if (!IsOwned)
      {
        IsOwned = true;
        EventHandler handler = Owned;
        if (handler != null)
        {
            handler(this, e);
        }
      }
    }    

    public virtual void OnUnowned(EventArgs e)
    {
      if (IsOwned)
      {
        IsOwned = false;
        EventHandler handler = Unowned;
        if (handler != null)
        {
            handler(this, e);
        }
      }
    }
  }
}