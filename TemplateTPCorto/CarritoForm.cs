using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateTPCorto
{
    public partial class CarritoForm : Form
    {
        private readonly HttpClient _httpClient;
        private List<Cliente> _clientes;
        private List<Producto> _productos;
        private List<CartItem> _carrito;

        public CarritoForm()
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://tuservidor.api/") // TODO: ajustar URL base
            };
            _carrito = new List<CartItem>();

            Load += CarritoForm_Load;
        }

        private async void CarritoForm_Load(object sender, EventArgs e)
        {
            await CargarClientesAsync();
            CargarCategorias();
            ActualizarCarritoUI();
        }

        private async Task CargarClientesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Cliente/GetClientes");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                _clientes = JsonSerializer.Deserialize<List<Cliente>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                cmbClientes.DataSource = _clientes;
                cmbClientes.DisplayMember = "Nombre";
                cmbClientes.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        private void CargarCategorias()
        {
            var categorias = new List<Categoria>
            {
                new Categoria(1, "Audio"),
                new Categoria(2, "Celulares"),
                new Categoria(3, "Electro Hogar"),
                new Categoria(4, "Informática"),
                new Categoria(5, "Smart TV")
            };
            cmbCategorias.DataSource = categorias;
            cmbCategorias.DisplayMember = "Descripcion";
            cmbCategorias.ValueMember = "Id";
        }

        private async void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (cmbCategorias.SelectedValue is int idCat)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"api/Producto/TraerProductosPorCategoria?idCategoria={idCat}");
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    _productos = JsonSerializer.Deserialize<List<Producto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    dgvProductos.DataSource = _productos;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener productos: " + ex.Message);
                }
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is Producto prod)
            {
                var existing = _carrito.FirstOrDefault(c => c.Producto.Id == prod.Id);
                if (existing != null)
                    existing.Cantidad++;
                else
                    _carrito.Add(new CartItem { Producto = prod, Cantidad = 1 });

                ActualizarCarritoUI();
            }
        }

        private async void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (cmbClientes.SelectedValue is int idCliente && _carrito.Any())
            {
                var ventaDto = new VentaDto
                {
                    ClienteId = idCliente,
                    Items = _carrito.Select(c => new VentaItemDto { ProductoId = c.Producto.Id, Cantidad = c.Cantidad }).ToList()
                };

                try
                {
                    var payload = JsonSerializer.Serialize(ventaDto);
                    var content = new StringContent(payload, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/Venta/AgregarVenta", content);
                    response.EnsureSuccessStatusCode();

                    MessageBox.Show("Venta registrada con éxito.");
                    _carrito.Clear();
                    ActualizarCarritoUI();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al confirmar venta: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un cliente y agregue al menos un producto.");
            }
        }

        private void ActualizarCarritoUI()
        {
            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = _carrito.Select(c => new
            {
                c.Producto.Descripcion,
                c.Cantidad,
                Precio = c.Producto.Precio,
                Subtotal = c.Cantidad * c.Producto.Precio
            }).ToList();

            decimal subtotal = _carrito.Sum(c => c.Cantidad * c.Producto.Precio);
            decimal descuento = CalcularDescuento();
            decimal total = subtotal - descuento;

            lblSubtotal.Text = $"Subtotal: {subtotal:C}";
            lblDescuento.Text = $"Descuento: {descuento:C}";
            lblTotal.Text = $"Total: {total:C}";
        }

        private decimal CalcularDescuento()
        {
            var totalElectro = _carrito
                .Where(c => c.Producto.CategoriaId == 3)
                .Sum(c => c.Cantidad * c.Producto.Precio);

            if (totalElectro > 1_000_000m)
                return totalElectro * 0.15m;
            return 0;
        }
    }

    // Modelos auxiliares
    public class Cliente { public int Id { get; set; } public string Nombre { get; set; } }
    public class Producto { public int Id { get; set; } public string Descripcion { get; set; } public decimal Precio { get; set; } public int CategoriaId { get; set; } }
    public class CartItem { public Producto Producto { get; set; } public int Cantidad { get; set; } }
    public class Categoria { public int Id { get; } public string Descripcion { get; } public Categoria(int id, string desc) { Id = id; Descripcion = desc; } }
    public class VentaDto { public int ClienteId { get; set; } public List<VentaItemDto> Items { get; set; } }
    public class VentaItemDto { public int ProductoId { get; set; } public int Cantidad { get; set; } }
}
