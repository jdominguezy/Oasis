
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Oasis.Models
{

using System;
    using System.Collections.Generic;
    
public partial class factura_cliente
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public factura_cliente()
    {

        this.factura_cliente1 = new HashSet<factura_cliente>();

    }


    public int id_factura_cliente { get; set; }

    public Nullable<System.DateTime> fecha_creacion { get; set; }

    public Nullable<System.DateTime> fecha_modificacion { get; set; }

    public Nullable<int> id_entidad_origen { get; set; }

    public string usuario_creacion { get; set; }

    public string usuario_modificacion { get; set; }

    public string archivo { get; set; }

    public Nullable<int> autorizacion_liberacion { get; set; }

    public int cantidad_garantias { get; set; }

    public string codigo_movil { get; set; }

    public string descripcion { get; set; }

    public string descripcion2 { get; set; }

    public Nullable<decimal> descuento { get; set; }

    public decimal descuento_impuesto { get; set; }

    public int dias_margen_tolerancia_puntos { get; set; }

    public string direccion_factura { get; set; }

    public string email { get; set; }

    public int estado { get; set; }

    public System.DateTime fecha { get; set; }

    public Nullable<System.DateTime> fecha_contabilizacion { get; set; }

    public Nullable<System.DateTime> fecha_vencimiento { get; set; }

    public decimal flete_internacional { get; set; }

    public decimal gastos_aduaneros { get; set; }

    public Nullable<int> id_dispositivo_sincronizacion { get; set; }

    public string id_externo { get; set; }

    public string id_factura_unosoft { get; set; }

    public int id_organizacion { get; set; }

    public decimal impuesto { get; set; }

    public string incoterm { get; set; }

    public bool indicador_aprobada_calidad { get; set; }

    public Nullable<bool> indicador_automatico { get; set; }

    public bool indicador_emision_retencion { get; set; }

    public bool indicador_genera_cxc { get; set; }

    public bool indicador_generada_por_mora { get; set; }

    public Nullable<bool> indicador_generado_prefactura { get; set; }

    public bool indicador_genero_transformacion { get; set; }

    public bool indicador_renegociada { get; set; }

    public bool indicador_saldo_inicial { get; set; }

    public string indicador_sincronizacion_uno_soft { get; set; }

    public bool indicador_solicitado_anulacion { get; set; }

    public bool maneja_descuento_adicional { get; set; }

    public string numero { get; set; }

    public int numero_cuotas { get; set; }

    public int origen { get; set; }

    public decimal otros_gastos_transporte { get; set; }

    public string pdf { get; set; }

    public decimal puntos { get; set; }

    public string referencia1 { get; set; }

    public string referencia10 { get; set; }

    public string referencia2 { get; set; }

    public string referencia3 { get; set; }

    public string referencia4 { get; set; }

    public string referencia5 { get; set; }

    public string referencia6 { get; set; }

    public string referencia7 { get; set; }

    public string referencia8 { get; set; }

    public string referencia9 { get; set; }

    public Nullable<bool> requiere_codeudor { get; set; }

    public Nullable<bool> requiere_garante { get; set; }

    public decimal saldo_puntos { get; set; }

    public decimal saldo_valor_puntos { get; set; }

    public decimal seguro_internacional { get; set; }

    public decimal total { get; set; }

    public string usuario_autoriza_ventas { get; set; }

    public decimal valor_cuota_inicial { get; set; }

    public Nullable<decimal> valor_devuelto { get; set; }

    public decimal valor_otros { get; set; }

    public decimal valor_puntos { get; set; }

    public Nullable<decimal> valor_referencia1 { get; set; }

    public Nullable<decimal> valor_referencia2 { get; set; }

    public Nullable<decimal> valor_referencia3 { get; set; }

    public Nullable<int> id_agente_comercial { get; set; }

    public Nullable<int> id_asiento { get; set; }

    public Nullable<int> id_autorizacion_documento_punto_de_venta_autoimpresor_SRI { get; set; }

    public Nullable<int> id_canal { get; set; }

    public Nullable<int> id_garante_codeudor { get; set; }

    public Nullable<int> id_condicion_pago { get; set; }

    public Nullable<int> id_despacho_cliente { get; set; }

    public Nullable<int> id_garante_deudor { get; set; }

    public int id_direccion_empresa { get; set; }

    public int id_documento { get; set; }

    public int id_empresa { get; set; }

    public Nullable<int> id_factura_cliente_padre { get; set; }

    public Nullable<int> id_forma_pago { get; set; }

    public Nullable<int> id_interfaz_contable_proceso { get; set; }

    public Nullable<int> id_ciudad_incoterm { get; set; }

    public Nullable<int> id_motivo_anulacion { get; set; }

    public Nullable<int> id_motivo_nota_credito_cliente { get; set; }

    public Nullable<int> id_numero_cuota { get; set; }

    public Nullable<int> id_pais_adquisicion { get; set; }

    public Nullable<int> id_pais_destino { get; set; }

    public Nullable<int> id_pais_origen { get; set; }

    public Nullable<int> id_pedido_cliente { get; set; }

    public Nullable<int> id_proyecto { get; set; }

    public Nullable<int> id_puerto_destino { get; set; }

    public Nullable<int> id_puerto_embarque { get; set; }

    public Nullable<int> id_subempresa { get; set; }

    public Nullable<int> id_sucursal { get; set; }

    public Nullable<int> id_transportista { get; set; }

    public Nullable<int> id_zona { get; set; }

    public string descripcion_anulacion { get; set; }

    public string descripcion_factura_flor { get; set; }

    public bool indicador_crear_factura_cliente_agil { get; set; }

    public bool indicador_recepcion_automatica { get; set; }

    public string ids_detalles_cobros_prestamos { get; set; }

    public Nullable<int> id_persona_responsable { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<factura_cliente> factura_cliente1 { get; set; }

    public virtual factura_cliente factura_cliente2 { get; set; }

    public virtual empresa empresa { get; set; }

    public virtual usuario usuario { get; set; }

    public virtual direccion_empresa direccion_empresa { get; set; }

}

}
