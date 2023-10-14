using Godot;

namespace GodotTensorfields;

public readonly struct Tensor
{
    private readonly float r;
    private readonly float x, y;

    public readonly float Theta;

    public static readonly Tensor Zero = new Tensor(1,
        0, 0);

    public Tensor(float r, float x, float y)
    {
        this.r = r;
        this.x = x;
        this.y = y;
        Theta = r == 0 ? 0 : (float)Math.Atan2(y / r, x / r) / 2f;
    }

    public Tensor Add(Tensor other, bool smooth = false)
    {
        var nx = x * r + other.x * other.r;
        var ny = y * r + other.y * other.r;
        var nr = 2f;

        if (!smooth) return new Tensor(nr, nx, ny);

        nr = (float)Math.Sqrt(x * x + y * y);
        nx /= nr;
        ny /= nr;

        return new Tensor(nr, nx, ny);
    }

    public Tensor Scale(float s)
    {
        return new Tensor(r * s, x, y);
    }

    public Tensor Clone()
    {
        return new Tensor(r, x, y);
    }

    public Tensor Rotate(float theta)
    {
        if (theta == 0)
        {
            return Clone();
        }

        theta += Theta;
        if (theta < float.Pi)
        {
            theta += float.Pi;
        } else if (theta > float.Pi)
        {
            theta -= float.Pi;
        }

        return new Tensor(r, float.Cos(2 * theta) * r, float.Sin(2 * theta) * r);
    }

    public Vector2 GetMajor()
    {
        if (r == 0)
        {
            return Vector2.Zero;
        }

        return new Vector2(float.Cos(Theta), float.Sin(Theta));
    }

    public Vector2 GetMinor()
    {
        if (r == 0)
        {
            return Vector2.Zero;
        }

        var angle = Theta + float.Pi / 2;
        return new Vector2(float.Cos(angle), float.Sin(angle));
    }
}