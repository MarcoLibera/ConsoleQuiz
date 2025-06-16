namespace ConsoleQuizUnitTests
{
    public class FileNameTests
    {
        [Fact]
        public void FileNameSpecified()
        {
            Assert.True(false);
        }

        /* This is how to create parameterized tests in xUnit.
         * The InlineData attribute allows you to specify parameters for the test method.
         * Each InlineData attribute represents a separate test case.
         * Can pass one or more parameters to the test method.
         * Paramters are specified within the brackets of the InlineData attribute
         * and correspond to the method parameters.
         */
        //[Theory]
        //[InlineData(1, 2)]
        //[InlineData(2, 2)]
        //[InlineData(3, 2)]
        //public void test2(int value1, int value2)
        //{
        //    Assert.True(value1 == value2);
        //}
    }
}