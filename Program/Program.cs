using System;
using System.Collections.Generic;

namespace laba2AOIS
{
    public class LogicFunctions
    {
        public static bool OperationAnd(bool a, bool b)
        {
            return a && b;
        }

        public static bool OperationOr(bool a, bool b)
        {
            return a || b;
        }

        public static bool OperationNot(bool a)
        {
            return !a;
        }

        public static bool OperationImplication(bool a, bool b)
        {
            return a ? b : true;
        }

        public static bool OperationEquivalence(bool a, bool b)
        {
            return (a && b) || (!a && !b);
        }

        private static int Prioritize(char op)
        {
            if (op == '!')
                return 3;
            if (op == '&' || op == '|')
                return 2;
            if (op == '>' || op == '~')
                return 1;
            return 0;
        }

        public static string OPZ(string s)
        {
            Stack<char> st = new Stack<char>();
            string opz = "";
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                if (Char.IsLetter(c))
                {
                    opz += c;
                }

                else if (c == '&' || c == '|' || c == '!' || c == '>' || c == '~')
                {
                    while (st.Count > 0 && Prioritize(s[i]) <= Prioritize(st.Peek()))
                    {
                        opz += st.Pop();
                    }
                    st.Push(c);
                }

                else if (c == '(')
                {
                    st.Push(c);
                }

                else if (c == ')')
                {
                    while (st.Count > 0 && st.Peek() != '(')
                    {
                        opz += st.Pop();
                    }
                    st.Pop();
                }
            }

            while (st.Count > 0)
            {
                opz += st.Pop();
            }

            return opz;
        }


        public static void PrintTruthTable(int n, string expression)
        {
            string postfixExpression = OPZ(expression);
            int Rows = (int)Math.Pow(2, n);
            Console.WriteLine("Truth Table:");
            foreach (char c in expression)
            {
                if (Char.IsLetter(c))
                {
                    Console.Write(c + "\t");
                }
            }
            foreach (char c in postfixExpression)
            {
                if (c == '&' || c == '|' || c == '!' || c == '>' || c == '~')
                {
                    Console.Write(c + "\t");
                }
            }
            Console.WriteLine();

            string SKNF = "";
            string SDNF = "";
            List<int> sknfIndices = new List<int>();
            List<int> sdnfIndices = new List<int>();
            List<int> decimalResult = new List<int>();
            int binaryResult = 0;

            for (int i = 0; i < Rows; ++i)
            {
                List<bool> values = new List<bool>();
                for (int j = 0; j < n; ++j)
                {
                    values.Insert(0, (i & (1 << j)) != 0);
                }

                foreach (bool value in values)
                {
                    Console.Write((value ? "1" : "0") + "\t");
                }

                List<bool> results = CalculatePostfixExpression(postfixExpression, values);
                foreach (bool result in results)
                {
                    Console.Write((result ? "1" : "0") + "\t");
                }
                Console.WriteLine();

                UpdateSKNFAndSDNF(results, values, n, i, ref SKNF, ref SDNF, sknfIndices, sdnfIndices);

                decimalResult.Add(results[results.Count - 1] ? 1 : 0);
            }

            if (!string.IsNullOrEmpty(SKNF)) SKNF = SKNF.Substring(0, SKNF.Length - 3);
            if (!string.IsNullOrEmpty(SDNF)) SDNF = SDNF.Substring(0, SDNF.Length - 3);

            Console.WriteLine("SKNF: " + SKNF);
            Console.Write("SKNF Indices: ");
            foreach (int index in sknfIndices)
            {
                Console.Write(index + " ");
            }
            Console.WriteLine();

            Console.WriteLine("SDNF: " + SDNF);
            Console.Write("SDNF Indices: ");
            foreach (int index in sdnfIndices)
            {
                Console.Write(index + " ");
            }
            Console.WriteLine();

            binaryResult = ConvertBinaryToDecimal(decimalResult);
            Console.WriteLine("Decimal result: " + binaryResult);
        }

        private static void UpdateSKNFAndSDNF(List<bool> results, List<bool> values, int n, int rowIndex, ref string SKNF, ref string SDNF, List<int> sknfIndices, List<int> sdnfIndices)
        {
            if (results[results.Count - 1] == false)
            {
                SKNF += ConstructSKNF(values, n);
                sknfIndices.Add(rowIndex);
            }
            else
            {
                SDNF += ConstructSDNF(values, n);
                sdnfIndices.Add(rowIndex);
            }
        }

        private static string ConstructSKNF(List<bool> values, int n)
        {
            string sknf = "(";
            for (int j = 0; j < n; ++j)
            {
                sknf += (values[j] ? "!" : "") + (char)('a' + j) + (j < n - 1 ? " | " : "");
            }
            sknf += ") & ";
            return sknf;
        }

        private static string ConstructSDNF(List<bool> values, int n)
        {
            string sdnf = "(";
            for (int j = 0; j < n; ++j)
            {
                sdnf += (values[j] ? "" : "!") + (char)('a' + j) + (j < n - 1 ? " & " : "");
            }
            sdnf += ") | ";
            return sdnf;
        }
        public static List<bool> CalculatePostfixExpression(string postfixExpression, List<bool> values)
        {
            Stack<bool> st = new Stack<bool>();
            List<bool> results = new List<bool>();
            foreach (char c in postfixExpression)
            {
                if (Char.IsLetter(c))
                {
                    st.Push(values[c - 'a']);
                }
                else
                {
                    bool result = false;
                    bool operand2 = st.Pop();
                    if (c == '!')
                    {
                        result = OperationNot(operand2);
                    }
                    else
                    {
                        bool operand1 = st.Pop();
                        if (c == '&')
                        {
                            result = OperationAnd(operand1, operand2);
                        }
                        else if (c == '|')
                        {
                            result = OperationOr(operand1, operand2);
                        }
                        else if (c == '~')
                        {
                            result = OperationEquivalence(operand1, operand2);
                        }
                        else if (c == '>')
                        {
                            result = OperationImplication(operand1, operand2);
                        }
                    }
                    st.Push(result);
                    results.Add(result);
                }
            }
            return results;
        }

        public static int ConvertBinaryToDecimal(List<int> binaryResult)
        {
            int decimalValue = 0;
            int baseValue = 1;


            for (int i = binaryResult.Count - 1; i >= 0; --i)
            {
                if (binaryResult[i] == 1)
                {
                    decimalValue += baseValue;
                }
                baseValue *= 2;
            }

            return decimalValue;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter expression : ");
            string expression = Console.ReadLine();

            HashSet<char> variables = new HashSet<char>();
            foreach (char c in expression)
            {
                if (Char.IsLetter(c))
                {
                    variables.Add(c);
                }
            }

            LogicFunctions.PrintTruthTable(variables.Count, expression);
            Console.ReadLine();
        }
    }

}