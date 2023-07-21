public class FGAdInfo
{
    public string AdUnitIdentifier { get; private set; }
    public string AdFormat { get; private set; }
    public string NetworkName { get; private set; }
    public string NetworkPlacement { get; private set; }
    public string Placement { get; private set; }
    public string CreativeIdentifier { get; private set; }
    public double Revenue { get; private set; }
    public string RevenuePrecision { get; private set; }


    public FGAdInfo(string AdUnitIdentifier, string AdFormat, string NetworkName, string NetworkPlacement,
        string Placement, string CreativeIdentifier, double Revenue, string RevenuePrecision)
    {
        this.AdUnitIdentifier = AdUnitIdentifier;
        this.AdFormat = AdFormat;
        this.NetworkName = NetworkName;
        this.NetworkPlacement = NetworkPlacement;
        this.Placement = Placement;
        this.CreativeIdentifier = CreativeIdentifier;
        this.Revenue = Revenue;
        this.RevenuePrecision = RevenuePrecision;
    }

    public override string ToString()
    {
        return "[AdInfo adUnitIdentifier: " + AdUnitIdentifier +
               ", adFormat: " + AdFormat +
               ", networkName: " + NetworkName +
               ", networkPlacement: " + NetworkPlacement +
               ", creativeIdentifier: " + CreativeIdentifier +
               ", placement: " + Placement +
               ", revenue: " + Revenue +
               ", revenuePrecision: " + RevenuePrecision + "]";
    }
}