using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Classico
{
    /// <summary>
    /// Classe principale di avvio dell'applicazione
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale per l'applicazione.
        /// Configura l'applicazione Windows Forms e avvia la finestra principale
        /// </summary>
        [STAThread] // Attributo necessario per il threading model di Windows Forms
        static void Main()
        {
            Application.EnableVisualStyles();             // Abilita stili visuali moderni
            Application.SetCompatibleTextRenderingDefault(false); // Configura il rendering del testo
            Application.Run(new Form1());                  // Crea e avvia la finestra principale
        }
    }
}