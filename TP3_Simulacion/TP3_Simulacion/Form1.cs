using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TP3_Simulacion.Generador;

namespace TP3_Simulacion
{
    public partial class Form1 : Form
    {

		DataTable tablaFrecuencias;
		int intervalos;
		int[] frecObs;

		public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
			this.chart1.Palette = ChartColorPalette.Excel;
			// Set title.
			this.chart1.Titles.Add("Histograma");

			//tabla
			tablaFrecuencias = new DataTable();
			tablaFrecuencias.Columns.Add("Lim. Inf.");
			tablaFrecuencias.Columns.Add("Lim. Sup.");
			tablaFrecuencias.Columns.Add("Frec. Obs.");
		}

        private void button1_Click(object sender, EventArgs e)
        {
			dgv_numeros.Rows.Clear();
            int semilla = int.Parse(txt_semilla.Text);
            double lambda = double.Parse(txt_lambda.Text);
            int valores = int.Parse(txt_valores.Text);
            intervalos = int.Parse(txt_intervalos.Text);
            List<double> valoresPoisson = new GeneradorValores().getValoresPoisson(lambda, valores, semilla);
            for(int i=0; i<valoresPoisson.Count; i++)
            {
                var index = this.dgv_numeros.Rows.Add();
                this.dgv_numeros.Rows[index].Cells[0].Value = i+1;
                this.dgv_numeros.Rows[index].Cells[1].Value = valoresPoisson[i];
            }
			CrearTablaFrecuencias(valoresPoisson);
        }

		private void CrearTablaFrecuencias(List<double> nums)
		{
			chart1.Series.Clear();
			tablaFrecuencias.Rows.Clear();
			dgv_tablaFrecuencias.DataSource = tablaFrecuencias;

			double max_value = nums.Max();
			double min_value = nums.Min();

			double paso = (double)(max_value - min_value) / intervalos;

			//contar frecuencias
			int[] contadorFrecuencia = new int[intervalos];
			foreach (double num in nums)
			{
				int c = 0;
				double limSupIntervalo = paso + min_value;
				while (num > limSupIntervalo)
				{
					limSupIntervalo += paso;
					c++;
				}
				if (limSupIntervalo > max_value)
				{
					contadorFrecuencia[c - 1]++;
				}
				else
				{
					contadorFrecuencia[c]++;
				}

			}
			frecObs = contadorFrecuencia;

			// cargar la tabla
			int num_intervalo = 0;

			foreach (int k in frecObs)
			{
				double Limite_Inferior = num_intervalo * paso + min_value;
				double Limite_Superior = ++num_intervalo * paso + min_value;
				int frecuencia_Esperada = k;
				tablaFrecuencias.Rows.Add(Math.Round(Limite_Inferior, 4), Math.Round(Limite_Superior, 4), frecuencia_Esperada);
			}

			//grafico
			chart1.Series.Add("Frecuencia Observada");
			double clase = (paso / 2) + min_value;
			foreach (double frec in frecObs)
			{
				chart1.ChartAreas["ChartArea1"].AxisY.Maximum = frecObs.Max()*1.2;
				chart1.Series["Frecuencia Observada"].Points.AddXY(Math.Round(clase, 4).ToString(), frec.ToString());
				clase += paso;
			}
		}
	}
}
