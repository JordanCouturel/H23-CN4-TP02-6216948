using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;

namespace AppIntroListView
{
    public partial class FrmPrincipal : Form
    {
        public const string APP_INFOS = "(Démo)";

        private List<Membre> m_colTousLesMembres = new List<Membre>(); // Collection de tous les membres du site de rencontre
        private List<Membre> m_colHommes = new List<Membre>(); // Collection contenant uniquement les membres qui sont des Hommes
        private List<Membre> m_colFemmes = new List<Membre>(); // Collection contenant uniquement les membres qui sont des Femmes

        #region Propriétés
        //=====================================================================================
        /// <summary>
        /// Obtient le membre de m_colHommes correspondant à la sélection dans cboMembresGauche ou null s'il n'y a aucune sélection
        /// </summary>
        public Membre MembreCourantGauche
        {
            // TODO F01 : Obtient le membre correspondant à la sélection dans cboMembresGauche ou null s'il n'y a aucune sélection 
            get
            {
                if (cboMembresGauche.SelectedIndex != -1)
                    return m_colHommes[cboMembresGauche.SelectedIndex];
                else
                    return null;
            }
        }
        //=====================================================================================
        /// <summary>
        /// Obtient le membre de m_colFemmes correspondant à la sélection dans cboMembresDroite ou null s'il n'y a aucune sélection
        /// </summary>
        public Membre MembreCourantDroite
        {
            // Obtient le membre correspondant à la sélection dans cboMembresGDroite ou null s'il n'y a aucune sélection
            get
            {
                if (cboMembresDroite.SelectedIndex != -1)
                    return m_colFemmes[cboMembresDroite.SelectedIndex];
                else
                    return null;
            }
        }
        #endregion

        #region Constructeur
        //=====================================================================================
        /// <summary>
        /// // Utilise ChargerListeMembres() pour charger la liste de tous les membres et peuple les deux combos
        /// Sélectionne le premier membre dans le cboGauche
        /// Sélectionne le dernier membre dans le cboDroite
        /// </summary>
        public FrmPrincipal()
        {
            InitializeComponent();
            //-------------------------------
            Text += APP_INFOS;

            ChargerListeMembres(); // Charge la liste de tous les membres et peuple les deux combos
            cboMembresGauche.SelectedIndex = 0; // Sélectionne le premier membre
            cboMembresDroite.SelectedIndex = cboMembresDroite.Items.Count - 1; // Sélectionne le dernier membre
        }
        #endregion

        #region ChargerListeMembres (Charge la liste de tous les membres)
        //=====================================================================================
        /// <summary>
        /// Lit les informations dans le fichier "Membres.txt", les charge en mémoire dans la collection de membres.
        /// Selon le genre du membre, peuple
        /// peuple les deux combos cboMembresGauche et cboMembresDroite et les collections m_colHommes et m_colFemmes
        /// </summary>
        private void ChargerListeMembres()
        {
            StreamReader objFichier = new StreamReader("Membres.txt");

            while (!objFichier.EndOfStream)
            {
                Membre membre = new Membre(objFichier.ReadLine());
                m_colTousLesMembres.Add(membre);
            }
            // TODO F02 : 
			Utilisez la méthode RéférencerMembresMatchs(...) pour ajouter les membres à la collection des "matchs" de chaque membre. 
            // Populez cboMembresGauche et cboMembresDroite avec les noms des membres et les collections m_colHommes et m_colFemmes avec les membres respectifs
            foreach (Membre membre in m_colTousLesMembres)
            {
                membre.RéférencerMembresMatchs(m_colTousLesMembres);
                if (membre.Genre == 'H')
                {
                    cboMembresGauche.Items.Add(membre.Nom);
                    m_colHommes.Add(membre);
                }
                else
                {
                    cboMembresDroite.Items.Add(membre.Nom);
                    m_colFemmes.Add(membre);
                }
            }

            objFichier.Close();
        }
        #endregion

