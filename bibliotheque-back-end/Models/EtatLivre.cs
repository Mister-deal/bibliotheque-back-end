namespace bibliotheque_back_end.Models
{
    //Enum Etat Livre
    //à améliorer: séparer état emprunt livre et état physique
    public enum EtatLivre
    {
        Disponible,
        Emprunte,
        Reserve,
        Endommage
    }
}
