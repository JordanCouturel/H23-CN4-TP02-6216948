using System;
namespace AppCamera
{
    /// <summary>
    /// Fournit les propriétés et méthodes pour simuler une photo numérique
    /// </summary>
    public interface IPhoto
    {
        /// <summary>
        /// Obtient la date et l’heure où la photo a été prise.
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// Obtient le facteur de compression de la photo.
        /// </summary>
        int FacteurCompression { get; }

        /// <summary>
        /// Obtient si le flash est activé ou désactivé sur la photo.
        /// </summary>
        enuFlash Flash { get; }

        /// <summary>
        /// Obtient la sensibilité utilisée pour  la photo.
        /// </summary>
        enuSensibilite Sensibilite { get; }

        /// <summary>
        /// Obtient la taille en octets de la photo 
        /// </summary>
        int TailleEnOctets { get; }

        /// <summary>
        /// Obtient la taille en pixels de la photo 
        /// </summary>
        System.Drawing.Size TailleEnPixels { get; }

        /// <summary>
        /// Retourne la représentation de cette instance sous forme de chaîne.
        /// </summary>
        /// <returns>Uune chaîne dont les données sont séparées par une virgule.</returns>
        string ToString();
    }
}
