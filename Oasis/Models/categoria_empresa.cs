
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
    
public partial class categoria_empresa
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public categoria_empresa()
    {

        this.empresa = new HashSet<empresa>();

    }


    public int id_categoria_empresa { get; set; }

    public Nullable<System.DateTime> fecha_creacion { get; set; }

    public Nullable<System.DateTime> fecha_modificacion { get; set; }

    public Nullable<int> id_entidad_origen { get; set; }

    public string usuario_creacion { get; set; }

    public string usuario_modificacion { get; set; }

    public bool activo { get; set; }

    public string codigo { get; set; }

    public Nullable<decimal> credito_maximo { get; set; }

    public string descripcion { get; set; }

    public int id_organizacion { get; set; }

    public int id_sucursal { get; set; }

    public bool indicador_cliente { get; set; }

    public bool indicador_empleado { get; set; }

    public bool indicador_proveedor { get; set; }

    public string nombre { get; set; }

    public Nullable<int> numero_maximo_documentos_sin_garantia { get; set; }

    public Nullable<decimal> porcentaje_maximo_morosidad { get; set; }

    public bool predeterminado { get; set; }

    public bool verificar_documentos { get; set; }

    public Nullable<int> id_cuenta_contable_2x1000 { get; set; }

    public Nullable<int> id_cuenta_contable_3x1000 { get; set; }

    public Nullable<int> id_cuenta_contable_anticipo_cliente { get; set; }

    public Nullable<int> id_cuenta_contable_anticipo_cliente_nota_credito { get; set; }

    public Nullable<int> id_cuenta_contable_anticipo_proveedor { get; set; }

    public Nullable<int> id_cuenta_contable_anticipo_proveedor_nota_credito { get; set; }

    public Nullable<int> id_cuenta_contable_cliente { get; set; }

    public Nullable<int> id_cuenta_contable_iva_presuntivo { get; set; }

    public Nullable<int> id_cuenta_contable_proveedor { get; set; }

    public Nullable<int> id_cuenta_contable_suedo_por_pagar { get; set; }

    public Nullable<int> longitud { get; set; }

    public string prefijo { get; set; }

    public Nullable<int> valor_maximo { get; set; }

    public Nullable<int> valor_minimo { get; set; }

    public Nullable<int> dias_despacho { get; set; }

    public Nullable<int> hora_desde { get; set; }

    public Nullable<int> hora_hasta { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<empresa> empresa { get; set; }

}

}
