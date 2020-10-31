﻿using System;
using MatrixCalc.Linalg;

namespace MatrixCalc
{
    public class Help : ICommand
    {
        public Help(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public string Run(string[] args)
        {
            // Тупо выводим текст с описанием всех команд.
            var sep = Environment.NewLine;
            var text = "Взаимодействие с этим калькулятором происходит с помощью консольных команд. " +
                       "Пользователь может создавать матрицы, давать им имена, а затем выполнять над ними" +
                       " различные операции." + sep + sep;
            text += "Какие ограничения присутствуют:" + sep;
            text += $"1. Длина каждого измерения матрицы не должна превышать {Matrix.MaxDimensionSize} {sep}";
            text += $"2. Каждое число в матрице по модулю не должно превышать {Matrix.MaxAbsValue}, отсюда" +
                    " следует, что нельзя перемножить матрицы, в которых хотя бы один элемент превышает" +
                    $" квадратный корень из этого ограничения.{sep}";
            text += "3. Все ограничения, накладываемые самой линейной алгеброй. Например, нельзя посчитать" +
                    " определитель неквадратной матрицы." + sep + sep;
            text += "Список доступных команд:" + sep;
            text +=
                "-> create <rows> <cols> [name] - создать матрицу из rows строк и cols столбцов. Матрице можно задать" +
                " имя. Если имя явно не указано, по умолчанию оно задается как matrixN, где N - порядковый номер" +
                " созданной матрицы в рамках данной системы." + sep;
            text += "-> setrnd <lower_bound> <upper_bound> - установить нижнее и верхнее значение числа в матрице" +
                    " для генератора рандомных матриц. По умолчанию нижнее = -100, верхнее = 100." + sep;
            text += "-> load <path_to_file> - загрузить в систему матрицу из файла (путь к файлу обязан быть" +
                    " абсолютным). При этом матрица принимает такое же имя, какое имеет файл. Файл должен быть" +
                    " набором из строк, в каждой из которых через пробел записано одинаковое количество" +
                    " вещественных чисел." + sep;
            text += "-> save <matrix_name> <path_to_file> - сохранить матрицу с именем matrix_name в файл." + sep;
            text += "-> list - вывести список всех существующих в системе матриц." + sep;
            text += "-> display <matrix_name> - вывести матрицу на экран." + sep;
            text += "-> det <matrix_name> - вывести определитель матрицы." + sep;
            text += "-> rank <matrix_name> - вывести ранг матрицы." + sep;
            text += "-> trace <matrix_name> - вывести след матрицы." + sep;
            text += "-> sum <matrix1_name> <matrix2_name> [output_name] - суммировать две матрицы. Если задан " +
                    " параметр output_name (имя матрицы-результата), то результат поместится в новую матрицу с этим" +
                    " именем. В противном случае результат операции будет выведен в консоль." + sep;
            text += "-> sub <matrix1_name> <matrix2_name> [output_name] - аналогично, только считается разность." + sep;
            text += "-> mul <matrix1_name> <matrix2_name> [output_name] - аналогично, только считается произведение" +
                    sep;
            text += "-> trans <matrix_name> [output_name] - аналогично, транспонирование матрицы." + sep;
            text += "-> muln <matrix1_name> <number> [output_name] - аналогично, умножение матрицы на скаляр. " + sep;

            return text;
        }
    }
}