namespace GodotTensorfields;

using Godot;


public abstract class BasisField
{
    public Vector2 center { get; protected set; }
    public abstract Tensor GetTensor(Vector2 point);
    
}