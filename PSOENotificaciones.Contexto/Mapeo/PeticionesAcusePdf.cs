using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("PeticionesAcusePdf")]
    public partial class PeticionesAcusePdf : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int usuariosField;
        private string envios_IdentificadorField;
        private DateTime fechaFiled;
        private string nifReceptorField;
        private int codigoOrigenField;
        private List<Opcion6> opcionesPeticionesAcusePdfField;
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

        public int Usuarios_ID
        {
            get
            {
                return this.usuariosField;
            }
            set
            {
                this.usuariosField = value;
                this.RaisePropertyChanged("usuariosField");
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
                this.RaisePropertyChanged("Envios_IdentificadorField");
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

        public int CodigoOrigen
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

        public List<Opcion6> OpcionesConsultaAcusePdf
        {
            get
            {
                return this.opcionesPeticionesAcusePdfField;
            }
            set
            {
                this.opcionesPeticionesAcusePdfField = value;
                this.RaisePropertyChanged("opcionesPeticionesAcusePdfField");
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

        public int InsertPeticionesAcusePdf(int CodigoOrigen,
           string Envios_identificador,
           //Envios envios,
           DateTime fecha,
           string NifReceptor,
           string NombreXml,
           int usuarios,
           GestNotifContext db
           )
        {
            int idPeticionesAcusePdf;

            db = new GestNotifContext();
            
                PeticionesAcusePdf peticionesAcusePdf = new PeticionesAcusePdf
                {
                    //ID = ID,
                    CodigoOrigen= CodigoOrigen,
                    Envios_Identificador= Envios_identificador,
                    //Envios= envios,
                    Fecha = fecha,
                    NifReceptor= NifReceptor,
                    NombreXml = NombreXml,
                    Usuarios_ID = usuarios
                };

                db.PeticionesAcusePdf.Add(peticionesAcusePdf);

                try
                {
                    db.SaveChanges();
                }
                catch (/*System.Data.Entity.Infrastructure.DbUpdateException*/ DbEntityValidationException ex /*DbUnexpectedValidationException ex*/)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.InnerException;
                            //.SelectMany(x => x.ValidationErrors)
                            //.Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }




           idPeticionesAcusePdf = peticionesAcusePdf.ID;
            

            return idPeticionesAcusePdf;
        }

        public PeticionesAcusePdf GetPeticionesAcusePdf(string identificador)
        {
            PeticionesAcusePdf peticionesAcusePdf = null;

            using (var db = new GestNotifContext())
            {
                peticionesAcusePdf = db.PeticionesAcusePdf
                    .Where(i => i.Envios_Identificador == identificador)
                    .OrderByDescending(i => i.Fecha)
                    .FirstOrDefault();
            }

            return peticionesAcusePdf;
        }
    }

    [Table("IdentificadoresAcusePdf")]
    public partial class IdentificadoresAcusePdf : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private PeticionesAcusePdf peticionesAcusePdfField;
        private string referenciaField;
        private string csvResguardoField;

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

        public PeticionesAcusePdf PeticionesAcusePdf
        {
            get
            {
                return this.peticionesAcusePdfField;
            }
            set
            {
                this.peticionesAcusePdfField = value;
                this.RaisePropertyChanged("peticionesAcusePdfField");
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
                this.RaisePropertyChanged("referenciaField");
            }
        }

        public string CsvResguardo
        {
            get
            {
                return this.csvResguardoField;
            }
            set
            {
                this.csvResguardoField = value;
                this.RaisePropertyChanged("csvResguardoField");
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
    }
}
