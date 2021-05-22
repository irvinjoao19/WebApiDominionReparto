using Entidades;
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
                Migracion migracion = null;           
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
                        return migracion;
                    }
                    else
                    {
                        // Servicios

                        migracion = new Migracion();

                        //SqlCommand cmdServicio = con.CreateCommand();
                        //cmdServicio.CommandTimeout = 0;
                        //cmdServicio.CommandType = CommandType.StoredProcedure;
                        //cmdServicio.CommandText = "USP_SERVICIOS";
                        //SqlDataReader drServicio = cmdServicio.ExecuteReader();
                        //if (drServicio.HasRows)
                        //{
                        //    List<Servicio> servicio = new List<Servicio>();
                        //    while (drServicio.Read())
                        //    {
                        //        servicio.Add(new Servicio()
                        //        {
                        //            id_servicio = drServicio.GetInt32(0),
                        //            nombre_servicio = drServicio.GetString(1),
                        //            estado = drServicio.GetInt32(2)
                        //        });
                        //    }
                        //    migracion.servicios = servicio;
                        //}

                        // Parametro

                        //SqlCommand cmdParametro = con.CreateCommand();
                        //cmdParametro.CommandTimeout = 0;
                        //cmdParametro.CommandType = CommandType.StoredProcedure;
                        //cmdParametro.CommandText = "USP_PARAMETROS";
                        //SqlDataReader drParametro = cmdParametro.ExecuteReader();
                        //if (drParametro.HasRows)
                        //{
                        //    List<Parametro> parametro = new List<Parametro>();
                        //    while (drParametro.Read())
                        //    {
                        //        parametro.Add(new Parametro()
                        //        {
                        //            id_Configuracion = drParametro.GetInt32(0),
                        //            nombre_parametro = drParametro.GetString(1),
                        //            valor = drParametro.GetInt32(2)
                        //        });
                        //    }
                        //    migracion.parametros = parametro;
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
         

        public static Mensaje SaveRegistroRxNew(Registro r)
        {
            try
            {
                int lastId;
                Mensaje m = null;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    var cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_SAVE_REGISTRO_REPARTO";

                    cmd.Parameters.AddWithValue("@ID_Registro", 1);
                    cmd.Parameters.AddWithValue("@id_operario_reparto", r.iD_Operario);
                    cmd.Parameters.AddWithValue("@id_reparto", r.iD_Suministro);
                    cmd.Parameters.AddWithValue("@registro_fecha_sqlite", r.registro_Fecha_SQLITE);
                    cmd.Parameters.AddWithValue("@registro_latitud", r.registro_Latitud);
                    cmd.Parameters.AddWithValue("@registro_longitud", r.registro_Longitud);
                    cmd.Parameters.AddWithValue("@id_Observacion", r.registro_Observacion);

                    var rd = cmd.ExecuteReader();

                    while (rd.Read())
                    {
                        lastId = rd.GetInt32(0);
                        if(lastId != 0)
                        {
                            #region Registro de fotos

                            m = new Mensaje
                            {
                                codigoBase = r.iD_Registro,
                                codigoRetorno = lastId,
                                mensaje = "Datos Enviados"
                            };


                            foreach (var i in r.photos)
                            {
                                var cmdP = con.CreateCommand();
                                cmdP.CommandType = CommandType.StoredProcedure;
                                cmdP.CommandText = "USP_SAVE_REGISTRO_REPARTO_FOTO";
                                cmdP.Parameters.AddWithValue("@ID_Registro", m.codigoRetorno);
                                cmdP.Parameters.AddWithValue("@RutaFoto", i.rutaFoto);
                                cmdP.Parameters.AddWithValue("@latitud", i.latitud);
                                cmdP.Parameters.AddWithValue("@longitud", i.longitud);

                                cmdP.ExecuteNonQuery();
                            }

                            #endregion

                            #region Registro de Recibos

                            var rr = r.recibo;
                            if (rr != null)
                            {
                                var cmdR = con.CreateCommand();
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

                            #endregion
                        }
                    }
                    
                    rd.Close();

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
                        m = new Mensaje
                        {
                            codigo = e.id,
                            mensaje = "Enviado"
                        };
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
                        m = new Mensaje
                        {
                            codigoBase = e.id,
                            mensaje = "Enviado"
                        };
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
                        m = new Mensaje
                        {
                            codigoBase = e.id,
                            mensaje = "Enviado"
                        };
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