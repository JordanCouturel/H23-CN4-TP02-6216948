namespace AppCamera
{
    /// <summary>
    /// Fournit les propriétés et méthodes pour simuler le fonctionnement d'une carte mémoire
    /// </summary>
    public interface ICarteMemoire
    {
        /// <summary>
        /// Ajoute la photo à la collection de photos.
        /// </summary>
        /// <param name="pobjPhoto">Photo à ajouter</param>
        void Ajouter(IPhoto pobjPhoto);

        /// <summary>
        /// Charge la collection de photos à partir du fichier texte.
        /// </summary>
        void ChargerPhotos();

        /// <summary>
        /// Enregistre la collection de photos dans le fichier texte.
        /// </summary>
        void EnregistrerPhotos();

        /// <summary>
        /// Obtient l’espace en octets disponible sur la carte mémoire
        /// </summary>
        long EspaceDisponible { get; }

        /// <summary>
        /// Obtient l’espace utilisé en octets.
        /// </summary>
        long EspaceUtilise { get; }

        /// <summary>
        /// Obtient le nombre de photos actuellement stockées dans la collection.
        /// </summary>
        int NbPhotos { get; }

        /// <summary>
        /// Obtient le nom de la carte mémoire.
        /// </summary>
        string Nom { get; }

        /// <summary>
        /// Obtient la taille en octets de la carte mémoire. 
        /// </summary>
        long TailleEnOctets { get; }

        /// <summary>
        /// Indique si l’espace disponible est suffisant pour éventuellement ajouter la photo sur la carte.
        /// </summary>
        /// <param name="pobjPhoto">Photo qui serait ajouté</param>
        /// <returns>True s'il y a assez d'espace, false sinon</returns>
        bool PeutAjouter(IPhoto pobjPhoto);

        /// <summary>
        /// Obtient l’objet photo dont la position est indiquée en paramètre.
        /// </summary>
        /// <param name="pPosition">Position de la photo dans la collection</param>
        /// <returns>L'objet photo à cette position</returns>
        IPhoto PhotoAt(int pPosition);

        /// <summary>
        /// Supprime de la collection, la photo dont la position est indiquée en paramètre
        /// </summary>
        /// <param name="pPosition">Position de la photo dans la collection</param>
        void SupprimerAt(int pPosition);

        /// <summary>
        /// Vide complètement la collection de photos.
        /// </summary>
        void Vider();
    }
}
