namespace DanilovSoft.Jpegli.Test;

public partial class JpegliTest
{
    [Theory]
    [InlineData(200, 300, 300)]
    [InlineData(300, 200, 400)]
    public void JpegRoundUp(long a, long b, long expectedResult)
    {
        var result = Jpegli.JRoundUp(a, b);
        Assert.Equal(expectedResult, result);
    }
}
