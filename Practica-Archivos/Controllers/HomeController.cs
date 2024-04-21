using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Practica_Archivos.Models;
using System.Diagnostics;

namespace Practica_Archivos.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> SubirArchivo(IFormFile archivo)
		{



			//Leemos el archivo subido. Stream archivoASubir archivo. OpenReadStream();
			Stream archivoAsubir = archivo.OpenReadStream();

			//Configuramos la conexion hacia FireBase
			string email = "jacquelinne.cruz@catolica.edu.sv"; // Correo para autenticar en FireBase
			string clave = "Hola1234"; // Contraseña establaecida en la autenticar en Firebase
			string ruta = "castorlandia-394d4.appspot.com"; // URL donde se guardaran los archivo. string api_key = "AIzaSyAhSFON@lsHDGE "
			string api_key = "AIzaSyDoUTbDpNDrYa7yz69IvWXX3GIhSHq7ia8";


			var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
			var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

			var cancellation = new CancellationTokenSource();
			var tokenUser = autenticarFireBase.FirebaseToken;


			var tareaCargarArchivo = new FirebaseStorage(ruta,
														new FirebaseStorageOptions
														{
															AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
															ThrowOnCancel = true
														}

													).Child("Arhivos")
													.Child(archivo.FileName)
													.PutAsync(archivoAsubir, cancellation.Token);

			var urlArchivoCargado = await tareaCargarArchivo;


			return RedirectToAction("Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
