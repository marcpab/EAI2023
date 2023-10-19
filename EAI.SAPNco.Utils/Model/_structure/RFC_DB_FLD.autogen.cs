namespace EAI.SAPNco.Utils.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Rfc structure : RFC_DB_FLD
    /// </summary>
    public partial class RFC_DB_FLD
    {

        /// <summary>
        /// Documentation: Feldname
        /// Type         : CHAR ( C), length 60, decimals 0
        /// </summary>
        public string FIELDNAME { get; set; }

        /// <summary>
        /// Documentation: Offset eines Feldes in Workarea
        /// Type         : NUM ( N), length 12, decimals 0
        /// </summary>
        public decimal OFFSET { get; set; }

        /// <summary>
        /// Documentation: Länge (Anzahl der Zeichen)
        /// Type         : NUM ( N), length 12, decimals 0
        /// </summary>
        public decimal LENGTH { get; set; }

        /// <summary>
        /// Documentation: ABAP-Datentyp (C,D,N,...)
        /// Type         : CHAR ( C), length 2, decimals 0
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// Documentation: Kurzbeschreibung von Repository-Objekten
        /// Type         : CHAR ( C), length 120, decimals 0
        /// </summary>
        public string FIELDTEXT { get; set; }
    }
}

