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
    public class PaisController : Controller
    {
        private ConsumoBackEndPaisesContext db = new ConsumoBackEndPaisesContext();
        const string url = "http://localhost:58525/api/pais/";

        // GET: Pais
        public async Task<ActionResult> Index()
        {
            try
            {
                HttpClient cliente = new HttpClient();
                HttpResponseMessage response = await cliente.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var paises = JsonConvert.DeserializeObject<List<Pais>>(responseBody);
                    return View(paises);
                }
                else
                {
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        // GET: Pais/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = new Pais();
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.GetAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    pais = JsonConvert.DeserializeObject<Pais>(responseBody);
                }
                else
                {
                    return HttpNotFound();
                }
            }            
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // GET: Pais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pais/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "idPais,NombrePais,descripcion")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                var content = JsonConvert.SerializeObject(pais);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using(HttpClient cliente = new HttpClient())
                {
                    HttpResponseMessage response = await cliente.PostAsync(url, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(pais);
                    }
                }
                //db.Pais.Add(pais);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            return View(pais);
        }

        // GET: Pais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = new Pais();
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.GetAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    pais = JsonConvert.DeserializeObject<Pais>(responseBody);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // POST: Pais/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "idPais,NombrePais,descripcion")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                var content = JsonConvert.SerializeObject(pais);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (HttpClient cliente = new HttpClient())
                {
                    HttpResponseMessage response = await cliente.PutAsync(url + pais.idPais, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(pais);
                    }
                }
                //db.Entry(pais).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            return View(pais);
        }

        // GET: Pais/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = new Pais();
            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.GetAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    pais = JsonConvert.DeserializeObject<Pais>(responseBody);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // POST: Pais/Delete/5
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
                    return RedirectToAction("Delete");
                }
            }
            Pais pais = await db.Pais.FindAsync(id);
            db.Pais.Remove(pais);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
