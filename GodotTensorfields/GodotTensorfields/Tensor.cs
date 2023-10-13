using Microsoft.VisualBasic;

namespace GodotTensorfields;

public class Tensor
{
    private bool oldTheta;
    private readonly float r;
    private readonly float[] matrix;
    private double theta;
    
    public double Theta
    {
        get
        {
            if (oldTheta)
            {
                theta = CalculateTheta();
                oldTheta = false;
            }

            return theta;
        }
        
        private set => theta = value;
    }


    public static readonly Tensor Zero = new Tensor(1,
        new float[] { 0, 0 });

    public Tensor(float r, float[] matrix)
    {
        if (matrix.Length != 2)
        {
            throw new ArgumentException("matrix must be 2 elements");
        }

        this.r = r;
        this.matrix = matrix;
        oldTheta = false;
        theta = CalculateTheta();
    }
    

    private double CalculateTheta()
    {
        if (r == 0)
        {
            return 0;
        }

        return Math.Atan2(matrix[1] / r, matrix[0] / r) / 2f;
    }

    public Tensor Add(Tensor other, bool smooth = false)
    {
        var newMatrix = matrix;
        var newR = 2f;
        for (var i = 0; i < 2; i++)
        {
            matrix[i] = matrix[i] * r + other.matrix[i] * other.r;
        }

        if (smooth)
        {
            newR = (float)Math.Sqrt(matrix[0] * matrix[0] + matrix[1] * matrix[1]);
            newMatrix[0] /= newR ;
            newMatrix[1] /= newR ;
        }

        return new Tensor(newR , newMatrix);
    }
}