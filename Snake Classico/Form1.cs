using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging; // Per la funzionalità di screenshot

namespace Snake_Classico
{
    public partial class Form1 : Form
    {
        // Elementi di gioco
        private List<Circle> Snake = new List<Circle>(); // Lista segmenti serpente
        private Circle food = new Circle();              // Cibo nel gioco

        // Dimensioni area di gioco
        int maxWidth;
        int maxHeight;

        // Sistema di punteggio
        int score;      // Punteggio corrente
        int highScore;  // Record personale

        Random rand = new Random(); // Generatore numeri casuali per posizione cibo

        // Stati direzionali
        bool goLeft, goRight, goDown, goUp;

        public Form1()
        {
            InitializeComponent();
            new Settings(); // Inizializza impostazioni di gioco
        }

        // Gestione input tastiera (premi tasto)
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // Controllo direzioni consentite (evita inversione immediata)
            if (e.KeyCode == Keys.Left && Settings.directions != "right") goLeft = true;
            if (e.KeyCode == Keys.Right && Settings.directions != "left") goRight = true;
            if (e.KeyCode == Keys.Up && Settings.directions != "down") goUp = true;
            if (e.KeyCode == Keys.Down && Settings.directions != "up") goDown = true;
        }

        // Gestione input tastiera (rilascio tasto)
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // Reset stati direzionali
            if (e.KeyCode == Keys.Left) goLeft = false;
            if (e.KeyCode == Keys.Right) goRight = false;
            if (e.KeyCode == Keys.Up) goUp = false;
            if (e.KeyCode == Keys.Down) goDown = false;
        }

        // Avvia/Riavvia il gioco
        private void StartGame(object sender, EventArgs e) => RestartGame();

        // Cattura screenshot con punteggio
        private void TakeSnapShot(object sender, EventArgs e)
        {
            // Creazione didascalia
            Label caption = new Label();
            caption.Text = $"Punteggio: {score} - Record: {highScore}";
            caption.Font = new Font("Arial", 12, FontStyle.Bold);
            caption.ForeColor = Color.White;
            caption.Size = new Size(picCanvas.Width, 30);
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            // Dialogo salvataggio
            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = "Screenshot_Snake",
                DefaultExt = "jpg",
                Filter = "Immagine JPG|*.jpg"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Cattura e salvataggio immagine
                Bitmap bmp = new Bitmap(picCanvas.Width, picCanvas.Height);
                picCanvas.DrawToBitmap(bmp, picCanvas.ClientRectangle);
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption); // Rimuove didascalia
            }
        }

        // Logica principale del gioco (chiamata a intervalli regolari)
        private void GameTimerEvent(object sender, EventArgs e)
        {
            // Aggiorna direzione ufficiale
            if (goLeft) Settings.directions = "left";
            if (goRight) Settings.directions = "right";
            if (goDown) Settings.directions = "down";
            if (goUp) Settings.directions = "up";

            // Movimento serpente (dall'ultimo segmento alla testa)
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0) // Testa del serpente
                {
                    switch (Settings.directions)
                    {
                        case "left": Snake[i].X--; break;
                        case "right": Snake[i].X++; break;
                        case "down": Snake[i].Y++; break;
                        case "up": Snake[i].Y--; break;
                    }

                    // Gestione bordi (effetto "wrap-around")
                    if (Snake[i].X < 0) Snake[i].X = maxWidth;
                    if (Snake[i].X > maxWidth) Snake[i].X = 0;
                    if (Snake[i].Y < 0) Snake[i].Y = maxHeight;
                    if (Snake[i].Y > maxHeight) Snake[i].Y = 0;

                    // Controllo collisione con cibo
                    if (Snake[i].X == food.X && Snake[i].Y == food.Y) EatFood();

                    // Controllo autocollisione
                    for (int j = 1; j < Snake.Count; j++)
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                            GameOver();
                }
                else // Segmenti del corpo
                {
                    // Segue il segmento precedente
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

            picCanvas.Invalidate(); // Aggiorna grafica
        }

        // Disegna gli elementi grafici
        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            // Disegna il serpente
            for (int i = 0; i < Snake.Count; i++)
            {
                Brush colore = (i == 0) ? Brushes.LightGray : Brushes.DarkGreen; // Colore serpente (testa ; corpo)
                canvas.FillEllipse(colore, new Rectangle(
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width,
                    Settings.Height
                ));
            }

            // Disegna il cibo (rosso)
            canvas.FillEllipse(Brushes.Red, new Rectangle(
                food.X * Settings.Width,
                food.Y * Settings.Height,
                Settings.Width,
                Settings.Height
            ));
        }

        // Prepara una nuova partita
        private void RestartGame()
        {
            // Calcola dimensioni area di gioco
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;

            // Reset elementi di gioco
            Snake.Clear();
            startButton.Enabled = false;
            snapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Punteggio: 0";

            // Crea serpente iniziale (testa + 3 segmenti)
            Snake.Add(new Circle { X = 10, Y = 5 }); // Testa
            for (int i = 0; i < 3; i++) Snake.Add(new Circle()); // Corpo

            // Posiziona cibo casualmente
            food = new Circle
            {
                X = rand.Next(2, maxWidth),
                Y = rand.Next(2, maxHeight)
            };

            gameTimer.Start(); // Avvia il gioco
        }

        // Gestione raccolta cibo
        private void EatFood()
        {
            score++;
            txtScore.Text = $"Punteggio: {score}";

            // Aggiungi nuovo segmento
            Snake.Add(new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            });

            // Rigenera cibo
            food = new Circle
            {
                X = rand.Next(2, maxWidth),
                Y = rand.Next(2, maxHeight)
            };
        }

        // Gestione fine partita
        private void GameOver()
        {
            gameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;

            // Aggiorna record se necessario
            if (score > highScore)
            {
                highScore = score;
                txtHighScore.Text = $"Record: {Environment.NewLine}{highScore}";
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}