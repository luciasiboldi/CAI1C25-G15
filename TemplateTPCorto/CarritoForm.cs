using Datos.Web;
using Negocio.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateTPCorto
{
    public partial class CarritoForm : Form
    {
        private List<Cliente> _clientes;
        private List<Producto> _productos;
        private List<Producto> _carrito;
        private string _usuario;

        public CarritoForm(string usuario)
        {
            InitializeComponent();
            _carrito = new List<Producto>();
            _usuario = usuario;
            Load += CarritoForm_Load;
        }

        private async void CarritoForm_Load(object sender, EventArgs e)
        {
            await CargarClientesAsync();
        }

        private async Task CargarClientesAsync()
        {
            try
            {
                _clientes = await ClienteService.GetClientes();
                cmbClientes.DataSource = _clientes;
                cmbClientes.DisplayMember = "Nombre";
                cmbClientes.ValueMember = "IdCliente";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        private async void btnBuscarProductos_Click(object sender, EventArgs e)
        {
            try
            {
                _productos = await ProductoService.TraerTodosLosProductos();
                dgvProductos.DataSource = _productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener productos: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is Producto prod)
            {
                _carrito.Add(prod);
                ActualizarCarritoUI();
            }
        }

        private async void btnConfirmarVenta_Click(object sender, EventArgs e)
        {
            if (cmbClientes.SelectedValue == null)
            {
                MessageBox.Show("Por favor, seleccione un cliente.");
                return;
            }
            if (_carrito.Count == 0)
            {
                MessageBox.Show("Por favor, agregue al menos un producto al carrito.");
                return;
            }

            var venta = new Venta
            {
                IdCliente = (Guid)cmbClientes.SelectedValue,
                Usuario = _usuario,
                Productos = MapearCarritoAVentaProductos()
            };

            try
            {
                bool exito = await VentaService.AgregarVenta(venta);
                if (exito)
                {
                    MessageBox.Show("Venta registrada con éxito.");
                    _carrito.Clear();
                    ActualizarCarritoUI();
                }
                else
                {
                    MessageBox.Show("Hubo un problema al registrar la venta.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar venta: " + ex.Message);
            }
        }

        private List<VentaProducto> MapearCarritoAVentaProductos()
        {
            var ventaProductos = new List<VentaProducto>();
            var gruposDeProductos = new Dictionary<Guid, int>();

            foreach (var producto in _carrito)
            {
                if (gruposDeProductos.ContainsKey(producto.Id))
                {
                    gruposDeProductos[producto.Id]++;
                }
                else
                {
                    gruposDeProductos[producto.Id] = 1;
                }
            }

            foreach (var par in gruposDeProductos)
            {
                ventaProductos.Add(new VentaProducto { IdProducto = par.Key, Cantidad = par.Value });
            }

            return ventaProductos;
        }

        private void ActualizarCarritoUI()
        {
            var tablaCarrito = new DataTable();
            tablaCarrito.Columns.Add("Producto");
            tablaCarrito.Columns.Add("Precio", typeof(double));

            foreach (var item in _carrito)
            {
                tablaCarrito.Rows.Add(item.Nombre, item.Precio);
            }

            dgvCarrito.DataSource = tablaCarrito;

            double subtotal = CalcularSubtotal();
            double descuento = CalcularDescuento(subtotal);
            double total = subtotal - descuento;

            lblSubtotal.Text = $"Subtotal: {subtotal:C}";
            lblDescuento.Text = $"Descuento: {descuento:C}";
            lblTotal.Text = $"Total: {total:C}";
        }

        private double CalcularSubtotal()
        {
            double subtotal = 0;
            foreach (var item in _carrito)
            {
                subtotal += item.Precio;
            }
            return subtotal;
        }

        private double CalcularDescuento(double subtotal)
        {           
            if (subtotal > 1000000)
            {
                return subtotal * 0.15;
            }
            return 0;
        }
    }
}
