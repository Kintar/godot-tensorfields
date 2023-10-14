using Godot;

namespace GodotTensorfields;

public enum BasisFieldType
{
    Radial,
    Grid,
}

public struct BasisField
{
    private readonly float size;
    private readonly float decay;
    private readonly float theta;
    public readonly BasisFieldType FieldType;
    public readonly Vector2 Center;
    
    private BasisField(Vector2 center, float size, float decay, BasisFieldType type, float theta)
    {
        this.size = size;
        Center = center;
        this.decay = decay;
        FieldType = type;
        this.theta = theta;
    }

    public static BasisField NewGrid(Vector2 center, float size, float decay, float theta)
    {
        return new BasisField(center, size, decay, BasisFieldType.Grid, theta);
    }

    public static BasisField NewRadial(Vector2 center, float size, float decay)
    {
        return new BasisField(center, size, decay, BasisFieldType.Radial, 0);
    }

    public float GetTensorWeight(Vector2 point, bool smooth = false)
    {
        var normalizedDistToCenter = (point - Center).Length() / size;
        if (smooth)
        {
            return float.Pow(normalizedDistToCenter, -decay);
        }

        if (decay == 0 && normalizedDistToCenter >= 1)
        {
            return 0;
        }
        
        return float.Max(0, float.Pow(1 - normalizedDistToCenter, decay));
    }

    public Tensor GetWeightedTensor(Vector2 point, bool smooth = false)
    {
        return GetTensor(point).Scale(GetTensorWeight(point, smooth));
    }
    
    public Tensor GetTensor(Vector2 position)
    {
        switch (FieldType)
        {
            case BasisFieldType.Grid:
                return GetGridTensor(position);
            case BasisFieldType.Radial:
                return GetRadialTensor(position);
            default:
                throw new Exception("somehow have a BasisField with an unknown field type!");
        }
    }

    private Tensor GetGridTensor(Vector2 position)
    {
        var twoTheta = theta * 2;
        return new Tensor(1, float.Cos(twoTheta), float.Sin(twoTheta));
    }

    private Tensor GetRadialTensor(Vector2 position)
    {
        var t = position - Center;
        var t1 = float.Pow(t.Y, 2) - float.Pow(t.X, 2);
        var t2 = -2 * t.X * t.Y;
        return new Tensor(1, t1, t2);
    }
}