        #region AfficherUneListeDeMembres (Affiche une liste de membre dans un ListView)
        //=====================================================================================
        /// <summary>
        /// Crée une ImageList dynamiquement qui contiendra les photos des membres.
        /// Affiche chaque membre de la collection dans une Liste utilisant l'option LargeIcon pour la propriété View. 
        ///  Assigne à la propriété LargeImageList de la liste l'ImageList contenant les photos des membres
        /// </summary>
        /// <param name="pListView">ListView à utiliser pour afficher les membres</param> 
        /// <param name="pListeMembres">Liste de membres à afficher</param>
        private void AfficherUneListeDeMembres(ListView pListView, List<Membre> pListeMembres)
        {
            pListView.BeginUpdate();
            pListView.Items.Clear();

            ImageList imageListLarge = new ImageList();
            imageListLarge.ImageSize = new Size(64, 96);
            imageListLarge.ColorDepth = ColorDepth.Depth32Bit;

            for (int index = 0; index < pListeMembres.Count; index++)
            {
                Membre membreCourant = pListeMembres[index];
                imageListLarge.Images.Add(membreCourant.Photo);
                pListView.Items.Add(new ListViewItem(membreCourant.Nom + "\n" + membreCourant.Age, index));
            }

            pListView.LargeImageList = imageListLarge;
            pListView.EndUpdate();
        }
        #endregion

        #region FrmPrincipal_FormClosing (Enregistre la liste des membres)
        //=====================================================================================
        /// <summary>
        /// Enregistre dans le fichier "Membres.txt" les informations des membres lors de la fermeture fu formulaire
        /// </summary>
        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter objFichier = new StreamWriter("Membres.txt");
            foreach (Membre membre in m_colTousLesMembres)
                objFichier.WriteLine(membre.ToString());
            objFichier.Close();
        }
        #endregion

