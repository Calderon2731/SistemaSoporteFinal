namespace Web.Models;

/// <summary>
/// ViewModel de una etiqueta para la lista y el detalle. Es la forma que ESTE proyecto necesita del JSON del API.
/// Se parece a EtiquetaDto del API, pero es OTRA clase, escrita aquí a propósito: el contrato entre ambos
/// proyectos es el JSON, no una clase compartida.
/// </summary>
public class EtiquetaVm
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
