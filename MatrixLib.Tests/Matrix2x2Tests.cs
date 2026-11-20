using System;
using System.IO;
using MatrixLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MatrixLib.Tests
{
    [TestClass]
    public class Matrix2x2Tests
    {
        // Тест: конструктор за замовчуванням створює нульову матрицю.
        [TestMethod]
        public void Constructor_Default_ReturnsZeroMatrix()
        {
            // Arrange
            var expected = new Matrix2x2(0, 0, 0, 0);
            // Act
            var actual = new Matrix2x2();
            // Assert
            Assert.AreEqual(expected, actual);
        }

        // Тест: конструктор з масиву неправильного розміру (3x3) генерує ArgumentException.
        [TestMethod]
        public void Constructor_ArrayOfWrongSize_ThrowsArgumentException()
        {
            // Arrange
            var wrong = new int[3, 3];
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => new Matrix2x2(wrong));
        }

        // Тест: конструктор копіювання створює незалежну копію (зміна копії не змінює оригінал).
        [TestMethod]
        public void Constructor_Copy_CreatesIndependentCopy()
        {
            // Arrange
            var original = new Matrix2x2(3, 1, 2, 4);
            // Act
            var copy = new Matrix2x2(original);
            copy[0, 0] = 100;
            // Assert
            Assert.AreEqual(3, original[0, 0]);
            Assert.AreEqual(100, copy[0, 0]);
        }

        // Тест: індексатор з індексом поза межами генерує IndexOutOfRangeException.
        [TestMethod]
        public void Indexer_IndexOutOfRange_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var m = new Matrix2x2(3, 1, 2, 4);
            // Act + Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => m[2, 0]);
        }

        // Тест: функція введення зчитує два рядки по два числа.
        [TestMethod]
        public void Input_TwoValidRows_FillsMatrix()
        {
            // Arrange
            var m = new Matrix2x2();
            var reader = new StringReader("1 2\n3 4");
            // Act
            m.Input(reader);
            // Assert
            Assert.AreEqual(new Matrix2x2(1, 2, 3, 4), m);
        }

        // Тест: функція введення з неправильною кількістю чисел у рядку генерує FormatException.
        [TestMethod]
        public void Input_WrongNumberCount_ThrowsFormatException()
        {
            // Arrange
            var m = new Matrix2x2();
            var reader = new StringReader("1 2 3\n4 5");
            // Act + Assert
            Assert.ThrowsException<FormatException>(() => m.Input(reader));
        }

        // Тест: ToString повертає матрицю у вигляді двох рядків.
        [TestMethod]
        public void ToString_Matrix_ReturnsTwoRows()
        {
            // Arrange
            var m = new Matrix2x2(3, 1, 2, 4);
            // Act
            string s = m.ToString();
            // Assert
            Assert.AreEqual(2, s.Split('\n').Length);
            StringAssert.Contains(s, "3");
        }

        // Тест: додавання двох матриць дає поелементну суму.
        [TestMethod]
        public void OperatorPlus_TwoMatrices_ReturnsElementwiseSum()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            var b = new Matrix2x2(1, 2, 0, -1);
            // Act
            var result = a + b;
            // Assert
            Assert.AreEqual(new Matrix2x2(4, 3, 2, 3), result);
        }

        // Тест: множення двох матриць за правилом "рядок на стовпець".
        [TestMethod]
        public void OperatorMultiply_TwoMatrices_ReturnsProduct()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            var b = new Matrix2x2(1, 2, 0, -1);
            // Act
            var result = a * b;
            // Assert
            Assert.AreEqual(new Matrix2x2(3, 5, 2, 0), result);
        }

        // Тест: множення матриць не комутативне (A*B != B*A).
        [TestMethod]
        public void OperatorMultiply_SwappedOperands_ReturnsDifferentResult()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            var b = new Matrix2x2(1, 2, 0, -1);
            // Act
            var ab = a * b;
            var ba = b * a;
            // Assert
            Assert.AreNotEqual(ab, ba);
        }

        // Тест: множення матриці на число (праворуч) множить кожен елемент.
        [TestMethod]
        public void OperatorMultiply_MatrixByNumber_MultipliesEachElement()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            // Act
            var result = a * 3;
            // Assert
            Assert.AreEqual(new Matrix2x2(9, 3, 6, 12), result);
        }

        // Тест: множення числа на матрицю (ліворуч) дає той самий результат.
        [TestMethod]
        public void OperatorMultiply_NumberByMatrix_SameAsMatrixByNumber()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            // Act
            var result = 3 * a;
            // Assert
            Assert.AreEqual(a * 3, result);
        }

        // Тест: визначник невиродженої матриці: 3*4 - 1*2 = 10.
        [TestMethod]
        public void OperatorTilde_NonSingularMatrix_ReturnsDeterminant()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            // Act
            int det = ~a;
            // Assert
            Assert.AreEqual(10, det);
        }

        // Тест: визначник виродженої матриці дорівнює 0.
        [TestMethod]
        public void OperatorTilde_SingularMatrix_ReturnsZero()
        {
            // Arrange
            var s = new Matrix2x2(1, 2, 2, 4);
            // Act
            int det = ~s;
            // Assert
            Assert.AreEqual(0, det);
        }

        // Тест: визначник добутку дорівнює добутку визначників.
        [TestMethod]
        public void OperatorTilde_ProductOfMatrices_EqualsProductOfDeterminants()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            var b = new Matrix2x2(1, 2, 0, -1);
            // Act
            int detOfProduct = ~(a * b);
            // Assert
            Assert.AreEqual(~a * ~b, detOfProduct);
        }

        // Тест: транспонування міняє місцями елементи [0,1] і [1,0].
        [TestMethod]
        public void OperatorNot_Matrix_ReturnsTransposed()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            // Act
            var t = !a;
            // Assert
            Assert.AreEqual(new Matrix2x2(3, 2, 1, 4), t);
        }

        // Тест: подвійне транспонування повертає вихідну матрицю.
        [TestMethod]
        public void OperatorNot_AppliedTwice_ReturnsOriginal()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            // Act
            var result = !!a;
            // Assert
            Assert.AreEqual(a, result);
        }

        // Тест: невироджена матриця в умові інтерпретується як true.
        [TestMethod]
        public void OperatorTrue_NonSingularMatrix_IsTrue()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            // Act
            bool result = a ? true : false;
            // Assert
            Assert.IsTrue(result);
        }

        // Тест: вироджена матриця в умові інтерпретується як false.
        [TestMethod]
        public void OperatorFalse_SingularMatrix_IsFalse()
        {
            // Arrange
            var s = new Matrix2x2(1, 2, 2, 4);
            // Act
            bool result = s ? true : false;
            // Assert
            Assert.IsFalse(result);
        }

        // Тест: неявне приведення числа k дає скалярну матрицю k*E.
        [TestMethod]
        public void ImplicitCast_FromInt_ReturnsScalarMatrix()
        {
            // Arrange
            int k = 5;
            // Act
            Matrix2x2 m = k;
            // Assert
            Assert.AreEqual(new Matrix2x2(5, 0, 0, 5), m);
        }

        // Тест: неявне приведення масиву 2x2 створює матрицю з тими самими елементами.
        [TestMethod]
        public void ImplicitCast_FromArray_ReturnsMatrixWithSameElements()
        {
            // Arrange
            var arr = new int[,] { { 1, 2 }, { 3, 4 } };
            // Act
            Matrix2x2 m = arr;
            // Assert
            Assert.AreEqual(new Matrix2x2(1, 2, 3, 4), m);
        }

        // Тест: явне приведення до масиву повертає незалежну копію.
        [TestMethod]
        public void ExplicitCast_ToArray_ReturnsIndependentCopy()
        {
            // Arrange
            var m = new Matrix2x2(1, 2, 3, 4);
            // Act
            int[,] arr = (int[,])m;
            arr[0, 0] = 99;
            // Assert
            Assert.AreEqual(1, m[0, 0]);
            Assert.AreEqual(3, arr[1, 0]);
        }

        // Тест: множення на одиничну матрицю не змінює матрицю.
        [TestMethod]
        public void OperatorMultiply_ByIdentity_ReturnsSameMatrix()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            Matrix2x2 e = 1;
            // Act
            var result = a * e;
            // Assert
            Assert.AreEqual(a, result);
        }

        // Тест: == для однакових матриць повертає true.
        [TestMethod]
        public void OperatorEquals_EqualMatrices_ReturnsTrue()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            var b = new Matrix2x2(3, 1, 2, 4);
            // Act
            bool result = a == b;
            // Assert
            Assert.IsTrue(result);
        }

        // Тест: != для різних матриць повертає true.
        [TestMethod]
        public void OperatorNotEquals_DifferentMatrices_ReturnsTrue()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            var b = new Matrix2x2(1, 2, 0, -1);
            // Act
            bool result = a != b;
            // Assert
            Assert.IsTrue(result);
        }

        // Тест: порівняння з null повертає false.
        [TestMethod]
        public void OperatorEquals_WithNull_ReturnsFalse()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            // Act
            bool result = a == null;
            // Assert
            Assert.IsFalse(result);
        }

        // Тест: рівні матриці мають однаковий хеш-код.
        [TestMethod]
        public void GetHashCode_EqualMatrices_ReturnsSameHash()
        {
            // Arrange
            var a = new Matrix2x2(3, 1, 2, 4);
            var b = new Matrix2x2(a);
            // Act
            int h1 = a.GetHashCode();
            int h2 = b.GetHashCode();
            // Assert
            Assert.AreEqual(h1, h2);
        }
    }
}
