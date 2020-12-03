using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplicacionVentaLINQ
{
    public partial class MateriaPrima : Form
    {
        private List<int> idProds = new List<int>();
        public MateriaPrima()
        {
            InitializeComponent();
        }

        private void MateriaPrima_Load(object sender, EventArgs e)
        {
            using (ventaEntities objBd = new ventaEntities())
            {
                cargarProductos(objBd); 
            }
            
        }
        private void cargarProductos(ventaEntities objBd)
        {
            var productos = from p in objBd.productos
                            join t in objBd.tipos
                            on p.idproducto equals t.idtipo
                            select new { producto = t.tipo, id = p.idproducto }d;

            foreach (var p in productos)
            {
                cmbProductos.Items.Add(p.producto);
                idProds.Add(p.id);
            }
            cmbProductos.Text = cmbProductos.Items[0].ToString();

        }
        private void cargarMaterias(ventaEntities objBd)
        {
            var materias = from m in objBd.materiasprimas
                           join p in objBd.productosmp
                           on m.idmateriaprima equals p.idmateriaprima
                           where p.idmateriaprima equals m.idmateria
                           select m.materiaprima;
            foreach (String m in materiasprimas)
            {
                clbMateriasPrimas.Items.Add(m);
            }
            clbMateriasPrimas.Text = clbMateriasPrimas.Items[0].ToString();



        }
}