        #region cboMembres SelectedIndexChanged (Modifie le membre courant)
        //=====================================================================================
        /// <summary>
        /// // TODO F03 : La sélection dans le cboGauche, entraine l'affichage :
        /// - Des informations du membre (photo, âge)
        /// - La liste des membres qui sont des "matchs" du membre
        /// Appeler MettreAJourSelonContexte() 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboMembres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MembreCourantGauche!= null)
            {
                picMembreGauche.Image = MembreCourantGauche.Photo;
                lblAgeGauche.Text = MembreCourantGauche.Age.ToString();
                AfficherUneListeDeMembres(lsvMatchsGauche, MembreCourantGauche.ColMatchs);
            }
            else
            {
                picMembreDroite.Image = null;
                lblAgeDroite.Text = "";
            }
            MettreAJourSelonContexte();
        }
        //=====================================================================================
        /// <summary>
        /// La sélection dans le cboDroite,entraine l'affichage :
        /// - Des informations du membre (photo, âge)
        /// - La liste des membres qui sont des "matchs" du membre
        /// Appeler MettreAJourSelonContexte() 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboMembresDroite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MembreCourantDroite != null)
            {
                picMembreDroite.Image = MembreCourantDroite.Photo;
                lblAgeDroite.Text = MembreCourantDroite.Age.ToString();
                AfficherUneListeDeMembres(lsvMatchsDroite, MembreCourantDroite.ColMatchs);
            }
            else
            {
                picMembreGauche.Image = null;
                lblAgeGauche.Text = "";
            }
            MettreAJourSelonContexte();
        }
        #endregion

        #region Événement : lsvMatchs SelectedIndexChanged et lsvMatchsDroite_SelectedIndexChanged
        //=====================================================================================
        /// <summary>
        /// Active/désactive btnSupprimerMatchGauche selon que des éléments de lsvMatchsGauche sont sélectionnés ou pas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsvMatchsGauche_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSupprimerMatchGauche.Enabled = lsvMatchsGauche.SelectedIndices.Count > 0;
        }
        //=====================================================================================
        /// Active/désactive btnSupprimerMatchDroite selon que des éléments de lsvMatchsDroite sont sélectionnés ou pas
        private void lsvMatchsDroite_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSupprimerMatchDroite.Enabled = lsvMatchsDroite.SelectedIndices.Count > 0;
        }
        #endregion

        #region Événement : Match DoubleClick (Doit connecter ce membre de l'autre côté)
        //=====================================================================================
        /// <summary>
        /// TODO F04 : Determine le membre sélectionné à partir de la sélection dans la liste de "matchs" du membre courant de gauche
        /// Utilise le nom du membre sélectionné pour le sélectionner dans le cboDroite. Voir démo
        /// Appeler MettreAJourSelonContexte();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsvMatchsGauche_DoubleClick(object sender, EventArgs e)
        {
            Membre membreSélectionné = MembreCourantGauche.ColMatchs[lsvMatchsGauche.SelectedIndices[0]];
            cboMembresDroite.Text = membreSélectionné.Nom;
            MettreAJourSelonContexte();
        }
        //=====================================================================================
        /// <summary>
        /// Determine le membre sélectionné à partir de la sélection dans la liste de "matchs" du membre courant de droite
        /// Utilise le nom du membre sélectionné pour le sélectionner dans le cboGauche. Voir démo. 
        /// Appeler MettreAJourSelonContexte();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsvMatchsDroite_DoubleClick(object sender, EventArgs e)
        {
            Membre membreSélectionné = MembreCourantDroite.ColMatchs[lsvMatchsDroite.SelectedIndices[0]];
            cboMembresGauche.Text = membreSélectionné.Nom;
            MettreAJourSelonContexte();
        }
        #endregion

        #region Événement : SupprimerMatch (Supprime un match dans les collections respectives des deux membres courants)
        //=====================================================================================
        /// <summary>
        /// // TODO F05 : Supprime le membre sélectionné dans la liste de "matchs" de gauche
        /// Supprime le membre courant dans les "matchs" à droite 
        /// Affiche la liste de "matchs" du membre courant de gauche et celle de droite
        /// Met-à-jour le contexte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerMatchGauche_Click(object sender, EventArgs e)
        {
            int selectedIndex = lsvMatchsGauche.SelectedIndices[0];
            Membre membreASupprimer = MembreCourantGauche.ColMatchs[selectedIndex];
            membreASupprimer.ColMatchs.Remove(MembreCourantGauche);
            MembreCourantGauche.ColMatchs.RemoveAt(selectedIndex);

            AfficherUneListeDeMembres(lsvMatchsGauche, MembreCourantGauche.ColMatchs);
            AfficherUneListeDeMembres(lsvMatchsDroite, MembreCourantDroite.ColMatchs);
            MettreAJourSelonContexte();
        }
        //=====================================================================================
        /// <summary>
        /// // TODO F06 : Supprime le membre sélectionné dans la liste de "matchs" de droite
        /// Supprime le membre courant des "matchs" à gauche
        /// Affiche la liste de "matchs" du membre courant de gauche
        /// Affiche la liste de "matchs" du membre courant de droite
        /// Met-à-jour le contexte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerMatchDroite_Click(object sender, EventArgs e)
        {
            int selectedIndex = lsvMatchsDroite.SelectedIndices[0];
            Membre membreASupprimer = MembreCourantDroite.ColMatchs[selectedIndex];
            membreASupprimer.ColMatchs.Remove(MembreCourantDroite);
            MembreCourantDroite.ColMatchs.RemoveAt(selectedIndex);

            AfficherUneListeDeMembres(lsvMatchsGauche, MembreCourantGauche.ColMatchs);
            AfficherUneListeDeMembres(lsvMatchsDroite, MembreCourantDroite.ColMatchs);
            MettreAJourSelonContexte();
        }
        #endregion

        #region Événement : btnNouveauMatch_Click (Ajoute un match pour les deux membres courants)
        //=====================================================================================
        /// <summary>
        /// TODO F07 : Ajoute les deux membres courants dans les matchs respectifs
        /// Affiche les "matchs" du membre courant gauche et droite
        /// Met-à-jour selon le contexte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNouveauMatch_Click(object sender, EventArgs e)
        {
            MembreCourantGauche.ColMatchs.Add(MembreCourantDroite);
            MembreCourantDroite.ColMatchs.Add(MembreCourantGauche);

            AfficherUneListeDeMembres(lsvMatchsGauche, MembreCourantGauche.ColMatchs);
            AfficherUneListeDeMembres(lsvMatchsDroite, MembreCourantDroite.ColMatchs);
            MettreAJourSelonContexte();
        }
        #endregion

        #region MettreAJourSelonContexte
        //=====================================================================================
        /// <summary>
        /// Met-à-jour l'interface selon le contexte
        /// voir démo
        /// </summary>
        private void MettreAJourSelonContexte()
        {
            bool estUnMatch = MembreCourantGauche.EstMatch(MembreCourantDroite);
            if (estUnMatch)
            {
                picMatch.Visible = true;
                panGauche.BackColor = Color.DarkViolet;
                panDroite.BackColor = Color.DarkViolet;
            }
            else
            {
                picMatch.Visible = false;
                panGauche.BackColor = Color.DimGray;
                panDroite.BackColor = Color.DimGray;
            }

            if (MembreCourantGauche != null)
                lblMatchGauche.Text = "Matchs de " + MembreCourantGauche.Nom;
            if (MembreCourantDroite != null)
                lblMatchDroite.Text = "Matchs de " + MembreCourantDroite.Nom;

            btnNouveauMatch.Enabled = cboMembresGauche.SelectedIndex != cboMembresDroite.SelectedIndex && !estUnMatch;

            btnSupprimerMatchGauche.Enabled = lsvMatchsGauche.SelectedIndices.Count > 0;
            btnSupprimerMatchDroite.Enabled = lsvMatchsDroite.SelectedIndices.Count > 0;
        }
        #endregion

    }
}