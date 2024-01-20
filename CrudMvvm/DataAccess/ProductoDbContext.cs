using CrudMvvm.Modelos;
using CrudMvvm.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace CrudMvvm.DataAccess
{
    //Hereda el DbContext para aplicar entityFramework
    public class ProductoDbContext : DbContext
    {
        //tablas de la base de datos
        public DbSet<Producto> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //configurar la cadena de conexion 
            string conexionDB = $"Filename={ConexionDB.DevolverRuta("productos.db")}";
            optionsBuilder.UseSqlite(conexionDB);
        }

        //modelado de la tabla 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(col => col.IdProducto);
                entity.Property(col => col.IdProducto).IsRequired().ValueGeneratedOnAdd(); //Para que se autogenere el id del producto
            });
        }
    }
}
