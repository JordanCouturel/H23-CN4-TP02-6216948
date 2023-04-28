// Séquence de tests pour la classe Photo
#define A1_PhotoConstructeur4P
#define A2_PhotoConstructeur4P
#define A3_PhotoConstructeur1P
#define A4_PhotoTailleEnOctets
#define A5_PhotoToString

//Séquence de tests pour la classe CarteMemoire
#define B1_CarteMemConstructeur
#define B2_CarteMemAjouter
#define B3_CarteMemVider
#define B4_CarteMemPhotoAt
#define B5_CarteMemSupprimer
#define B6_CarteMemPeutAjouter
#define B7_CarteMemChargerPhotos
#define B8_CarteMemEnregistrerPhotos

//Séquence de tests pour la classe Camera
#define C1_CameraConstructeur
#define C2_CameraInfoSuivante
#define C3_CameraFacteurCompression
#define C4_CameraCarteMemoire
#define C5_CameraPhotoCourante
#define C6_CameraTaillePhoto
#define C7_CameraPrendrePhoto
#define C8_CameraFichierParamètres

using AppCamera;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestAppCamera
{
    /// <summary>
    /// Contient tous les tests pour les classes Photo, CarteMemoire et Camera
    /// </summary>
    [TestClass()]
    public class Correcteur
    {
        #region Attributs de tests
        public static string m_version = "H21 Correcteur version 1.0";

        private TestContext testContextInstance;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        #region Attributs de tests supplémentaires
        private static int m_totalScore;
        private static int m_maxScore;
        private static Random m_objRandom = new Random();

        //Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test dans la classe
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            m_totalScore = 0;
            m_maxScore = 0;
        }
        //
        //Utilisez ClassCleanup pour exécuter du code après que tous les tests ont été exécutés dans une classe
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            StreamWriter ficScore = new StreamWriter("../../../Score.txt");
            ficScore.Write(m_version + " : " + FrmPrincipal.APP_INFO + "\n");
            ficScore.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            ficScore.Write("\nRésultat de la correction\nScore : " + m_totalScore + "/" + m_maxScore, Correcteur.m_version, MessageBoxButtons.OK, MessageBoxIcon.Information);
            ficScore.Close();
        }

        #endregion

        #region Tests pour la classe Photo

#if A1_PhotoConstructeur4P
        /// <summary>
        ///Test le Constructeur à 4 paramètres de Photo avec des valeurs valides
        ///</summary>
        [TestMethod()]
        public void A1_PhotoConstructeur4P_ArgumentsValides()
        {
            m_maxScore += 5;

            Size taille = new Size(m_objRandom.Next(Photo.LARGEUR_MIN, Photo.LARGEUR_MAX), m_objRandom.Next(Photo.HAUTEUR_MIN, Photo.HAUTEUR_MAX));
            enuSensibilite sensibilité = (enuSensibilite)m_objRandom.Next(2, 5);
            enuFlash flash = (enuFlash)m_objRandom.Next(0, 2);
            int facteurCompression = m_objRandom.Next(Photo.COMPRESSION_MIN, Photo.COMPRESSION_MAX);
            Photo target = null;

            target = new Photo(taille, facteurCompression, sensibilité, flash);
            Assert.AreEqual(taille, target.TailleEnPixels);

            target = new Photo(new Size(Photo.LARGEUR_MIN, Photo.HAUTEUR_MIN), facteurCompression, sensibilité, flash);
            Assert.AreEqual(new Size(Photo.LARGEUR_MIN, Photo.HAUTEUR_MIN), target.TailleEnPixels);

            target = new Photo(new Size(Photo.LARGEUR_MIN, Photo.HAUTEUR_MAX), facteurCompression, sensibilité, flash);
            Assert.AreEqual(new Size(Photo.LARGEUR_MIN, Photo.HAUTEUR_MAX), target.TailleEnPixels);

            target = new Photo(new Size(Photo.LARGEUR_MAX, Photo.HAUTEUR_MIN), facteurCompression, sensibilité, flash);
            Assert.AreEqual(new Size(Photo.LARGEUR_MAX, Photo.HAUTEUR_MIN), target.TailleEnPixels);

            target = new Photo(new Size(Photo.LARGEUR_MAX, Photo.HAUTEUR_MAX), facteurCompression, sensibilité, flash);
            Assert.AreEqual(new Size(Photo.LARGEUR_MAX, Photo.HAUTEUR_MAX), target.TailleEnPixels);

            m_totalScore++;

            target = new Photo(new Size(Photo.LARGEUR_MAX, Photo.HAUTEUR_MAX), Photo.COMPRESSION_MIN, sensibilité, flash);
            Assert.AreEqual(target.FacteurCompression, Photo.COMPRESSION_MIN);

            target = new Photo(new Size(Photo.LARGEUR_MAX, Photo.HAUTEUR_MAX), Photo.COMPRESSION_MAX, sensibilité, flash);
            Assert.AreEqual(target.FacteurCompression, Photo.COMPRESSION_MAX);

            m_totalScore++;

            Assert.AreEqual(sensibilité, target.Sensibilite);
            m_totalScore++;

            Assert.AreEqual(flash, target.Flash);
            m_totalScore++;

            // on va vérifier la date de la nouvelle photo
            TimeSpan delaiMax = new TimeSpan(0, 1, 0); // une minute 
            Assert.IsTrue(DateTime.Now - target.Date < delaiMax); // H14 24-02
            m_totalScore++;
        }

        /// <summary>
        /// Permet d'instancier une photo avec des données aléatoires valides.
        /// </summary>
        /// <returns></returns>
        private Photo CréerUnePhotoTest()
        {
            Size taille = new Size(m_objRandom.Next(Photo.LARGEUR_MIN, Photo.LARGEUR_MAX), m_objRandom.Next(Photo.HAUTEUR_MIN, Photo.HAUTEUR_MAX));
            enuSensibilite sensibilité = (enuSensibilite)m_objRandom.Next(0, 5);
            enuFlash flash = (enuFlash)m_objRandom.Next(0, 2);
            int facteurCompression = m_objRandom.Next(Photo.COMPRESSION_MIN, Photo.COMPRESSION_MAX);

            return new Photo(taille, facteurCompression, sensibilité, flash);
        }

