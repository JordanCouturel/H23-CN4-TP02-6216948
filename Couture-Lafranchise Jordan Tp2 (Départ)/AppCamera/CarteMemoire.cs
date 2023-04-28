using System;
using System.Collections.Generic;
using System.IO;

namespace AppCamera
{
    public class CarteMemoire : ICarteMemoire
    {

        //constantes
        public const int TAILLE_MIN = 8; //  8 Mo
        public const int TAILLE_MAX = 8192; // 8192 Mo
        public const string ERR_MSG_ESPACE_INSUFFISANT = "Espace insuffisant";



        long m_espaceUtilise;
        string m_nom;
        long m_tailleEnOctets;
        private List<IPhoto> m_colPhotos;

        //proprietes

        public long EspaceDisponible
        {
            get { return m_tailleEnOctets - m_espaceUtilise; }
        }

        public long EspaceUtilise
        {
            get { return m_espaceUtilise; }
        }

        public int NbPhotos
        {
            get { return m_colPhotos.Count; }
        }

        public string Nom
        {
            get { return m_nom; }
        }

        public long TailleEnOctets
        {
            get { return m_tailleEnOctets; }
        }


        //constructeur                                                                              
        public CarteMemoire(string pNom, int pTailleEnMegaOctets)
        {

            if (pTailleEnMegaOctets > TAILLE_MAX || pTailleEnMegaOctets < TAILLE_MIN)
            {
                throw new ArgumentOutOfRangeException();
            }

            m_nom = pNom;
            m_tailleEnOctets = pTailleEnMegaOctets * 1024L * 1024;
            m_colPhotos = new List<IPhoto>();
            m_espaceUtilise = 0;
        }
        //methodes
        public void Ajouter(IPhoto pobjPhoto)
        {
            if (pobjPhoto == null)
            {
                throw new ArgumentNullException();
            }

            if ((EspaceDisponible - pobjPhoto.TailleEnOctets) < 0)
            {
                throw new InvalidOperationException(ERR_MSG_ESPACE_INSUFFISANT);
            }
            m_colPhotos.Add(pobjPhoto);
            m_espaceUtilise += pobjPhoto.TailleEnOctets;
        }

        public void ChargerPhotos()
        {
            Vider();
            StreamReader fichier = new StreamReader(m_nom + ".txt");
            while (!fichier.EndOfStream)
            {
                string ligne = fichier.ReadLine();
                Ajouter(new Photo(ligne));
            }
            fichier.Close();


        }

        public void EnregistrerPhotos()
        {
            StreamWriter fichier = new StreamWriter($"{m_nom}.txt");
            foreach (Photo photo in m_colPhotos)
            {
                fichier.WriteLine(photo.ToString());
            }

            fichier.Close();


        }

        public bool PeutAjouter(IPhoto pobjPhoto)
        {
            if (pobjPhoto != null && EspaceDisponible >= pobjPhoto.TailleEnOctets)
            {
                return true;
            }
            return false;
        }

        public IPhoto PhotoAt(int pPosition)
        {
            if (pPosition < 0 || pPosition >= NbPhotos)
            {
                throw new ArgumentOutOfRangeException();
            }
            return m_colPhotos[pPosition];
        }

        public void SupprimerAt(int pPosition)
        {
            if (pPosition < 0 || pPosition >= NbPhotos)
            {
                throw new ArgumentOutOfRangeException();
            }
            m_espaceUtilise -= m_colPhotos[pPosition].TailleEnOctets;
            m_colPhotos.RemoveAt(pPosition);


        }

        public void Vider()
        {
            m_colPhotos.Clear();
            m_espaceUtilise = 0;
        }
    }
}
