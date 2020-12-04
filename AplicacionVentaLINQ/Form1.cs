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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            //Comprobar que no exista ninguna categoría con ese mismo nombre antes 
            //(independientemente de las mayúsculas/minúsculas y espacios en blanco al principio/final)

            using (ventaEntities objBd = new ventaEntities())
            {
                //creamos el objeto categoria
                materiasprimas objMatPrima = new materiasprimas();
                String materia = txtDescripcion.Text;
                var materias = from m in objBd.materiasprimas

                              select m.materiaprima;

                if (materias.Contains(materia.Trim().ToUpper()))
                {
                    MessageBox.Show("La materia ya existe, y no se puede añadir");
                }
                else
                {
                    objMatPrima.materiaprima = materia;
                    //se añade el objeto a la tabla, para incluirlo como nuevo registro
                    objBd.materiasprimas.Add(objMatPrima);
                    //se guardan cambios
                    objBd.SaveChanges();
                    MessageBox.Show("Materia prima insertada correctamente. ");
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //Comprobar que no exista ninguna categoría con ese mismo nombre antes 
            //(independientemente de las mayúsculas/minúsculas y espacios en blanco al principio/final)
            
            using (ventaEntities objBd = new ventaEntities())
            {
                int id = int.Parse(txtId.Text);
                //Recuperamos el objeto de la bd, filtrando por el campo categoría.
                materiasprimas objMP = objBd.materiasprimas.First(x => x.idmateriaprima.Equals(id));
                //creamos el objeto categoria
                String materia = txtDescripcion.Text;
                var idMaterias = from m in objBd.materiasprimas
                                 select m.idmateriaprima;

                var materias = from m in objBd.materiasprimas
                               select m.materiaprima.ToUpper();
                if (idMaterias.Contains(id))
                {
                    if (!materias.Contains(materia.ToUpper().Trim()))
                    {
                        MessageBox.Show("No puedes poner esta descripción, ya existe.");
                    }else
                    {
                        //se elimina el objeto de la tabla, para quitarlo como registro.
                        objMP.materiaprima = materia;
                        //Se guardan los cambios
                        objBd.SaveChanges();
                        MessageBox.Show("Materia prima modificada correctamente");
                    }
                }else
                {
                    MessageBox.Show("La materia no existe, por eso no se puede modificar");
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Comprobar antes si hay algún producto que esté relacionado con esa materia prina,
            // si lo hay, mensaje de aviso
            
            using (ventaEntities objBd = new ventaEntities())
            {
                int id = int.Parse(txtId.Text);
                //Recuperamos el objeto de la bd, filtrando por el campo categoría.
                materiasprimas objMP = objBd.materiasprimas.First(x => x.idmateriaprima.Equals(id));

                var consulta = from p in objBd.productosmp
                               select p.idmateriaprima;

                if (consulta.Contains(id))
                {
                    MessageBox.Show("No se puede eliminar, porque está asignado a productos");
                }
                else
                {
                    //se elimina el objeto de la tabla, para quitarlo como registro.
                    objBd.materiasprimas.Remove(objMP);

                    //Se guardan los cambios
                    objBd.SaveChanges();
                    MessageBox.Show("Materia prima eliminada correctamente");
                }
            }
        }

        private void btnConsultas_Click(object sender, EventArgs e)
        {
            Principal principal = new Principal();
            
            principal.Show();
            this.Hide();
        }

        private void btnMateriasPrimas_Click(object sender, EventArgs e)
        {
            MateriaPrima materiaPrima = new MateriaPrima();
            materiaPrima.ShowDialog();
            //this.Hide();
        }
    }
}
