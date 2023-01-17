using PSOENotificaciones.Contexto;
using PSOENotificaciones.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PSOENotificaciones.Servicios
{
    public class PsoePeticionesLocalizaServicio : IPsoePeticionesLocaliza
    {
        public PsoePeticionesLocalizaServicio()
        {

        }

        public string EnviarPeticion(DateTime? fechadesde, DateTime? fechahasta, int idTipoEnvioPeticion, int idPeticionLocaliza)
        {
            string respuestaPeticion = "";

            if (idTipoEnvioPeticion == (int)TipoPeticionesLocaliza.Comunicaciones)
            {
                try
                {
                    EjecuteClientComunicaciones cliente = new EjecuteClientComunicaciones();
                    cliente.Llama(fechadesde, fechahasta, idPeticionLocaliza);
                }
                catch { }

                respuestaPeticion = EjecuteClientComunicaciones.respuesta;
            }

            if (idTipoEnvioPeticion == (int)TipoPeticionesLocaliza.Notificaciones)
            {
                try
                {
                    EjecuteClientNotificaciones cliente = new EjecuteClientNotificaciones();
                    cliente.Llama(fechadesde, fechahasta, idPeticionLocaliza);
                }
                catch { }

                respuestaPeticion = EjecuteClientNotificaciones.respuesta;
            }

            return respuestaPeticion;
        }

        public enum TipoPeticionesLocaliza
        {
            Comunicaciones = 1,
            Notificaciones = 2,
        }
    }

    public static class FirstXTransform
    {
        public static List<object> XElementTransforms(XElement xElement)
        {
            List<object> ts = new List<object>();

            List<XElement> xelement = new List<XElement>();
            List<XName> xNames = new List<XName>();
            List<XAttribute> listAttribute = new List<XAttribute>();
            List<string> InnerXmlNode = new List<string>();

            if (xElement != null)
            {
                foreach (var item in xElement.Elements())
                {
                    xelement.Add(item);
                    xNames.Add(item.Name);
                }

                foreach (var item in xelement.Attributes())
                {
                    listAttribute.Add(item);
                }

                foreach (var item in xElement.Nodes())
                {
                    var objet = item.CreateReader();
                    while (objet.Read())
                    {
                        InnerXmlNode.Add(objet.ReadInnerXml());
                    }
                }

                ts.Add(xNames);
                ts.Add(listAttribute);
                ts.Add(InnerXmlNode);
                ts.Add(xelement);
            }

            return ts;
        }

        public static object[] ReturnTypeListNode(List<object> ts)
        {
            object[] _type = new object[ts.Count];
            int j = 0;

            foreach (var item in ts)
            {
                _type[j] = item;
                j++;
            }
            return _type;
        }
    }
}