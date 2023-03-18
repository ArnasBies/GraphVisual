using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NCalc.Domain;

namespace GraphVisual
{
    internal class Graph
    {
        //fields
        public int Size;
        private NCalc.Expression Function;

        //constructors
        public Graph(int size, string function)
        {
            Size = size;
            Function = new NCalc.Expression(function);

            if (Function.HasErrors()) Function = new("0");
        }

        //methods
        public void SetNewFunction(string expression)
        {
            NCalc.Expression exp;

            exp = new NCalc.Expression(expression);

            if(!exp.HasErrors())
            Function = exp;

            else Function = new("0");
        }

        public double GetEvaluation(double x)
        {
            Function.Parameters["x"] = x;
            Function.Parameters["X"] = x;


            return (double)Function.Evaluate();
        }
    }
}
