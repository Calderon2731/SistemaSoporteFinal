using Web.Models;

namespace Web.Servicios;

/// <summary>
/// Lo que el MVC sabe hacer con las etiquetas del API. El controlador depende de esta interfaz (no de HttpClient):
/// así se puede cambiar el HTTP sin tocar controladores.
/// </summary>
public interface IEtiquetasApi
{
    Task<List<EtiquetaVm>> ListarAsync(CancellationToken ct = default);

    /// <summary>Devuelve null si la etiqueta no existe (404). Cualquier otro error lanza ApiException.</summary>
    Task<EtiquetaVm?> ObtenerAsync(int id, CancellationToken ct = default);

    Task<EtiquetaVm> CrearAsync(EtiquetaCrearVm datos, CancellationToken ct = default);
}
