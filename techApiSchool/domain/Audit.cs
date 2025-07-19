using System.ComponentModel.DataAnnotations;

namespace domain;

//clase auditoria
public class Audit
{
    #region columnas
    public int Id 
    { get; set; }

    #region nombre de la tabla
    public string? TableName 
    { get; set; } = string.Empty;

    #endregion

    #region id de la tabla
    public string? RecordId 
    { get; set; } = string.Empty;
    #endregion

    #region valor nuevo o modificado
    public string? Infovalue 
    { get; set; }
    #endregion

    #region accion realizada
    public string? Action 
    { get; set; } = string.Empty;
    #endregion

    #region quien realizó la acción
    public string? ModifiedBy 
    { get; set; } = string.Empty;
    #endregion

    #region cuando se realizó la acción
    public DateTime? ModifiedAt 
    { get; set; } = DateTime.UtcNow;
    #endregion

    #endregion


    #region tablas relacionadas 

    #endregion
}
