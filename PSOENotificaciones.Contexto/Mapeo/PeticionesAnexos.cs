using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("PeticionesAnexo")]
    public partial class PeticionesAnexo : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string envios_IdentificadorField;
        private int usuariosFieldId;
        private DateTime fechaFiled;
        private string nifReceptorField;
        private string codigoOrigenField;
        private string referenciaField;
        private List<Opcion4> opcionesConsultaAnexosField;
        private string nombreXml;

        [Key]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("idField");
            }
        }

        public string Envios_Identificador
        {
            get
            {
                return this.envios_IdentificadorField;
            }
            set
            {
                this.envios_IdentificadorField = value;
                this.RaisePropertyChanged("envios_IdentificadorField");
            }
        }

        public int Usuarios_ID
        {
            get
            {
                return this.usuariosFieldId;
            }
            set
            {
                this.usuariosFieldId = value;
            }
        }

        public DateTime Fecha
        {
            get
            {
                return this.fechaFiled;
            }
            set
            {
                this.fechaFiled = value;
                this.RaisePropertyChanged("fecha");
            }
        }

        [MaxLength(9)]
        public string NifReceptor
        {
            get
            {
                return this.nifReceptorField;
            }
            set
            {
                this.nifReceptorField = value;
                this.RaisePropertyChanged("nifReceptor");
            }
        }

        public string CodigoOrigen
        {
            get
            {
                return this.codigoOrigenField;
            }
            set
            {
                this.codigoOrigenField = value;
                this.RaisePropertyChanged("codigoOrigen");
            }
        }

        public string Referencia
        {
            get
            {
                return this.referenciaField;
            }
            set
            {
                this.referenciaField = value;
                this.RaisePropertyChanged("referencia");
            }
        }

        public List<Opcion4> OpcionesConsultaAnexos
        {
            get
            {
                return this.opcionesConsultaAnexosField;
            }
            set
            {
                this.opcionesConsultaAnexosField = value;
                this.RaisePropertyChanged("opcionesConsultaAnexos");
            }
        }

        public string NombreXml
        {
            get
            {
                return this.nombreXml;
            }
            set
            {
                this.nombreXml = value;
                this.RaisePropertyChanged("nombreXml");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public int InsertPeticionesAnexo(string CodigoOrigen, string envio_identificador,
           DateTime fecha, string NifReceptor, string NombreXml, List<Opcion4> OpcionesConsultaAnexos,
           string Referencia, int usuarios, GestNotifContext db)
        {
            int idPeticionAnexo = 0;

            try
            {
                PeticionesAnexo peticionesAnexo = new PeticionesAnexo
                {
                    ID = ID,
                    CodigoOrigen = CodigoOrigen,
                    Envios_Identificador = envio_identificador,
                    Fecha = fecha,
                    NifReceptor = NifReceptor,
                    NombreXml = NombreXml,
                    OpcionesConsultaAnexos = OpcionesConsultaAnexos,
                    Referencia = Referencia,
                    Usuarios_ID = usuarios
                };

                db.PeticionesAnexo.Add(peticionesAnexo);
                db.SaveChanges();
                idPeticionAnexo = peticionesAnexo.ID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return idPeticionAnexo;
        }

        public List<PeticionesAnexo> GetPeticionesAnexos(string identificador)
        {
            List<PeticionesAnexo> peticionesAnexos;

            using (var db = new GestNotifContext())
            {
                peticionesAnexos = db.PeticionesAnexo
                    .Where(i => i.Envios_Identificador == identificador).ToList();
            }

            return peticionesAnexos;
        }
    }
}
