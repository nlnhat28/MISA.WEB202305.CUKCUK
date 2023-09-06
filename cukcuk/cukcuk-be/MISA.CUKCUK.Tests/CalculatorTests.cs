//using MISA.CUKCUK.Api;

//namespace MISA.CUKCUK.Tests
//{
//    /// <summary>
//    /// Unit test class calculator 
//    /// </summary>
//    /// Created by: nlnhat (13/07/2023)
//    [Ignore("Tạm thời ignore")]
//    [TestFixture]
//    public class CalculatorTests
//    {
//        /// <summary>
//        /// Check test case khi ký tự hợp lệ
//        /// </summary>
//        /// <param name="text">Chuỗi đầu vào</param>
//        /// <param name="expected">Kết quả mong muốn</param>
//        /// Created by: nlnhat (13/07/2023)
//        [TestCase("", 0)]
//        [TestCase("1", 1)]
//        [TestCase("1, 2, 3", 6)]
//        [TestCase("10,,2,,2000", 2012)]
//        public void Add_ValidInput_ValidResult(string text, int expected)
//        {
//            var actual = Calculator.Add(text);
//            Assert.That(actual, Is.EqualTo(expected));
//        }
//        /// <summary>
//        /// Test case khi có ký tự không hợp lệ
//        /// </summary>
//        /// Created by: nlnhat (13/07/2023)
//        [Test]
//        public void Add_InValidInput_ReturnsException()
//        {
//            var text = "1,3.5,  five";
//            var expectedExceptionMessage = "Chuỗi không hợp lệ: 3.5 five";
//            var actualException = Assert.Throws<Exception>(() => Calculator.Add(text));
//            Assert.That(actualException.Message, Is.EqualTo(expectedExceptionMessage));
//        }
//        /// <summary>
//        /// Test case khi có số âm
//        /// </summary>
//        /// Created by: nlnhat (13/07/2023)
//        [Test]
//        public void Add_NegativeInput_ReturnsException()
//        {
//            var text = "2,5 , -10,       -2, 28";
//            var expectedExceptionMessage = "Không chấp nhận toán tử âm: -10, -2";
//            var actualException = Assert.Throws<Exception>(() => Calculator.Add(text));
//            Assert.That(actualException.Message, Is.EqualTo(expectedExceptionMessage));
//        }
//        /// <summary>
//        /// Test case hàm cộng 2 số
//        /// </summary>
//        /// <param name="x">Số thứ nhất</param>
//        /// <param name="y">Số thứ hai</param>
//        /// <param name="expectedResult">Giá trị mong muốn</param>
//        /// Created by: nlnhat (13/07/2023)
//        [TestCase(1, 2, 3)]
//        [TestCase(2, 3, 5)]
//        [TestCase(int.MaxValue, 1, int.MaxValue + (long)1)]
//        public void Add_ValidInput_ValidResult(int x, int y, long expectedResult)
//        {
//            var actualResult = Calculator.Add(x, y);
//            Assert.That(actualResult, Is.EqualTo(expectedResult));
//        }
//        /// <summary>
//        /// Test case hàm trừ 2 số
//        /// </summary>
//        /// <param name="x">Số bị trừ</param>
//        /// <param name="y">Số trừ</param>
//        /// <param name="expectedResult">Giá trị mong muốn</param>
//        /// Created by: nlnhat (13/07/2023)
//        [TestCase(1, 2, -1)]
//        [TestCase(2, 3, -1)]
//        [TestCase(int.MaxValue, int.MinValue, int.MaxValue - (long)int.MinValue)]
//        public void Sub_ValidInput_ValidResult(int x, int y, long expectedResult)
//        {
//            var actualResult = Calculator.Sub(x, y);
//            Assert.That(actualResult, Is.EqualTo(expectedResult));
//        }
//        /// <summary>
//        /// Test case hàm nhân 2 số
//        /// </summary>
//        /// <param name="x">Số thứ nhất</param>
//        /// <param name="y">Số thứ hai</param>
//        /// <param name="expectedResult">Giá trị mong muốn</param>
//        /// Created by: nlnhat (13/07/2023)
//        [TestCase(1, 2, 2)]
//        [TestCase(2, 3, 6)]
//        [TestCase(int.MaxValue, int.MinValue, int.MaxValue * (long)int.MinValue)]
//        public void Mul_ValidInput_ValidResult(int x, int y, long expectedResult)
//        {
//            var actualResult = Calculator.Mul(x, y);
//            Assert.That(actualResult, Is.EqualTo(expectedResult));
//        }
//        /// <summary>
//        /// Test case khi hàm chia số chia bằng 0
//        /// </summary>
//        /// Created by: nlnhat (13/07/2023)
//        [Test]
//        public void Div_DivideByZero_ReturnsException()
//        {
//            var x = 1;
//            var y = 0;
//            var expectedExceptionMessage = "Không chia được cho 0";

//            var exception = Assert.Throws<Exception>(() => Calculator.Div(x, y));

//            Assert.That(exception.Message, Is.EqualTo(expectedExceptionMessage));
//        }
//        /// <summary>
//        /// Test case hàm chia 2 số
//        /// </summary>
//        /// <param name="x">Số bị chia</param>
//        /// <param name="y">Số chia</param>
//        /// <param name="expectedResult">Giá trị mong muốn</param>
//        /// Created by: nlnhat (13/07/2023)
//        [TestCase(1, 2, 0.5)]
//        [TestCase(2, 3, 0.66666)]
//        public void Div_ValidInput_ValidResult(int x, int y, double expectedResult)
//        {
//            var actualResult = Calculator.Div(x, y);
//            Assert.That(Math.Abs(actualResult - expectedResult), Is.LessThan(1e-3));
//        }
//    }
//}