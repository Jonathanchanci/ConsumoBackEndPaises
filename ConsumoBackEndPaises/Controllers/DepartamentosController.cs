using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConsumoBackEndPaises.Data;
using ConsumoBackEndPaises.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace ConsumoBackEndPaises.Controllers
{
    public class DepartamentosController : Controller
    {
        private ConsumoBackEndPaisesContext db = new ConsumoBackEndPaisesContext();
        const string url = "http://localhost:58525/api/departamentos/";
        const string urlP = "http://localhost:58525/api/pais/";
        // GET: Departamentos
        public async Task<ActionResult> Index()
        {
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var dep = JsonConvert.DeserializeObject<List<Departamento>>(responseBody);
                    foreach (var item in dep)
                    {
                        HttpResponseMessage responsePais = await cliente.GetAsync(urlP + item.idPais);
                        if (responsePais.IsSuccessStatusCode)
                        {
                            var responseBodyP = await responsePais.Content.ReadAsStringAsync();
                            item.pais = JsonConvert.DeserializeObject<Pais>(responseBodyP);
                        }
                    }
                    
                    return View(dep);
                }
                else
                {
                    return View();
                }
            }
            //var departamentoes = db.Departamentoes.Include(d => d.pais);
            //return View(await departamentoes.ToListAsync());
        }

        // GET: Departamentos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.GetAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var dep = JsonConvert.DeserializeObject<Departamento>(responseBody);
                    
                    HttpResponseMessage responsePais = await cliente.GetAsync(urlP + dep.idPais);
                    if (responsePais.IsSuccessStatusCode)
                    {
                        var responseBodyP = await responsePais.Content.ReadAsStringAsync();
                        dep.pais = JsonConvert.DeserializeObject<Pais>(responseBodyP);
                    }                    

                    return View(dep);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            //if (departamento == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(departamento);
        }

        // GET: Departamentos/Create
        public ActionResult Create()
        {
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage responsePais = cliente.GetAsync(urlP).Result;
                if (responsePais.IsSuccessStatusCode)
                {
                    var responseBodyP = responsePais.Content.ReadAsStringAsync().Result;
                    var paises = JsonConvert.DeserializeObject<List<Pais>>(responseBodyP);
                    ViewBag.idPais = new SelectList(paises, "idPais", "NombrePais");
                }
            }
            return View();
        }

        // POST: Departamentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,nombre,descripcion,idPais")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                var content = JsonConvert.SerializeObject(departamento);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (HttpClient cliente = new HttpClient())
                {
                    HttpResponseMessage response = await cliente.PostAsync(url, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(departamento);
                    }
                }
                //db.Departamentoes.Add(departamento);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage responsePais = cliente.GetAsync(urlP).Result;
                if (responsePais.IsSuccessStatusCode)
                {
                    var responseBodyP = responsePais.Content.ReadAsStringAsync().Result;
                    var paises = JsonConvert.DeserializeObject<List<Pais>>(responseBodyP);
                    ViewBag.idPais = new SelectList(paises, "idPais", "NombrePais", departamento.idPais);
                }
            }
            return View(departamento);
        }

        // GET: Departamentos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento departamento = new Departamento();
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.GetAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    departamento = JsonConvert.DeserializeObject<Departamento>(responseBody);                    
                }
                else
                {
                    return HttpNotFound();
                }
            }
            if (departamento == null)
            {
                return HttpNotFound();
            }
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage responsePais = cliente.GetAsync(urlP).Result;
                if (responsePais.IsSuccessStatusCode)
                {
                    var responseBodyP = responsePais.Content.ReadAsStringAsync().Result;
                    var paises = JsonConvert.DeserializeObject<List<Pais>>(responseBodyP);
                    ViewBag.idPais = new SelectList(paises, "idPais", "NombrePais", departamento.idPais);
                }
            }
            return View(departamento);
        }

        // POST: Departamentos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,nombre,descripcion,idPais")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                var content = JsonConvert.SerializeObject(departamento);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (HttpClient cliente = new HttpClient())
                {
                    HttpResponseMessage response = await cliente.PutAsync(url + departamento.id, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(departamento);
                    }
                }
                //db.Entry(departamento).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage responsePais = cliente.GetAsync(urlP).Result;
                if (responsePais.IsSuccessStatusCode)
                {
                    var responseBodyP = responsePais.Content.ReadAsStringAsync().Result;
                    var paises = JsonConvert.DeserializeObject<List<Pais>>(responseBodyP);
                    ViewBag.idPais = new SelectList(paises, "idPais", "NombrePais", departamento.idPais);
                }
            }
            return View(departamento);
        }

        // GET: Departamentos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento departamento = new Departamento();
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.GetAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    departamento = JsonConvert.DeserializeObject<Departamento>(responseBody);

                    HttpResponseMessage responsePais = await cliente.GetAsync(urlP + departamento.idPais);
                    if (responsePais.IsSuccessStatusCode)
                    {
                        var responseBodyP = await responsePais.Content.ReadAsStringAsync();
                        departamento.pais = JsonConvert.DeserializeObject<Pais>(responseBodyP);
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        // POST: Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.DeleteAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            //Departamento departamento = await db.Departamentoes.FindAsync(id);
            //db.Departamentoes.Remove(departamento);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
