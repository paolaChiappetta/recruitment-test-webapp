using InterviewTest.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InterviewTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            PrepareDB();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            // Inject employee store
            services.AddScoped<IEmployeeStore, EmployeeStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private void PrepareDB()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var delTableCmd = connection.CreateCommand();
                delTableCmd.CommandText = "DROP TABLE IF EXISTS Employees";
                delTableCmd.ExecuteNonQuery();

                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = "CREATE TABLE Employees(Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(50), Lastname  VARCHAR(50), Value INT, Address  VARCHAR(50), Phone  VARCHAR(50))";
                createTableCmd.ExecuteNonQuery();

                //Fill with data
                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = @"INSERT INTO Employees(Name, Lastname, Value, Address, Phone) VALUES
                        ('Abul', 'James', 1357, '44 La Trinidad Street', '+3493196631'),
                        ('Adolfo', 'Smith', 1224, '37 Velez Street', '+34602363636'),
                        ('Alexander', 'Williams', 2296, '11 Lope de Vega Street', '+346016666'),
                        ('Amber', 'Brown', 1145, '15 San Pablo Street', '+34802395216'),
                        ('Amy', 'Jones', 4359, '37 Velez Street', '+34602355656'),
                        ('Andy', 'James', 1966, '8 Rosario Street', '+34602363637'),
                        ('Anna', 'Gelbero', 4040, '51 Larios Street', '+34602368114'),
                        ('Antony', 'Ramires', 449, '17 Boqueron Street', '+3460236627'),
                        ('Ashley', 'Franco', 8151, '37 Cartago Street', '+34602333551'),
                        ('Borja', 'Lopes', 9428, '88 Tiro Street', '+34602410527'),
                        ('Cecilia', 'Alvarez', 2136, '13 Zamorano Street', '+34602598761'),
                        ('Christopher', 'Santana', 9035, '20 Lemus Street', '+34602620356'),
                        ('Dan', 'Willis', 1475, '66 Fermin Street', '+34602684792'),
                        ('Dario', 'Sztajnsrajber', 284, '37 Argentina Street', '+34602654785'),
                        ('David', 'Letterman', 948, '22 Hoover Street', '+346051323785'),
                        ('Elike', 'Rajan', 1860, '57 Albuquerque Street', '+34602854199'),
                        ('Ella', 'James', 4549, '37 Velez Street', '+34602361991'),
                        ('Ellie', 'Williams', 5736, '74 Tiron Street', '+346023777844'),
                        ('Elliot', 'Ness', 1020, '14 Torino Street', '+34602363636'),
                        ('Emily', 'Rose', 7658, '31 Tomsthon Street', '+34602541785'),
                        ('Faye', 'Rico', 7399, '15 Zimbabwe Street', '+34602514568'),
                        ('Fern', 'Miras', 1422, '77 Riverdale Street', '+34602541786'),
                        ('Francisco', 'Toledo', 5028, '88 Bernal Street', '+34602654456'),
                        ('Frank', 'Grace', 3281, '31 Village Street', '+34602311223'),
                        ('Gary', 'Liam', 9190, '54 Twins Street', '+34602855469'),
                        ('Germaine', 'Griffin', 6437, '61 O’Higgins Street', '+34602654844'),
                        ('Greg', 'O´Brien', 5929, '11 Berries Street', '+34605904784'),
                        ('Harvey', 'Dent', 8471, '22 Gottam Street', '+34602223366'),
                        ('Helen', 'Parrish', 963, '17 Fortfield Street', '+34602355648'),
                        ('Huzairi', 'Hallam', 9491, '35 Colombus Street', '+34602846331'),
                        ('Izmi', 'Palem', 8324, '57 Torin Street', '+34602846201'),
                        ('James', 'Diaz', 6994, '97 Tobias Street', '+34602852852'),
                        ('Jarek', 'Ruiz', 6581, '87 Goldfarb Street', '+34602521654'),
                        ('Jim', 'Torino', 202, '37 Venecia Street', '+34602348487'),
                        ('John', 'Smiths', 261, '34 Greenroad Street', '+34602319877'),
                        ('Jose', 'Velez', 1605, '26 Standford Street', '+34503648795'),
                        ('Josef', 'Rot', 3714, '44 Altamira Street', '+3460250465'),
                        ('Karthik', 'Monroe', 4828, '10 Pluton Street', '+34602522478'),
                        ('Katrin', 'Jones', 5393, '31 New Mexico Street', '+34602524545'),
                        ('Lee', 'Wong', 269, '7 Buffalo Street', '+346025044456'),
                        ('Luke', 'Marbel', 5926, '8 Jedi Street', '+34602254565'),
                        ('Madiha', 'Oman', 2329, '34 San Antonio Street', '+34602578487'),
                        ('Marc', 'Weiss', 3651, '77 Cabo Verde Street', '+34602654981'),
                        ('Marina', 'Magritte', 6903, '14 Goodwinds Street', '+34602987654'),
                        ('Mark', 'Smith', 3368, '86 Trujillo Street', '+3460285014'),
                        ('Marzena', 'Morello', 7515, '74 Esperanza Street', '+34602654921'),
                        ('Mohamed', 'Ammuni', 1080, '16 Saturn Street', '+34602323790'),
                        ('Nichole', 'keyly', 1221, '74 Arkham Street', '+34602371990'),
                        ('Nikita', 'Morales', 8520, '55 Loke Street', '+346023669987'),
                        ('Oliver', 'Twist', 2868, '74 Eggstown Street', '+34602223547'),
                        ('Patryk', 'Montaner', 1418, '7 Felicity Street', '+346023654852'),
                        ('Paul', 'McCan', 4332, '34 Perez Street', '+34602367415'),
                        ('Ralph', 'Gorgorie', 1581, '123 Frias Street', '+34602108901'),
                        ('Raymond', 'Ayala Rodríguez', 7393, '13 Bronx Street', '+3460246852'),
                        ('Roman', 'Palacios', 4056, '78 Caminito Street', '+346028556987'),
                        ('Ryan', 'Raynols', 252, '22 Deadpool Street', '+34602741852'),
                        ('Sara', 'Cars', 2618, '91 Colorado Street', '+34601566988'),
                        ('Sean', 'Diaz', 691, '14 Solar Street', '+346028547785'),
                        ('Seb', 'Aleph', 5395, '77 Sanchez Street', '+34602385256'),
                        ('Sergey', 'Rumanoff', 8282, '37 Altamira Street', '+34602398778'),
                        ('Shaheen', 'Katamaka', 3721, '10 Orks Street', '+34602395195'),
                        ('Sharni', 'Brown', 7737, '34 Antartic Street', '+34602145687'),
                        ('Sinu', 'Meves', 3349, '1 Notoi Street', '+34602363636'),
                        ('Stephen', 'Buley', 8105, '1408 Polar Street', '+34602395195'),
                        ('Tim', 'Lopez', 8386, '12 Frankenweenie Street', '+34602154877'),
                        ('Tina', 'Turner', 5133, '37 Las Vegas Street', '+346023458965'),
                        ('Tom', 'Andy', 7553, '12 Ralph Street', '+346023951478'),
                        ('Tony', 'Mars', 4432, '22 Italia Street', '+346023123658'),
                        ('Tracy', 'Yale', 1771, '25, Unversity Street', '+34602852456'),
                        ('Tristan', 'Diaz Ocampo', 2030, '37, Pergamino Street', '+3460285264'),
                        ('Victor', 'Linz', 1046, '14 Survivor Street', '+34602852477'),
                        ('Yury', 'Carnevale', 1854, '44, Deroux Street', '+34602366332')";

                    insertCmd.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }
    }
}
