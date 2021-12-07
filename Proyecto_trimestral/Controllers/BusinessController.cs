﻿using Microsoft.AspNetCore.Mvc;
using Proyecto_trimestral.Models;
using Proyecto_trimestral.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_trimestral.Controllers
{
    public class BusinessController : Controller
    {
        public IEnumerable<Business> Index()
        {
            string sql = "SELECT nombre FROM negocio";
            DataBase db = new DataBase();
            DataTable dt = db.getTabla(sql);

            List<Business> listBusinesses = new List<Business>();

            listBusinesses = (from DataRow dr in dt.Rows
                              select new Business()
                              {
                                  name = dr["nombre"].ToString()
                              }).ToList();
            return listBusinesses;
        }


        [HttpPost]

        public string Create([FromBody] Business business)
        {

            
            string sql = "INSERT INTO negocio(nombre, categoria) VALUES('" + business.name + "', '"+ business.categories +"');" + Environment.NewLine;

            foreach (Products p in business.products)
            {
                sql += "INSERT INTO productos(nombre, precio, negocio) VALUES( '" + p.name + "', '"+ p.price + "', (select max(idnegocio) from negocio));" + Environment.NewLine;
            }
            sql += "" + Environment.NewLine;

            foreach (Services services in business.services )
            {
                sql += "INSERT INTO servicios(nombre, negocio) VALUES('"+ services.name + "', (select max(idnegocio) from negocio));" + Environment.NewLine;
            }
            sql += "" + Environment.NewLine;
            DataBase db = new DataBase();
            string result = db.consultaSQL(sql);
            return result;
     
        }
    }
}