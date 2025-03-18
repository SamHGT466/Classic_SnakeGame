using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Classico
{
    /// <summary>
    /// Classe che rappresenta un segmento del serpente o il cibo
    /// </summary>
    class Circle
    {
        // Coordinate sulla griglia di gioco
        public int X { get; set; }  // Posizione orizzontale (in unità griglia)
        public int Y { get; set; }  // Posizione verticale (in unità griglia)

        /// <summary>
        /// Costruttore base - Inizializza le coordinate a (0,0)
        /// I valori verranno sovrascritti durante il posizionamento nel gioco
        /// </summary>
        public Circle()
        {
            X = 0;  // Valore di default che verrà modificato
            Y = 0;  // Valore di default che verrà modificato
        }
    }
}