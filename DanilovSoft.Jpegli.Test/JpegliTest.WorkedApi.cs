namespace DanilovSoft.Jpegli.Test;

public partial class JpegliTest
{
    [Theory]
    [InlineData(200, 300, 300)]
    [InlineData(300, 200, 400)]
    public void JpegRoundUp(int a, int b, int expectedResult)
    {
        var result = Jpegli.JRoundUp(a, b);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(100, 0)]
    [InlineData(90, 20)]
    [InlineData(89, 22)]
    [InlineData(80, 40)]
    public void JpegQualityScaling(int quality, int expectedScaling)
    {
        var scaling = Jpegli.JpegQualityScaling(quality);
        Assert.Equal(expectedScaling, scaling);
    }
}
