using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Servicios;

namespace Web.Controllers;

/// <summary>
/// Controlador MVC de etiquetas: lista, detalle y creación. Todo el HTTP vive en IEtiquetasApi;
/// los errores inesperados del API los atrapa ApiExceptionFilter. Aquí solo se decide qué respuesta darle al navegador.
/// </summary>
public class EtiquetasController : Controller
{
    private readonly IEtiquetasApi _api;

    public EtiquetasController(IEtiquetasApi api) => _api = api;

    // ---------------- Lectura ----------------

    public async Task<IActionResult> Index(CancellationToken ct = default) =>
        View(await _api.ListarAsync(ct));

    public async Task<IActionResult> Detalle(int id, CancellationToken ct = default)
    {
        var etiqueta = await _api.ObtenerAsync(id, ct);
        return etiqueta is null ? NotFound() : View(etiqueta);
    }

    // ---------------- Formularios ----------------

    [HttpGet]
    public IActionResult Crear() => View(new EtiquetaCrearVm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(EtiquetaCrearVm datos, CancellationToken ct = default)
    {
        // 1) El propio MVC valida las anotaciones del ViewModel.
        if (!ModelState.IsValid) return View(datos);

        try
        {
            var creada = await _api.CrearAsync(datos, ct);
            TempData["Exito"] = $"Etiqueta '{creada.Nombre}' creada.";

            // Post-Redirect-Get: si se devolviera View(...) aquí, F5 reenviaría el formulario y fallaría
            // por nombre repetido (409). Con la redirección, F5 solo repite un GET al detalle.
            return RedirectToAction(nameof(Detalle), new { id = creada.Id });
        }
        // 2) Si el API rechaza (400 por campo o 409 por nombre repetido), el mensaje vuelve al formulario.
        catch (ApiException ex) when (ex.Codigo is 400 or 409 or 422)
        {
            AgregarErroresDelApi(ex);
            return View(datos);
        }
    }

    private void AgregarErroresDelApi(ApiException ex)
    {
        // Sin errores por campo (409 o 422): un mensaje general. Con errores por campo (400): uno por campo.
        if (ex.Errores.Count == 0) ModelState.AddModelError(string.Empty, ex.Message);
        foreach (var (campo, mensajes) in ex.Errores)
            foreach (var mensaje in mensajes)
                ModelState.AddModelError(campo, mensaje);
    }
}
