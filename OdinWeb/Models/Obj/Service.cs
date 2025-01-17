﻿using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class Service
    {
        public int id { get; set; }

        [StringLength(100, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        [Display(Name = "Nombre")]
        public string name { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(1000, ErrorMessage = "El campo no puede contener más de 500 caracteres")]
        public string description { get; set; }
        [Display(Name = "Activo")]
        public bool active { get; set; }
        public string photo { get; set; }
        [Display(Name = "Requerimientos")]
        [StringLength(1000, ErrorMessage = "El campo no puede contener más de 500 caracteres")]
        public string? requirements { get; set; }
        public int? idServiceMain { get; set; }
        [Display(Name = "Necesita transporte")]
        public bool transport { get; set; }
        [Display(Name = "Asignar al administrador")]
        public bool toAdministrator { get; set; }
        public List<Ticket>? tickets { get; set; }
        public List<Service>? services { get; set; }
        public Service? serviceMain { get; set; }
    }

    public class ServiceUDP
    {
        public int id { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(100, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        public string name { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(1000, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        public string description { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Estado")]
        public bool active { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string photo { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Requerimientos")]
        [StringLength(1000, ErrorMessage = "El campo no puede contener más de 50 caracteres")]
        public string? requirements { get; set; }
        public int? idServiceMain { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public IFormFile image { get; set; }
        public IFormFile? imageU { get; set; }
        [Display(Name = "Necesita transporte")]
        public bool transport { get; set; }
        [Display(Name = "Asignar al administrador")]
        public bool toAdministrator { get; set; }
        public List<Ticket>? tickets { get; set; }
        public List<Service>? services { get; set; }
        public Service? serviceMain { get; set; }
    }
}