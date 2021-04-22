using BE;
using BLL;
using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class LoginController : GenericController
    {
        private BitacoraBLL bitacoraBll = new BitacoraBLL();
        UsuarioBLL bllUsuario = new UsuarioBLL();
        PermisoBLL bllPermiso = new PermisoBLL();
        IdiomaBLL bllIdioma = new IdiomaBLL();
        ClienteBLL clienteBLL = new ClienteBLL();

        // GET: Login

        public ActionResult Index()
        {
            Session["intentos"] = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {

            Usuario u = bllUsuario.Consulta_login(UserName, Password);
            if (u != null)//existe usuario
            {
                Session["intentos"] = 0;
                Session["usuario"] = u;
                Bitacora b = new Bitacora();
                b.fecha = DateTime.Now;
                b.mensaje = "Login OK";
                b.Tipo = TipoLog.INFO;
                b.Usuario = u;
                bitacoraBll.Grabar(b);

                List<Permiso> permisos = bllPermiso.VerificarPermisosUsuario(u.ID);
                List<string> permisos_usuario = new List<string>();
                foreach (Permiso permiso in permisos)
                {
                    permisos_usuario.Add(permiso.Nombre);
                }
                Session["permisos"] = permisos_usuario;


                List<Etiqueta> etiquetas = new List<Etiqueta>();
                etiquetas = bllIdioma.ListarEtiquetasPorIdioma(bllIdioma.ListarIdioma(u.idioma.Id).Descripcion);
                Dictionary<object, object> list = new Dictionary<object, object>();
                foreach (Etiqueta etiqueta in etiquetas)
                {
                    list.Add(etiqueta.Descripcion, etiqueta.Traduccion);
                }
                Session["EtiquetasIDIOMA"] = list;

                List<Idioma> idiomas = new List<Idioma>();
                idiomas = bllIdioma.ListarIdiomas();
                Session["Idiomas"] = idiomas;

                //se calcula de la tabla de usuario el DVV
                string dvv = bllUsuario.calcularDVV();
                //Busco el que esta guardado en la tabla
                string dvvUsuarioTabla = bllUsuario.getDVV();
                if (dvvUsuarioTabla.Equals(dvv))
                {
                    if(permisos_usuario.Contains("Cliente")) 
                    {
                        //buscar el cliente
                        Cliente c = clienteBLL.ListarPorUsuario(u.ID);
                        if (c != null)
                        {
                            Session["cliente"] = c;
                            return RedirectToAction("Index", "Poliza");
                        }
                        else {
                            Session["usuario"] = null;
                            Session["permisos"] = null;
                            ViewBag.mensaje = "Error en los permisos";
                            return View("Index");
                        }

                    }
                    //Redireccion segun usuario
                    return RedirectToAction("Bienvenido", "Home");
                }
                else
                {
                    //Si tiene permiso para accionar
                    if (verificarPermiso("Administrador", (Usuario)Session["usuario"]))
                    {
                        return RedirectToAction("Bienvenido", "Home");
                    }
                    else
                    {
                        Session["usuario"] = null;
                        Session["permisos"] = null;
                        ViewBag.mensaje = "Error Integridad de datos";
                        return View("Index");
                    }
                    
                }



            }
            else
            {
                ViewBag.mensaje = "Credenciales Incorrectas";
                Session["intentos"] = int.Parse(Session["intentos"].ToString()) + 1;
                if (int.Parse(Session["intentos"].ToString()) == 3)
                {
                    bllUsuario.Bloquear(UserName);
                    ViewBag.mensaje = "Usuario Bloqueado";
                    Session["intentos"] = 0;
                }
                return View("Index");
            }
        }




        public ActionResult LogOut()
        {
            Session["usuario"] = null;
            Session["permisos"] = null;
            Session["intentos"] = 0;
            Session["Idiomas"] = null;
            Session["cliente"] = null;
            Session["medico"] = null;
            Session["vendedor"] = null;
            Session["EtiquetasIDIOMA"] = null;
            
            return RedirectToAction("Index", "Login");
        }

        public ActionResult OlvidoContrasena()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OlvidoContrasena(Usuario user)
        {
            UsuarioBLL usuarioBll = new UsuarioBLL();
            Usuario usuario = usuarioBll.Listar(user.usuario);

            if (usuario != null)
            {
                string pass = Encriptor.GenerarPassword(10);
                MailMessage Correo = new MailMessage();

                string mailFrom = WebConfigurationManager.AppSettings["from_mail"];
                string usuarioMail = WebConfigurationManager.AppSettings["usuario_mail"];
                string passMail = WebConfigurationManager.AppSettings["pass_mail"];

                Correo.From = new MailAddress(mailFrom);
                Correo.To.Add(usuario.correo);
                Correo.Subject = "Password";
                Correo.Body = "pass: " + pass;
                Correo.Priority = MailPriority.Normal;

                SmtpClient ServerEmail = new SmtpClient();
                ServerEmail.Credentials = new System.Net.NetworkCredential(usuarioMail, passMail);
                ServerEmail.Host = "smtp.gmail.com";
                ServerEmail.Port = 587;
                ServerEmail.EnableSsl = true;

                Bitacora b = new Bitacora();
                b.fecha = DateTime.Now;
                b.mensaje = "Recupero de pass";
                b.Usuario = usuario;

                try
                {
                    ServerEmail.Send(Correo);
                    usuario.password = pass;
                    usuarioBll.Grabar(usuario);
                    b.Tipo = TipoLog.INFO;

                }
                catch (Exception e)
                {
                    b.Tipo = TipoLog.ERROR;
                    Console.Write(e);
                }
                bitacoraBll.Grabar(b);
                Correo.Dispose();
                return View(@"~\Views\Login\CambioPassOK.cshtml");
            }
            else
            {
                ViewBag.existe = "Usuario inexistente";
                return View();
            }

        }
    }
}
