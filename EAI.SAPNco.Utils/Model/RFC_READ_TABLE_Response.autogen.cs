namespace EAI.SAPNco.Utils.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Description : FUNCTION RFC_READ_TABLE
    /// </summary>
    public partial class RFC_READ_TABLE_Response
    {

        /// <summary>
        /// Documentation: Gelesene Daten (Lange Zeilen)
        /// Direction    : EXPORT
        /// Type         : TABLE ( h), length 8, decimals 0
        /// Optional     : False
        /// DefaultValue : 
        /// </summary>
        public List<SDTI_RESULT> ET_DATA { get; set; }

        /// <summary>
        /// Documentation: Gelesene Daten (512 Zeichen je Zeile)
        /// Direction    : TABLES
        /// Type         : TABLE ( h), length 8, decimals 0
        /// Optional     : True
        /// DefaultValue : 
        /// </summary>
        public List<TAB512> DATA { get; set; }

        /// <summary>
        /// Documentation: Namen (in) und Struktur (out) gelesener Felder
        /// Direction    : TABLES
        /// Type         : TABLE ( h), length 8, decimals 0
        /// Optional     : True
        /// DefaultValue : 
        /// </summary>
        public List<RFC_DB_FLD> FIELDS { get; set; }

        /// <summary>
        /// Documentation: Selektionsangaben, "WHERE-Klauseln" (in)
        /// Direction    : TABLES
        /// Type         : TABLE ( h), length 8, decimals 0
        /// Optional     : True
        /// DefaultValue : 
        /// </summary>
        public List<RFC_DB_OPT> OPTIONS { get; set; }
    }
}

