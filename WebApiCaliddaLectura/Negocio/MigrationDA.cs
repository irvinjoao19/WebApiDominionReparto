using Entidades;
using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class MigrationDA
    {
        private static string db = ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public static Migracion GetMigracion(int operarioId, string version)
        {
            try
            {
                Migracion migracion = new Migracion();

                migracion.migrationId = 1;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    // Version

                    SqlCommand cmdVersion = con.CreateCommand();
                    cmdVersion.CommandTimeout = 0;
                    cmdVersion.CommandType = CommandType.StoredProcedure;
                    cmdVersion.CommandText = "USP_GET_VERSION";
                    cmdVersion.Parameters.Add("@version", SqlDbType.VarChar).Value = version;

                    SqlDataReader drVersion = cmdVersion.ExecuteReader();
                    if (!drVersion.HasRows)
                    {
                        migracion.mensaje = "Actualizar Versión del Aplicativo.";
                    }
                    else
                    {
                        // Servicios
                        SqlCommand cmdServicio = con.CreateCommand();
                        cmdServicio.CommandTimeout = 0;
                        cmdServicio.CommandType = CommandType.StoredProcedure;
                        cmdServicio.CommandText = "USP_SERVICIOS";
                        SqlDataReader drServicio = cmdServicio.ExecuteReader();
                        if (drServicio.HasRows)
                        {
                            List<Servicio> servicio = new List<Servicio>();
                            while (drServicio.Read())
                            {
                                servicio.Add(new Servicio()
                                {
                                    id_servicio = drServicio.GetInt32(0),
                                    nombre_servicio = drServicio.GetString(1),
                                    estado = drServicio.GetInt32(2)
                                });
                            }
                            migracion.servicios = servicio;
                        }

                        // Parametro

                        SqlCommand cmdParametro = con.CreateCommand();
                        cmdParametro.CommandTimeout = 0;
                        cmdParametro.CommandType = CommandType.StoredProcedure;
                        cmdParametro.CommandText = "USP_PARAMETROS";
                        SqlDataReader drParametro = cmdParametro.ExecuteReader();
                        if (drParametro.HasRows)
                        {
                            List<Parametro> parametro = new List<Parametro>();
                            while (drParametro.Read())
                            {
                                parametro.Add(new Parametro()
                                {
                                    id_Configuracion = drParametro.GetInt32(0),
                                    nombre_parametro = drParametro.GetString(1),
                                    valor = drParametro.GetInt32(2)
                                });
                            }
                            migracion.parametros = parametro;
                        }

                        // Suministro
                        //SqlCommand cmdSuministro = con.CreateCommand();
                        //cmdSuministro.CommandTimeout = 0;
                        //cmdSuministro.CommandType = CommandType.StoredProcedure;
                        //cmdSuministro.CommandText = "USP_LIST_SUMINISTRO";
                        //cmdSuministro.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                        //SqlDataReader drSuministro = cmdSuministro.ExecuteReader();
                        //if (drSuministro.HasRows)
                        //{
                        //    List<Suministro> suministro = new List<Suministro>();
                        //    int i = 1;
                        //    while (drSuministro.Read())
                        //    {
                        //        suministro.Add(new Suministro()
                        //        {
                        //            iD_Suministro = drSuministro.GetInt32(0),
                        //            suministro_Numero = drSuministro.GetString(1),
                        //            suministro_Medidor = drSuministro.GetString(2),
                        //            suministro_Cliente = drSuministro.GetString(3),
                        //            suministro_Direccion = drSuministro.GetString(4),
                        //            suministro_UnidadLectura = drSuministro.GetString(5),
                        //            suministro_TipoProceso = drSuministro.GetString(6),
                        //            suministro_LecturaMinima = drSuministro.GetString(7),
                        //            suministro_LecturaMaxima = drSuministro.GetString(8),
                        //            suministro_Fecha_Reg_Movil = drSuministro.GetDateTime(9).ToString("dd/MM/yyyy"),
                        //            suministro_UltimoMes = drSuministro.GetString(10),
                        //            consumoPromedio = drSuministro.GetDecimal(11),
                        //            lecturaAnterior = drSuministro.GetString(12),
                        //            suministro_Instalacion = drSuministro.GetString(13),
                        //            valida1 = drSuministro.GetInt32(14),
                        //            valida2 = drSuministro.GetInt32(15),
                        //            valida3 = drSuministro.GetInt32(16),
                        //            valida4 = drSuministro.GetInt32(17),
                        //            valida5 = drSuministro.GetInt32(18),
                        //            valida6 = drSuministro.GetInt32(19),
                        //            tipoCliente = drSuministro.GetInt32(20),
                        //            estado = drSuministro.GetInt32(21),
                        //            suministroOperario_Orden = drSuministro.GetInt32(22),
                        //            flagObservada = drSuministro.GetInt32(23),
                        //            latitud = drSuministro.GetString(24),
                        //            longitud = drSuministro.GetString(25),
                        //            fechaAsignacion = drSuministro.GetString(26),
                        //            orden = i++,
                        //            activo = 1
                        //        });
                        //    }
                        //    migracion.suministroLecturas = suministro;
                        //}

                        // SuministroCorte 
                        //SqlCommand cmdCortes = con.CreateCommand();
                        //cmdCortes.CommandTimeout = 0;
                        //cmdCortes.CommandType = CommandType.StoredProcedure;
                        //cmdCortes.CommandText = "USP_LIST_SUMINISTRO_CORTES";
                        //cmdCortes.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                        //cmdCortes.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = "3";
                        //SqlDataReader drCortes = cmdCortes.ExecuteReader();
                        //if (drCortes.HasRows)
                        //{
                        //    List<Suministro> suministroCorte = new List<Suministro>();
                        //    int y = 1;
                        //    while (drCortes.Read())
                        //    {
                        //        suministroCorte.Add(new Suministro()
                        //        {
                        //            iD_Suministro = drCortes.GetInt32(0),
                        //            suministro_Numero = drCortes.GetString(1),
                        //            suministro_Medidor = drCortes.GetString(2),
                        //            suministro_Cliente = drCortes.GetString(3),
                        //            suministro_Direccion = drCortes.GetString(4),
                        //            suministro_UnidadLectura = drCortes.GetString(5),
                        //            suministro_TipoProceso = drCortes.GetString(6),
                        //            suministro_LecturaMinima = drCortes.GetString(7),
                        //            suministro_LecturaMaxima = drCortes.GetString(8),
                        //            suministro_Fecha_Reg_Movil = drCortes.GetDateTime(9).ToString("dd/MM/yyyy"),
                        //            suministro_UltimoMes = drCortes.GetString(10),
                        //            suministro_NoCortar = drCortes.GetInt32(11),
                        //            estado = drCortes.GetInt32(12),
                        //            suministroOperario_Orden = drCortes.GetInt32(13),
                        //            latitud = drCortes.GetString(14),
                        //            longitud = drCortes.GetString(15),
                        //            fechaAsignacion = "",
                        //            orden = y++,
                        //            activo = 1
                        //        });
                        //    }
                        //    migracion.suministroCortes = suministroCorte;
                        //}

                        // SuministroReconexiones
                        //SqlCommand cmdReconexiones = con.CreateCommand();
                        //cmdReconexiones.CommandTimeout = 0;
                        //cmdReconexiones.CommandType = CommandType.StoredProcedure;
                        //cmdReconexiones.CommandText = "USP_LIST_SUMINISTRO_CORTES";
                        //cmdReconexiones.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                        //cmdReconexiones.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = "4";
                        //
                        //SqlDataReader drReconexiones = cmdReconexiones.ExecuteReader();
                        //if (drReconexiones.HasRows)
                        //{
                        //    List<Suministro> suministroReconexiones = new List<Suministro>();
                        //    int z = 1;
                        //    while (drReconexiones.Read())
                        //    {
                        //        suministroReconexiones.Add(new Suministro()
                        //        {
                        //            iD_Suministro = drReconexiones.GetInt32(0),
                        //            suministro_Numero = drReconexiones.GetString(1),
                        //            suministro_Medidor = drReconexiones.GetString(2),
                        //            suministro_Cliente = drReconexiones.GetString(3),
                        //            suministro_Direccion = drReconexiones.GetString(4),
                        //            suministro_UnidadLectura = drReconexiones.GetString(5),
                        //            suministro_TipoProceso = drReconexiones.GetString(6),
                        //            suministro_LecturaMinima = drReconexiones.GetString(7),
                        //            suministro_LecturaMaxima = drReconexiones.GetString(8),
                        //            suministro_Fecha_Reg_Movil = drReconexiones.GetDateTime(9).ToString("dd/MM/yyyy"),
                        //            suministro_UltimoMes = drReconexiones.GetString(10),
                        //            suministro_NoCortar = drReconexiones.GetInt32(11),
                        //            estado = drReconexiones.GetInt32(12),
                        //            suministroOperario_Orden = drReconexiones.GetInt32(13),
                        //            latitud = drReconexiones.GetString(14),
                        //            longitud = drReconexiones.GetString(15),
                        //            fechaAsignacion = "",
                        //            orden = z++,
                        //            activo = 1
                        //        });
                        //    }
                        //    migracion.suministroReconexiones = suministroReconexiones;
                        //}

                        // Tipo Lectura

                        SqlCommand cmdTipo = con.CreateCommand();
                        cmdTipo.CommandTimeout = 0;
                        cmdTipo.CommandType = CommandType.StoredProcedure;
                        cmdTipo.CommandText = "USP_LIST_TIPO_LECTURA";
                        SqlDataReader drTipo = cmdTipo.ExecuteReader();
                        if (drTipo.HasRows)
                        {
                            List<TipoLectura> tipoLectura = new List<TipoLectura>();
                            while (drTipo.Read())
                            {
                                tipoLectura.Add(new TipoLectura()
                                {
                                    iD_TipoLectura = drTipo.GetInt32(0),
                                    tipoLectura_Descripcion = drTipo.GetString(1),
                                    tipoLectura_Abreviatura = drTipo.GetString(2),
                                    tipoLectura_Estado = drTipo.GetString(3)
                                });
                            }
                            migracion.tipoLecturas = tipoLectura;
                        }

                        //Detalle Grupo

                        //SqlCommand cmdGrupo = con.CreateCommand();
                        //cmdGrupo.CommandTimeout = 0;
                        //cmdGrupo.CommandType = CommandType.StoredProcedure;
                        //cmdGrupo.CommandText = "USP_LIST_DETALLE_GRUPO";
                        //SqlDataReader rdGrupo = cmdGrupo.ExecuteReader();
                        //if (rdGrupo.HasRows)
                        //{
                        //    List<DetalleGrupo> detalleGrupo = new List<DetalleGrupo>();
                        //    int j = 1;
                        //    while (rdGrupo.Read())
                        //    {
                        //        detalleGrupo.Add(new DetalleGrupo()
                        //        {
                        //            id = j++,
                        //            iD_DetalleGrupo = rdGrupo.GetInt32(0),
                        //            grupo = rdGrupo.GetString(1),
                        //            codigo = rdGrupo.GetString(2),
                        //            descripcion = rdGrupo.GetString(3),
                        //            abreviatura = rdGrupo.GetString(4),
                        //            estado = rdGrupo.GetString(5),
                        //            descripcionGrupo = rdGrupo.GetString(6),
                        //            pideFoto = rdGrupo.GetString(7),
                        //            noPideFoto = rdGrupo.GetString(8),
                        //            pideLectura = rdGrupo.GetString(9),
                        //            id_Servicio = rdGrupo.GetInt32(10)
                        //        });
                        //    }
                        //    migracion.detalleGrupos = detalleGrupo;
                        //}
                        // Reparto

                        SqlCommand cmdReparto = con.CreateCommand();
                        cmdReparto.CommandTimeout = 0;
                        cmdReparto.CommandType = CommandType.StoredProcedure;
                        cmdReparto.CommandText = "USP_LIST_REPARTO";
                        cmdReparto.Parameters.AddWithValue("@id_operario_reparto", operarioId);
                        SqlDataReader drReparto = cmdReparto.ExecuteReader();
                        if (drReparto.HasRows)
                        {
                            List<Reparto> reparto = new List<Reparto>();
                            while (drReparto.Read())
                            {
                                reparto.Add(new Reparto()
                                {
                                    id_Reparto = drReparto.GetInt32(0),
                                    id_Operario_Reparto = drReparto.GetInt32(1),
                                    foto_Reparto = drReparto.GetInt32(2),
                                    estado = drReparto.GetInt32(3),
                                    activo = drReparto.GetInt32(4),
                                    Suministro_Medidor_reparto = drReparto.GetString(5),
                                    Suministro_Numero_reparto = drReparto.GetString(6),
                                    Cod_Actividad_Reparto = drReparto.GetString(7),
                                    Cod_Orden_Reparto = drReparto.GetString(8),
                                    Direccion_Reparto = drReparto.GetString(9),
                                    Cliente_Reparto = drReparto.GetString(10),
                                    CodigoBarra = drReparto.GetString(11),
                                    latitud = drReparto.GetString(12),
                                    longitud = drReparto.GetString(13)
                                });
                            }
                            migracion.repartoLectura = reparto;
                        }

                        // MOTIVO

                        SqlCommand cmdM = con.CreateCommand();
                        cmdM.CommandTimeout = 0;
                        cmdM.CommandType = CommandType.StoredProcedure;
                        cmdM.CommandText = "USP_LIST_MOTIVO";
                        SqlDataReader drM = cmdM.ExecuteReader();
                        if (drM.HasRows)
                        {
                            List<Motivo> m = new List<Motivo>();
                            while (drM.Read())
                            {
                                m.Add(new Motivo()
                                {
                                    motivoId = drM.GetInt32(0),
                                    grupo = drM.GetString(1),
                                    codigo = drM.GetInt32(2),
                                    descripcion = drM.GetString(3)

                                });
                            }
                            migracion.motivos = m;
                        }

                        // ESTADOS CHECK LIST
                        //SqlCommand cmdE = con.CreateCommand();
                        //cmdE.CommandTimeout = 0;
                        //cmdE.CommandType = CommandType.StoredProcedure;
                        //cmdE.CommandText = "Movil_Inspeccion_Estados";
                        //SqlDataReader drE = cmdE.ExecuteReader();
                        //if (drE.HasRows)
                        //{
                        //    List<Estado> e = new List<Estado>();
                        //    while (drE.Read())
                        //    {
                        //        e.Add(new Estado()
                        //        {
                        //            estadoId = drE.GetInt32(0),
                        //            nombre = drE.GetString(1),
                        //            abreviatura = drE.GetString(2),
                        //            grupo = drE.GetInt32(3),
                        //            estado = drE.GetInt32(4)
                        //        });
                        //    }
                        //    migracion.estados = e;
                        //}

                        // CHECK LIST
                        //SqlCommand cmdC = con.CreateCommand();
                        //cmdC.CommandTimeout = 0;
                        //cmdC.CommandType = CommandType.StoredProcedure;
                        //cmdC.CommandText = "Movil_CheckList";
                        //SqlDataReader drC = cmdC.ExecuteReader();
                        //if (drC.HasRows)
                        //{
                        //    List<CheckList> c = new List<CheckList>();
                        //    while (drC.Read())
                        //    {
                        //        c.Add(new CheckList()
                        //        {
                        //            checkListId = drC.GetInt32(0),
                        //            formatoId = drC.GetInt32(1),
                        //            titulo = drC.GetInt32(2),
                        //            codigo = drC.GetInt32(3),
                        //            descripcion = drC.GetString(4),
                        //            orden = drC.GetInt32(5),
                        //            aplicaFecha = drC.GetInt32(6),
                        //            aplicaObs1 = drC.GetInt32(7),
                        //            aplicaObs2 = drC.GetInt32(8),
                        //            grupo = drC.GetInt32(9),
                        //            campo1 = drC.GetString(10),
                        //            campo2 = drC.GetString(11),
                        //            estado = drC.GetInt32(12),
                        //            valor1 = drC.GetString(13),
                        //            valor2 = drC.GetString(14),
                        //            valor3 = drC.GetString(15),
                        //            valor4 = drC.GetString(16)
                        //        });
                        //    }
                        //    migracion.checkList = c;
                        //}

                        // FORMATO

                        SqlCommand cmdF = con.CreateCommand();
                        cmdF.CommandTimeout = 0;
                        cmdF.CommandType = CommandType.StoredProcedure;
                        cmdF.CommandText = "Movil_List_Formato_Cargo";
                        SqlDataReader drF = cmdF.ExecuteReader();
                        if (drF.HasRows)
                        {
                            List<FormatoCargo> f = new List<FormatoCargo>();
                            while (drF.Read())
                            {
                                f.Add(new FormatoCargo()
                                {
                                    formatoId = drF.GetInt32(0),
                                    tipo = drF.GetInt32(1),
                                    nombre = drF.GetString(2),
                                    abreviatura = drF.GetString(3),
                                    estado = drF.GetInt32(4)
                                });
                            }
                            migracion.formatos = f;
                        }

                        // CLIENTES

                        //SqlCommand cmdCl = con.CreateCommand();
                        //cmdCl.CommandTimeout = 0;
                        //cmdCl.CommandType = CommandType.StoredProcedure;
                        //cmdCl.CommandText = "USP_LIST_GRANDES_CLIENTE";
                        //cmdCl.Parameters.Add("@operarioId", SqlDbType.Int).Value = operarioId;
                        //SqlDataReader drCl = cmdCl.ExecuteReader();
                        //if (drCl.HasRows)
                        //{
                        //    List<GrandesClientes> c = new List<GrandesClientes>();
                        //    while (drCl.Read())
                        //    {
                        //        c.Add(new GrandesClientes()
                        //        {
                        //            clienteId = drCl.GetInt32(0),
                        //            fechaImportacion = drCl.GetDateTime(1).ToString("dd/MM/yyyy HH:mm:ss"),
                        //            archivoImportacion = drCl.GetString(2),
                        //            codigoEMR = drCl.GetString(3),
                        //            nombreCliente = drCl.GetString(4),
                        //            direccion = drCl.GetString(5),
                        //            distrito = drCl.GetString(6),
                        //            fechaAsignacion = drCl.GetDateTime(7).ToString("dd/MM/yyyy"),
                        //            fechaEnvioCelular = drCl.GetDateTime(8).ToString("dd/MM/yyyy"),
                        //            operarioId = drCl.GetInt32(9),
                        //            ordenLectura = drCl.GetInt32(10),
                        //            fechaRegistroInicio = drCl.GetDateTime(11).ToString("dd/MM/yyyy"),
                        //            clientePermiteAcceso = drCl.GetString(12),
                        //            fotoConstanciaPermiteAcceso = drCl.GetString(13),
                        //            porMezclaExplosiva = drCl.GetString(14),
                        //            vManoPresionEntrada = drCl.GetString(15),
                        //            fotovManoPresionEntrada = drCl.GetString(16),
                        //            marcaCorrectorId = drCl.GetInt32(17),
                        //            fotoMarcaCorrector = drCl.GetString(18),
                        //            vVolumenSCorreUC = drCl.GetString(19),
                        //            fotovVolumenSCorreUC = drCl.GetString(20),
                        //            vVolumenSCorreMedidor = drCl.GetString(21),
                        //            fotovVolumenSCorreMedidor = drCl.GetString(22),
                        //            vVolumenRegUC = drCl.GetString(23),
                        //            fotovVolumenRegUC = drCl.GetString(24),
                        //            vPresionMedicionUC = drCl.GetString(25),
                        //            fotovPresionMedicionUC = drCl.GetString(26),
                        //            vTemperaturaMedicionUC = drCl.GetString(27),
                        //            fotovTemperaturaMedicionUC = drCl.GetString(28),
                        //            tiempoVidaBateria = drCl.GetString(29),
                        //            fotoTiempoVidaBateria = drCl.GetString(30),
                        //            fotoPanomarica = drCl.GetString(31),
                        //            tieneGabinete = drCl.GetString(32),
                        //            foroSitieneGabinete = drCl.GetString(33),
                        //            presenteCliente = drCl.GetString(34),
                        //            contactoCliente = drCl.GetString(35),
                        //            latitud = drCl.GetString(36),
                        //            longitud = drCl.GetString(37),
                        //            estado = drCl.GetInt32(38),
                        //            fotoInicioTrabajo = drCl.GetString(39),
                        //            comentario = drCl.GetString(40),
                        //            observacionId = drCl.GetInt32(41),
                        //            nombreObservaciones = drCl.GetString(42)
                        //        });
                        //    }
                        //
                        //    migracion.clientes = c;
                        //}

                        //SqlCommand cmdMa = con.CreateCommand();
                        //cmdMa.CommandTimeout = 0;
                        //cmdMa.CommandType = CommandType.StoredProcedure;
                        //cmdMa.CommandText = "USP_LIST_MARCA_MEDIDOR";
                        //SqlDataReader drMa = cmdMa.ExecuteReader();
                        //if (drMa.HasRows)
                        //{
                        //    List<Marca> m = new List<Marca>();
                        //    while (drMa.Read())
                        //    {
                        //        m.Add(new Marca()
                        //        {
                        //            marcaMedidorId = drMa.GetInt32(0),
                        //            nombre = drMa.GetString(1)
                        //        });
                        //    }
                        //
                        //    migracion.marcas = m;
                        //}


                        //SqlCommand cmdArea = con.CreateCommand();
                        //cmdArea.CommandTimeout = 0;
                        //cmdArea.CommandType = CommandType.StoredProcedure;
                        //cmdArea.CommandText = "USP_LIST_AREA";
                        //SqlDataReader drArea = cmdArea.ExecuteReader();
                        //if (drArea.HasRows)
                        //{
                        //    List<Area> m = new List<Area>();
                        //    while (drArea.Read())
                        //    {
                        //        m.Add(new Area()
                        //        {
                        //            areaId = drArea.GetInt32(0),
                        //            nombre = drArea.GetString(1),
                        //            estado = drArea.GetInt32(2)
                        //        });
                        //    }
                        //
                        //    migracion.areas = m;
                        //}

                        //SqlCommand cmdt = con.CreateCommand();
                        //cmdt.CommandTimeout = 0;
                        //cmdt.CommandType = CommandType.StoredProcedure;
                        //cmdt.CommandText = "USP_LIST_TIPO_TRANSLADO";
                        //SqlDataReader drt = cmdt.ExecuteReader();
                        //if (drt.HasRows)
                        //{
                        //    List<TipoTraslado> m = new List<TipoTraslado>();
                        //    while (drt.Read())
                        //    {
                        //        m.Add(new TipoTraslado()
                        //        {
                        //            trasladoId = drt.GetInt32(0),
                        //            nombre = drt.GetString(1),
                        //            estado = drt.GetInt32(2)
                        //        });
                        //    }
                        //
                        //    migracion.tipoTraslados = m;
                        //}

                        //SqlCommand cmdV = con.CreateCommand();
                        //cmdV.CommandTimeout = 0;
                        //cmdV.CommandType = CommandType.StoredProcedure;
                        //cmdV.CommandText = "USP_LIST_VEHICULOS";
                        //SqlDataReader drV = cmdV.ExecuteReader();
                        //if (drV.HasRows)
                        //{
                        //    List<Vehiculo> m = new List<Vehiculo>();
                        //    while (drV.Read())
                        //    {
                        //        m.Add(new Vehiculo()
                        //        {
                        //            vehiculoId = drV.GetInt32(0),
                        //            placa = drV.GetString(1),
                        //            marca = drV.GetString(2),
                        //            modelo = drV.GetString(3),
                        //            anioFabricacion = drV.GetString(4),
                        //            cilindrada = drV.GetString(5),
                        //            inspeccionTecnica = drV.GetDateTime(6).ToString("dd/MM/yyyy"),
                        //            soat = drV.GetDateTime(7).ToString("dd/MM/yyyy"),
                        //            estado = drV.GetInt32(8)
                        //        });
                        //    }
                        //
                        //    migracion.vehiculos = m;
                        //}


                        //Observaciones Cliente
                        //SqlCommand cmdO = con.CreateCommand();
                        //cmdO.CommandTimeout = 0;
                        //cmdO.CommandType = CommandType.StoredProcedure;
                        //cmdO.CommandText = "Movil_Observacion_Cliente";
                        //SqlDataReader drO = cmdO.ExecuteReader();
                        //if (drO.HasRows)
                        //{
                        //    List<Observaciones> o = new List<Observaciones>();
                        //    while (drO.Read())
                        //    {
                        //        o.Add(new Observaciones()
                        //        {
                        //            observacionId = drO.GetInt32(0),
                        //            abreviatura = drO.GetString(1),
                        //            descripcion = drO.GetString(2),
                        //            estado = drO.GetInt32(3)
                        //        });
                        //    }
                        //
                        //    migracion.observaciones = o;
                        //}

                        migracion.mensaje = "Sincronización Completada.";
                    }
                    con.Close();
                }
                return migracion;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // Sincronizar
        public static Sincronizar sincronizar(int operarioId)
        {
            try
            {
                Sincronizar sincronizar = new Sincronizar();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();

                    sincronizar.sincronizarId = 1;

                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "USP_LIST_SUMINISTRO_CORTES";
                    cmd.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = "3";
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        List<Suministro> suministroCorte = new List<Suministro>();
                        int i = 1;
                        while (dr.Read())
                        {
                            suministroCorte.Add(new Suministro()
                            {
                                iD_Suministro = Convert.ToInt32(dr["ID_Suministro"]),
                                suministro_Numero = dr["Suministro_Numero"].ToString(),
                                suministro_Medidor = dr["Suministro_Medidor"].ToString(),
                                suministro_Cliente = dr["Suministro_Cliente"].ToString(),
                                suministro_Direccion = dr["Suministro_Direccion"].ToString(),
                                suministro_UnidadLectura = dr["Suministro_UnidadLectura"].ToString(),
                                suministro_TipoProceso = dr["Suministro_TipoProceso"].ToString(),
                                suministro_LecturaMinima = dr["Suministro_LecturaMinima"].ToString(),
                                suministro_LecturaMaxima = dr["Suministro_LecturaMaxima"].ToString(),
                                suministro_Fecha_Reg_Movil = dr["Suministro_Fecha_Reg_Movil"].ToString(),
                                suministro_UltimoMes = dr["Suministro_UltimoMes"].ToString(),
                                suministro_NoCortar = Convert.ToInt32(dr["Suministro_NoCortar"]),
                                estado = Convert.ToInt32(dr["Estado"]),
                                suministroOperario_Orden = Convert.ToInt32(dr["SuministroOperario_Orden"]),
                                latitud = dr["latitud"].ToString(),
                                longitud = dr["longitud"].ToString(),
                                fechaAsignacion = "",
                                orden = i++,
                                activo = 1
                            });
                        }
                        sincronizar.suministrosCortes = suministroCorte;
                    }

                    SqlCommand cmdR = cn.CreateCommand();
                    cmdR.CommandType = CommandType.StoredProcedure;
                    cmdR.CommandTimeout = 0;
                    cmdR.CommandText = "USP_LIST_SUMINISTRO_CORTES";
                    cmdR.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                    cmdR.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = "4";
                    SqlDataReader drR = cmdR.ExecuteReader();

                    if (dr.HasRows)
                    {
                        List<Suministro> suministroReconexiones = new List<Suministro>();
                        int y = 1;
                        while (dr.Read())
                        {
                            suministroReconexiones.Add(new Suministro()
                            {
                                iD_Suministro = Convert.ToInt32(dr["ID_Suministro"]),
                                suministro_Numero = dr["Suministro_Numero"].ToString(),
                                suministro_Medidor = dr["Suministro_Medidor"].ToString(),
                                suministro_Cliente = dr["Suministro_Cliente"].ToString(),
                                suministro_Direccion = dr["Suministro_Direccion"].ToString(),
                                suministro_UnidadLectura = dr["Suministro_UnidadLectura"].ToString(),
                                suministro_TipoProceso = dr["Suministro_TipoProceso"].ToString(),
                                suministro_LecturaMinima = dr["Suministro_LecturaMinima"].ToString(),
                                suministro_LecturaMaxima = dr["Suministro_LecturaMaxima"].ToString(),
                                suministro_Fecha_Reg_Movil = dr["Suministro_Fecha_Reg_Movil"].ToString(),
                                suministro_UltimoMes = dr["Suministro_UltimoMes"].ToString(),
                                suministro_NoCortar = Convert.ToInt32(dr["Suministro_NoCortar"]),
                                estado = Convert.ToInt32(dr["Estado"]),
                                suministroOperario_Orden = Convert.ToInt32(dr["SuministroOperario_Orden"]),
                                latitud = dr["latitud"].ToString(),
                                longitud = dr["longitud"].ToString(),
                                fechaAsignacion = "",
                                orden = y++,
                                activo = 1
                            });
                        }
                        sincronizar.suministroReconexion = suministroReconexiones;
                    }

                    cn.Close();

                }
                return sincronizar;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Mensaje saveRegistroRxNew(Registro r)
        {
            try
            {
                int lastId;
                Mensaje m = null;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USP_SAVE_REGISTRO_REPARTO", con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@id_operario_reparto", SqlDbType.Int).Value = r.iD_Operario;
                        cmd.Parameters.Add("@id_reparto", SqlDbType.Int).Value = r.iD_Suministro;
                        cmd.Parameters.Add("@registro_fecha_sqlite", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;
                        cmd.Parameters.Add("@registro_latitud", SqlDbType.VarChar).Value = r.registro_Latitud;
                        cmd.Parameters.Add("@registro_longitud", SqlDbType.VarChar).Value = r.registro_Longitud;
                        cmd.Parameters.Add("@id_Observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                lastId = dr.GetInt32(0);

                                if (lastId != 0)
                                {
                                    m = new Mensaje();
                                    m.codigoBase = r.iD_Registro;
                                    m.codigoRetorno = dr.GetInt32(0);
                                    foreach (var i in r.photos)
                                    {
                                        SqlCommand cmdP = con.CreateCommand();
                                        cmdP.CommandType = CommandType.StoredProcedure;
                                        cmdP.CommandText = "USP_SAVE_REGISTRO_REPARTO_FOTO";
                                        cmdP.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = m.codigoRetorno;
                                        cmdP.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = i.rutaFoto;
                                        cmdP.Parameters.Add("@latitud", SqlDbType.VarChar).Value = i.latitud;
                                        cmdP.Parameters.Add("@longitud", SqlDbType.VarChar).Value = i.longitud;
                                        cmdP.ExecuteNonQuery();
                                    }
                                    var rr = r.recibo;
                                    if (rr != null)
                                    {
                                        SqlCommand cmdR = con.CreateCommand();
                                        cmdR.CommandType = CommandType.StoredProcedure;
                                        cmdR.CommandText = "Movil_SaveCargoRecibo";
                                        cmdR.Parameters.Add("@id_registro", SqlDbType.Int).Value = m.codigoRetorno;
                                        cmdR.Parameters.Add("@id_reparto", SqlDbType.Int).Value = r.iD_Suministro;
                                        cmdR.Parameters.Add("@id_operario_reparto", SqlDbType.Int).Value = rr.operarioId;
                                        cmdR.Parameters.Add("@tiporecibo", SqlDbType.Int).Value = rr.tipo;
                                        cmdR.Parameters.Add("@ciclo_cargorecibo", SqlDbType.VarChar).Value = rr.ciclo;
                                        cmdR.Parameters.Add("@anio_cargorecibo", SqlDbType.Int).Value = rr.year;
                                        cmdR.Parameters.Add("@piso", SqlDbType.Int).Value = rr.piso;
                                        cmdR.Parameters.Add("@id_formatocargo_vivienda", SqlDbType.Int).Value = rr.formatoVivienda;
                                        cmdR.Parameters.Add("@otrosvivienda_cargorecibo", SqlDbType.VarChar).Value = rr.otrosVivienda;
                                        cmdR.Parameters.Add("@id_formatocargo_color", SqlDbType.Int).Value = rr.formatoCargoColor;
                                        cmdR.Parameters.Add("@otroscolor_cargorecibo", SqlDbType.VarChar).Value = rr.otrosCargoColor;
                                        cmdR.Parameters.Add("@id_formatocargo_puerta", SqlDbType.Int).Value = rr.formatoCargoPuerta;
                                        cmdR.Parameters.Add("@otrospuerta_cargorecibo", SqlDbType.VarChar).Value = rr.otrosCargoPuerta;
                                        cmdR.Parameters.Add("@id_formatocargo_colorpuerta", SqlDbType.Int).Value = rr.formatoCargoColorPuerta;
                                        cmdR.Parameters.Add("@otroscolorpuerta_cargorecibo", SqlDbType.VarChar).Value = rr.otrosCargoColorPuerta;
                                        cmdR.Parameters.Add("@id_formatocargo_recibido", SqlDbType.Int).Value = rr.formatoCargoRecibo;
                                        cmdR.Parameters.Add("@dni_cargorecibo", SqlDbType.VarChar).Value = rr.dniCargoRecibo;
                                        cmdR.Parameters.Add("@parentesco_cargorecibo", SqlDbType.VarChar).Value = rr.parentesco;
                                        cmdR.Parameters.Add("@id_formatocargo_devuelto", SqlDbType.Int).Value = rr.formatoCargoDevuelto;
                                        cmdR.Parameters.Add("@fechamax_cargorecibo", SqlDbType.VarChar).Value = rr.fechaMax;
                                        cmdR.Parameters.Add("@fechaentrega_cargorecibo", SqlDbType.VarChar).Value = rr.fechaEntrega;
                                        cmdR.Parameters.Add("@obs_cargorecibo", SqlDbType.VarChar).Value = rr.observacionCargo;
                                        cmdR.Parameters.Add("@firmacliente_cargorecibo", SqlDbType.VarChar).Value = rr.firmaCliente;
                                        cmdR.ExecuteNonQuery();
                                    }
                                    m.mensaje = "Datos Enviados";
                                }

                            }
                        }
                    }
                    con.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Login GetOne(string user, string password, string version, string imei, string token)
        {
            try
            {
                Login l = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("USP_ACCESO_LOGIN", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@User", SqlDbType.VarChar).Value = user;
                        cmd.Parameters.Add("@version", SqlDbType.VarChar).Value = version;
                        cmd.Parameters.Add("@imei", SqlDbType.VarChar).Value = imei;
                        cmd.Parameters.Add("@token", SqlDbType.VarChar).Value = token;
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            l = new Login();
                            if (password == dr.GetString(2))
                            {
                                l.iD_Operario = dr.GetInt32(0);
                                l.operario_Login = dr.GetString(1);
                                l.operario_Contrasenia = dr.GetString(2);
                                l.operario_Nombre = dr.GetString(3);
                                l.operario_EnvioEn_Linea = dr.GetInt32(4);
                                l.tipoUsuario = dr.GetString(5);
                                l.estado = dr.GetString(6);
                                l.lecturaManual = dr.GetInt32(7);
                                l.vehiculoId = dr.GetInt32(8);
                                l.placa = dr.GetString(9);
                                l.marca = dr.GetString(10);
                                l.modelo = dr.GetString(11);
                                l.anioFabricacion = dr.GetString(12);
                                l.cilindrada = dr.GetString(13);
                                l.inspeccionTecnica = dr.GetDateTime(14).ToString("dd/MM/yyyy");
                                l.soat = dr.GetDateTime(15).ToString("dd/MM/yyyy");
                                l.licenciaConducir = dr.GetDateTime(16).ToString("dd/MM/yyyy");
                                l.constanciaVigente = dr.GetDateTime(17).ToString("dd/MM/yyyy");

                                l.mensaje = "Go";
                            }
                            else
                            {
                                l.mensaje = "Pass";
                            }
                        }
                    }
                    cn.Close();
                }
                return l;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        // nuevo irvin

        public static Mensaje saveEstadoMovil(EstadoMovil e)
        {
            try
            {
                Mensaje m = new Mensaje();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "USP_SAVE_ESTADOCELULAR";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@gpsActivo", SqlDbType.Bit).Value = e.gpsActivo;
                    cmd.Parameters.Add("@estadoBateria", SqlDbType.Int).Value = e.estadoBateria;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@modoAvion", SqlDbType.Int).Value = e.modoAvion;
                    cmd.Parameters.Add("@planDatos", SqlDbType.Bit).Value = e.planDatos;

                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje();
                        m.codigo = 1;
                        m.mensaje = "Enviado";
                    }

                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje saveOperarioGps(EstadoOperario e)
        {
            try
            {
                Mensaje m = new Mensaje();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_SAVE_GPS";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                    cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;

                    int a = cmd.ExecuteNonQuery();

                    if (a == 1)
                    {
                        m = new Mensaje();
                        m.codigoBase =1;
                        m.mensaje = "Enviado";
                    }

                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveGpsOperario(EstadoOperario e)
        {
            try
            {
                Mensaje m = new Mensaje();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_SAVE_GPS";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                    cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;

                    int a = cmd.ExecuteNonQuery();

                    if (a == 1)
                    {
                        m = new Mensaje();
                        m.codigoBase = e.id;
                        m.mensaje = "Enviado";
                    }

                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<Email> GetEmail(int areaId)
        {
            try
            {
                List<Email> e = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("Movil_Emails", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@servicioId", SqlDbType.Int).Value = areaId;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            e = new List<Email>();
                            while (dr.Read())
                            {
                                e.Add(new Email()
                                {
                                    emailId = dr.GetInt32(0),
                                    servicioId = dr.GetInt32(1),
                                    email = dr.GetString(2),
                                    estado = dr.GetInt32(3)
                                });
                            }
                        }
                    }
                    cn.Close();
                }
                return e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static void SaveRegistroPhoto(string fileName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USP_SAVE_REGISTRO_LECTURA", con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = fileName;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveReparto(RepartoCargo r)
        {
            try
            {
                int lastId;
                Mensaje m = null;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("Movil_SaveRepartoCargo", con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@cargoId", SqlDbType.Int).Value = r.cargoId;
                        cmd.Parameters.Add("@id_tipocargo", SqlDbType.Int).Value = r.tipoCargoId;
                        cmd.Parameters.Add("@id_reparto", SqlDbType.Int).Value = r.repartoId;
                        cmd.Parameters.Add("@suministro_numero_cargo", SqlDbType.VarChar).Value = r.suministroNumero;
                        cmd.Parameters.Add("@nombreapellidos_cargo", SqlDbType.VarChar).Value = r.nombreApellido;

                        cmd.Parameters.Add("@dni_cargo", SqlDbType.VarChar).Value = r.dni;
                        cmd.Parameters.Add("@predio_cargo", SqlDbType.VarChar).Value = r.predio;
                        cmd.Parameters.Add("@quienrecibe_cargo", SqlDbType.Int).Value = r.quienRecibeCargo;
                        cmd.Parameters.Add("@nombreempresa", SqlDbType.VarChar).Value = r.nombreEmpresa;

                        cmd.Parameters.Add("@lectura_cargo", SqlDbType.VarChar).Value = r.lecturaCargo;
                        cmd.Parameters.Add("@latitud_cargo", SqlDbType.VarChar).Value = r.latitud;
                        cmd.Parameters.Add("@longitud_cargo", SqlDbType.VarChar).Value = r.longitud;
                        cmd.Parameters.Add("@fecha_cargo", SqlDbType.VarChar).Value = r.fecha;
                        cmd.Parameters.Add("@hora_cargo", SqlDbType.VarChar).Value = r.hora;
                        cmd.Parameters.Add("@registro_fecha_sqlite", SqlDbType.VarChar).Value = r.fechaMovil;

                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                lastId = dr.GetInt32(0);
                                if (lastId != 0)
                                {
                                    m = new Mensaje();

                                    foreach (var p in r.photos)
                                    {
                                        SqlCommand cmds = con.CreateCommand();
                                        cmds.CommandType = CommandType.StoredProcedure;
                                        cmds.CommandText = "Movil_SaveRepartoCargoFoto";
                                        cmds.Parameters.Add("@id_cargo", SqlDbType.Int).Value = lastId;
                                        cmds.Parameters.Add("@rutafoto", SqlDbType.VarChar).Value = p.rutaFoto;
                                        cmds.ExecuteNonQuery();
                                    }

                                    foreach (var p in r.suministros)
                                    {
                                        SqlCommand cmds = con.CreateCommand();
                                        cmds.CommandType = CommandType.StoredProcedure;
                                        cmds.CommandText = "Movil_SaveRepartoCargoSuministro";
                                        cmds.Parameters.Add("@id_cargo", SqlDbType.Int).Value = lastId;
                                        cmds.Parameters.Add("@id_reparto", SqlDbType.Int).Value = p.repartoId;
                                        cmds.Parameters.Add("@suministro_numero_cargo", SqlDbType.VarChar).Value = p.suministroNumero;
                                        cmds.ExecuteNonQuery();
                                    }
                                    m.codigo = r.cargoId;
                                    m.mensaje = "Datos Enviados";
                                }
                            }
                        }
                    }
                    con.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}