using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CenterSpace.NMath.Core;
using CenterSpace.NMath.Stats;
using MathNet.Numerics.Random;

namespace TP3_Simulacion.Generador
{
    class GeneradorValores
    {
        List<double> valores = new List<double>();

        public List<double> getValoresPoisson(double lambda, int cantidad, int semilla)
        {
            var poisson = new RandGenPoisson(lambda, semilla);
            for (int i = 0; i < cantidad; i++)
            {
                 valores.Add(poisson.NextDouble());
            }
            return valores;
        }
    }
}