#endif
#if A2_PhotoConstructeur4P
        /// <summary>
        ///Test le Constructeur à 4 paramètres de Photo avec des valeurs non valides
        ///</summary>
        [TestMethod()]
        public void A2_PhotoConstructeur4P_Exceptions()
        {
            m_maxScore += 3;

            // tests des exceptions pour les valeurs limites de la taille
            int compMoy = (Photo.COMPRESSION_MAX - Photo.COMPRESSION_MIN) / 2 + Photo.COMPRESSION_MIN;
            int largeurMoy = (Photo.LARGEUR_MAX - Photo.LARGEUR_MIN) / 2 + Photo.LARGEUR_MIN;
            int hauteurMoy = (Photo.HAUTEUR_MAX - Photo.HAUTEUR_MIN) / 2 + Photo.HAUTEUR_MIN;

            Assert.IsTrue(InstancierUnePhotoAvecArgumentOutOfRange(int.MinValue, hauteurMoy, compMoy), "ArgumentOutOfRangeException attendue");
            Assert.IsTrue(InstancierUnePhotoAvecArgumentOutOfRange(int.MaxValue, hauteurMoy, compMoy), "ArgumentOutOfRangeException attendue");
            Assert.IsTrue(InstancierUnePhotoAvecArgumentOutOfRange(largeurMoy, int.MinValue, compMoy), "ArgumentOutOfRangeException attendue");
            Assert.IsTrue(InstancierUnePhotoAvecArgumentOutOfRange(largeurMoy, int.MaxValue, compMoy), "ArgumentOutOfRangeException attendue");
            m_totalScore += 2;

            // tests des exceptions pour les valeurs limites pour le facteur de compression
            Assert.IsTrue(InstancierUnePhotoAvecArgumentOutOfRange(largeurMoy, hauteurMoy, int.MinValue), "ArgumentOutOfRangeException attendue");
            Assert.IsTrue(InstancierUnePhotoAvecArgumentOutOfRange(largeurMoy, hauteurMoy, int.MaxValue), "ArgumentOutOfRangeException attendue");
            m_totalScore++;
        }
        private bool InstancierUnePhotoAvecArgumentOutOfRange(int pLargeur, int pHauteur, int pCompression)
        {
            try
            {
                Photo objPhoto = new Photo(new Size(pLargeur, pHauteur), pCompression, enuSensibilite.ISO_100, enuFlash.Activé);
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

#endif
#if A3_PhotoConstructeur1P
        /// <summary>
        ///Test le Constructeur à 1 paramètre de Photo avec des valeurs valides
        ///</summary>
        [TestMethod()]
        public void A3_PhotoConstructeur1P()
        {
            m_maxScore += 5;

            Size taille = new Size(m_objRandom.Next(Photo.LARGEUR_MIN, Photo.LARGEUR_MAX), m_objRandom.Next(Photo.HAUTEUR_MIN, Photo.HAUTEUR_MAX));
            enuSensibilite sensibilité = (enuSensibilite)m_objRandom.Next(0, 5);
            enuFlash flash = (enuFlash)m_objRandom.Next(0, 2);
            int facteurCompression = m_objRandom.Next(Photo.COMPRESSION_MIN, Photo.COMPRESSION_MAX);
            DateTime date = new DateTime(m_objRandom.Next(2000, 2010), m_objRandom.Next(1, 12), m_objRandom.Next(1, 29), m_objRandom.Next(0, 24), m_objRandom.Next(0, 60), m_objRandom.Next(0, 60));
            Photo target = new Photo(taille.Width + "," + taille.Height + "," + (int)flash + "," + (int)sensibilité + "," + facteurCompression + "," + date.ToString("yyyy-MM-dd HH:mm:ss"));

            Assert.AreEqual(taille, target.TailleEnPixels);
            m_totalScore++;

            Assert.AreEqual(facteurCompression, target.FacteurCompression);
            m_totalScore++;

            Assert.AreEqual(sensibilité, target.Sensibilite);
            m_totalScore++;

            Assert.AreEqual(flash, target.Flash);
            m_totalScore++;

            Assert.AreEqual(date, target.Date);
            m_totalScore++;
        }
#endif
#if A4_PhotoTailleEnOctets
        /// <summary>
        ///Test pour la propriété calculée : TailleEnOctets
        ///</summary>
        [TestMethod()]
        public void A4_PhotoTailleEnOctets()
        {
            m_maxScore += 3;
            int facteur = 4;
            Size taille = new Size(m_objRandom.Next(Photo.LARGEUR_MIN, Photo.LARGEUR_MAX), m_objRandom.Next(Photo.HAUTEUR_MIN, Photo.HAUTEUR_MAX));
            Photo target = new Photo(taille, facteur, 0, 0);
            int expected = taille.Width * taille.Height * 3 / facteur;

            Assert.AreEqual(expected, target.TailleEnOctets);
            m_totalScore += 3;
        }
#endif
#if A5_PhotoToString
        /// <summary>
        ///Test pour le fonctionnement de la méthode : ToString
        ///</summary>
        [TestMethod()]
        public void A5_PhotoToString()
        {
            m_maxScore += 3;

            Size taille = new Size(m_objRandom.Next(Photo.LARGEUR_MIN, Photo.LARGEUR_MAX), m_objRandom.Next(Photo.HAUTEUR_MIN, Photo.HAUTEUR_MAX));
            string expected = taille.Width + "," + taille.Height + ",";
            enuSensibilite sensibilité = (enuSensibilite)m_objRandom.Next(0, 5);
            enuFlash flash = (enuFlash)m_objRandom.Next(0, 2);
            expected += (int)flash + "," + (int)sensibilité + ",";
            int facteurCompression = m_objRandom.Next(Photo.COMPRESSION_MIN, Photo.COMPRESSION_MAX);
            expected += facteurCompression + ",";
            DateTime date = new DateTime(m_objRandom.Next(2000, 2010), m_objRandom.Next(1, 12), m_objRandom.Next(1, 29), m_objRandom.Next(0, 24), m_objRandom.Next(0, 60), m_objRandom.Next(0, 60));
            expected += date.ToString("yyyy-MM-dd HH:mm:ss");

            Photo target = new Photo(taille.Width + "," + taille.Height + "," + (int)flash + "," + (int)sensibilité + "," + facteurCompression + "," + date.ToString("yyyy-MM-dd HH:mm:ss"));

            Assert.AreEqual(expected, target.ToString());
            m_totalScore += 3;
        }
#endif
        #endregion

        #region Tests pour la classe CarteMemoire

#if  B1_CarteMemConstructeur
        /// <summary>
        ///Test du constructeur CarteMemoire avec des valeurs valides
        ///</summary>
        [TestMethod()]
        public void B1_CarteMemConstructeur()
        {
            m_maxScore += 5;

            string nom = "";
            int longueurChaine = m_objRandom.Next(25);
            for (int index = 0; index < longueurChaine; index++)
                nom += ((char)(m_objRandom.Next(0, 26) + 'A')).ToString();
            int tailleEnMo = m_objRandom.Next(CarteMemoire.TAILLE_MAX / 2, CarteMemoire.TAILLE_MAX);
            long tailleEnOctets = tailleEnMo * 1024L * 1024;
            ICarteMemoire target = new CarteMemoire(nom, tailleEnMo);

            //Test les 5 propriétés en lecture
            Assert.AreEqual(nom, target.Nom);
            Assert.AreEqual(tailleEnOctets, target.TailleEnOctets);
            Assert.AreEqual(0, target.EspaceUtilise);
            Assert.AreEqual(0, target.NbPhotos);
            Assert.AreEqual(tailleEnOctets, target.EspaceDisponible);
            m_totalScore += 3;

            // tests des valeurs limites de la taille

            Assert.IsTrue(InstancierUneCarteMemoireAvecArgumentOutOfRange(int.MinValue), "CarteMemoireAvecArgumentOutOfRange");
            Assert.IsTrue(InstancierUneCarteMemoireAvecArgumentOutOfRange(CarteMemoire.TAILLE_MIN - 1), "CarteMemoireAvecArgumentOutOfRange");
            Assert.IsFalse(InstancierUneCarteMemoireAvecArgumentOutOfRange(CarteMemoire.TAILLE_MIN), "limite inférieure");
            Assert.IsFalse(InstancierUneCarteMemoireAvecArgumentOutOfRange(CarteMemoire.TAILLE_MAX), "limite suppérieure");
            Assert.IsTrue(InstancierUneCarteMemoireAvecArgumentOutOfRange(CarteMemoire.TAILLE_MAX + 1), "CarteMemoireAvecArgumentOutOfRange");
            Assert.IsTrue(InstancierUneCarteMemoireAvecArgumentOutOfRange(int.MaxValue), "CarteMemoireAvecArgumentOutOfRange");
            m_totalScore += 2;

        }
        /// <summary>
        /// Permet d'instancier une carte mémoire avec des données aléatoires valides.
        /// </summary>
        /// <returns></returns>
        private CarteMemoire CréerUneCarteMemoireTest()
        {
            string nom = "";
            for (int index = 0; index < m_objRandom.Next(25); index++)
                nom += ((char)(m_objRandom.Next(0, 26) + 'A')).ToString();
            int tailleEnMo = m_objRandom.Next(CarteMemoire.TAILLE_MAX / 2, CarteMemoire.TAILLE_MAX);
            long tailleEnOctets = tailleEnMo * 1024L * 1024;
            return new CarteMemoire(nom, tailleEnMo);
        }
        private bool InstancierUneCarteMemoireAvecArgumentOutOfRange(int pTailleEnMegaOctets)
        {
            try
            {
                CarteMemoire objCarte = new CarteMemoire("ABC", pTailleEnMegaOctets);
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
#endif
#if B2_CarteMemAjouter
        /// <summary>
        /// CarteMemoire, test de l'ajout d'une photo et des propriétés affectées.
        ///</summary>
        [TestMethod()]
        public void B2_CarteMemAjouter()
        {
            m_maxScore += 5;

            string nom = "";
            for (int index = 0; index < m_objRandom.Next(25); index++)
                nom += ((char)(m_objRandom.Next(0, 26) + 'A')).ToString();
            int tailleEnMo = m_objRandom.Next(CarteMemoire.TAILLE_MAX / 2, CarteMemoire.TAILLE_MAX);
            long tailleEnOctetsDeLaCarte = tailleEnMo * 1024L * 1024;
            ICarteMemoire target = new CarteMemoire(nom, tailleEnMo);

            IPhoto objPhotoTest = CréerUnePhotoTest();

            // On va ajouter une photo sur la carte mémoire
            target.Ajouter(objPhotoTest);

            //ça ne change pas la taille de la carte mémoire
            Assert.AreEqual(tailleEnOctetsDeLaCarte, target.TailleEnOctets);
            m_totalScore++;

            Assert.AreEqual(1, target.NbPhotos);
            m_totalScore++;

            Assert.AreEqual(objPhotoTest.TailleEnOctets, target.EspaceUtilise);
            Assert.AreEqual(tailleEnOctetsDeLaCarte - objPhotoTest.TailleEnOctets, target.EspaceDisponible);
            m_totalScore++;

            // On va essayer d'ajouter une photo --> null
            // à ce stade il n'y pas de carte dans la caméra
            try
            {
                target.Ajouter(null);
                Assert.Fail("ArgumentNullException attendue");
            }
            catch (ArgumentNullException)
            {
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("ArgumentNullException attendue");
            }

            // On va essayer d'ajouter une photo sur une carte ne disposant pas suffisamment d'espace libre
            target = new CarteMemoire(nom, CarteMemoire.TAILLE_MIN);
            Size taille = new Size(Photo.LARGEUR_MAX, Photo.HAUTEUR_MAX);
            IPhoto objPhoto = new Photo(taille, Photo.COMPRESSION_MIN, enuSensibilite.ISO_100, enuFlash.Activé);
            try
            {
                target.Ajouter(objPhoto);
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(CarteMemoire.ERR_MSG_ESPACE_INSUFFISANT, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }
        }
#endif
#if B3_CarteMemVider
        /// <summary>
        /// CarteMemoire, test la méthode Vider et les propriétés affectées.
        ///</summary>
        [TestMethod()]
        public void B3_CarteMemVider()
        {
            m_maxScore += 4;

            ICarteMemoire target = CréerUneCarteMemoireTest();
            IPhoto objPhotoTest = CréerUnePhotoTest();

            // on va ajouter quelques photos
            int nbPhotos = m_objRandom.Next(5, 10);
            for (int index = 0; index < nbPhotos; index++)
                target.Ajouter(CréerUnePhotoTest());
            Assert.AreEqual(nbPhotos, target.NbPhotos);
            m_totalScore++;

            // On va tester la méthode permettant de vider la carte
            target.Vider();

            Assert.AreEqual(0, target.NbPhotos);
            m_totalScore++;

            Assert.AreEqual(0, target.EspaceUtilise);
            m_totalScore++;

            Assert.AreEqual(target.EspaceDisponible, target.TailleEnOctets);
            m_totalScore++;
        }
#endif
#if B4_CarteMemPhotoAt
        /// <summary>
        /// CarteMemoire, test l'accès à la collection des photos
        ///</summary>
        [TestMethod()]
        public void B4_CarteMemPhotoAt()
        {
            m_maxScore += 3;

            ICarteMemoire target = CréerUneCarteMemoireTest();

            for (int index = 0; index < m_objRandom.Next(10, 20); index++)
            {
                Photo objPhoto = CréerUnePhotoTest();
                target.Ajouter(objPhoto);
                Assert.AreEqual(objPhoto, target.PhotoAt(index));
            }
            m_totalScore++;

            try
            {
                target.PhotoAt(-1);
                Assert.Fail("ArgumentOutOfRangeException attendue");
            }
            catch (ArgumentOutOfRangeException)
            {
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("ArgumentOutOfRangeException attendue");
            }

            try
            {
                target.PhotoAt(target.NbPhotos);
                Assert.Fail("ArgumentOutOfRangeException attendue");
            }
            catch (ArgumentOutOfRangeException)
            {
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("ArgumentOutOfRangeException attendue");
            }
        }
#endif
#if B5_CarteMemSupprimer
        /// <summary>
        /// CarteMemoire, test la supression de photos de la carte et les propriérés affectées
        ///</summary>
        [TestMethod()]
        public void B5_CarteMemSupprimer()
        {
            m_maxScore += 5;

            ICarteMemoire target = CréerUneCarteMemoireTest();
            IPhoto objPhotoTest1 = CréerUnePhotoTest();
            IPhoto objPhotoTest2 = CréerUnePhotoTest();

            // On va ajouter deux photos sur la carte mémoire avant d'essayer de supprimer
            target.Ajouter(objPhotoTest1);
            target.Ajouter(objPhotoTest2);

            long espaceUtiliséAvantSupp = target.EspaceUtilise;
            long espaceDispoAvantSupp = target.EspaceDisponible;
            // On va supprimer la deuxième photo
            target.SupprimerAt(1);

            Assert.AreEqual(1, target.NbPhotos);
            m_totalScore++;

            Assert.AreEqual(espaceUtiliséAvantSupp - objPhotoTest2.TailleEnOctets, target.EspaceUtilise);
            Assert.AreEqual(espaceDispoAvantSupp + objPhotoTest2.TailleEnOctets, target.EspaceDisponible);
            m_totalScore++;

            // On va ajouter quelques photos sur la carte et les supprimer
            target.Vider();
            int nbPhotos = m_objRandom.Next(5, 10);
            for (int index = 0; index < m_objRandom.Next(5, 10); index++)
                target.Ajouter(CréerUnePhotoTest());

            for (int index = 0; index < target.NbPhotos; index++)
            {
                espaceUtiliséAvantSupp = target.EspaceUtilise;
                espaceDispoAvantSupp = target.EspaceDisponible;
                int randomIndex = m_objRandom.Next(0, target.NbPhotos);
                IPhoto objPhotoASupprimer = target.PhotoAt(randomIndex);
                target.SupprimerAt(randomIndex);
                Assert.AreEqual(espaceUtiliséAvantSupp - objPhotoASupprimer.TailleEnOctets, target.EspaceUtilise);
                Assert.AreEqual(espaceDispoAvantSupp + objPhotoASupprimer.TailleEnOctets, target.EspaceDisponible);
            }
            m_totalScore++;

            // on va supprimer une photo avec des arguments non valides
            try
            {
                target.SupprimerAt(-1);
                Assert.Fail("ArgumentOutOfRangeException attendue");
            }
            catch (ArgumentOutOfRangeException)
            {
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("ArgumentOutOfRangeException attendue");
            }

            try
            {
                target.SupprimerAt(target.NbPhotos);
                Assert.Fail("ArgumentOutOfRangeException attendue");
            }
            catch (ArgumentOutOfRangeException)
            {
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("ArgumentOutOfRangeException attendue");
            }
        }
#endif
#if B6_CarteMemPeutAjouter
        /// <summary>
        /// CarteMemoire test de la méthode PeutAjouter
        ///</summary>
        [TestMethod()]
        public void B6_CarteMemPeutAjouter()
        {
            m_maxScore += 4;

            int tailleEnMo = CarteMemoire.TAILLE_MIN;
            long tailleEnOctets = tailleEnMo * 1024L * 1024;
            ICarteMemoire target = new CarteMemoire("test", tailleEnMo);
            IPhoto objPhotoTest = new Photo(new Size(1000, 1000), 1, 0, 0);

            int taillePhoto = objPhotoTest.TailleEnOctets;

            Assert.IsTrue(target.PeutAjouter(objPhotoTest));
            target.Ajouter(objPhotoTest);
            m_totalScore++;

            Assert.IsTrue(target.PeutAjouter(objPhotoTest));
            target.Ajouter(objPhotoTest);
            m_totalScore++;

            Assert.IsFalse(target.PeutAjouter(objPhotoTest));
            m_totalScore++;

            Assert.IsFalse(target.PeutAjouter(null));
            m_totalScore++;
        }
#endif
#if B7_CarteMemChargerPhotos
        /// <summary>
        /// CarteMemoire test le fonctionnement de la méthode ChargerPhotos
        ///</summary>
        [TestMethod()]
        public void B7_CarteMemChargerPhotos()
        {
            m_maxScore += 5;

            const string NOM_FICHIER_TEST = "CarteTest";
            Directory.SetCurrentDirectory("../../../AppCamera/bin/Debug");
            StreamWriter objSW = new StreamWriter(NOM_FICHIER_TEST + ".txt");
            int nbPhotos = m_objRandom.Next(10, 15);
            int espaceUtilisé = 0;
            for (int index = 0; index < nbPhotos; index++)
            {
                IPhoto objPhoto = CréerUnePhotoTest();
                objSW.WriteLine(objPhoto);
                espaceUtilisé += objPhoto.TailleEnOctets;
            }
            objSW.Close();

            int tailleEnMo = CarteMemoire.TAILLE_MAX;
            long tailleEnOctets = tailleEnMo * 1024L * 1024;
            ICarteMemoire target = new CarteMemoire(NOM_FICHIER_TEST, tailleEnMo);
            target.ChargerPhotos();
            Assert.AreEqual(nbPhotos, target.NbPhotos);

            // On va recharger les photos, cela devrait donner le même résultat
            target.ChargerPhotos();
            Assert.AreEqual(nbPhotos, target.NbPhotos);
            m_totalScore += 3;

            // on va vérifier l'espace utilisé et disponible sur la carte
            Assert.AreEqual(espaceUtilisé, target.EspaceUtilise);
            Assert.AreEqual(tailleEnOctets - espaceUtilisé, target.EspaceDisponible);
            m_totalScore += 1;

            // on va vérifier le fonctionnement avec un fichier vide
            objSW = new StreamWriter(NOM_FICHIER_TEST + ".txt");
            objSW.Close();
            target.ChargerPhotos();
            Assert.AreEqual(0, target.NbPhotos);
            Assert.AreEqual(0, target.EspaceUtilise);
            Assert.AreEqual(tailleEnOctets, target.EspaceDisponible);
            m_totalScore += 1;
        }
#endif
#if B8_CarteMemEnregistrerPhotos
        /// <summary>
        /// CarteMemoire test le fonctionnement de la méthode EnregistrerPhotos
        ///</summary>
        [TestMethod()]
        public void B8_CarteMemEnregistrerPhotos()
        {
            m_maxScore += 5;

            const string NOM_FICHIER_TEST = "CarteTest";
            Directory.SetCurrentDirectory("../../../AppCamera/bin/Debug");

            int tailleEnMo = CarteMemoire.TAILLE_MAX;
            long tailleEnOctets = tailleEnMo * 1024L * 1024;
            ICarteMemoire target = new CarteMemoire(NOM_FICHIER_TEST, tailleEnMo);
            ICarteMemoire backup = new CarteMemoire(NOM_FICHIER_TEST, tailleEnMo);
            int nbPhotos = m_objRandom.Next(10, 15);
            target.Vider();
            for (int index = 0; index < nbPhotos; index++)
            {
                Photo photoTest = CréerUnePhotoTest();
                target.Ajouter(photoTest);
                backup.Ajouter(photoTest);
            }
            target.EnregistrerPhotos();
            target.Vider();
            target.ChargerPhotos();

            Assert.AreEqual(nbPhotos, target.NbPhotos);
            m_totalScore += 2;
            // H15 Ici on va s'assurer que les photos chargées sont identiques à celles qui devaient être enregistrées
            for (int index = 0; index < backup.NbPhotos; index++)
            {
                IPhoto photoChargée = target.PhotoAt(index);
                IPhoto photoAEnregistrer = backup.PhotoAt(index);
                Assert.AreEqual(photoChargée.TailleEnPixels, photoAEnregistrer.TailleEnPixels);
                Assert.AreEqual(photoChargée.TailleEnOctets, photoAEnregistrer.TailleEnOctets);
                Assert.AreEqual(photoChargée.FacteurCompression, photoAEnregistrer.FacteurCompression);
                Assert.AreEqual(photoChargée.Sensibilite, photoAEnregistrer.Sensibilite);
                Assert.AreEqual(photoChargée.Flash, photoAEnregistrer.Flash);
            }
            m_totalScore += 3;
        }
#endif
        #endregion

        #region Tests pour la classe Camera

#if C1_CameraConstructeur
        /// <summary>
        /// Camera test le fonctionnement du constructeur
        ///</summary>
        [TestMethod()]
        public void C1_CameraConstructeur()
        {
            m_maxScore += 3;

            Camera target = new Camera();
            Assert.IsNull(target.CarteMemoireCourante, "erreur CarteMemoire");
            Assert.AreEqual(-1, target.PosPhotoCourante, "erreur PosPhotoCourante");
            Assert.AreEqual(0, (int)target.Dimension, "erreur Dimension");
            Assert.AreEqual(0, (int)target.Qualite, "erreur Qualite");
            Assert.AreEqual(0, (int)target.Sensibilite, "erreur Sensibilite");
            Assert.AreEqual(0, (int)target.Flash, "erreur Flash");

            m_totalScore += 3;
        }
#endif
#if C2_CameraInfoSuivante
        /// <summary>
        /// Camera test le fonctionnement de la Dimension, la Qualité et le Flash 
        ///</summary>
        [TestMethod()]
        public void C2_CameraInfoSuivante()
        {
            m_maxScore += 5;
            ICamera target = new Camera();

            // test de la Dimension
            target.Dimension = (enuDimension)1;
            Assert.AreEqual(1, (int)target.Dimension);
            // test de la méthode DimensionSuivante
            target.DimensionSuivante();
            Assert.AreEqual(2, (int)target.Dimension);
            m_totalScore++;
            target.Dimension = enuDimension.Petite;
            target.DimensionSuivante();
            Assert.AreEqual(enuDimension.Maximale, target.Dimension);
            m_totalScore++;

            // test de la Qualité
            target.Qualite = (enuQualite)1;
            Assert.AreEqual(1, (int)target.Qualite);
            // test de la méthode QualitéSuivante
            target.QualiteSuivante();
            target.Qualite = 0;
            target.QualiteSuivante();
            Assert.AreEqual(1, (int)target.Qualite);

            target.Qualite = enuQualite.Faible;
            target.QualiteSuivante();
            Assert.AreEqual(enuQualite.Excellente, target.Qualite);
            m_totalScore++;

            // test de la Sensibilité H15
            target.Sensibilite = (enuSensibilite)1;
            Assert.AreEqual(1, (int)target.Sensibilite);
            // test de la méthode SensibilitéSuivante
            target.SensibiliteSuivante();
            target.Sensibilite = 0;
            target.SensibiliteSuivante();
            Assert.AreEqual(1, (int)target.Sensibilite);

            target.Sensibilite = enuSensibilite.ISO_800;
            target.SensibiliteSuivante();
            Assert.AreEqual(enuSensibilite.ISO_64, target.Sensibilite);
            m_totalScore++;

            // test le Flash
            target.Flash = enuFlash.Activé;
            Assert.AreEqual(enuFlash.Activé, target.Flash);
            target.Flash = enuFlash.Désactivé;
            Assert.AreEqual(enuFlash.Désactivé, target.Flash);
            m_totalScore++;
        }
#endif

#if C3_CameraFacteurCompression
        /// <summary>
        /// Camera test le fonctionnement de : FacteurCompression
        ///</summary>
        [TestMethod()]
        public void C3_CameraFacteurCompression()
        {
            m_maxScore += 3;
            ICamera target = new Camera();
            int[] tabFacteurs = { 4, 12, 20, 50 };
            for (int q = 0; q < tabFacteurs.Length; q++)
            {
                target.Qualite = (enuQualite)q;
                Assert.AreEqual(tabFacteurs[q], target.FacteurCompression);
            }
            m_totalScore += 3;
        }
#endif
#if C4_CameraCarteMemoire
        /// <summary>
        /// Camera test le fonctionnement relatif à l'utilisation d'une carte mémoire
        ///</summary>
        [TestMethod()]
        public void C4_CameraCarteMemoire()
        {
            m_maxScore += 6;

            ICamera target = new Camera();
            Assert.IsNull(target.CarteMemoireCourante);

            ICarteMemoire objCarteMem = CréerUneCarteMemoireTest();
            target.InsererCarteMemoire(objCarteMem);
            Assert.AreEqual(objCarteMem, target.CarteMemoireCourante);
            m_totalScore++;

            target.ViderLaCarte();
            Assert.AreEqual(0, objCarteMem.NbPhotos);

            target.EjecterCarteMemoire();
            Assert.IsNull(target.CarteMemoireCourante);
            Assert.AreEqual(-1, target.PosPhotoCourante);
            m_totalScore++;

            // à ce stade il n'y pas de carte mémoire dans la caméra
            try
            {
                target.EjecterCarteMemoire();
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(Camera.ERR_MSG_CARTE_MANQUANTE, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }

            // à ce stade il n'y pas de carte dans la caméra
            try
            {
                target.ViderLaCarte();
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(Camera.ERR_MSG_CARTE_MANQUANTE, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }

            target.InsererCarteMemoire(objCarteMem);
            // on va essayer d'insérer une autre carte dans la caméra
            try
            {
                target.InsererCarteMemoire(objCarteMem);
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(Camera.ERR_MSG_CARTE_PRÉSENTE, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }

            // on va essayer d'introduire une carte null
            try
            {
                target.EjecterCarteMemoire();
                target.InsererCarteMemoire(null);
                Assert.Fail("ArgumentNullException attendue");
            }
            catch (ArgumentNullException)
            {
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("ArgumentNullException attendue");
            }
        }
#endif
#if C5_CameraPhotoCourante
        /// <summary>
        /// Camera test le fonctionnement de la gestion de la photo courante
        ///</summary>
        [TestMethod()]
        public void C5_CameraPhotoCourante()
        {
            m_maxScore += 7;

            ICamera target = new Camera();
            ICarteMemoire objCarteMem = CréerUneCarteMemoireTest();

            Assert.IsNull(target.PhotoCourante); // H14 24-12

            for (int index = 0; index < m_objRandom.Next(10, 20); index++)
                objCarteMem.Ajouter(CréerUnePhotoTest());

            target.InsererCarteMemoire(objCarteMem);
            Assert.AreEqual(0, target.PosPhotoCourante);
            Assert.AreEqual(objCarteMem.PhotoAt(0), target.PhotoCourante); // H19
            m_totalScore++;

            target.PhotoSuivante();
            Assert.AreEqual(1, target.PosPhotoCourante);
            Assert.AreEqual(objCarteMem.PhotoAt(1), target.PhotoCourante); // H14 24-12

            target.PhotoPrecedente();
            Assert.AreEqual(0, target.PosPhotoCourante);
            Assert.AreEqual(objCarteMem.PhotoAt(0), target.PhotoCourante); // H14 24-12

            target.PhotoPrecedente();
            Assert.AreEqual(objCarteMem.NbPhotos - 1, target.PosPhotoCourante);
            Assert.AreEqual(objCarteMem.PhotoAt(objCarteMem.NbPhotos - 1), target.PhotoCourante); // H14 24-12

            target.PhotoSuivante();
            Assert.AreEqual(0, target.PosPhotoCourante);
            m_totalScore++;

            // à ce stade la carte mémoire est vide
            target.ViderLaCarte();
            ExécuterPhotoSuivanteEtPrecedente(target, Camera.ERR_MSG_CARTE_VIDE);

            //à ce stade il n'y pas de carte dans la caméra
            target.EjecterCarteMemoire();
            try
            {
                target.ViderLaCarte();
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(Camera.ERR_MSG_CARTE_MANQUANTE, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }
            ExécuterPhotoSuivanteEtPrecedente(target, Camera.ERR_MSG_CARTE_MANQUANTE);
        }
        private void ExécuterPhotoSuivanteEtPrecedente(ICamera pTarget, string pMessage)
        {
            try
            {
                pTarget.PhotoSuivante();
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(pMessage, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }

            try
            {
                pTarget.PhotoPrecedente();
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(pMessage, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }
        }
#endif
#if C6_CameraTaillePhoto
        /// <summary>
        /// Camera test les propriétés concernant la taille d'une photo
        ///</summary>
        [TestMethod()]
        public void C6_CameraTaillePhoto()
        {
            //Maximale, Large, Grande, Moyenne, Petite
            m_maxScore += 5;
            ICamera target = new Camera();
            Size actual;
            target.Dimension = enuDimension.Maximale;
            actual = target.TailleEnPixelsSelonDimension;
            Assert.AreEqual(new Size(4000, 3000), actual, "enuDimension.Maximale");
            m_totalScore++;

            target.Dimension = enuDimension.Large;
            actual = target.TailleEnPixelsSelonDimension;
            Assert.AreEqual(new Size(3840, 2160), actual, "enuDimension.Large");
            m_totalScore++;

            target.Dimension = enuDimension.Grande;
            actual = target.TailleEnPixelsSelonDimension;
            Assert.AreEqual(new Size(3000, 2000), actual, "enuDimension.Grande");
            m_totalScore++;

            target.Dimension = enuDimension.Moyenne;
            actual = target.TailleEnPixelsSelonDimension;
            Assert.AreEqual(new Size(1600, 1200), actual, "enuDimension.Moyenne");
            m_totalScore++;

            target.Dimension = enuDimension.Petite;
            actual = target.TailleEnPixelsSelonDimension;
            Assert.AreEqual(new Size(640, 480), actual, "enuDimension.Petite");
            m_totalScore++;

            m_maxScore += 2;
            target.Dimension = enuDimension.Moyenne;
            target.Qualite = enuQualite.Moyenne;
            Assert.AreEqual(1600 * 1200 * 3 / 20, target.TailleEnOctetsEstimee, "Dimension moyenne et qualité moyenne");
            m_totalScore++;
            target.Dimension = enuDimension.Petite;
            target.Qualite = enuQualite.Excellente;
            Assert.AreEqual(640 * 480 * 3 / 4, target.TailleEnOctetsEstimee, "Dimension petite et qualité excellente");
            m_totalScore++;
        }
#endif

#if C7_CameraPrendrePhoto
        /// <summary>
        /// Camera test les méthodes PrendreUnePhoto et SupprimerPhoto
        ///</summary>
        [TestMethod()]
        public void C7a_PeutPrendreUnePhoto()
        {
            m_maxScore += 2;

            ICamera target = new Camera();
            // on test si on peut prendre une photo sans carte mémoire dans la caméra
            Assert.IsFalse(target.PeutPrendreUnePhoto(), "Pas de carte mémoire");

            // on insere une carte mémoire dans la caméra
            ICarteMemoire objCarteMemoire = CréerUneCarteMemoireTest();
            target.InsererCarteMemoire(objCarteMemoire);
            Assert.IsTrue(target.PeutPrendreUnePhoto(), "Il y a une carte mémoire et de l'espace suffisant");
            m_totalScore++;

            // on vérifier si on peut prendre une photo trop grande pour l'espace disponible sur la carte mémoire
            objCarteMemoire = new CarteMemoire("test", CarteMemoire.TAILLE_MIN);
            target.EjecterCarteMemoire();
            target.InsererCarteMemoire(objCarteMemoire);
            target.Qualite = enuQualite.Excellente;
            target.Dimension = enuDimension.Maximale;
            Assert.IsFalse(target.PeutPrendreUnePhoto(), "Il y a une carte mémoire mais pas assez d'espace");
            m_totalScore++;
        }

        /// <summary>
        /// Camera test les méthodes PrendreUnePhoto et SupprimerPhoto
        ///</summary>

        [TestMethod()]
        public void C7b_CameraPrendrePhoto()
        {
            m_maxScore += 7;

            ICamera target = new Camera();

            // on va prendre une photo sans carte mémoire dans la caméra
            try
            {
                target.PrendrePhoto();
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(Camera.ERR_MSG_CARTE_MANQUANTE, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }

            // on va prendre une photo avec une carte mémoire dans la caméra
            ICarteMemoire objCarteMemoire = CréerUneCarteMemoireTest();
            target.InsererCarteMemoire(objCarteMemoire);
            DateTime dateCourante = DateTime.Now;
            Assert.IsTrue(target.PeutPrendreUnePhoto());
            target.PrendrePhoto();

            // on va vérifier si la photo à été ajoutée sur la carte mémoire
            Assert.AreEqual(1, objCarteMemoire.NbPhotos);

            // on va vérifier la date de la nouvelle photo
            IPhoto objNouvellePhoto = objCarteMemoire.PhotoAt(0);
            TimeSpan delaiMax = new TimeSpan(0, 1, 0); // une minute 
            Assert.IsTrue(objNouvellePhoto.Date - dateCourante < delaiMax);
            m_totalScore++;

            // on va supprimer la photo courante
            target.SupprimerPhotoCourante();
            Assert.AreEqual(0, objCarteMemoire.NbPhotos);
            Assert.AreEqual(-1, target.PosPhotoCourante);
            m_totalScore++;

            // on va supprimer la photo courante avec une carte vide
            try
            {
                target.SupprimerPhotoCourante();
                Assert.Fail();
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(Camera.ERR_MSG_CARTE_VIDE, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }

            // on va prendre quelques photos
            for (int index = 0; index < m_objRandom.Next(5, 10); index++)
            {
                target.PrendrePhoto();
                Assert.AreEqual(index, target.PosPhotoCourante);
                dateCourante = DateTime.Now;
                objNouvellePhoto = objCarteMemoire.PhotoAt(index);
                Assert.IsTrue(objNouvellePhoto.Date - dateCourante < delaiMax);
            }
            Assert.AreEqual(objCarteMemoire.NbPhotos - 1, target.PosPhotoCourante); // H14 24-12

            m_totalScore++;

            // on va supprimer les photos en partant de la dernière
            target.PhotoPrecedente();
            while (objCarteMemoire.NbPhotos > 0)
            {
                target.SupprimerPhotoCourante();
                Assert.AreEqual(objCarteMemoire.NbPhotos - 1, target.PosPhotoCourante);
            }
            m_totalScore++;

            // on va prendre une photo trop grande pour l'espace disponible sur la carte mémoire
            objCarteMemoire = new CarteMemoire("test", CarteMemoire.TAILLE_MIN);
            target.EjecterCarteMemoire();
            target.InsererCarteMemoire(objCarteMemoire);
            target.Qualite = enuQualite.Excellente;
            target.Dimension = enuDimension.Maximale;


            Assert.IsFalse(target.PeutPrendreUnePhoto());//déja testé
            try
            {
                target.PrendrePhoto();
                Assert.Fail("InvalidOperationException attendue");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(CarteMemoire.ERR_MSG_ESPACE_INSUFFISANT, e.Message);
                m_totalScore++;
            }
            catch (Exception)
            {
                Assert.Fail("InvalidOperationException attendue");
            }
        }
#endif

#if C8_CameraFichierParamètres
        /// <summary>
        /// Camera les méthodes ChargerParamètres et EnregistrerParamètres
        ///</summary>
        [TestMethod()]
        public void C8_CameraFichierParamètres()
        { 
            m_maxScore += 5;

            ICamera target = new Camera();
            enuDimension dimension = (enuDimension)m_objRandom.Next(0, 4);
            target.Dimension = dimension;
            enuQualite qualité = (enuQualite)m_objRandom.Next(0, 4);
            target.Qualite = qualité;
            enuFlash flash = (enuFlash)m_objRandom.Next(0, 2);
            target.Flash = flash;
            enuSensibilite sensibilité = (enuSensibilite)m_objRandom.Next(0, 4);
            target.Sensibilite = sensibilité;
            // on va enregistrer les paramètres courants
            target.EnregistrerParametres();

            // on va créer une nouvelle caméra et charger les paramètres
            target = new Camera();
            target.ChargerParametres();

            if (target.Dimension != dimension || target.Qualite != qualité || target.Flash != flash || target.Sensibilite != sensibilité)
                Assert.Fail("ChargerParametres et/ou EnregistrerParametres");
            m_totalScore += 5;
        }
#endif
        #endregion
    }
}