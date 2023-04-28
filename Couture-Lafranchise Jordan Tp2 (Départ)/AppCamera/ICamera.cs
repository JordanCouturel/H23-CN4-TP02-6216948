namespace AppCamera
{
    /// <summary>
    /// Fournit les propriétés et méthodes pour simuler le fonctionnement d'une caméra numérique
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// Obtient la carte mémoire qui est actuellement dans la caméra.
        /// </summary>
        ICarteMemoire CarteMemoireCourante { get; }

        /// <summary>
        /// Obtient le facteur de compression déterminé à partir de la qualité sélectionnée
        /// </summary>
        int FacteurCompression { get; }

        /// <summary>
        /// Obtient et définit le paramètre flash de la caméra.
        /// </summary>
        enuFlash Flash { get; set; }

        /// <summary>
        /// Obtient l’objet photo à la position courante.
        /// </summary>
        IPhoto PhotoCourante { get; }

        /// <summary>
        /// Obtient et définit le paramètre dimension de la caméra.
        /// </summary>
        enuDimension Dimension { get; set; }

        /// <summary>
        /// Obtient la position de la photo courante. 
        /// </summary>
        int PosPhotoCourante { get; }

        /// <summary>
        /// Obtient et définit le paramètre qualité de la caméra.
        /// </summary>
        enuQualite Qualite { get; set; }

        /// <summary>
        /// Obtient la taille de la photo 
        /// </summary>
        long TailleEnOctetsEstimee { get; }

        /// <summary>
        /// Obtient la taille en pixels selon la dimension 
        /// </summary>
        System.Drawing.Size TailleEnPixelsSelonDimension { get; }

        /// <summary>
        /// Vide la carte mémoire actuellement dans la caméra.
        /// </summary>
        void ViderLaCarte();

        /// <summary>
        /// Obtient et définit le paramètre sensibilité de la caméra.
        /// </summary>
        enuSensibilite Sensibilite { get; set; }

        /// <summary>
        /// Permet de passer à la prochaine valeur de la dimension
        /// </summary>
        void DimensionSuivante();

        /// <summary>
        /// Permet de sauvegarder les 4 paramètres de la caméra dans le fichier.
        /// </summary>
        void EnregistrerParametres();

        /// <summary>
        /// Définit la réréfence de la carte mémoire courante
        /// </summary>
        /// <param name="pCarteMemoire">carte mémoire insérée</param>
        void InsererCarteMemoire(ICarteMemoire pCarteMemoire);

        /// <summary>
        /// Enlève toute référence à une carte mémoire.
        /// </summary>
        void EjecterCarteMemoire();

        /// <summary>
        /// Recule à la photo précédente. 
        /// </summary>
        void PhotoPrecedente();

        /// <summary>
        /// Avance à la photo suivante.
        /// </summary>
        void PhotoSuivante();

        /// <summary>
        /// Indique s’il est possible de prendre une photo actuellement avec les paramètres courant
        /// de la caméra.
        /// </summary>
        bool PeutPrendreUnePhoto();

        /// <summary>
        /// Crée une photo avec les paramètres actuels de la caméra. 
        /// </summary>
        void PrendrePhoto();

        /// <summary>
        /// Permet de passer à la prochaine valeur de la qualité
        /// </summary>
        void QualiteSuivante();

        /// <summary>
        /// Permet de passer à la prochaine valeur de la sensibilité
        /// </summary>
        void SensibiliteSuivante();

        /// <summary>
        /// Permet de passer à la prochaine valeur de la sensibilité
        /// </summary>
        void SupprimerPhotoCourante();

        /// <summary>
        /// Charge les paramètres à partir du fichier
        /// </summary>
        void ChargerParametres();

    }
}
