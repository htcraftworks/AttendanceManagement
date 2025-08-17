using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.DbContexts
{
    /// <summary>
    /// インスタンスを作成するためのファクトリをDbContext定義
    /// </summary>
    public class MyDbContextFactory : IDbContextFactory<MyDbContext>
    {
        /// <summary>
        /// DbContextのインスタンス作成
        /// </summary>
        /// <returns>DbContext</returns>
        public MyDbContext CreateDbContext()
        {
            return Create(AppContext.BaseDirectory);
        }

        /// <summary>
        /// 基本設定ファイルからDB接続文字列を取得
        /// </summary>
        /// <param name="basePath">アプリパス</param>
        /// <returns>DbContext</returns>
        /// <exception cref="InvalidOperationException">無効な呼出しエラー</exception>
        private MyDbContext Create(string basePath)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(basePath).AddJsonFile("appsettings.json").AddInMemoryCollection();

            var config = builder.Build();

            var connectionString = config.GetConnectionString("DbConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find a connection string.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
