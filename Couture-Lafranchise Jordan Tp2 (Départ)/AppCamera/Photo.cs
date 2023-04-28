using System;
using System.Drawing;

namespace AppCamera
{
    public class Photo : IPhoto
    {
        //constantes
        public const int TAILLE_PIXEL = 3;
        public const int LARGEUR_MIN = 640;
        public const int LARGEUR_MAX = 8192;
        public const int HAUTEUR_MIN = 480;
        public const int HAUTEUR_MAX = 8192;
        public const int COMPRESSION_MIN = 1;
        public const int COMPRESSION_MAX = 500;

        //proprietes

        DateTime m_date;
        int m_facteurCompression;
        enuFlash m_flash;
        enuSensibilite m_sensibilite;
        Size m_tailleEnPixels;

        //champs

        public DateTime Date
        {
            get { return m_date; }
        }

        public int FacteurCompression
        {
            get { return m_facteurCompression; }
        }

        public enuFlash Flash
        {
            get { return m_flash; }
        }

        public enuSensibilite Sensibilite
        {
            get { return m_sensibilite; }
        }

        public int TailleEnOctets
        {
            get { return m_tailleEnPixels.Width * m_tailleEnPixels.Height * TAILLE_PIXEL / m_facteurCompression; }
        }

        public Size TailleEnPixels
        {
            get { return m_tailleEnPixels = new Size(m_tailleEnPixels.Width, m_tailleEnPixels.Height); }
        }


        //constructeur
        public Photo(string pInfosPhoto)
        {
            string[] infos = pInfosPhoto.Split(',');
            m_tailleEnPixels = new Size(int.Parse(infos[0]), int.Parse(infos[1]));
            m_flash = (enuFlash)int.Parse(infos[2]);
            m_sensibilite = (enuSensibilite)int.Parse(infos[3]);
            m_facteurCompression = int.Parse(infos[4]);
            m_date = DateTime.Parse(infos[5]);

        }


        public Photo(Size pTailleEnPixels, int pFacteurCompression, enuSensibilite pSensibilite, enuFlash pFlash)
        {
            m_facteurCompression = pFacteurCompression;
            m_flash = pFlash;
            m_sensibilite = pSensibilite;
            m_tailleEnPixels = pTailleEnPixels;
            m_date = DateTime.Now;

            if (pTailleEnPixels.Width < LARGEUR_MIN || pTailleEnPixels.Width > LARGEUR_MAX || pTailleEnPixels.Height < HAUTEUR_MIN || pTailleEnPixels.Height > HAUTEUR_MAX || pFacteurCompression < COMPRESSION_MIN || pFacteurCompression > COMPRESSION_MAX)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return m_tailleEnPixels.Width + "," + m_tailleEnPixels.Height + ',' + (int)m_flash + ',' + (int)m_sensibilite + ',' +
                   m_facteurCompression + ',' + m_date.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
