using BD;
using Candidatos.DataAccess.Interfaces;
using Candidatos.DataAccess.Servicios;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Candidatos
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Repositorys
            services.AddScoped<IRepositoryAsync<Candidates>, RepositoryAsync<Candidates>>();

            services.AddScoped<IRepositoryAsync<CandidateExperiences>, RepositoryAsync<CandidateExperiences>>();
            #endregion

            #region Connection
            //Una BD con Code-First la podemos versionar, al correr el comando EntityFrameworkCore\Add-Migration
            //(Utilizamos EntityFrameworkCore ya que es mas compatible con .Net Framework 6.0 y el soporte)
            //se nos crea la carpeta Migrations en donde visualizaremos lo cambios que haremos en la BD,
            //luego corremos el comando update-database para que corra todas las migraciones que esten pendientes
            //de esta manera ya veremos los cambios evidenciados en el proveedor de Bases de datos


            // Configuramos la inyección de dependencias para el DbContext.
            // Esto separa la responsabilidad de creación de objetos, donde una clase no necesita
            // ser responsable de crear sus propios objetos, sino que los recibe como dependencias.
            // Aquí, registramos el DbContext 'TestContext' y configuramos su conexión a la base de datos
            // mediante Entity Framework Core, utilizando la cadena de conexión especificada en la configuración.
            services.AddDbContext<TestContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Connection"))
            );
            #endregion

            return services;
        }
    }
}
