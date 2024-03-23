using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using laba2AOIS;
using System.IO;
using System.Reflection.Emit;



namespace Tests.Laba2AOIS
{
    [TestClass()]
    public class LogicFunctionsTests
    {
        [TestMethod()]
        public void LogicalAndTest()
        {
            bool a = false;
            bool b = true;

            bool expected = false;

            bool result = LogicFunctions.OperationAnd(a, b);

            Assert.AreEqual(expected, result);

        }

        [TestMethod()]
        public void LogicalOrTest()
        {
            bool a = false;
            bool b = true;

            bool expected = true;

            bool result = LogicFunctions.OperationOr(a, b);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void LogicalNotTest()
        {
            bool a = true;

            bool expected = false;

            bool result = LogicFunctions.OperationNot(a);

            Assert.AreEqual(expected, result);

        }

        [TestMethod()]

        public void ImlicationTest()
        {
            bool a = true;
            bool b = false;

            bool expected = false;

            bool result = LogicFunctions.OperationImplication(a, b);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void EquivalentionTest()
        {
            bool a = true;
            bool b = false;

            bool expected = false;

            bool result = LogicFunctions.OperationEquivalence(a, b);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void TestOPZ1()
        {
            string input = "a&b|c";
            string expected = "ab&c|";

            string result = LogicFunctions.OPZ(input);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void TestOPZ2()
        {
            string input = "(a&b)&!c";
            string expected = "ab&c!&";

            string result = LogicFunctions.OPZ(input);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void TestCalculatePostfixExpression()
        {
            string postfixExpression = "ab|c!&";
            List<bool> values = new List<bool>() { false, false, false };
            List<bool> expected = new List<bool>() { false, true, false };

            List<bool> result = LogicFunctions.CalculatePostfixExpression(postfixExpression, values);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void TestCalculatePostfixExpression2()
        {
            string postfixExpression = "ab>c&";
            List<bool> values = new List<bool>() { false, false, false };
            List<bool> expected = new List<bool>() { true, false };

            List<bool> result = LogicFunctions.CalculatePostfixExpression(postfixExpression, values);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void TestCalculatePostfixExpression3()
        {
            string postfixExpression = "ab~c!|";
            List<bool> values = new List<bool>() { false, false, false };
            List<bool> expected = new List<bool>() { true, true, true };

            List<bool> result = LogicFunctions.CalculatePostfixExpression(postfixExpression, values);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void ConvertBinaryToDecimalTest()
        {
            // Arrange
            List<int> binaryResult = new List<int> { 1, 0, 1 }; // Бинарное представление числа
            int expectedDecimalValue = 5; // Ожидаемое десятичное значение

            // Act
            int actualDecimalValue = LogicFunctions.ConvertBinaryToDecimal(binaryResult);

            // Assert
            Assert.AreEqual(expectedDecimalValue, actualDecimalValue);
        }

        [TestMethod()]
        public void PrintTruthTable_Test()
        {
            // Arrange
            int n = 3; // Количество переменных
            string expression = "a & b | c"; // Выражение для тестирования

            // Ожидаемые результаты
            string expectedSKNF = "(a | b | c) & (a | !b | c) & (!a | b | c)";
            List<int> expectedSKNFIndices = new List<int> { 0, 2, 4 };
            List<int> expectedSDNFIndices = new List<int> { 1, 3, 5, 6, 7 };
            string expectedSDNF = "(!a & !b & c) | (!a & b & c) | (a & !b & c) | (a & b & !c) | (a & b & c)";
            string expectedDecimalResult = "Decimal result: 87";

            // Сохраняем текущий поток вывода в памяти
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            LogicFunctions.PrintLogicTable(n, expression);

            // Получаем вывод в виде строки
            string actualOutput = sw.ToString();

            // Assert
            Assert.IsTrue(actualOutput.Contains(expectedSKNF));
            Assert.IsTrue(actualOutput.Contains(expectedSDNF));
            Assert.IsTrue(actualOutput.Contains(expectedDecimalResult));

        }



    }
}