using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFusionTools.Helper
{
    public interface IOperation<T>
    {

        T Add(T T1, T T2);
        T Subtract(T T1, T T2);
        T Multiply(T T1, T T2);
        T Div(T T1, T T2);

        T Add(T T1);
        T Subtract(T T1);
        T Multiply(T T1);
        T Div(T T1);
    }
}
