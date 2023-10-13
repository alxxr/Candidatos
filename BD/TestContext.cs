using BD;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class TestContext : DbContext
{
    #region Constructor
    //El contrutor va recibir un conjunto de opciones DbContextOptions
    //El DbContextOptions tiene un Generics
    //Usamos una variable llamada " options "
    //Luego la asignamos al padre la conexion
    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {

    }
    #endregion

    #region CallTablas
    //Con DbSet, Especificamos que esta BD tiene las siguientes tablas
    //Candidates & CandidatesExperiences
    public virtual DbSet<Candidates> Candidates { get; set; }

    public virtual DbSet<CandidateExperiences> CandidateExperiences { get; set; }
    #endregion

    #region OnConfiguring
    //Configuramos el proveedor de BD para el DbContext, anulando la Configuración
    //con OnConfiguring agregando una cadena de conexión a DbContextOptionsBuilder:
    //de esta manera el DbContext acepta el objeto DbContextOptions<TContext>
    //en su constructor y lo pasa al constructor base para DbContext
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Configura la cadena de conexión aquí
            optionsBuilder.UseSqlServer("Connection");
        }
    }
    #endregion

    #region OnModelCreating
    //Creamos el metodo OnModelCreating:
    //Con Fluent API, creamos las relaciones e indicamos las PK, AK.
    //Tambien le damos la longitud deseada a las columnas
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Metodo para eliminar datos en cascada, elimina todas las relaciones que tenga el Candidate y luego elimina el Candiate
        //No quise utilizar este metodo, cree otro directamente en el boton delete en mi Controller
        //modelBuilder.Entity<Candidates>().HasMany(c => c.CandidateExperiences).WithOne(e => e.IdCandidateFK).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Candidates>(entity =>
        {
            entity.HasKey(e => e.IdCandidate);

            entity.ToTable("candidates");

            entity.HasIndex(e => e.Email, "AK_candidates_Email").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(150);
        });

        modelBuilder.Entity<CandidateExperiences>(entity =>
        {
            entity.HasKey(e => e.IdCandidateExperience);

            entity.ToTable("candidateExperiences");

            entity.HasIndex(e => e.IdCandidate, "IX_candidateExperiences_IdCandidate");

            entity.Property(e => e.Company).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Job).HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("numeric(8, 2)");

            entity.HasOne(d => d.IdCandidateFK).WithMany(p => p.CandidateExperiences).HasForeignKey(d => d.IdCandidate);
        });

    }
    #endregion
}
