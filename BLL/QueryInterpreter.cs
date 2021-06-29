using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class QueryInterpreter
    {

        public String getQueryFromRegla (Regla regla)
        {
            var condicion = regla.condicion;
            var query = "SELECT * FROM " + condicion.entidad + " WHERE " + condicion.atributo + " " 
                        + getSymbolOperator(condicion.operacion.ToString()) + condicion.valor;
            return query;
        }

        public String getSymbolOperator (String operacion)
        {
            switch (operacion)
            {
                case "MayorA":
                    return ">";                    
                case "MenorA":
                    return "<";
                case "Contiene":
                    return "=";
                case "NoContiene":
                    return "<>";
                case "EsIgual":
                    return "=";
                case "NoEsIgual":
                    return "<>";
                default:
                    return "=";                   
            }
        }       

        public String getRealVal (String valor, String tipoValor)
        {
            return "";
        }
    }
}
