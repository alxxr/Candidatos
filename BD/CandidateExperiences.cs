using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD
{
    public class CandidateExperiences
    {
        public int IdCandidateExperience { get; set; }

        public string Company { get; set; } = null!;

        public string Job { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Salary { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public virtual int IdCandidate { get; set; }

        [ForeignKey("IdCandidate")]
        public Candidates? IdCandidateFK { get; set; }
    }
}
