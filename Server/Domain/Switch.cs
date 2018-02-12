using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Server.Domain
{
  public class Switch
  {
    public Alliance Alliance { get; private set; }
    public bool IsOwned { get; private set; }
    public Plate LHSPlate { get; private set; }
    public Plate RHSPlate { get; private set; }
    private HttpClient httpClient;
    public Plate RedPlate {
      get
      {
        if (LHSPlate.Alliance.IsRed)
          return LHSPlate;
        else
          return RHSPlate;
      }
    }
    public Plate BluePlate {
      get
      {
        if (LHSPlate.Alliance.IsBlue)
          return LHSPlate;
        else
          return RHSPlate;
      }
    }
    public event EventHandler Owned;
    public event EventHandler Unowned;

    public Switch(SwitchSettings switchSettings)
    {
      Alliance = switchSettings.Alliance;
      LHSPlate = new Plate(switchSettings.LHSAlliance, false); // TODO: Read current state
      RHSPlate = new Plate(switchSettings.RHSAlliance, false); // TODO: Read current state
      httpClient = new HttpClient();
      httpClient.BaseAddress = new Uri($"http://{switchSettings.IP.ToString()}:80/");
      httpClient.DefaultRequestHeaders.Accept.Clear();
      httpClient.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
      LHSPlate.Owned += Plate_Owned;
      RHSPlate.Owned += Plate_Owned;
      LHSPlate.Unowned += Plate_Unowned;
      RHSPlate.Unowned += Plate_Unowned;
    }

    private void Plate_Owned(object sender, EventArgs e)
    {
      Plate plate = (Plate) sender;
      if (LHSPlate == plate && LHSPlate.Alliance == Alliance)
      {
        if (!IsOwned)
          OnOwned(EventArgs.Empty);
      }
      if (RHSPlate == plate && RHSPlate.Alliance == Alliance)
      {
        if (!IsOwned)
          OnOwned(EventArgs.Empty);
      }
    }

    private void Plate_Unowned(object sender, EventArgs e)
    {
      Plate plate = (Plate) sender;
      if (LHSPlate == plate && LHSPlate.Alliance == Alliance)
      {
        if (IsOwned)
          OnUnowned(EventArgs.Empty);
      }
      if (RHSPlate == plate && RHSPlate.Alliance == Alliance)
      {
        if (IsOwned)
          OnUnowned(EventArgs.Empty);
      }
    }

    protected virtual void OnOwned(EventArgs e)
    {
      IsOwned = true;
      EventHandler handler = Owned;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnUnowned(EventArgs e)
    {
      IsOwned = false;
      EventHandler handler = Unowned;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    public async Task Safe()
    {
      HttpResponseMessage response = await httpClient.PostAsync("", new StringContent("state=safe"));
      response.EnsureSuccessStatusCode();
    }

    public async Task StaffSafe()
    {
      HttpResponseMessage response = await httpClient.PostAsync("", new StringContent("state=staffsafe"));
      response.EnsureSuccessStatusCode();
    }

    public async Task Play()
    {
      HttpResponseMessage response = await httpClient.PostAsync("", new StringContent("state=play"));
      response.EnsureSuccessStatusCode();
    }

    public async Task Randomize()
    {
      HttpResponseMessage response = await httpClient.PostAsync("", new StringContent("state=randomize"));
      response.EnsureSuccessStatusCode();
    }

    public async Task Off()
    {
      HttpResponseMessage response = await httpClient.PostAsync("", new StringContent("state=off"));
      response.EnsureSuccessStatusCode();
    }
  }
}