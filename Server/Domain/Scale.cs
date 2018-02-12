using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Server.Domain
{
  public class Scale
  {
    public Alliance FieldLeftAlliance { get; private set; }
    public Plate LHSPlateAsAllianceFieldLeft { get; private set; }
    public Plate RHSPlateAsAllianceFieldLeft { get; private set; }
    public bool IsRedOwned { get; private set; }
    public bool IsBlueOwned { get; private set; }
    private HttpClient httpClient;


    public Plate RedPlate {
      get
      {
        if (LHSPlateAsAllianceFieldLeft.Alliance.IsRed)
          return LHSPlateAsAllianceFieldLeft;
        else
          return RHSPlateAsAllianceFieldLeft;
      }
    }
    public Plate BluePlate {
      get
      {
        if (LHSPlateAsAllianceFieldLeft.Alliance.IsBlue)
          return LHSPlateAsAllianceFieldLeft;
        else
          return RHSPlateAsAllianceFieldLeft;
      }
    }
    public event EventHandler BlueOwned;
    public event EventHandler BlueUnowned;
    public event EventHandler RedOwned;
    public event EventHandler RedUnowned;

    public Scale(ScaleSettings scaleSettings)
    {
      FieldLeftAlliance = scaleSettings.FieldLeftAlliance;
      LHSPlateAsAllianceFieldLeft = new Plate(scaleSettings.LHSAlliance(FieldLeftAlliance), false); //TODO: Get it in real time
      RHSPlateAsAllianceFieldLeft = new Plate(scaleSettings.RHSAlliance(FieldLeftAlliance), false); //TODO: Get it in real time
      httpClient = new HttpClient();
      httpClient.BaseAddress = new Uri($"http://{scaleSettings.IP.ToString()}:80/");
      httpClient.DefaultRequestHeaders.Accept.Clear();
      httpClient.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));      
      LHSPlateAsAllianceFieldLeft.Owned += Plate_Owned;
      RHSPlateAsAllianceFieldLeft.Owned += Plate_Owned;
      LHSPlateAsAllianceFieldLeft.Unowned += Plate_Unowned;
      RHSPlateAsAllianceFieldLeft.Unowned += Plate_Unowned;
    }

    private void Plate_Owned(object sender, EventArgs e)
    {
      Plate plate = (Plate) sender;
      if (plate == RedPlate)
        OnRedOwned(EventArgs.Empty);
      else
        OnBlueOwned(EventArgs.Empty);
    }

    private void Plate_Unowned(object sender, EventArgs e)
    {
      Plate plate = (Plate) sender;
      if (plate == RedPlate)
        OnRedUnowned(EventArgs.Empty);
      else
        OnBlueUnowned(EventArgs.Empty);
    }

    protected virtual void OnBlueOwned(EventArgs e)
    {
      IsBlueOwned = true;
      EventHandler handler = BlueOwned;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnBlueUnowned(EventArgs e)
    {
      IsBlueOwned = false;
      EventHandler handler = BlueUnowned;
      if (handler != null)
      {
        handler(this, e);
      }
    }    
    protected virtual void OnRedOwned(EventArgs e)
    {
      IsRedOwned = true;
      EventHandler handler = RedOwned;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnRedUnowned(EventArgs e)
    {
      IsRedOwned = false;
      EventHandler handler = RedUnowned;
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