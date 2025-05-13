using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;

namespace AnalisisNumericoWebApp.Entities
{
    public class DoubleVector
    {
        public DoubleVector(List<double> values) 
        {
            _values = values;
        }
        private List<double> _values { get; set; }
        public DoubleVector Map(Func<double, double> transform)
        {
            var values = _values.Select(transform).ToList();
            return new DoubleVector(values);
        }
        public DoubleVector MapWhere(Func<double, double> transform, Func<double, bool> selector)
        {
            var values = new List<double>();
            for(int i = 0; i < _values.Count; i++)
            {
                if(selector(_values[i]))
                    values.Add( transform(_values[i]) );
            }
            return new DoubleVector(values);
        }
        public static DoubleVector operator + (DoubleVector left, DoubleVector right)
        {
            if (left._values.Count != right._values.Count)
                throw new ArgumentException("No se pueden sumar dos vectores de distintas dimensiones.");

            var values = new List<double>();
            for(int i = 0; i < left._values.Count; i++)
            {
                values.Add(left._values[i] + right._values[i]);
            }
            return new DoubleVector(values);
        }
        public static DoubleVector operator - (DoubleVector left, DoubleVector right)
        {
            if (left._values.Count != right._values.Count)
                throw new ArgumentException("No se pueden restar dos vectores de distintas dimensiones.");

            var values = new List<double>();
            for (int i = 0; i < left._values.Count; i++)
            {
                values.Add(left._values[i] - right._values[i]);
            }
            return new DoubleVector(values);
        }
        public static DoubleVector operator / (DoubleVector left, double right)
        {
            return left.Map(x => x / right);
        }
        public static DoubleVector operator * (DoubleVector left, double right)
        {
            return left.Map(x => x * right);
        }
        public List<double> ToList()
        {
            return _values;
        }
    }
}
