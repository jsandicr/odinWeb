namespace OdinWeb.Entities
{
    public class UserEntiti
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public int IdSucursal { get; set; }

    }
}
