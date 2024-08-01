using Microsoft.EntityFrameworkCore;
using UtilityTools.Data.Domain;

namespace UtilityTools.Data
{
    public class SqliteDbContext : DbContext, IDbContext
    {
        #region Ctor

        public SqliteDbContext()
        {
        }

        static SqliteDbContext()
        {
            // Before running the app for the first time, follow these steps:
            // 0- Put this static methon in comments.
            // 1- Build -> Build the Project
            // 2- Tools –> NuGet Package Manager –> Package Manager Console
            // 3- Run "Add-Migration MyFirstMigration" to scaffold a migration to create the initial set of tables for your model
            // See here for more information https://docs.efproject.net/en/latest/platforms/uwp/getting-started.html#create-your-database
            // 4- Uncomment this method again.

            using (var database = new SqliteDbContext())
            {
                database.Database.Migrate();
            }
        }

        #endregion

        #region Utilities


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=UtilityTools.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Id required.
            modelBuilder.Entity<SearchHistoryItem>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<UtilityToolsSetting>()
               .Property(p => p.Id)
               .IsRequired();

            modelBuilder.Entity<MediaSymbol>()
              .Property(p => p.Id)
              .IsRequired();

            modelBuilder.Entity<ResourceUser>()
             .Property(p => p.Id)
             .IsRequired();

            modelBuilder.Entity<MediaKeyword>()
             .Property(p => p.Id)
             .IsRequired();

            modelBuilder.Entity<MediaHistory>()
            .Property(p => p.Id)
            .IsRequired();

            modelBuilder.Entity<SellerHub>()
          .Property(p => p.Id)
          .IsRequired();

            //png


            modelBuilder.Entity<PngImageCategoryRelation>()
                .HasKey(bc => new { bc.PngImageId, bc.LabelId });

            modelBuilder.Entity<PngImageCategoryRelation>()
                .Ignore(bc => bc.Id);

            modelBuilder.Entity<PngImageCategoryRelation>()
                .HasOne(bc => bc.PngCategory)
                .WithMany(b => b.PngImageLables)
                .HasForeignKey(bc => bc.LabelId);

            modelBuilder.Entity<PngImageCategoryRelation>()
                .HasOne(bc => bc.PngImage)
                .WithMany(b => b.PngImageLables)
                .HasForeignKey(bc => bc.PngImageId);

            //book

            modelBuilder.Entity<BookHistory>()
                .Property<int>("BookForeignKey");

            modelBuilder.Entity<BookHistory>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<BookHistory>()
                .HasOne(p => p.Book)
                .WithMany(bc => bc.BookHistories)
                .HasForeignKey("BookForeignKey");

            modelBuilder.Entity<BookCategory>()
                .Property(p => p.Id)
                .IsRequired();

            // Make Name required.
            modelBuilder.Entity<BookCategoryRelation>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });

            modelBuilder.Entity<BookCategoryRelation>()
                .Ignore(bc => bc.Id);

            modelBuilder.Entity<BookCategoryRelation>()
                .HasOne(bc => bc.BookCategory)
                .WithMany(b => b.BookCategoryRelations)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<BookCategoryRelation>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategoryRelations)
                .HasForeignKey(bc => bc.BookId);

            modelBuilder.Entity<Book>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<BookMark>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<BookMark>()
                .Property<int>("BookForeignKey");

            modelBuilder.Entity<BookMark>()
                .HasOne(p => p.Book)
                .WithMany(bc => bc.BookMarks)
                .HasForeignKey("BookForeignKey");
        }




        #endregion

        #region Methods



        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        ///// <summary>
        ///// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        ///// </summary>
        ///// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        ///// <param name="sql">The SQL query string.</param>
        ///// <param name="parameters">The parameters to apply to the SQL query string.</param>
        ///// <returns>Result</returns>
        //public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        //{
        //    this.Database.ExecuteSqlCommand(sql, parameters);
        //    return this.Database.ExecuteSqlCommand<TElement>(sql, parameters);
        //}

        ///// <summary>
        ///// Detach an entity
        ///// </summary>
        ///// <param name="entity">Entity</param>
        //public void Detach(object entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException("entity");

        //    ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        //}

        #endregion

        //#region Properties

        ///// <summary>
        ///// Gets or sets a value indicating whether proxy creation setting is enabled (used in EF)
        ///// </summary>
        //public virtual bool ProxyCreationEnabled
        //{
        //    get
        //    {
        //        return this.Configuration.ProxyCreationEnabled;
        //    }
        //    set
        //    {
        //        this.Configuration.ProxyCreationEnabled = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether auto detect changes setting is enabled (used in EF)
        ///// </summary>
        //public virtual bool AutoDetectChangesEnabled
        //{
        //    get
        //    {
        //        return this.Configuration.AutoDetectChangesEnabled;
        //    }
        //    set
        //    {
        //        this.Configuration.AutoDetectChangesEnabled = value;
        //    }
        //}

        //#endregion
    }
}
