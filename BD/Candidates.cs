using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BD
{
    public class Candidates
    {
        public int IdCandidate { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public DateTime Birthdate { get; set; }

        public string Email { get; set; } = null!;

        public DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public virtual ICollection<CandidateExperiences> CandidateExperiences { get; set; } = new List<CandidateExperiences>();

    }
}