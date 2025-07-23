using bibliotheque_back_end.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace bibliotheque_back_end.Models.DTO
{
    public class EmpruntDto
    {
        [SwaggerSchema("Id de l'emprunt")]
        public int Id { get; set; }

        [Required]
        [SwaggerSchema("Date de l'emprunt")]
        public DateOnly DateEmprunt { get; set; }

        [SwaggerSchema("Date de retour")]
        public DateOnly? DateRetour { get; set; }

        //Tuple comme dans LivreDTO
        public EmpruntDto(Emprunt emprunt) =>
            (Id, DateEmprunt, DateRetour) = (emprunt.Id, emprunt.DateEmprunt, emprunt.DateRetour);
        
        //public EmpruntDto(Emprunt emprunt) // sans Tuple (à titre d'exemple(la manière dont j'ai l'habitude))
        //{
        //    this.Id = emprunt.Id;
        //    this.DateEmprunt = emprunt.DateEmprunt;
        //    this.DateRetour = emprunt.DateRetour;
        //}
    }
}
