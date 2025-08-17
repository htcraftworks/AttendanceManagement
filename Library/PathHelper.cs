namespace Library
{
    /// <summary>
    /// パス操作ヘルパークラス
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// 指定した階層数だけ親ディレクトリを取得します。
        /// </summary>
        /// <param name="startPath">開始パス（例: AppContext.BaseDirectory）</param>
        /// <param name="levelsUp">親にさかのぼる階層数（例: 2）</param>
        /// <returns>指定した階層上のディレクトリパス</returns>
        /// <exception cref="ArgumentNullException">startPath が null または空</exception>
        /// <exception cref="InvalidOperationException">指定階層まで遡れなかった場合</exception>
        public static string GetAncestorDirectory(string startPath, int levelsUp)
        {
            if (string.IsNullOrWhiteSpace(startPath))
            {
                throw new ArgumentNullException(nameof(startPath), "開始パスが null または空です。");
            }

            var dir = new DirectoryInfo(startPath);

            for (int i = 0; i < levelsUp; i++)
            {
                if (dir.Parent == null)
                {
                    throw new InvalidOperationException($"'{startPath}' から {levelsUp} 階層上には移動できません。");
                }

                dir = dir.Parent;
            }

            return dir.FullName;
        }

        /// <summary>
        /// 指定階層上のディレクトリに指定フォルダ名を結合して取得します。
        /// </summary>
        /// <param name="startPath">開始パス（通常は AppContext.BaseDirectory）</param>
        /// <param name="levelsUp">親にさかのぼる階層数</param>
        /// <param name="folderName">結合したいフォルダ名（例: "XML"）</param>
        /// <returns>結合されたパス</returns>
        public static string GetPathFromAncestor(string startPath, int levelsUp, string folderName)
        {
            var ancestor = GetAncestorDirectory(startPath, levelsUp);
            return Path.Combine(ancestor, folderName);
        }
    }
}
