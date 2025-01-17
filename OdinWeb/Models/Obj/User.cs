﻿using Microsoft.AspNetCore.Mvc;
using OdinApi.Models.Obj;
using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class User
    {
        public int id { get; set; }
        [StringLength(50, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string name { get; set; }
        [StringLength(50, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo 'Email' debe ser una dirección de correo electrónico válida")]
        [StringLength(50, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        [Display(Name = "Correo")]
        public string mail { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo 'phone' debe ser un número de teléfono válido")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "El número de teléfono debe contener exactamente 8 dígitos.")]
        [Display(Name = "Teléfono")]
        public string phone { get; set; }
        [Display(Name = "Foto")]
        public string photo { get; set; } = "user.png";
        [Required(ErrorMessage = "El campo es requerido")]
        [RegularExpression(@"^(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,16}$", ErrorMessage = "La contraseña debe tener al entre 8 y 16 caracteres, al menos un dígito, al menos una minúscula, al menos una mayúscula y al menos un caracter no alfanumérico.")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
        [Display(Name = "Activo")]
        [Required(ErrorMessage = "El campo es requerido")]
        public bool active { get; set; } = true;
        public bool restorePass { get; set; } = false;
        [Display(Name = "Id Rol")]
        public int idRol { get; set; }
        [Display(Name = "Selecione la sucursal más cercana")]
        public int? idBranch { get; set; }
        public string? token { get; set; }
        [Display(Name = "Rol")]
        public Rol? rol { get; set; }
        [Display(Name = "Ubicación")]
        public Branch? branch { get; set; }
        public List<Ticket>? ticketsS { get; set; }
        public List<Ticket>? ticketsC { get; set; }
        public List<Comment>? comments { get; set; }
        public List<Documento>? documents { get; set; }
        public List<ErrorLog>? errorsLog { get; set; }
        public List<TransactionalLog>? transactionsLog { get; set; }
        public IFormFile Imagen { get; set; }
    }

    public class UserRegister
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Nombre")]
        [StringLength(50, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        public string name { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Apellido")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo debe ser una dirección de correo electrónico válida")]
        [Display(Name = "Correo")]
        public string mail { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo debe ser un número de teléfono válido")]
        [StringLength(8, ErrorMessage = "El campo debe tener 8 números", MinimumLength = 8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Debe ingresar solo números.")]

        [Display(Name = "Teléfono")]
        public string phone { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [RegularExpression(@"^(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,16}$", ErrorMessage = "La contraseña debe tener al entre 8 y 16 caracteres, al menos un dígito, al menos una minúscula, al menos una mayúscula y al menos un caracter no alfanumérico.")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }

        [Display(Name = "Sucursal más Cercana")]
        [Required(ErrorMessage = "El campo es requerido")]
        public int idBranch { get; set; }

        [Display(Name = "Confirmar Contraseña")]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden.")]
        [Required(ErrorMessage = "El campo es requerido.")]
        public string confirmpassword { get; set; }

        [Display(Name = "Activo")]
        public bool active { get; set; }
    }
    public class RestorePassword
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo debe ser una dirección de correo electrónico válida")]
        public string mail { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo debe ser un número de teléfono válido")]
        [StringLength(8, ErrorMessage = "El campo debe tener 8 números", MinimumLength = 8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Debe ingresar solo números.")]
        public string phone { get; set; }
    }

    public class LoginViewModel
    {
        public UserDTO User { get; set; }
        public RestorePassword RestorePassword { get; set; }
    }
    public class ChangePassword
    {
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Contraseña Actual")]
        public string oldPassword { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,16}$", ErrorMessage = "La contraseña debe tener al entre 8 y 16 caracteres, al menos un dígito, al menos una minúscula, al menos una mayúscula y al menos un caracter no alfanumérico.")]
        [Display(Name = "Nueva Contraseña")]
        [Required(ErrorMessage = "El campo es requerido.")]
        public string password { get; set; }

        [Display(Name = "Confirnar Contraseña")]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden.")]
        [Required(ErrorMessage = "El campo es requerido.")]
        public string confirmPassword { get; set; }
    }
    public class UpdateUser
    {
        public int id { get; set; }

        [StringLength(50, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        public string Nombre { get; set; }
        [StringLength(50, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        public string Apellidos { get; set; }

        [StringLength(50, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        public string CorreoElectronico { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = "Debe ingresar solo números.")]
        [StringLength(8, ErrorMessage = "El campo debe tener 8 números", MinimumLength = 8)]
        public string Telefono { get; set; }
        [Display(Name = "Sucursal más Cercana")]
        public int IdBranch { get; set; }
        public IFormFile Imagen { get; set; }
        public string rutaImagen { get; set; }
    }
   
}