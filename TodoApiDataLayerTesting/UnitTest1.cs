using System.Threading.Tasks;



namespace TodoApi.UnitTests
{
    public class CalculatorTests
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
        [Fact]
        public void TestAddMethod()
        {
            // Arrange
            var calculator = new CalculatorTests();

            // Act
            int result = calculator.Add(3, 5);

            // Assert
            Assert.Equal(8, result);
        }


    }
}
