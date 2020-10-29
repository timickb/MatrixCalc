namespace MatrixCalc
{
    public enum MatrixType
    {
        /// <summary>
        /// Нулевая матрица
        /// </summary>
        Zeros,
        /// <summary>
        /// Матрица из единиц
        /// </summary>
        Ones,
        /// <summary>
        /// Единичная матрица
        /// (единицы по главной диагонали, остальные - нули)
        /// </summary>
        Eye,
        /// <summary>
        /// Матрица со случайными вещественными числами
        /// </summary>
        Random,
        /// <summary>
        /// Матрица со случайными целыми числами
        /// </summary>
        RandomInt
    }
}