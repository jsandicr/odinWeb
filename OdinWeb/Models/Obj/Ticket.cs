﻿using Microsoft.AspNetCore.Mvc;
using OdinApi.Models.Obj;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Obj
{
    public class Ticket
    {
        public int id { get; set; }
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string title { get; set; }
        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string description { get; set; }
        [Display(Name = "Fecha Creacion")]
        [Required(ErrorMessage = "El campo es requerido")]
        public DateTime creationDate { get; set; }
        [Display(Name = "Fecha Actualizado")]
        public DateTime updateDate { get; set; }
        [Display(Name = "Fecha Cierre")]
        public DateTime? closeDate { get; set; }
        [Display(Name = "Fecha Estimada")]
        public DateTime? estimatedDate { get; set; }
        [Display(Name = "Activo")]
        public bool active { get; set; }
        public int idClient { get; set; }
        public int idSupervisor { get; set; }
        public int idService { get; set; }
        public int idStatus { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string? ubication { get; set; }

        [Display(Name = "Cliente")]
        public User? client { get; set; }
        [Display(Name = "Supervisor")]
        public User? supervisor { get; set; }
        [Display(Name = "Servicio")]
        public Service? service { get; set; }
        [Display(Name = "Estado")]
        public Status? status { get; set; }
        [Display(Name = "Comentarios")]
        public List<Comment>? comments { get; set; }

        public List<Documento>? documents { get; set; }

        public List<IFormFile>? Archivos { get; set; }
    }

}
