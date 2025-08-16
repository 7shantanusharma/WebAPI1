namespace WebAPITest
{
    public class UnitTest1
    {
        [Fact]
        public void Add_ShouldReturnSum()
        {
            Assert.Equal(5, 2 + 3);
        }

        [Fact]
        public void Add_ShouldReturnSum2()
        {
            Assert.Equal(4, 2 + 2);
        }

        [Theory]
        [InlineData(2, 3, 5)]
        [InlineData(10, 5, 15)]
        public void Add_TheoryTests(int a, int b, int expected)
        {
            Assert.Equal(expected, a + b);
        }
    }
}