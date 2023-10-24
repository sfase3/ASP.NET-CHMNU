namespace MyApp.services.CalcService
{
    public interface ICalcService
    {
        float Sum(float a, float b);

        float Multiply(float a, float b);

        float Subtract(float a, float b);

        float Divide(float a, float b);
    }
}
