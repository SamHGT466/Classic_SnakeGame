using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Classico
{
    /// <summary>
    /// Classe di configurazione globale del gioco
    /// </summary>
    class Settings
    {
        // Dimensioni di ogni segmento/cella nella griglia di gioco
        public static int Width { get; set; }  // Larghezza in pixel di un segmento
        public static int Height { get; set; } // Altezza in pixel di un segmento

        // Direzione corrente del serpente (condivisa globalmente)
        public static string directions;      // "left", "right", "up", "down"

        /// <summary>
        /// Costruttore - Imposta i valori di default per le impostazioni
        /// Viene chiamato all'avvio del form principale
        /// </summary>
        public Settings()
        {
            Width = 16;   // Dimensione base per segmenti/celle (16x16 pixel)
            Height = 16;
            directions = "left"; // Direzione iniziale del serpente
        }
    }
}