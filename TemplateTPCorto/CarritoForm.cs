using Datos.Web;
using Negocio.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateTPCorto
{
    public partial class CarritoForm : Form
    {
        private List<Cliente> _clientes;
        private List<Producto> _productos;
        private List<Producto> _todosLosProductos; // Almacenará todos los productos
        private List<Categoria> _categorias;
        private List<Producto> _carrito;
        private string _usuario;

        public CarritoForm(string usuario)
        {
            InitializeComponent();
            _carrito = new List<Producto>();
            _usuario = usuario;
            ConfigurarDataGridViews();
            Load += CarritoForm_Load;
        }

        private void ConfigurarDataGridViews()
        {
            // Configurar dgvProductos
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.ReadOnly = true;
            dgvProductos.MultiSelect = false;

            // Configurar dgvCarrito
            dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.AllowUserToDeleteRows = false;
            dgvCarrito.ReadOnly = true;
            dgvCarrito.MultiSelect = false;
        }

        private async void CarritoForm_Load(object sender, EventArgs e)
        {
            await CargarClientesAsync();
            await CargarDatosIniciales();
        }

        private async Task CargarClientesAsync()
        {
            try
            {
                var todosLosClientes = await ApiClient.GetClientes();
                // Re-introducimos el filtro para mostrar solo clientes activos
                _clientes = todosLosClientes.Where(c => c.FechaBaja == null).ToList();

                cmbClientes.DataSource = _clientes;
                cmbClientes.DisplayMember = "DisplayValue";
                cmbClientes.ValueMember = "IdCliente";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        private async Task CargarDatosIniciales()
        {
            try
            {
                // 1. Cargar todos los productos desde la API
                _todosLosProductos = await ApiClient.TraerTodosLosProductos();

                // 2. Crear la lista de categorías fija
                _categorias = new List<Categoria>
                {
                    new Categoria { Id = 0, Nombre = "Todas las categorías" },
                    new Categoria { Id = 1, Nombre = "Audio" },
                    new Categoria { Id = 2, Nombre = "Celulares" },
                    new Categoria { Id = 3, Nombre = "Electro Hogar" },
                    new Categoria { Id = 4, Nombre = "Informática" },
                    new Categoria { Id = 5, Nombre = "Smart TV" }
                };

                cmbCategorias.DataSource = _categorias;
                cmbCategorias.DisplayMember = "Nombre";
                cmbCategorias.ValueMember = "Id";

                // 3. Filtrar y mostrar productos (inicialmente "Todas")
                FiltrarProductosPorCategoria();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos iniciales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarProductosPorCategoria()
        {
            if (cmbCategorias.SelectedItem == null || !(cmbCategorias.SelectedItem is Categoria categoriaSeleccionada))
            {
                return;
            }

            IEnumerable<Producto> productosFiltrados;

            if (categoriaSeleccionada.Id == 0) // "Todas"
            {
                productosFiltrados = _todosLosProductos;
            }
            else
            {
                productosFiltrados = _todosLosProductos.Where(p => p.IdCategoria == categoriaSeleccionada.Id);
            }

            _productos = productosFiltrados.Where(p => p.Stock > 0).ToList();
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = _productos;

            if (dgvProductos.Columns.Count > 0)
            {
                dgvProductos.Columns["Id"].Visible = false;
                dgvProductos.Columns["IdCategoria"].Visible = false;
                dgvProductos.Columns["FechaAlta"].Visible = false;
                dgvProductos.Columns["FechaBaja"].Visible = false;
                dgvProductos.Columns["IdUsuario"].Visible = false;
                dgvProductos.Columns["IdProveedor"].Visible = false;
                dgvProductos.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvProductos.Columns["Precio"].Width = 100;
                dgvProductos.Columns["Stock"].Width = 80;
            }

            lblProductosDisponibles.Text = $"Productos encontrados: {_productos.Count}";
        }

        private async void btnBuscarProductos_Click(object sender, EventArgs e)
        {
            await CargarDatosIniciales();
        }

        private void cmbCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarProductosPorCategoria();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                if (dgvProductos.SelectedRows[0].DataBoundItem is Producto prod)
                {
                    int cantidadEnCarrito = _carrito.FindAll(p => p.Id == prod.Id).Count;
                    int cantidadAAgregar = (int)numCantidad.Value;

                    if (prod.Stock < cantidadEnCarrito + cantidadAAgregar)
                    {
                        MessageBox.Show($"No hay suficiente stock para '{prod.Nombre}'. Stock disponible: {prod.Stock}. En carrito: {cantidadEnCarrito}", "Stock Insuficiente",
                                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    for (int i = 0; i < cantidadAAgregar; i++)
                    {
                        _carrito.Add(prod);
                    }

                    ActualizarCarritoUI();
                    MessageBox.Show($"{cantidadAAgregar} x '{prod.Nombre}' agregado(s) al carrito.", "Producto Agregado",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto de la lista.", "Selección Requerida",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count > 0)
            {
                var selectedRow = dgvCarrito.SelectedRows[0];
                var productoNombre = selectedRow.Cells["Producto"].Value.ToString();

                // Buscar el producto en la lista de productos para obtener su ID
                var productoOriginal = _productos.Find(p => p.Nombre == productoNombre);
                if (productoOriginal != null)
                {
                    // Eliminar todas las instancias de este producto del carrito
                    _carrito.RemoveAll(p => p.Id == productoOriginal.Id);

                    ActualizarCarritoUI();
                    MessageBox.Show($"Producto '{productoNombre}' eliminado del carrito.", "Producto Eliminado",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto del carrito para eliminar.", "Selección Requerida",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLimpiarCarrito_Click(object sender, EventArgs e)
        {
            if (_carrito.Count > 0)
            {
                var resultado = MessageBox.Show("¿Está seguro de que desea limpiar todo el carrito?",
                                               "Confirmar Limpieza",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    _carrito.Clear();
                    ActualizarCarritoUI();
                    MessageBox.Show("El carrito ha sido limpiado.", "Carrito Limpio",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("El carrito ya está vacío.", "Carrito Vacío",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarCliente.Text))
            {
                MessageBox.Show("Por favor, ingrese el ID de un cliente para buscar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Guid.TryParse(txtBuscarCliente.Text, out Guid clienteId))
            {
                try
                {
                    var cliente = await ApiClient.GetCliente(clienteId);
                    if (cliente != null)
                    {
                        cmbClientes.SelectedValue = cliente.IdCliente;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún cliente con ese ID.", "No Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar el cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("El ID del cliente ingresado no es válido.", "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Usamos el ID de cliente y de usuario fijos para el entorno de pruebas.
            var idCliente = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var idUsuario = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

            // Agrupar productos en el carrito para obtener las cantidades
            var productosAgrupados = _carrito
                .GroupBy(p => p.Id)
                .Select(g => new { IdProducto = g.Key, Cantidad = g.Count() })
                .ToList();

            int ventasExitosas = 0;
            foreach (var item in productosAgrupados)
            {
                var ventaInput = new VentaProductoInput
                {
                    IdCliente = idCliente, // Se asigna el ID fijo
                    IdUsuario = idUsuario,
                    IdProducto = item.IdProducto,
                    Cantidad = item.Cantidad
                };

                try
                {
                    bool exito = await ApiClient.AgregarVenta(ventaInput);
                    if (exito)
                    {
                        ventasExitosas++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar la venta para el producto ID {item.IdProducto}: {ex.Message}", "Error de Venta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (ventasExitosas == productosAgrupados.Count)
            {
                MessageBox.Show("¡Venta registrada con éxito para todos los productos!", "Venta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _carrito.Clear();
                ActualizarCarritoUI();
            }
            else
            {
                MessageBox.Show($"Se registraron {ventasExitosas} de {productosAgrupados.Count} tipos de productos. Algunos productos no se pudieron registrar.", "Venta Parcial", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarCarritoUI()
        {
            var tablaCarrito = new DataTable();
            tablaCarrito.Columns.Add("Producto");
            tablaCarrito.Columns.Add("Precio", typeof(double));
            tablaCarrito.Columns.Add("Cantidad", typeof(int));

            // Agrupar productos por ID para contar cantidades
            var gruposDeProductos = new Dictionary<Guid, (Producto producto, int cantidad)>();
            foreach (var item in _carrito)
            {
                if (gruposDeProductos.ContainsKey(item.Id))
                {
                    var (producto, cantidad) = gruposDeProductos[item.Id];
                    gruposDeProductos[item.Id] = (producto, cantidad + 1);
                }
                else
                {
                    gruposDeProductos[item.Id] = (item, 1);
                }
            }

            foreach (var (producto, cantidad) in gruposDeProductos.Values)
            {
                tablaCarrito.Rows.Add(producto.Nombre, producto.Precio, cantidad);
            }

            dgvCarrito.DataSource = tablaCarrito;

            // Configurar columnas del carrito
            if (dgvCarrito.Columns.Count > 0)
            {
                dgvCarrito.Columns["Producto"].Width = 300;
                dgvCarrito.Columns["Precio"].Width = 100;
                dgvCarrito.Columns["Cantidad"].Width = 80;
            }

            double subtotal = CalcularSubtotal();
            double descuento = CalcularDescuento(subtotal);
            double total = subtotal - descuento;

            lblSubtotal.Text = $"Subtotal: {subtotal:C}";
            lblDescuento.Text = $"Descuento: {descuento:C}";
            lblTotal.Text = $"Total: {total:C}";

            // Actualizar estado del carrito
            if (_carrito.Count == 0)
            {
                lblEstadoCarrito.Text = "Estado del Carrito: Vacío";
                lblEstadoCarrito.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                lblEstadoCarrito.Text = $"Estado del Carrito: {_carrito.Count} producto(s)";
                lblEstadoCarrito.ForeColor = System.Drawing.Color.Black;
            }
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
            // La condición dice "si una venta tiene un monto mayor a $1.000.000"
            // No especifica que sea de una categoría, por lo que se aplica al subtotal general.
            if (subtotal > 1000000)
            {
                return subtotal * 0.15;
            }
            return 0;
        }
    }
}
