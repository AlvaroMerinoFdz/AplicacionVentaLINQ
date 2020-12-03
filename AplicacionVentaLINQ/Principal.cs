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
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();

        }
        

        private void btnConsulta1_Click(object sender, EventArgs e)
        {
            using (ventaEntities objBd = new ventaEntities())
            {
                txtConsulta.Text = "Consultar todos los productos simples que hayan sido borrados logicamente.";
                var subconsulta = from pp in objBd.productospack select pp.idproductocompuesto;
                var consulta1 = from prod in objBd.productos
                                where prod.borrado == -1 && !subconsulta.Contains(prod.idproducto)
                                select
                                new { prod.idproducto, prod.idtipo, prod.idmedida, prod.precio, prod.borrado };

            dgvConsulta.DataBindings.Clear();
                dgvConsulta.DataSource = consulta1.ToList();
                dgvConsulta.Refresh();
            }
                
                        
        }

        private void btnConsulta2_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Mostrar el tipo y la medida de los productos simples junto con las materias primas que utilizan";
            using (ventaEntities objBd = new ventaEntities())
            {
                var compuestos = from p in objBd.productospack select p.idproductocompuesto;
                var query = from p in objBd.productos
                            join t in objBd.tipos
                            on p.idtipo equals t.idtipo
                            join m in objBd.medidas
                            on p.idmedida equals m.idmedida
                            join pmd in objBd.productosmp
                            on p.idproducto equals pmd.idproducto
                            join mat in objBd.materiasprimas
                            on pmd.idmateriaprima equals mat.idmateriaprima
                            where !compuestos.Contains(p.idproducto)
                            select new { t.tipo, m.medida, mat.materiaprima, pmd.cantidad };


                dgvConsulta.DataBindings.Clear();
                dgvConsulta.DataSource = query.ToList();
                dgvConsulta.Refresh();
            }
        }

        private void btnConsulta3_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Mostrar tipo, medida y precio de el/los producto(s) más barato(s)";
            using (ventaEntities objBd = new ventaEntities())
            {
                var compuestos = from p in objBd.productospack select p.idproductocompuesto;

                var precios = from p in objBd.productos
                              where compuestos.Contains(p.idproducto)
                              select new { p.precio };

                var precioMin = precios.Min(x => x.precio);

                var query = from p in objBd.productos
                            join t in objBd.tipos
                            on p.idtipo equals t.idtipo
                            join m in objBd.medidas
                            on p.idmedida equals m.idmedida
                            where compuestos.Contains(p.idproducto) && p.precio == precioMin
                            select new { t.tipo, m.medida, p.precio };
                dgvConsulta.DataBindings.Clear();
                dgvConsulta.DataSource = query.ToList();
                dgvConsulta.Refresh();
            }
        }

        private void btnConsulta4_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Mostrar tipo y medida de los productos compuestos junto con el tipo y la medida de los productos simple que los componen.";
            using (ventaEntities objBd = new ventaEntities())
            {

                var query = from pp in objBd.productospack
                            from prod1 in objBd.productos
                            from tip1 in objBd.tipos
                            from med1 in objBd.medidas
                            from prod2 in objBd.productos
                            from tip2 in objBd.tipos
                            from med2 in objBd.medidas
                            where pp.idproductocompuesto == prod1.idproducto &&
                            prod1.idtipo == tip1.idtipo &&
                            prod1.idmedida == med1.idmedida &&
                            pp.idproductosimple == prod2.idproducto &&
                            prod2.idtipo == tip2.idtipo &&
                            prod2.idmedida == med2.idmedida
                            orderby tip1.tipo, med1.medida, tip2.tipo, med2.medida
                            select 
                            new { TipoCompuesto = tip1.tipo, MedidaCompuesto = med1.medida,
                                TipoSimple = tip2.tipo, MedidaSimple = med2.medida, pp.cantidad };

                dgvConsulta.DataBindings.Clear();
                dgvConsulta.DataSource = query.ToList();
                dgvConsulta.Refresh();
            }
        }

        private void btnConsulta5_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Mostrar para cada producto compuesto el tipo, la medida y el ahorro conseguido al comprar el producto compuesto directamente.";
            using (ventaEntities objBd = new ventaEntities())
            {
                var consultaPrecioCompuestos = (from pp in objBd.productospack
                                                join prod1 in objBd.productos on pp.idproductocompuesto equals prod1.idproducto
                                                select new { pp.idproductocompuesto, PrecioCompuesto = prod1.precio }).Distinct();

                var consultaPrecioSimples = from pp in objBd.productospack
                                            join prod2 in objBd.productos on pp.idproductosimple equals prod2.idproducto
                                            select new { pp.idproductocompuesto, pp.idproductosimple, pp.cantidad, PrecioSimple = prod2.precio };

                var consultaPrecioSimplesAgrupados = from cs in consultaPrecioSimples.ToList()
                                                     group cs by cs.idproductocompuesto into g
                                                     select new
                                                     {
                                                         IdProductoCompuesto = g.Key,
                                                         CosteTotalPorSeparado = g.Sum(x => x.cantidad * x.PrecioSimple)
                                                     };

                var query = from pc in consultaPrecioCompuestos.ToList()
                            from psa in consultaPrecioSimplesAgrupados.ToList()
                            from prod in objBd.productos
                            join tip in objBd.tipos on prod.idtipo equals tip.idtipo
                            join med in objBd.medidas on prod.idmedida equals med.idmedida
                            where pc.idproductocompuesto == psa.IdProductoCompuesto &&
                            prod.idproducto == pc.idproductocompuesto
                            select
                            new { tip.tipo, med.medida, Ahorro = (psa.CosteTotalPorSeparado - pc.PrecioCompuesto)};

                dgvConsulta.DataBindings.Clear();
                dgvConsulta.DataSource = query.ToList();
                dgvConsulta.Refresh();
            }

            }

        private void btnConsulta6_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Sacar tipo y medida, de los productos borrados logicamente, el que fuera más caro.";
            using (ventaEntities objBd = new ventaEntities())
            {
                var subconsultaPrecioBorrados = from prod in objBd.productos where prod.borrado == -1 select prod.precio;
                var maxPrecio = subconsultaPrecioBorrados.Max();

                var query = from prod in objBd.productos
                            join tip in objBd.tipos on prod.idtipo equals tip.idtipo
                            join med in objBd.medidas on prod.idmedida equals med.idmedida
                            where prod.borrado == -1 &&
                            prod.precio == maxPrecio
                            select
                            new { tip.tipo, med.medida, prod.precio }
                            ;

                dgvConsulta.DataBindings.Clear();
                dgvConsulta.DataSource = query.ToList();
                dgvConsulta.Refresh();
            }
        }

        private void btnConsulta7_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Sacar tipo, medida y precio, de los productos simples, el que esté formado por más materias primas.";
            using (ventaEntities objBd = new ventaEntities())
            {
                var subconsultaCompuestos = from pp in objBd.productospack select pp.idproductocompuesto;
                var subconsultaProductosSimples = from prod in objBd.productos
                                                  where !subconsultaCompuestos.Distinct().Contains(prod.idproducto)
                                                  select prod.idproducto;

                var consultaProdSimplesMMPPAgrupadas = from pmp in objBd.productosmp
                                                       where subconsultaProductosSimples.Contains(pmp.idproducto)
                                                       group pmp by pmp.idproducto into g
                                                       select new
                                                       {
                                                           idProducto = g.Key,
                                                           NumMateriasPrimas = g.Count()
                                                       };
                /*var maxValue = consultaProdSimplesMMPPAgrupadas.Max(x => x.NumMateriasPrimas);
                var subconsultaProductosMasMMPP = consultaProdSimplesMMPPAgrupadas.Where(x => NumMateriasPrimas == maxValue);


                var query = from pmasmpp in subconsultaProductosMasMMPP.toList()
                            from prod in objBd.productos
                            join tip in objBd.tipos on prod.idtipo equals tip.idtipo
                            join med in objBd.medidas on prod.idmedida equals med.idmedida
                            where pmasmpp.IdProducto == prod.idproducto
                            select
                            new { tip.tipo, med.medida, pmasmmpp.NumMateriasPrimas };

                dgvConsulta.DataBindings.Clear();
                dgvConsulta.DataSource = query.ToList();
                dgvConsulta.Refresh();*/
            }
            }

        private void btnConsulta8_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Sacar tipo, medida y precio, ordenados ascendentemente, de los productos simples";
        }

        private void btnConsulta9_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Sacar el nombre de la materia prima que haya sido utilizada en más cantidad, teniendo en cuenta todos los productos.";
        }

        private void btnConsulta10_Click(object sender, EventArgs e)
        {
            txtConsulta.Text = "Sacar el nombre de la medida, como el número de veces utilizada, de aquella/s medida/s que haya/n sido utlizada/s más veces por los productos simples.";
        }

        private void Principal_Load(object sender, EventArgs e)
        {

        }

        private void Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
