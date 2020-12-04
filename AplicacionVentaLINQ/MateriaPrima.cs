using System;
using System.Collections;
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
        private List<int> listaIdProductos = new List<int>();
        private List<int> listaIdMaterias = new List<int>();
        public MateriaPrima()
        {
            InitializeComponent();
        }

        private void MateriaPrima_Load(object sender, EventArgs e)
        {
            using (ventaEntities objBd = new ventaEntities())
            {
                iniciarProductos();
                iniciarMaterias();
                cargarMaterias();
            }

        }
       
        private void iniciarMaterias()
        {
            using (ventaEntities BDventas = new ventaEntities())
            {
                var materias = from m in BDventas.materiasprimas
                               select new { materia = m.materiaprima, id = m.idmateriaprima };

                foreach (var m in materias)
                {
                    clbMaterias.Items.Add(m.materia);
                    listaIdMaterias.Add(m.id);
                }
                cmbProductos.Text = cmbProductos.Items[0].ToString();
            }
        }

        private void cargarMaterias()
        {
            using (ventaEntities BDventas = new ventaEntities())
            {
                int productoSelected = listaIdProductos[cmbProductos.SelectedIndex];

                var materias = from m in BDventas.materiasprimas
                               join p in BDventas.productosmp
                               on m.idmateriaprima equals p.idmateriaprima
                               where p.idproducto == productoSelected
                               select new { idmateria = m.idmateriaprima };

                desmarcarMaterias();
                foreach (var m in materias)
                {
                    for (int i = 0; i < listaIdMaterias.Count(); i++)
                    {
                        if (m.idmateria == listaIdMaterias[i])
                        {
                            clbMaterias.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        private void desmarcarMaterias()
        {
            for (int i = 0; i < clbMaterias.Items.Count; i++)
            {
                clbMaterias.SetItemChecked(i, false);
            }
        }

        private void iniciarProductos()
        {
            using (ventaEntities BDventas = new ventaEntities())
            {
                var productos = from p in BDventas.productos
                                join t in BDventas.tipos
                                on p.idproducto equals t.idtipo
                                select new { producto = t.tipo, id = p.idproducto };

                foreach (var p in productos)
                {
                    cmbProductos.Items.Add(p.producto);
                    listaIdProductos.Add(p.id);
                }
                cmbProductos.Text = cmbProductos.Items[0].ToString();
            }
        }

        private void cmbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarMaterias();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            using (ventaEntities BDventas = new ventaEntities())
            {
                int idProducto = listaIdProductos[cmbProductos.SelectedIndex];
                //Creamos el objeto categoria
                productosmp nuevo = new productosmp();
                nuevo.idproducto = idProducto;

                var materias = from p in BDventas.productosmp
                               where p.idproducto == idProducto
                               select p.idmateriaprima;

                var lista = clbMaterias.CheckedIndices;

                for (int i = 0; i < clbMaterias.CheckedItems.Count; i++)
                {
                    int idMateria = listaIdMaterias[lista[i]];

                    if (!materias.Contains(idMateria))
                    {
                        nuevo.idmateriaprima = idMateria;
                        nuevo.cantidad = 1;
                        //Se añade el objeto a la tabla, para incluirlo como nuevo registro
                        BDventas.productosmp.Add(nuevo);
                        //Se guardan los cambios
                        BDventas.SaveChanges();
                    }
                    else
                    {
                        productosmp modificado = BDventas.productosmp.First(x => x.idproducto == idProducto && x.idmateriaprima == idMateria);
                        modificado.cantidad += 1;
                        BDventas.SaveChanges();
                    }
                }
                MessageBox.Show("Proceso completado con éxito.");
            }
        }
    }
}

        /*private void cmbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            clbMateriasPrimas.Items.Clear();
            int pos = cmbProductos.SelectedIndex;
            using (ventaEntities objBd = new ventaEntities())
            {
                //Recupero el id  de producto que hay guardado en su arraylist.
                int idProducto = (int)lstidproductos[pos];

                //consulto todas las materias primas que estén relacionadas con ese producto
                var consulta = from pmp in objBd.producto
            }

        }*/

