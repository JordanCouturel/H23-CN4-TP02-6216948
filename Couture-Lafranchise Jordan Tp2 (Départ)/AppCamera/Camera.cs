using System;
using System.Drawing;
using System.IO;

namespace AppCamera
{
    public class Camera : ICamera
    {
        //constantes
        public const string ERR_MSG_CARTE_MANQUANTE = "Carte manquante";
        public const string ERR_MSG_CARTE_PRÉSENTE = "Carte déjà présente";
        public const string ERR_MSG_CARTE_VIDE = "Carte vide";
        private const string NOM_FICHIER_PARAMETRES = "parametres.txt";

        //champs
        private enuDimension m_dimension;
        private enuFlash m_flash;
        private int m_posPhotoCourante;
        private ICarteMemoire m_objCarteMemoireCourante;
        private enuQualite m_qualite;
        private enuSensibilite m_sensibilite;

        //propriete

        public ICarteMemoire CarteMemoireCourante
        {
            get { return m_objCarteMemoireCourante; }
            set { m_objCarteMemoireCourante = value; }
        }

        public int FacteurCompression                              //////////////////////
        {
            get
            {
                if ((int)m_qualite == 0)
                {
                    return 4;
                }
                else if ((int)m_qualite == 1)
                {
                    return 12;
                }
                else if ((int)m_qualite == 2)
                {
                    return 20;
                }
                else if ((int)m_qualite == 3)
                {
                    return 50;
                }
                else
                {
                    return 0;
                }
            }
        }

        public enuFlash Flash
        {
            get { return m_flash; }
            set { m_flash = value; }
        }

        public IPhoto PhotoCourante                                       ///////////////////////
        {
            get
            {
                if (PosPhotoCourante < 0 || CarteMemoireCourante == null)
                {
                    return null;
                }
                else
                {
                    return CarteMemoireCourante.PhotoAt(PosPhotoCourante);
                }
            }
        }

        public enuDimension Dimension
        {
            get { return m_dimension; }
            set { m_dimension = value; }
        }

        public int PosPhotoCourante                               //////////////////////////////////
        {
            get { return m_posPhotoCourante; }
            set { m_posPhotoCourante = value; }
        }

        public enuQualite Qualite
        {
            get { return m_qualite; }
            set { m_qualite = value; }
        }

        public long TailleEnOctetsEstimee
        {
            get { return TailleEnPixelsSelonDimension.Width * TailleEnPixelsSelonDimension.Height * 3 / FacteurCompression; }
        }

        public Size TailleEnPixelsSelonDimension
        {
            get
            {
                Size[] TabSize = { new Size(4000, 3000), new Size(3840, 2160), new Size(3000, 2000), new Size(1600, 1200), new Size(640, 480) };
                return TabSize[(int)Dimension];
            }
        }

        public enuSensibilite Sensibilite
        {
            get { return m_sensibilite; }
            set { m_sensibilite = value; }
        }

        //constructeur
        public Camera()
        {
            PosPhotoCourante = -1;
            m_dimension = enuDimension.Maximale;
            m_qualite = enuQualite.Excellente;
            m_sensibilite = enuSensibilite.ISO_64;
            m_flash = enuFlash.Activé;
        }

        public void ChargerParametres()
        { 

            StreamReader fichier = new StreamReader(NOM_FICHIER_PARAMETRES);

            string ligne1 = fichier.ReadLine();
            Dimension = (enuDimension)int.Parse(ligne1);
            string ligne2 = fichier.ReadLine();
            Qualite = (enuQualite)int.Parse(ligne2);
            string ligne3= fichier.ReadLine();
            Sensibilite = (enuSensibilite)int.Parse(ligne3);
            string ligne4=fichier.ReadLine();
            Flash = (enuFlash)int.Parse(ligne4);

            fichier.Close();

        }

        public void DimensionSuivante()
        {
            m_dimension++;

            if ((int)m_dimension == 5)
            {
                m_dimension = 0;
            }


        }

        public void EjecterCarteMemoire()
        {
            if (m_objCarteMemoireCourante == null)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_MANQUANTE);
            }
            else
            {
                CarteMemoireCourante = null;
                m_posPhotoCourante = -1;
            }
          
        }

        public void EnregistrerParametres()
        {
            StreamWriter fichier = new StreamWriter(NOM_FICHIER_PARAMETRES);
            fichier.WriteLine((int)Dimension);
            fichier.WriteLine((int)Qualite);
            fichier.WriteLine((int)Sensibilite);
            fichier.WriteLine((int)Flash);
            fichier.Close();

        }

        public void InsererCarteMemoire(ICarteMemoire pCarteMemoire)
        {
            if (pCarteMemoire == null)
            {
                throw new ArgumentNullException(ERR_MSG_CARTE_MANQUANTE);
            }
            if (m_objCarteMemoireCourante != null)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_PRÉSENTE);
            }

            CarteMemoireCourante = pCarteMemoire;
            m_posPhotoCourante = 0;
        }

        public bool PeutPrendreUnePhoto()
        {
          
            if (CarteMemoireCourante != null && CarteMemoireCourante.EspaceDisponible >= TailleEnOctetsEstimee)
            {
                return true;
            }
            return false;
        }

        public void PhotoPrecedente()
        {
            if (CarteMemoireCourante == null)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_MANQUANTE);
            }
            if (CarteMemoireCourante.NbPhotos == 0)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_VIDE);
            }

            m_posPhotoCourante--;
            if (m_posPhotoCourante == -1)
            {
                m_posPhotoCourante = CarteMemoireCourante.NbPhotos - 1;
            }
        }

        public void PhotoSuivante()
        {

            if (CarteMemoireCourante == null)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_MANQUANTE);
            }
            if (CarteMemoireCourante.NbPhotos == 0)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_VIDE);
            }


            m_posPhotoCourante++;
            if (m_posPhotoCourante == CarteMemoireCourante.NbPhotos)
            {
                m_posPhotoCourante = 0;
            }

        }

        public void PrendrePhoto()
        {
         
            if (PeutPrendreUnePhoto())
            {
                CarteMemoireCourante.Ajouter(new Photo(TailleEnPixelsSelonDimension, FacteurCompression, Sensibilite, Flash));
                m_posPhotoCourante = CarteMemoireCourante.NbPhotos - 1;
            }
            else if(CarteMemoireCourante==null)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_MANQUANTE);
            }
            else
            {
                throw new InvalidOperationException(CarteMemoire.ERR_MSG_ESPACE_INSUFFISANT);
            }
        }

        public void QualiteSuivante()
        {
            m_qualite++;

            if ((int)m_qualite == 4)
            {
                m_qualite = 0;
            }

        }

        public void SensibiliteSuivante()
        {
            m_sensibilite++;

            if ((int)m_sensibilite == 5)
            {
                m_sensibilite = 0;
            }

        }

        public void SupprimerPhotoCourante()
        {
            if (CarteMemoireCourante.NbPhotos == 0)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_VIDE);
            }
            else
            {
                CarteMemoireCourante.SupprimerAt(m_posPhotoCourante);
                PosPhotoCourante = CarteMemoireCourante.NbPhotos - 1;
            }
           
           
        }

        public void ViderLaCarte()
        {
            if (CarteMemoireCourante == null)
            {
                throw new InvalidOperationException(ERR_MSG_CARTE_MANQUANTE);
            }
            CarteMemoireCourante.Vider();
        }
    }
}
